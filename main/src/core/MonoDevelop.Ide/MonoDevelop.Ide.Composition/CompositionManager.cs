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

	// oe NOTICE method name changed...
	//oe	async Task InitializeInstanceAsync ()
		static CompositionManager ()
		{

Console.WriteLine( "oeDEBUG :: CompositionManager.ctor :: start" );

		//oe	ComposableCatalog catalog = ComposableCatalog.Create (StandardResolver)
		//oe		.WithCompositionService ()
		//oe		.WithDesktopSupport ();

			var assemblies = new HashSet<Assembly> ();

			// oe add...
			assemblies.Add ( typeof ( MonoDevelop.Ide.TypeSystem.MonoDevelopWorkspace ).Assembly );
			foreach (var asmName in mefHostServices) {

Console.WriteLine( "oeDEBUG :: CompositionManager :: INIT-1 " + asmName );

				try {
					var asm = Assembly.Load (asmName);
					if (asm == null)
						continue;
					assemblies.Add (asm);
				} catch (Exception ex) {
					LoggingService.LogError ("Error - can't load host service assembly: " + asmName, ex);
				}
			}

Console.WriteLine( "oeDEBUG :: CompositionManager :: INIT-addins-from-path " + "/MonoDevelop/Ide/TypeService/PlatformMefHostServices" );
			ReadAssembliesFromAddins (assemblies, "/MonoDevelop/Ide/TypeService/PlatformMefHostServices");
Console.WriteLine( "oeDEBUG :: CompositionManager :: INIT-addins-from-path " + "/MonoDevelop/Ide/TypeService/MefHostServices" );
			ReadAssembliesFromAddins (assemblies, "/MonoDevelop/Ide/TypeService/MefHostServices");
Console.WriteLine( "oeDEBUG :: CompositionManager :: INIT-addins-from-path " + "/MonoDevelop/Ide/Composition" );
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
		//oe		foreach	(var error in discoveryErrors) {
		//oe			LoggingService.LogInfo ("MEF discovery error", error);
		//oe		}
		//oe		// throw new ApplicationException ("MEF discovery errors");
		//oe	}
		//oe	CompositionConfiguration configuration = CompositionConfiguration.Create (catalog);
		//oe	if (!configuration.CompositionErrors.IsEmpty) {
		//oe		// capture the errors in an array for easier debugging
		//oe		var errors = configuration.CompositionErrors.SelectMany (e => e).ToArray ();
		//oe		foreach	(var error in errors) {
		//oe			LoggingService.LogInfo ("MEF composition error: " + error.Message);
		//oe		}
		//oe		// For now while we're still transitioning to VSMEF it's useful to work
		//oe		// even if the composition has some errors. TODO: re-enable this.
		//oe		//configuration.ThrowOnErrors ();
		//oe	}
		//oe	RuntimeComposition = RuntimeComposition.CreateRuntimeComposition (configuration);
		//oe	ExportProviderFactory = RuntimeComposition.CreateExportProviderFactory ();
		//oe	ExportProvider = ExportProviderFactory.CreateExportProvider ();
		//oe	HostServices = MefV1HostServices.Create (ExportProvider.AsExportProvider ());
		//oe	ExportProviderV1 = NetFxAdapters.AsExportProvider (ExportProvider);

			services = MefHostServices.Create (assemblies);

			ExportProviderMD.Init( assemblies.ToArray() );

Console.WriteLine( "oeDEBUG :: CompositionManager.ctor :: completed" );

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

// 20200731 : block some assemblies from loading, since they import dependencies to packages we don't have available.
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
// earlier version (with no problems):	Microsoft.CodeAnalysis.Features, Version=2.6.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// the current one (blocking needed):	Microsoft.CodeAnalysis.Features, Version=2.8.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// -> plus the other "Microsoft.CodeAnalysis..." packages :: Microsoft.CodeAnalysis... 2.8.0-beta3-62728-05

//if ( assemblyNode.FileName.StartsWith( "RefactoringEssentials") ) continue; this is OK (BTW blocking this will affect/impair completions).

if ( assemblyNode.FileName.StartsWith( "Microsoft.CodeAnalysis.EditorFeatures") ) continue;
if ( assemblyNode.FileName.StartsWith( "Microsoft.CodeAnalysis.CSharp.EditorFeatures") ) continue;
	// 20200804 :: if the above are allowed, we get this:
	// FATAL ERROR [2020-08-04 21:49:39Z]: MonoDevelop failed to start. Some of the assemblies required to run MonoDevelop (for example gtk-sharp)may not be properly installed in the GAC.
	// System.TypeInitializationException: The type initializer for 'MonoDevelop.Ide.Composition.CompositionManager' threw an exception. ---> System.Reflection.ReflectionTypeLoadException: Exception of type 'System.Reflection.ReflectionTypeLoadException' was thrown.
	// Could not load file or assembly 'Microsoft.VisualStudio.Text.Logic, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a' or one of its dependencies.
	// Could not load file or assembly 'Microsoft.VisualStudio.Text.UI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a' or one of its dependencies.

if ( assemblyNode.FileName.StartsWith( "Microsoft.CodeAnalysis.Workspaces.Desktop") ) continue;
	// 20200731 NOTICE allowing this will cause the app CRASH soon after opening a project (since we no longer have the classes required):
	// ERROR [2020-07-31 11:25:46Z]: Add-in error (MonoDevelop.CSharpBinding,7.6): Error while getting object for node in path '/MonoDevelop/Ide/TypeService/OptionProviders'.
	// System.InvalidOperationException: Type 'MonoDevelop.CSharp.OptionProvider.EditorConfigDocumentOptionsProviderFactory' not found in add-in 'MonoDevelop.CSharpBinding,7.6'

// 20200805 : NOTE that the above problems are caused by the fact that Microsoft.CodeAnalysis.EditorFeatures contains (or is based on)
// dependencies to Microsoft.VisualStudio.* packages. in earlier MD versions Microsoft.CodeAnalysis.EditorFeatures has not been used.

// THEREFORE THIS APP VERSION IS A BIT BROKEN, no need to further study why the QuickFix menu is not working as it should...
// THEREFORE THIS APP VERSION IS A BIT BROKEN, no need to further study why the QuickFix menu is not working as it should...
// THEREFORE THIS APP VERSION IS A BIT BROKEN, no need to further study why the QuickFix menu is not working as it should...

Console.WriteLine( "oeDEBUG :: CompositionManager :: INIT-2 " + assembly );

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

			// oe ADDED these trying to get QuickFix menu working (oe-20180416-7.6-build551).
			"Microsoft.CodeAnalysis.Diagnostics",
			"Microsoft.CodeAnalysis.ICodeFixService"

// oe NOTICE : not sure if there is real need to add anything here.
// -> see ReadAssembliesFromAddins() above...

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

