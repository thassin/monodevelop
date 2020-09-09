
this version is based on:
	*) commit 7cfeee47505f82c952b163c33955bda9677abad9 
	*) tagged monodevelop-7.6.11.7 
	*) dated 2018-10-30 

some package versions:
	Microsoft.CodeAnalysis.*	"2.9.0-beta4-63001-02" (left as it was)
	Microsoft.VisualStudio.*	removed (from vs-editor-api)

modifications:

	 added/reverted:
	^^^^^^^^^^^^^^^^^
main/external/vs-editor-api		<new submodule>
main/msbuild/RoslynVersion.props
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/AbstractKeywordHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/AbstractAsyncHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/AsyncAnonymousMethodHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/AsyncMethodHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/AsyncParenthesizedLambdaHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/AsyncSimpleLambdaHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/AwaitHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/CheckedExpressionHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/CheckedStatementHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/ConditionalPreprocessorHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/IfStatementHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/LockStatementHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/LoopHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/RegionHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/ReturnStatementHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/SwitchStatementHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/TryStatementHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/UnsafeStatementHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/UsingStatementHighlighter.cs
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/KeywordHighlighters/YieldStatementHighlighter.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor.Highlighting/RoslynClassificationHighlighting.cs

	 modified:
	^^^^^^^^^^^
main/Directory.Build.props
main/Main.sln
main/msbuild/ReferencesVSEditor.props
main/src/addins/CSharpBinding/CSharpBinding.csproj
main/src/addins/CSharpBinding/MonoDevelop.CSharp.Highlighting/HighlightUsagesExtension.cs
main/src/addins/CSharpBinding/MonoDevelop.Ide.Completion.Presentation/RoslynCompletionPresenterSession.View.cs
main/src/addins/MonoDevelop.SourceEditor2/Mono.TextEditor/Gui/MdTextViewLineCollection.MdTextViewLine.cs
main/src/addins/MonoDevelop.SourceEditor2/Mono.TextEditor/Gui/MonoTextEditor.ITextView.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/IMdTextView.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/Language/Impl/Intellisense/CurrentLineSpaceReservationAgent.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/Language/Impl/Intellisense/IntellisenseSessionStack.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/Language/Impl/Intellisense/IntellisenseSpaceReservationManagers.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/TagBasedSyntaxHighlighting.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/Text/Def/TextUIWpf/Editor/ISpaceReservationAgent.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/Text/Def/TextUIWpf/Editor/ISpaceReservationManager.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/Text/Def/TextUIWpf/Editor/SpaceReservationAgentChangedEventArgs.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/Text/Def/TextUIWpf/Editor/SpaceReservationManagerDefinition.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/Text/Impl/WpfToolTipAdornment/BaseWpfToolTipPresenter.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/Text/Impl/WpfToolTipAdornment/Legacy/ToolTipProvider.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/Text/Impl/WpfToolTipAdornment/MouseTrackingWpfToolTipPresenter.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/Text/Impl/WpfToolTipAdornment/ViewElementFactories/WpfImageElementViewElementFactory.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/Text/Impl/WpfView/PopupAgent.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/Text/Impl/WpfView/SpaceReservationManager.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/Text/Impl/WpfView/SpaceReservationStack.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/TextEditorFactoryService.cs
main/src/addins/MonoDevelop.UnitTesting.NUnit/NUnit3Runner/NUnit3Runner.csproj
main/src/addins/MonoDevelop.UnitTesting.NUnit/NUnitRunner/NUnitRunner.csproj
main/src/core/Mono.TextEditor.Shared/Mono.TextEditor/Document/TextDocument.cs
main/src/core/MonoDevelop.Core/MonoDevelop.Core/FilePath.cs
main/src/core/MonoDevelop.Core/MonoDevelop.Projects.MSBuild/MSBuildEvaluationContext.cs
main/src/core/MonoDevelop.Core/MonoDevelop.Projects.MSBuild/MSBuildProjectService.cs
main/src/core/MonoDevelop.Core/MonoDevelop.Projects.MSBuild/RemoteBuildEngineManager.cs
main/src/core/MonoDevelop.Core/packages.config
main/src/core/MonoDevelop.Ide/ExtensionModel/MonoDevelop.Ide.addin.xml
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/CompositionManager.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/PlatformCatalog.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Desktop/PlatformService.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.csproj

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
*) THE BUILD SYSTEM IS BROKEN (bug will be fixed in MonoDevelop-8.1); might work for Mono versions < 6?
*) win10 wsl : the process seems to hang and keep running after closing the app.

