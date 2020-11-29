
this version is based on:
	*) commit add3a4998a5cb5b081e0404e1fe13acfecb7801d 
	*) tagged monodevelop-8.3.10.2 
	*) dated 2019-11-20 

some package versions:
	Microsoft.CodeAnalysis.*	(left as it was)
	Microsoft.VisualStudio.*	removed (from vs-editor-api)

modifications:

	 added/reverted:
	^^^^^^^^^^^^^^^^^
main/external/vs-editor-api			<new submodule>

main/msbuild/ReferencesVSEditor.Gtk.props
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/TextEditorFactoryService.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/ITextEditorFactoryService.cs

	 modified:
	^^^^^^^^^^^
NuGet.config
main/Directory.Build.props
main/Main.sln
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/ExceptionCaught/ExceptionCaughtAdornmentManager.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/ExceptionCaught/ExceptionCaughtProvider.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/BreakpointGlyphFactoryProvider.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/BreakpointGlyphMouseProcessor.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/BreakpointGlyphMouseProcessorProvider.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/BreakpointGlyphTag.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/BreakpointGlyphTagger.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/BreakpointGlyphTaggerProvider.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/CurrentStatementGlyphFactoryProvider.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/CurrentStatementGlyphTag.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/CurrentStatementGlyphTagger.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/CurrentStatementGlyphTaggerProvider.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/GlyphCommandType.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/IActiveGlyphDropHandler.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/IInteractiveGlyph.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/ImageSourceGlyphFactory.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/ReturnStatementGlyphFactoryProvider.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/ReturnStatementGlyphTag.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/ReturnStatementGlyphTagger.cs
main/src/addins/MonoDevelop.Debugger/MonoDevelop.Debugger.VSTextView/Glyphs/ReturnStatementGlyphTaggerProvider.cs
main/src/addins/MonoDevelop.SourceEditor2/Mono.TextEditor/Gui/MdTextViewLineCollection.MdTextViewLine.cs
main/src/addins/MonoDevelop.SourceEditor2/Mono.TextEditor/Gui/MonoTextEditor.ITextView.cs
main/src/addins/MonoDevelop.SourceEditor2/MonoDevelop.SourceEditor.csproj
main/src/addins/MonoDevelop.SourceEditor2/MonoDevelop.SourceEditor/DebugValueTooltipProvider.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/IMdTextView.cs
main/src/addins/MonoDevelop.SourceEditor2/VSEditor/SmartIndentationService.cs
main/src/addins/MonoDevelop.TextEditor/MonoDevelop.TextEditor.Wpf/Undo/UndoHistoryRegistry.cs
main/src/addins/MonoDevelop.TextEditor/MonoDevelop.TextEditor/EditorCommandHandlers.cs
main/src/addins/MonoDevelop.TextEditor/MonoDevelop.TextEditor/TextViewContent.Commands.cs
main/src/addins/MonoDevelop.TextEditor/MonoDevelop.TextEditor/TextViewContent.Toolbox.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Commands/WindowCommands.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/CompositionManager.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Composition/PlatformCatalog.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/TextEditor.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Gui.Dialogs/CommonAboutDialog.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Gui/ViewCommandHandlers.cs
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.csproj
main/src/core/MonoDevelop.Ide/MonoDevelop.Ide/IdeStartup.cs

	 moved/renamed:
	^^^^^^^^^^^^^^^^

	 deleted:		TODO...
	^^^^^^^^^^
main/external/libgit-binary			<submodule>
main/external/libgit2				<submodule>
main/external/libgit2sharp			<submodule>
main/src/addins/VersionControl/MonoDevelop.VersionControl.Git	<directory>

##############################################################################################

	 how to build:
	^^^^^^^^^^^^^^^

$ git submodule init 
$ git submodule update 

$ ./configure --profile=gnome 

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
*) GIT support has been removed.
*) the QuickFix menu is not working in editor.
*) win10 wsl : the process seems to hang and keep running after closing the app.

