
this version is based on:
	*) commit d2c212a04a47c3176572edcc85319459646f9e19 
	*) tagged monodevelop-7.4.3.8 
	*) dated 2018-04-06 

some package versions:
	Microsoft.CodeAnalysis.*	2.7.0
	Microsoft.VisualStudio.*	15.6.27740

modifications:

	 added/reverted:
	^^^^^^^^^^^^^^^^^
main/external/vs-editor-api		<new submodule>
main/msbuild/RoslynVersion.props

	 modified:
	^^^^^^^^^^^
main/Directory.Build.props
main/Main.sln
main/msbuild/ReferencesVSEditor.props
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/TagBasedSyntaxHighlighting.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/TextEditorFactoryService.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/TextView.cs
main/src/addins/MonoDevelop.UnitTesting.NUnit/NUnit3Runner/NUnit3Runner.csproj
main/src/addins/MonoDevelop.UnitTesting.NUnit/NUnitRunner/NUnitRunner.csproj
main/src/core/Mono.TextEditor.Shared/Mono.TextEditor/Document/TextDocument.cs
main/src/core/MonoDevelop.Core/MonoDevelop.Projects.MSBuild/MSBuildProjectService.cs
main/src/core/MonoDevelop.Core/MonoDevelop.Projects.MSBuild/RemoteBuildEngineManager.cs
main/src/core/MonoDevelop.Core/packages.config
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/CompositionManager.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/PlatformCatalog.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/PlatformExtensions.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/InternalExtensionAPI/ITextEditorImpl.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/TextEditor.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.TypeSystem/MonoDevelopWorkspace.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.csproj
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide/IdeStartup.cs

	 moved/renamed:
	^^^^^^^^^^^^^^^^

	 deleted:
	^^^^^^^^^^

##############################################################################################

	 how to build:
	^^^^^^^^^^^^^^^

$ git submodule init 
$ git submodule update 

$ ./configure 

	The build profile 'default' does not exist. A new profile will be created.
	Select the packages to include in the build for the profile 'default':

	1. [X] main
	2. [ ] extras/MonoDevelop.Database

	Enter the number of an add-in to enable/disable,
	(q) quit, (c) clear all, (s) select all, or ENTER to continue:  <ENTER>

$ make clean 
$ make 

	IF NEEDED, make copies of the following files with fixed filenames:
$ cp ./main/external/fsharpbinding/packages/FSharp.Core/fsharp.core.4.1.0.2.nupkg ./main/external/fsharpbinding/packages/FSharp.Core/FSharp.Core.4.1.0.2.nupkg
$ cp ./main/external/fsharpbinding/packages/System.ValueTuple/system.valuetuple.4.4.0.nupkg ./main/external/fsharpbinding/packages/System.ValueTuple/System.ValueTuple.4.4.0.nupkg
	THEN re-try building ( make clean && make ).

	 how to run:
	^^^^^^^^^^^^^

$ make run 

	or

$ mono ./main/build/bin/MonoDevelop.exe --no-redirect 

	or (with no logging output to console)

$ mono ./main/build/bin/MonoDevelop.exe 

	 known problems:
	^^^^^^^^^^^^^^^^^
*) the QuickFix menu is not working in editor.
*) win10 wsl : the process seems to hang and keep running after closing the app.

