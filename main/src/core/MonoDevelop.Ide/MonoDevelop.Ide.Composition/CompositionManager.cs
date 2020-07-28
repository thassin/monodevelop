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

using MonoDevelop.Ide.TypeSystem; // for MonoDevelopWorkspace - is this needed???

namespace MonoDevelop.Ide.Composition
{
	/// <summary>
	/// The host of the MonoDevelop MEF composition. Uses https://github.com/Microsoft/vs-mef.
	/// </summary>
	public class CompositionManager
	{

	// oe REMOVED...
	// oe	static Task<CompositionManager> creationTask;
	//oe	static readonly Resolver StandardResolver = Resolver.DefaultInstance;
	//oe	static readonly PartDiscovery Discovery = PartDiscovery.Combine (
	//oe		new AttributedPartDiscoveryV1 (StandardResolver),
	//oe		new AttributedPartDiscovery (StandardResolver, true));
	//oe	public static Task<CompositionManager> InitializeAsync ()

		static CompositionManager instance;

		public static CompositionManager Instance {
			get {
				if (instance == null) {
				//	instance = InitializeAsync ().Result;
					instance = new CompositionManager();
				}

				return instance;
			}
		}

		/// <summary>
		/// Returns an instance of type T that is exported by some composition part. The instance is shared (singleton).
		/// </summary>
		public static T GetExportedValue<T> ()
		{
		//oe	return Instance.ExportProvider.GetExportedValue<T> ();
Console.WriteLine( "CompositionManager.GetExportedValue :: " + typeof(T).FullName );
			return MDExportProvider.GetExport<T>();
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

		//	ComposableCatalog catalog = ComposableCatalog.Create (StandardResolver)
		//		.WithCompositionService ()
		//		.WithDesktopSupport ();

			var assemblies = new HashSet<Assembly> ();

			assemblies.Add (typeof (MonoDevelopWorkspace).Assembly);
			foreach (var asmName in mefHostServices) {

Console.WriteLine( "CompositionManager :: INIT " + asmName );

				try {
					var asm = Assembly.Load (asmName);
					if (asm == null)
						continue;
					assemblies.Add (asm);
				} catch (Exception ex) {
					LoggingService.LogError ("Error - can't load host service assembly: " + asmName, ex);
				}
			}

			ReadAssembliesFromAddins (assemblies, "/MonoDevelop/Ide/TypeService/PlatformMefHostServices");
			ReadAssembliesFromAddins (assemblies, "/MonoDevelop/Ide/TypeService/MefHostServices");
			ReadAssembliesFromAddins (assemblies, "/MonoDevelop/Ide/Composition");

		//	// spawn discovery tasks in parallel for each assembly
		//	var tasks = new List<Task<DiscoveredParts>> (assemblies.Count);
		//	foreach (var assembly in assemblies) {
		//		var task = Task.Run (() => Discovery.CreatePartsAsync (assembly));
		//		tasks.Add (task);
		//	}
		//	foreach (var task in tasks) {
		//		catalog = catalog.AddParts (await task);
		//	}
		//	var discoveryErrors = catalog.DiscoveredParts.DiscoveryErrors;
		//	if (!discoveryErrors.IsEmpty) {
		//		throw new ApplicationException ($"MEF catalog scanning errors encountered.\n{string.Join ("\n", discoveryErrors)}");
		//	}
		//	CompositionConfiguration configuration = CompositionConfiguration.Create (catalog);
		//	if (!configuration.CompositionErrors.IsEmpty) {
		//		// capture the errors in an array for easier debugging
		//		var errors = configuration.CompositionErrors.ToArray ();
		//		// For now while we're still transitioning to VSMEF it's useful to work
		//		// even if the composition has some errors. TODO: re-enable this.
		//		//var messages = errors.SelectMany (e => e).Select (e => e.Message);
		//		//var text = string.Join (Environment.NewLine, messages);
		//		//Xwt.Clipboard.SetText (text);
		//		//configuration.ThrowOnErrors ();
		//	}

		//	RuntimeComposition = RuntimeComposition.CreateRuntimeComposition (configuration);
		//	ExportProviderFactory = RuntimeComposition.CreateExportProviderFactory ();
		//	ExportProvider = ExportProviderFactory.CreateExportProvider ();
		//	HostServices = MefV1HostServices.Create (ExportProvider.AsExportProvider ());
		//	ExportProviderV1 = NetFxAdapters.AsExportProvider (ExportProvider);

			services = MefHostServices.Create (assemblies);

			MDExportProvider.Init( assemblies.ToArray() );

Console.WriteLine( "CompositionManager.ctor :: completed" );

		}

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

