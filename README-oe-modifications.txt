
this version is based on commit 5fb15f227aa93a5f615047d9ccf7faa333ff662f dated 2018-04-16
with the following modifications:

	 deleted:
	^^^^^^^^^^
main/src/addins/MonoDevelop.SourceEditor2/VSEditor	<directory>
main/src/core/Mono.TextEditor.Platform			<directory>	## contains just Makefile.am

main/msbuild/ReferencesVSEditor.props

main/src/addins/CSharpBinding/MonoDevelop.Ide.Completion.Presentation/MonoDevelopContainedDocument.cs

main/src/addins/MonoDevelop.SourceEditor2/Mono.TextEditor/Gui/MdTextViewLineCollection.MdTextViewLine.cs
main/src/addins/MonoDevelop.SourceEditor2/Mono.TextEditor/Gui/MdTextViewLineCollection.cs
main/src/addins/MonoDevelop.SourceEditor2/Mono.TextEditor/Gui/MonoTextEditor.ITextView.MouseHover.cs
main/src/addins/MonoDevelop.SourceEditor2/Mono.TextEditor/Gui/MonoTextEditor.ITextView.cs
main/src/addins/MonoDevelop.SourceEditor2/Mono.TextEditor/Gui/MonoTextEditor.IViewScroller.cs

main/src/core/Mono.TextEditor.Shared/Mono.TextEditor/CaretImpl.ITextCaret.cs

main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/JoinableTaskContextHost.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/PlatformCatalog.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/PlatformExtensions.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/ITextEditorFactoryService.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/CommonEditorAssetServiceFactory.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/SuggestedActionCategoryRegistryService.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/EditorConfigService.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.TypeSystem/IMonoDevelopHostDocument.cs

	 moved/renamed:
	^^^^^^^^^^^^^^^^

	 modified:
	^^^^^^^^^^^
main/configure.ac
main/Directory.Build.props

main/msbuild/ReferencesRoslyn.props

main/src/addins/CSharpBinding/CSharpBinding.csproj

main/src/addins/CSharpBinding/MonoDevelop.CSharp.Completion/CSharpCompletionData.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Completion/CSharpCompletionTextEditorExtension.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/HighlightUsagesExtension.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.OptionProvider/CSharpDocumentOptionsProvider.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.OptionProvider/OptionProviderFactory.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Tooltips/QuickInfoProvider.cs

main/src/addins/CSharpBinding/MonoDevelop.Ide.Completion.Presentation/IMyRoslynCompletionDataProvider.cs
main/src/addins/CSharpBinding/MonoDevelop.Ide.Completion.Presentation/MonoDevelopFormattingRuleFactoryServiceFactory.cs
main/src/addins/CSharpBinding/MonoDevelop.Ide.Completion.Presentation/MyCSharpCompletionData.cs
main/src/addins/CSharpBinding/MonoDevelop.Ide.Completion.Presentation/MyCSharpCompletionDataProvider.cs
main/src/addins/CSharpBinding/MonoDevelop.Ide.Completion.Presentation/MyRoslynCompletionData.cs
main/src/addins/CSharpBinding/MonoDevelop.Ide.Completion.Presentation/RoslynCommandTarget.cs
main/src/addins/CSharpBinding/MonoDevelop.Ide.Completion.Presentation/RoslynCompletionPresenter.cs
main/src/addins/CSharpBinding/MonoDevelop.Ide.Completion.Presentation/RoslynCompletionPresenterSession.ICompletionPresenterSession.cs
main/src/addins/CSharpBinding/MonoDevelop.Ide.Completion.Presentation/RoslynCompletionPresenterSession.IIntelliSensePresenterSession.cs
main/src/addins/CSharpBinding/MonoDevelop.Ide.Completion.Presentation/RoslynCompletionPresenterSession.View.cs

main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.csproj

main/src/addins/MonoDevelop.Refactoring/MonoDevelop.Refactoring.csproj
main/src/addins/MonoDevelop.Refactoring/MonoDevelop.CodeActions/CodeActionEditorExtension.cs

main/src/addins/MonoDevelop.SourceEditor2/MonoDevelop.SourceEditor.csproj
main/src/addins/MonoDevelop.SourceEditor2/MonoDevelop.SourceEditor/SourceEditorView.cs
main/src/addins/MonoDevelop.SourceEditor2/MonoDevelop.SourceEditor/SourceEditorView.IMonoDevelopEditorOperations.cs
main/src/addins/MonoDevelop.SourceEditor2/Mono.TextEditor/Gui/TextArea.cs

main/src/addins/VersionControl/MonoDevelop.VersionControl/MonoDevelop.VersionControl.csproj

main/src/core/Makefile.am

main/src/core/MonoDevelop.Core/MonoDevelop.Core.csproj
main/src/core/MonoDevelop.Core/MonoDevelop.Core.Assemblies/TargetRuntime.cs
main/src/core/MonoDevelop.Core/MonoDevelop.Projects.MSBuild/MSBuildProjectService.cs
main/src/core/MonoDevelop.Core/MonoDevelop.Projects.MSBuild/RemoteBuildEngineManager.cs

main/src/core/MonoDevelop.Core/packages.config

main/src/core/MonoDevelop.Ide/ExtensionModel/MonoDevelop.Ide.addin.xml

main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.csproj
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.CodeCompletion/RoslynCompletionData.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/CompositionManager.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/InlineRenameService.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/PreviewFactoryService.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/RoslynWaitIndicator.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/StreamingFindUsagesPresenter.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Desktop/PlatformService.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/DefaultSourceEditorOptions.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor.Extension/LineSeparatorTextEditorExtension.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/InternalExtensionAPI/IMonoDevelopEditorOperations.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/InternalExtensionAPI/ITextEditorImpl.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/TextEditor.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/TextEditorViewContent.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide/IdeStartup.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Templates/SingleFileDescriptionTemplate.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.TypeSystem/MonoDevelopSourceTextContainer.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.TypeSystem/MonoDevelopSourceText.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.TypeSystem/MonoDevelopTextLoader.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.TypeSystem/MonoDevelopWorkspace.cs

main/src/core/MonoDevelop.TextEditor.Tests/MonoDevelop.TextEditor.Tests.csproj

main/src/core/Mono.TextEditor.Shared/Mono.TextEditor/Actions/MiscActions.cs
main/src/core/Mono.TextEditor.Shared/Mono.TextEditor/CaretImpl.cs
main/src/core/Mono.TextEditor.Shared/Mono.TextEditor/Document/TextDocument.cs
main/src/core/Mono.TextEditor.Shared/Mono.TextEditor.Shared.projitems
main/src/core/Mono.TextEditor.Shared/Mono.TextEditor/TextLinkEditMode.cs

main/tests/MonoDevelop.Refactoring.Tests/CodeActionEditorExtensionTests.cs
main/tests/MonoDevelop.Refactoring.Tests/ResultsEditorExtensionTests.cs

	 added/reverted:
	^^^^^^^^^^^^^^^^^
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/ExportProviderMD.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/InternalExtensionAPI/IEditorOperationsMD.cs

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

//		IF NEEDED, make copies of the following files with fixed filenames:
//	$ cp ./main/external/fsharpbinding/packages/FSharp.Core/fsharp.core.4.1.0.2.nupkg ./main/external/fsharpbinding/packages/FSharp.Core/FSharp.Core.4.1.0.2.nupkg
//	$ cp ./main/external/fsharpbinding/packages/System.ValueTuple/system.valuetuple.4.4.0.nupkg ./main/external/fsharpbinding/packages/System.ValueTuple/System.ValueTuple.4.4.0.nupkg
//		THEN re-try building ( make clean && make ).

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

