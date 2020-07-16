
this version is based on commit 8e442b4f07fae2048e2bb1157f618952586b55d2 dated 2017-10-24
with the following modifications:


	 deleted:
	^^^^^^^^^^
main/src/core/Mono.TextEditor.Platform/			<directory>
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/	<directory>

main/msbuild/ReferencesVSEditor.props

main/src/addins/CSharpBinding/MonoDevelop.CSharp.Completion/RoslynParameterHintingEngine.cs

main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.CodeCompletion/CompletionPresenterSession.cs

main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.CodeCompletion/RoslynCompletionData.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.CodeCompletion/SignatureHelpParameterHintingData.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.CodeCompletion/TaggedTextUtil.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/CompositionManager.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/JoinableTaskContextHost.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/PlatformCatalog.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/PlatformExtensions.cs

main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/ITextEditorFactoryService.cs


	 moved/renamed:
	^^^^^^^^^^^^^^^^
main/src/addins/AspNet/Templates/StylesheetTemplate.css main/src/addins/AspNet/Templates/StyleSheetTemplate.css


	 modified:
	^^^^^^^^^^^
Makefile

main/Directory.Build.props

main/external/fsharpbinding/MonoDevelop.FSharp.Tests/MonoDevelop.FSharp.Tests.fsproj
main/external/fsharpbinding/MonoDevelop.FSharpBinding/FSharpTextEditorCompletion.fs

main/src/addins/AspNet/MonoDevelop.AspNet.csproj
main/src/addins/AspNet/Properties/MonoDevelop.AspNet.addin.xml
main/src/addins/AspNet/Templates/EmptyCssFile.xft.xml
main/src/addins/AspNet/Templates/EmptyLessFile.xft.xml
main/src/addins/AspNet/Templates/EmptyScssFile.xft.xml

main/src/addins/CSharpBinding/CSharpBinding.csproj
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Completion/CSharpCompletionTextEditorExtension.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Completion/ImportSymbolCompletionData.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Tooltips/LanguageItemTooltipProvider.cs

main/src/addins/MonoDevelop.SourceEditor2/MonoDevelop.SourceEditor.csproj
main/src/addins/MonoDevelop.SourceEditor2/MonoDevelop.SourceEditor/SourceEditorView.cs

main/src/core/Mono.TextEditor.Shared/Mono.TextEditor/Actions/MiscActions.cs
main/src/core/Mono.TextEditor.Shared/Mono.TextEditor/Document/TextDocument.cs
main/src/core/Mono.TextEditor.Shared/Mono.TextEditor/TextLinkEditMode.cs

main/src/core/MonoDevelop.Core/MonoDevelop.Core.Assemblies/TargetRuntime.cs
main/src/core/MonoDevelop.Core/MonoDevelop.Core.csproj
main/src/core/MonoDevelop.Core/MonoDevelop.Projects.MSBuild/MSBuildProjectService.cs
main/src/core/MonoDevelop.Core/MonoDevelop.Projects.MSBuild/RemoteBuildEngineManager.cs
main/src/core/MonoDevelop.Core/packages.config

main/src/core/MonoDevelop.Ide/ExtensionModel/MonoDevelop.Ide.addin.xml
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.CodeCompletion/CompletionData.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.CodeCompletion/ParameterHintingData.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.CodeCompletion/ParameterHintingResult.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.CodeCompletion/ParameterInformationWindow.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.CodeCompletion/ParameterInformationWindowManager.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor.Extension/CompletionTextEditorExtension.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/Projection/ProjectedCompletionExtension.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/Projection/ProjectedFilterCompletionTextEditorExtension.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/TextEditor.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.TypeSystem/MonoDevelopSourceText.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.TypeSystem/MonoDevelopSourceTextContainer.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.TypeSystem/MonoDevelopTextLoader.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.TypeSystem/MonoDevelopWorkspace.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.csproj

main/src/core/MonoDevelop.TextEditor.Tests/MonoDevelop.TextEditor.Tests.csproj

main/tests/MonoDevelop.CSharpBinding.Tests/MonoDevelop.CSharpBinding.Tests.csproj


	 added/reverted:
	^^^^^^^^^^^^^^^^^
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Completion/AnonymousMethodCompletionData.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Completion/CreateOverrideCompletionData.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Completion/CreatePartialCompletionData.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Completion/EventCreationCompletionData.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Completion/IExtensionContextHandler.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Completion/ObjectCreationCompletionData.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Completion/RoslynCodeCompletionFactory.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Completion/RoslynCompletionCategory.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Completion/RoslynCompletionData.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Completion/RoslynParameterHintingFactory.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Completion/RoslynSymbolCompletionData.cs

main/src/addins/CSharpBinding/MonoDevelop.CSharp.Features/Completion/		<directory>
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Features/ParameterHinting/	<directory>

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

	 how to run:
	^^^^^^^^^^^^^

$ make run 

	or

$ mono ./main/build/bin/MonoDevelop.exe --no-redirect 

	or (with no logging output to console)

$ mono ./main/build/bin/MonoDevelop.exe 

	 known problems:
	^^^^^^^^^^^^^^^^^
*) win10 wsl : the process seems to hang and keep running after closing the app.

