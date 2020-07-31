
// tommih 2020-07-23.
// this is a replacement for Microsoft.VisualStudio.Text.Operations.IEditorOperations interface.

// https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.operations.ieditoroperations?view=visualstudiosdk-2019
// also see SourceEditorView.IMonoDevelopEditorOperations.cs (where the methods are (mostly?) implemented).

namespace MonoDevelop.Ide.Editor
{
	interface IEditorOperationsMD
	{
		// properties.

		bool CanCut { get; }
		bool CanDelete { get; }
		bool CanPaste { get; }

		string SelectedText { get; }

		// methods.

		void MoveLineDown (bool extendSelection);
		void MoveLineUp (bool extendSelection);

		void MoveToPreviousCharacter (bool extendSelection);
		void MoveToNextCharacter (bool extendSelection);

		void MoveToEndOfLine (bool extendSelection);
		void MoveToStartOfLine (bool extendSelection);

		void MoveToStartOfDocument (bool extendSelection);
		void MoveToEndOfDocument (bool extendSelection);

		bool Backspace ();

		bool CopySelection ();
		bool CutSelection ();
		bool Paste ();

		bool InsertNewLine ();

		bool Tabify ();
		bool Untabify ();

		bool DeleteWordToLeft ();
		bool DeleteWordToRight ();

		void ScrollLineCenter ();

		void MoveToNextWord (bool extendSelection);
		void MoveToPreviousWord (bool extendSelection);

		void PageUp (bool extendSelection);
		void PageDown (bool extendSelection);

		bool DeleteFullLine ();
		bool DeleteToEndOfLine ();

		void ScrollLineTop ();
		void ScrollLineBottom ();

		void ScrollPageUp ();
		void ScrollPageDown ();

		bool Indent ();
		bool Unindent ();

	// oe REMOVED...
	//oe	void SelectAndMoveCaret (VirtualSnapshotPoint anchorPoint, VirtualSnapshotPoint activePoint);
	//oe	void SelectAndMoveCaret (VirtualSnapshotPoint anchorPoint, VirtualSnapshotPoint activePoint, TextSelectionMode selectionMode);
	//oe	void SelectAndMoveCaret (VirtualSnapshotPoint anchorPoint, VirtualSnapshotPoint activePoint, TextSelectionMode selectionMode, EnsureSpanVisibleOptions? scrollOptions);

		void MoveToHome (bool extendSelection);
		void GotoLine (int lineNumber);

		void MoveCurrentLineToTop ();
		void MoveCurrentLineToBottom ();

		void MoveToStartOfLineAfterWhiteSpace (bool extendSelection);
		void MoveToStartOfNextLineAfterWhiteSpace (bool extendSelection);
		void MoveToStartOfPreviousLineAfterWhiteSpace (bool extendSelection);
		void MoveToLastNonWhiteSpaceCharacter (bool extendSelection);

		void MoveToTopOfView (bool extendSelection);
		void MoveToBottomOfView (bool extendSelection);

		void SwapCaretAndAnchor ();

		bool DeleteToBeginningOfLine ();
		bool DeleteBlankLines ();
		bool DeleteHorizontalWhiteSpace ();

		bool OpenLineAbove ();
		bool OpenLineBelow ();

		bool IncreaseLineIndent ();
		bool DecreaseLineIndent ();

		bool InsertText (string text);

	// oe REMOVED...
	//oe	bool InsertTextAsBox (string text, out VirtualSnapshotPoint boxStart, out VirtualSnapshotPoint boxEnd);

		bool InsertProvisionalText (string text);

		bool Delete ();

		bool ReplaceSelection (string text);

		bool TransposeCharacter ();
		bool TransposeLine ();
		bool TransposeWord ();

		bool MakeLowercase ();
		bool MakeUppercase ();

		bool ToggleCase ();

		bool Capitalize ();

	// oe REMOVED... Microsoft.VisualStudio.Text.Span
	//oe	bool ReplaceText (Span replaceSpan, string text);

		int ReplaceAllMatches (string searchText, string replaceText, bool matchCase, bool matchWholeWord, bool useRegularExpressions);

		bool InsertFile (string filePath);

		bool ConvertSpacesToTabs ();
		bool ConvertTabsToSpaces ();

		bool NormalizeLineEndings (string replacement);

		void SelectCurrentWord ();
		void SelectEnclosing ();
		void SelectFirstChild ();

		void SelectNextSibling (bool extendSelection);
		void SelectPreviousSibling (bool extendSelection);

	// oe REMOVED...
	//oe	void SelectLine (ITextViewLine viewLine, bool extendSelection);

		void SelectAll ();

		void ExtendSelection (int newEnd);

	// oe REMOVED...
	//oe	void MoveCaret (ITextViewLine textLine, double horizontalOffset, bool extendSelection);

		void ResetSelection ();

		bool CutFullLine ();

		void ScrollUpAndMoveCaretIfNecessary ();
		void ScrollDownAndMoveCaretIfNecessary ();

		void ScrollColumnLeft ();
		void ScrollColumnRight ();

		void AddBeforeTextBufferChangePrimitive ();
		void AddAfterTextBufferChangePrimitive ();

		void ZoomIn ();
		void ZoomOut ();

		void ZoomTo (double zoomLevel);

	// oe REMOVED...
	//oe	string GetWhitespaceForVirtualSpace (VirtualSnapshotPoint point);

	}
}

