// CompositionManager.cs
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
//using Microsoft.VisualStudio.Composition; oe removed...
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.AddIns;

// oe NOTICE, lots of changes here since we need to use our own ExportProviderMD.
// oe NOTICE, lots of changes here since we need to use our own ExportProviderMD.
// oe NOTICE, lots of changes here since we need to use our own ExportProviderMD.

namespace MonoDevelop.Ide.Composition
{
	/// <summary>
	/// The host of the MonoDevelop MEF composition. Uses https://github.com/Microsoft/vs-mef.
	/// </summary>
	public class CompositionManager
	{

	// oe REMOVED...
	//oe	static Task<CompositionManager> creationTask;
	//oe	static readonly Resolver StandardResolver = Resolver.DefaultInstance;
	//oe	static readonly PartDiscovery Discovery = PartDiscovery.Combine (
	//oe		new AttributedPartDiscoveryV1 (StandardResolver),
	//oe		new AttributedPartDiscovery (StandardResolver, true));

		static CompositionManager instance;

		public static CompositionManager Instance {
			get {
				if (instance == null) {
				//oe	instance = InitializeAsync ().Result;
					instance = new CompositionManager();
				}

				return instance;
			}
		}

	// oe REMOVED...
	//oe	public static Task<CompositionManager> InitializeAsync ()

		/// <summary>
		/// Returns an instance of type T that is exported by some composition part. The instance is shared (singleton).
		/// </summary>
		public static T GetExportedValue<T> ()
		{
		//oe	return Instance.ExportProvider.GetExportedValue<T> ();
			Console.WriteLine( "oeDEBUG CompositionManager.GetExportedValue :: " + typeof(T).FullName );
			return ExportProviderMD.GetExport<T>();
		}

		/// <summary>
		/// Returns all instance of type T that are exported by some composition part. The instances are shared (singletons).
		/// </summary>
		public static IEnumerable<T> GetExportedValues<T> ()
		{
		//oe	return Instance.ExportProvider.GetExportedValues<T> ();
			Console.WriteLine( "oeDEBUG CompositionManager.GetExportedValues :: " + typeof(T).FullName );
			return ExportProviderMD.GetExports<T>();
		}

	// oe REMOVED...
	//oe	public RuntimeComposition RuntimeComposition { get; private set; }
	//oe	public IExportProviderFactory ExportProviderFactory { get; private set; }
	//oe	public ExportProvider ExportProvider { get; private set; }
	//oe	public HostServices HostServices { get; private set; }
	//oe	public System.ComponentModel.Composition.Hosting.ExportProvider ExportProviderV1 { get; private set; }
	//oe	internal CompositionManager ()
	//oe	static async Task<CompositionManager> CreateInstanceAsync ()

		static CompositionManager ()
		{

Console.WriteLine( "CompositionManager.ctor :: start" );

		//oe	ComposableCatalog catalog = ComposableCatalog.Create (StandardResolver)
		//oe		.WithCompositionService ()
		//oe		.WithDesktopSupport ();

			var assemblies = new HashSet<Assembly> ();

			// oe add...
			assemblies.Add ( typeof ( MonoDevelop.Ide.TypeSystem.MonoDevelopWorkspace ).Assembly );
			foreach (var asmName in mefHostServices) {

Console.WriteLine( "CompositionManager :: INIT-1 " + asmName );

				try {
					var asm = Assembly.Load (asmName);
					if (asm == null)
						continue;
					assemblies.Add (asm);
				} catch (Exception ex) {
					LoggingService.LogError ("Error - can't load host service assembly: " + asmName, ex);
				}
			}

Console.WriteLine( "CompositionManager :: INIT-addins-from-path " + "/MonoDevelop/Ide/TypeService/PlatformMefHostServices" );
			ReadAssembliesFromAddins (assemblies, "/MonoDevelop/Ide/TypeService/PlatformMefHostServices");
Console.WriteLine( "CompositionManager :: INIT-addins-from-path " + "/MonoDevelop/Ide/TypeService/MefHostServices" );
			ReadAssembliesFromAddins (assemblies, "/MonoDevelop/Ide/TypeService/MefHostServices");
Console.WriteLine( "CompositionManager :: INIT-addins-from-path " + "/MonoDevelop/Ide/Composition" );
			ReadAssembliesFromAddins (assemblies, "/MonoDevelop/Ide/Composition");

		//oe	// spawn discovery tasks in parallel for each assembly
		//oe	var tasks = new List<Task<DiscoveredParts>> (assemblies.Count);
		//oe	foreach (var assembly in assemblies) {
		//oe		var task = Task.Run (() => Discovery.CreatePartsAsync (assembly));
		//oe		tasks.Add (task);
		//oe	}
		//oe	foreach (var task in tasks) {
		//oe		catalog = catalog.AddParts (await task);
		//oe	}
		//oe	var discoveryErrors = catalog.DiscoveredParts.DiscoveryErrors;
		//oe	if (!discoveryErrors.IsEmpty) {
		//oe		throw new ApplicationException ($"MEF catalog scanning errors encountered.\n{string.Join ("\n", discoveryErrors)}");
		//oe	}
		//oe	CompositionConfiguration configuration = CompositionConfiguration.Create (catalog);
		//oe	if (!configuration.CompositionErrors.IsEmpty) {
		//oe		// capture the errors in an array for easier debugging
		//oe		var errors = configuration.CompositionErrors.ToArray ();
		//oe		// For now while we're still transitioning to VSMEF it's useful to work
		//oe		// even if the composition has some errors. TODO: re-enable this.
		//oe		//var messages = errors.SelectMany (e => e).Select (e => e.Message);
		//oe		//var text = string.Join (Environment.NewLine, messages);
		//oe		//Xwt.Clipboard.SetText (text);
		//oe		//configuration.ThrowOnErrors ();
		//oe	}
		//oe	RuntimeComposition = RuntimeComposition.CreateRuntimeComposition (configuration);
		//oe	ExportProviderFactory = RuntimeComposition.CreateExportProviderFactory ();
		//oe	ExportProvider = ExportProviderFactory.CreateExportProvider ();
		//oe	HostServices = MefV1HostServices.Create (ExportProvider.AsExportProvider ());
		//oe	ExportProviderV1 = NetFxAdapters.AsExportProvider (ExportProvider);

			services = MefHostServices.Create (assemblies);

			ExportProviderMD.Init( assemblies.ToArray() );

Console.WriteLine( "CompositionManager.ctor :: completed" );

		}

	//oe	void ReadAssembliesFromAddins (HashSet<Assembly> assemblies, string extensionPath)
		static void ReadAssembliesFromAddins (HashSet<Assembly> assemblies, string extensionPath)
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

Console.WriteLine( "CompositionManager :: INIT-2 " + assembly );

						assemblies.Add (assembly);
					}
					catch (Exception e) {
						LoggingService.LogError ("Composition can't load assembly " + assemblyNode.FileName, e);
					}
				}
			}
		}

//////////////////////////////////////////////////////////////////////////////////////////////

// these added parts are from MD-7.1 MonoDevelopWorkspace.cs (from versions prior to introduction of CompositionManager).
// these added parts are from MD-7.1 MonoDevelopWorkspace.cs (from versions prior to introduction of CompositionManager).
// these added parts are from MD-7.1 MonoDevelopWorkspace.cs (from versions prior to introduction of CompositionManager).

		// oe add...
		static string[] mefHostServices = new [] {
			"Microsoft.CodeAnalysis.Workspaces",
			//FIXME: this does not load yet. We should provide alternate implementations of its services.
			//"Microsoft.CodeAnalysis.Workspaces.Desktop",
			"Microsoft.CodeAnalysis.Features",
			"Microsoft.CodeAnalysis.CSharp",
			"Microsoft.CodeAnalysis.CSharp.Workspaces",
			"Microsoft.CodeAnalysis.CSharp.Features",
			"Microsoft.CodeAnalysis.VisualBasic",
			"Microsoft.CodeAnalysis.VisualBasic.Workspaces",
			"Microsoft.CodeAnalysis.VisualBasic.Features",
		};

		// oe add...
		readonly static HostServices services;

		// oe add...
		internal static HostServices HostServices {
			get {
				return services;
			}
		}
	}
}

