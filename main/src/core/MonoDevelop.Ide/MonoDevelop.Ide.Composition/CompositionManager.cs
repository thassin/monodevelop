﻿// CompositionManager.cs
//
// Author:
//   Kirill Osenkov <https://github.com/KirillOsenkov>
//
// Copyright (c) 2017 Microsoft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.VisualStudio.Composition;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.AddIns;

namespace MonoDevelop.Ide.Composition
{
	/// <summary>
	/// The host of the MonoDevelop MEF composition. Uses https://github.com/Microsoft/vs-mef.
	/// </summary>
	public class CompositionManager
	{
		static Task<CompositionManager> creationTask;
		static CompositionManager instance;

		static readonly Resolver StandardResolver = Resolver.DefaultInstance;
		static readonly PartDiscovery Discovery = PartDiscovery.Combine (
			new AttributedPartDiscoveryV1 (StandardResolver),
			new AttributedPartDiscovery (StandardResolver, true));

		public static CompositionManager Instance {
			get {
				if (instance == null) {
					instance = InitializeAsync ().Result;
				}

				return instance;
			}
		}

		/// <summary>
		/// Starts initializing the MEF composition on a background thread. Thread-safe.
		/// </summary>
		public static Task<CompositionManager> InitializeAsync ()
		{
			if (creationTask == null) {
				lock (typeof (CompositionManager)) {
					if (creationTask == null) {
						creationTask = Task.Run (() => CreateInstanceAsync ());
					}
				}
			}

			return creationTask;
		}

		/// <summary>
		/// Returns an instance of type T that is exported by some composition part. The instance is shared (singleton).
		/// </summary>
		public static T GetExportedValue<T> ()
		{
			return Instance.ExportProvider.GetExportedValue<T> ();
		}

		public RuntimeComposition RuntimeComposition { get; private set; }
		public IExportProviderFactory ExportProviderFactory { get; private set; }
		public ExportProvider ExportProvider { get; private set; }
		public HostServices HostServices { get; private set; }
		public System.ComponentModel.Composition.Hosting.ExportProvider ExportProviderV1 { get; private set; }

		internal CompositionManager ()
		{
		}

		static async Task<CompositionManager> CreateInstanceAsync ()
		{
			var compositionManager = new CompositionManager ();
			await compositionManager.InitializeInstanceAsync ();
			return compositionManager;
		}

		async Task InitializeInstanceAsync ()
		{
			ComposableCatalog catalog = ComposableCatalog.Create (StandardResolver)
				.WithCompositionService ()
				.WithDesktopSupport ();

			var assemblies = new HashSet<Assembly> ();

			string[] extraLibraries = new [] {

				"MonoDevelop.Ide",	// PlaformCatalog

			//	// for ContentTypeRegistryImpl etc...
			//	"Microsoft.VisualStudio.CoreUtilityImplementation",
			//	"Microsoft.VisualStudio.CoreUtilityImplementation

			// TODO all needed???

				"Microsoft.VisualStudio.CoreUtility",
				"Microsoft.VisualStudio.CoreUtilityImplementation",
				"Microsoft.VisualStudio.Language.Implementation",
				"Microsoft.VisualStudio.Logic.Text.Classification.Aggregator.Implementation",
				"Microsoft.VisualStudio.Logic.Text.Navigation.Implementation",
				"Microsoft.VisualStudio.Logic.Text.Tagging.Aggregator.Implementation",
				"Microsoft.VisualStudio.Text.BraceCompletion.Implementation",
				"Microsoft.VisualStudio.Text.Data.Utilities",
				"Microsoft.VisualStudio.Text.Differencing.Implementation",
				"Microsoft.VisualStudio.Text.EditorOptions.Implementation",
				"Microsoft.VisualStudio.Text.Model.Implementation",
				"Microsoft.VisualStudio.Text.MultiCaret.Implementation",
				"Microsoft.VisualStudio.Text.UI.Utilities",
				"Microsoft.VisualStudio.UI.Text.Commanding.Implementation"

			};

			foreach (var asmName in extraLibraries) {

Console.WriteLine( "oeDEBUG :: CompositionManager :: INIT-1 " + asmName );

				try {
					var asm = Assembly.Load (asmName);
					if (asm == null)
						continue;

//	foreach ( Type t in asm.GetTypes() ) {
//		Console.WriteLine( "oeDEBUG :: CompositionManager :: I1-type " + t.Name );
//	}

					assemblies.Add (asm);
				} catch (Exception ex) {
					LoggingService.LogError ("Error - can't load extra assembly: " + asmName, ex);
				}
			}

Console.WriteLine( "oeDEBUG :: CompositionManager :: INIT-addins-start" );

			ReadAssembliesFromAddins (assemblies, "/MonoDevelop/Ide/TypeService/PlatformMefHostServices");
			ReadAssembliesFromAddins (assemblies, "/MonoDevelop/Ide/TypeService/MefHostServices");
			ReadAssembliesFromAddins (assemblies, "/MonoDevelop/Ide/Composition");

Console.WriteLine( "oeDEBUG :: CompositionManager :: INIT-addins-completed" );

			// spawn discovery tasks in parallel for each assembly
			var tasks = new List<Task<DiscoveredParts>> (assemblies.Count);
			foreach (var assembly in assemblies) {

Console.WriteLine( "oeDEBUG :: discovering " + assembly.FullName );

				var task = Task.Run (() => Discovery.CreatePartsAsync (assembly));
				tasks.Add (task);
			}

			foreach (var task in tasks) {
				catalog = catalog.AddParts (await task);
			}

			var discoveryErrors = catalog.DiscoveredParts.DiscoveryErrors;
			if (!discoveryErrors.IsEmpty) {
				throw new ApplicationException ($"MEF catalog scanning errors encountered.\n{string.Join ("\n", discoveryErrors)}");
			}

			CompositionConfiguration configuration = CompositionConfiguration.Create (catalog);

			if (!configuration.CompositionErrors.IsEmpty) {
				// capture the errors in an array for easier debugging
				var errors = configuration.CompositionErrors.ToArray ();

				// For now while we're still transitioning to VSMEF it's useful to work
				// even if the composition has some errors. TODO: re-enable this.

				var messages = errors.SelectMany (e => e).Select (e => e.Message);
				var text = string.Join (Environment.NewLine, messages);

				//Xwt.Clipboard.SetText (text);
Console.WriteLine( "" );
Console.WriteLine( "" );
Console.WriteLine( "oeDEBUG :: SOME MEF PROBLEMS:" );
Console.WriteLine( text );
Console.WriteLine( "" );
Console.WriteLine( "" );

				// un-comment this to stop on errors:
				//configuration.ThrowOnErrors ();
			}

			RuntimeComposition = RuntimeComposition.CreateRuntimeComposition (configuration);
			ExportProviderFactory = RuntimeComposition.CreateExportProviderFactory ();
			ExportProvider = ExportProviderFactory.CreateExportProvider ();
			HostServices = MefV1HostServices.Create (ExportProvider.AsExportProvider ());
			ExportProviderV1 = NetFxAdapters.AsExportProvider (ExportProvider);
		}

		void ReadAssembliesFromAddins (HashSet<Assembly> assemblies, string extensionPath)
		{
			foreach (var node in AddinManager.GetExtensionNodes (extensionPath)) {
				var assemblyNode = node as AssemblyExtensionNode;
				if (assemblyNode != null) {
					try {
						// Make sure the add-in that registered the assembly is loaded, since it can bring other
						// other assemblies required to load this one
						AddinManager.LoadAddin (null, assemblyNode.Addin.Id);

						var assemblyFilePath = assemblyNode.Addin.GetFilePath (assemblyNode.FileName);
						var assembly = Runtime.SystemAssemblyService.LoadAssemblyFrom (assemblyFilePath);
						assemblies.Add (assembly);
					}
					catch (Exception e) {
						LoggingService.LogError ("Composition can't load assembly " + assemblyNode.FileName, e);
					}
				}
			}
		}
	}
}
