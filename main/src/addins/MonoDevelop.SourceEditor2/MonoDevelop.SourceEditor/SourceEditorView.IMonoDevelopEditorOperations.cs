//
// SourceEditorView.IEditorOperationsMD.cs
//
// Author:
//       Mike Krüger <mikkrg@microsoft.com>
//
// Copyright (c) 2018 Microsoft Corporation. All rights reserved.
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

using System;
//using Microsoft.VisualStudio.Text; oe removed...
//using Microsoft.VisualStudio.Text.Editor; oe removed...
//using Microsoft.VisualStudio.Text.Formatting; oe removed...
//using Microsoft.VisualStudio.Text.Operations; oe removed...
using Mono.TextEditor;
using MonoDevelop.Ide.Editor;

// oe NOTICE : convert each "IEditorOperations." => "IEditorOperationsMD." in this file.
// oe NOTICE : convert each "IEditorOperations." => "IEditorOperationsMD." in this file.
// oe NOTICE : convert each "IEditorOperations." => "IEditorOperationsMD." in this file.

namespace MonoDevelop.SourceEditor
{
	partial class SourceEditorView : IMonoDevelopEditorOperations
	{
		bool IEditorOperationsMD.CanPaste => this.EnablePaste;

		bool IEditorOperationsMD.CanDelete => this.EnableDelete;

		bool IEditorOperationsMD.CanCut => this.EnableCut;

	//oe	public ITextView TextView { get; set; }

	//oe	IEditorOptions IEditorOperationsMD.Options => throw new NotImplementedException ();

	//oe	ITrackingSpan IEditorOperationsMD.ProvisionalCompositionSpan => throw new NotImplementedException ();

		string IEditorOperationsMD.SelectedText => TextEditor.SelectedText;

		void IEditorOperationsMD.MoveLineDown (bool extendSelection)
		{
			if (extendSelection) {
				SelectionActions.MoveDown (TextEditor.GetTextEditorData ());
			} else {
				CaretMoveActions.Down (TextEditor.GetTextEditorData ());
			}
		}

		void IEditorOperationsMD.MoveLineUp (bool extendSelection)
		{
			if (extendSelection) {
				SelectionActions.MoveUp (TextEditor.GetTextEditorData ());
			} else {
				CaretMoveActions.Up (TextEditor.GetTextEditorData ());
			}
		}

		void IEditorOperationsMD.MoveToPreviousCharacter (bool extendSelection)
		{
			if (extendSelection) {
				SelectionActions.MoveLeft (TextEditor.GetTextEditorData ());
			} else {
				CaretMoveActions.Left (TextEditor.GetTextEditorData ());
			}
		}

		void IEditorOperationsMD.MoveToNextCharacter (bool extendSelection)
		{
			if (extendSelection) {
				SelectionActions.MoveRight (TextEditor.GetTextEditorData ());
			} else {
				CaretMoveActions.Right (TextEditor.GetTextEditorData ());
			}
		}

		void IEditorOperationsMD.MoveToEndOfLine (bool extendSelection)
		{
			if (extendSelection) {
				SelectionActions.MoveLineEnd (TextEditor.GetTextEditorData ());
			} else {
				CaretMoveActions.LineEnd (TextEditor.GetTextEditorData ());
			}
		}

		void IEditorOperationsMD.MoveToStartOfLine (bool extendSelection)
		{
			if (extendSelection) {
				TextEditor.RunAction (SelectionActions.MoveLineHome);
			} else {
				TextEditor.RunAction (CaretMoveActions.LineHome);
			}
		}

		void IEditorOperationsMD.MoveToStartOfDocument (bool extendSelection)
		{
			if (extendSelection) {
				TextEditor.RunAction (SelectionActions.MoveToDocumentStart);
			} else {
				TextEditor.RunAction (CaretMoveActions.ToDocumentStart);
			}
		}

		void IEditorOperationsMD.MoveToEndOfDocument (bool extendSelection)
		{
			if (extendSelection) {
				TextEditor.RunAction (SelectionActions.MoveToDocumentEnd);
			} else {
				TextEditor.RunAction (CaretMoveActions.ToDocumentEnd);
			}
		}

		bool IEditorOperationsMD.Backspace ()
		{
			TextEditor.RunAction (DeleteActions.Backspace);
			return true;
		}

		bool IEditorOperationsMD.CopySelection ()
		{
			TextEditor.RunAction (ClipboardActions.Copy);
			return true;
		}

		bool IEditorOperationsMD.CutSelection ()
		{
			ClipboardActions.Cut (TextEditor.GetTextEditorData ());
			return true;
		}

		bool IEditorOperationsMD.Paste ()
		{
			return ClipboardActions.PasteWithResult (TextEditor.GetTextEditorData ());
		}

		bool IEditorOperationsMD.InsertNewLine ()
		{
			TextEditor.RunAction (MiscActions.InsertNewLine);
			return true;
		}

		bool IEditorOperationsMD.Tabify ()
		{
			TextEditor.RunAction (MiscActions.InsertTab);
			return true;
		}

		bool IEditorOperationsMD.Untabify ()
		{
			TextEditor.RunAction (MiscActions.RemoveTab);
			return true;
		}

		bool IEditorOperationsMD.DeleteWordToLeft ()
		{
			TextEditor.RunAction (DeleteActions.PreviousWord);
			return true;
		}

		bool IEditorOperationsMD.DeleteWordToRight ()
		{
			TextEditor.RunAction (DeleteActions.NextWord);
			return true;
		}

		void IEditorOperationsMD.ScrollLineCenter ()
		{
			TextEditor.RunAction (MiscActions.RecenterEditor);
		}

		void IEditorOperationsMD.MoveToNextWord (bool extendSelection)
		{
			if (extendSelection) {
				TextEditor.RunAction (SelectionActions.MoveNextWord);
			} else {
				TextEditor.RunAction (CaretMoveActions.NextWord);
			}
		}

		void IEditorOperationsMD.MoveToPreviousWord (bool extendSelection)
		{
			if (extendSelection) {
				TextEditor.RunAction (SelectionActions.MovePreviousWord);
			} else {
				TextEditor.RunAction (CaretMoveActions.PreviousWord);
			}
		}

		void IEditorOperationsMD.PageUp (bool extendSelection)
		{
			if (extendSelection) {
				TextEditor.RunAction (SelectionActions.MovePageUp);
			} else {
				TextEditor.RunAction (CaretMoveActions.PageUp);
			}
		}

		void IEditorOperationsMD.PageDown (bool extendSelection)
		{
			if (extendSelection) {
				TextEditor.RunAction (SelectionActions.MovePageDown);
			} else {
				TextEditor.RunAction (CaretMoveActions.PageDown);
			}
		}

		bool IEditorOperationsMD.DeleteFullLine ()
		{
			TextEditor.RunAction (DeleteActions.CaretLine);
			return true;
		}

		bool IEditorOperationsMD.DeleteToEndOfLine ()
		{
			TextEditor.RunAction (DeleteActions.CaretLineToEnd);
			return true;
		}

		void IEditorOperationsMD.ScrollLineTop ()
		{
			TextEditor.RunAction (ScrollActions.Up);
		}

		void IEditorOperationsMD.ScrollLineBottom ()
		{
			TextEditor.RunAction (ScrollActions.Down);
		}

		void IEditorOperationsMD.ScrollPageUp ()
		{
			TextEditor.RunAction (ScrollActions.PageUp);
		}

		void IEditorOperationsMD.ScrollPageDown ()
		{
			TextEditor.RunAction (ScrollActions.PageDown);
		}

		bool IEditorOperationsMD.Indent ()
		{
			if (widget.TextEditor.IsSomethingSelected) {
				MiscActions.IndentSelection (widget.TextEditor.GetTextEditorData ());
			} else {
				int offset = widget.TextEditor.LocationToOffset (widget.TextEditor.Caret.Line, 1);
				widget.TextEditor.Insert (offset, widget.TextEditor.Options.IndentationString);
			}
			return true;
		}

		bool IEditorOperationsMD.Unindent ()
		{
			MiscActions.RemoveTab (widget.TextEditor.GetTextEditorData ());
			return true;
		}

	//oe	void IEditorOperationsMD.SelectAndMoveCaret (VirtualSnapshotPoint anchorPoint, VirtualSnapshotPoint activePoint)
	//oe	{
	//oe		throw new NotImplementedException ();
	//oe	}

	//oe	void IEditorOperationsMD.SelectAndMoveCaret (VirtualSnapshotPoint anchorPoint, VirtualSnapshotPoint activePoint, TextSelectionMode selectionMode)
	//oe	{
	//oe		throw new NotImplementedException ();
	//oe	}

	//oe	void IEditorOperationsMD.SelectAndMoveCaret (VirtualSnapshotPoint anchorPoint, VirtualSnapshotPoint activePoint, TextSelectionMode selectionMode, EnsureSpanVisibleOptions? scrollOptions)
	//oe	{
	//oe		throw new NotImplementedException ();
	//oe	}

		void IEditorOperationsMD.MoveToHome (bool extendSelection)
		{
			if (extendSelection) {
				TextEditor.RunAction (SelectionActions.MoveLineHome);
			} else {
				TextEditor.RunAction (CaretMoveActions.LineHome);
			}
		}

		void IEditorOperationsMD.GotoLine (int lineNumber)
		{
			TextEditor.Caret.Line = lineNumber;
			TextEditor.ScrollToCaret ();
		}

		void IEditorOperationsMD.MoveCurrentLineToTop ()
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.MoveCurrentLineToBottom ()
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.MoveToStartOfLineAfterWhiteSpace (bool extendSelection)
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.MoveToStartOfNextLineAfterWhiteSpace (bool extendSelection)
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.MoveToStartOfPreviousLineAfterWhiteSpace (bool extendSelection)
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.MoveToLastNonWhiteSpaceCharacter (bool extendSelection)
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.MoveToTopOfView (bool extendSelection)
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.MoveToBottomOfView (bool extendSelection)
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.SwapCaretAndAnchor ()
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.DeleteToBeginningOfLine ()
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.DeleteBlankLines ()
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.DeleteHorizontalWhiteSpace ()
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.OpenLineAbove ()
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.OpenLineBelow ()
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.IncreaseLineIndent ()
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.DecreaseLineIndent ()
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.InsertText (string text)
		{
			TextEditor.InsertAtCaret (text);
			return true;
		}

	//oe	bool IEditorOperationsMD.InsertTextAsBox (string text, out VirtualSnapshotPoint boxStart, out VirtualSnapshotPoint boxEnd)
	//oe	{
	//oe		throw new NotImplementedException ();
	//oe	}

		bool IEditorOperationsMD.InsertProvisionalText (string text)
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.Delete ()
		{
			TextEditor.RunAction (DeleteActions.Delete);
			return true;
		}

		bool IEditorOperationsMD.ReplaceSelection (string text)
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.TransposeCharacter ()
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.TransposeLine ()
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.TransposeWord ()
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.MakeLowercase ()
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.MakeUppercase ()
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.ToggleCase ()
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.Capitalize ()
		{
			throw new NotImplementedException ();
		}

	//oe	bool IEditorOperationsMD.ReplaceText (Span replaceSpan, string text)
	//oe	{
	//oe		throw new NotImplementedException ();
	//oe	}

		int IEditorOperationsMD.ReplaceAllMatches (string searchText, string replaceText, bool matchCase, bool matchWholeWord, bool useRegularExpressions)
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.InsertFile (string filePath)
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.ConvertSpacesToTabs ()
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.ConvertTabsToSpaces ()
		{
			throw new NotImplementedException ();
		}

		bool IEditorOperationsMD.NormalizeLineEndings (string replacement)
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.SelectCurrentWord ()
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.SelectEnclosing ()
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.SelectFirstChild ()
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.SelectNextSibling (bool extendSelection)
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.SelectPreviousSibling (bool extendSelection)
		{
			throw new NotImplementedException ();
		}

	//oe	void IEditorOperationsMD.SelectLine (ITextViewLine viewLine, bool extendSelection)
	//oe	{
	//oe		throw new NotImplementedException ();
	//oe	}

		void IEditorOperationsMD.SelectAll ()
		{
			TextEditor.RunAction (SelectionActions.SelectAll);
		}

		void IEditorOperationsMD.ExtendSelection (int newEnd)
		{
			throw new NotImplementedException ();
		}

	//oe	void IEditorOperationsMD.MoveCaret (ITextViewLine textLine, double horizontalOffset, bool extendSelection)
	//oe	{
	//oe		throw new NotImplementedException ();
	//oe	}

		void IEditorOperationsMD.ResetSelection ()
		{
			TextEditor.RunAction (SelectionActions.ClearSelection);
		}

		bool IEditorOperationsMD.CutFullLine ()
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.ScrollUpAndMoveCaretIfNecessary ()
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.ScrollDownAndMoveCaretIfNecessary ()
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.ScrollColumnLeft ()
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.ScrollColumnRight ()
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.AddBeforeTextBufferChangePrimitive ()
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.AddAfterTextBufferChangePrimitive ()
		{
			throw new NotImplementedException ();
		}

		void IEditorOperationsMD.ZoomIn ()
		{
			TextEditor.Options.ZoomIn ();
		}

		void IEditorOperationsMD.ZoomOut ()
		{
			TextEditor.Options.ZoomOut ();
		}

		void IEditorOperationsMD.ZoomTo (double zoomLevel)
		{
			TextEditor.Options.Zoom = zoomLevel;
		}

	//oe	string IEditorOperationsMD.GetWhitespaceForVirtualSpace (VirtualSnapshotPoint point)
	//oe	{
	//oe		throw new NotImplementedException ();
	//oe	}

#region IMonoDevelopEditorOperations members

		void IMonoDevelopEditorOperations.SwitchCaretMode ()
		{
			TextEditor.RunAction (MiscActions.SwitchCaretMode);
		}

		void IMonoDevelopEditorOperations.DeletePreviousSubword ()
		{
			TextEditor.RunAction (DeleteActions.PreviousSubword);
		}

		void IMonoDevelopEditorOperations.DeleteNextSubword ()
		{
			TextEditor.RunAction (DeleteActions.NextSubword);
		}

		void IMonoDevelopEditorOperations.StartCaretPulseAnimation ()
		{
			TextEditor.StartCaretPulseAnimation ();
		}

		void IMonoDevelopEditorOperations.JoinLines ()
		{
			using (var undo = Document.OpenUndoGroup ()) {
				TextEditor.RunAction (Mono.TextEditor.Vi.ViActions.Join);
			}
		}

		void IMonoDevelopEditorOperations.MoveToNextSubWord ()
		{
			TextEditor.RunAction (SelectionActions.MoveNextSubword);
		}

		void IMonoDevelopEditorOperations.MoveToPrevSubWord ()
		{
			TextEditor.RunAction (SelectionActions.MovePreviousSubword);
		}

		void IMonoDevelopEditorOperations.ShowQuickInfo ()
		{
			widget.TextEditor.TextArea.ShowQuickInfo ();
		}

		void IMonoDevelopEditorOperations.MoveBlockUp ()
		{
			using (var undo = TextEditor.OpenUndoGroup ()) {
				TextEditor.RunAction (MiscActions.MoveBlockUp);
				CorrectIndenting ();
			}
		}

		void IMonoDevelopEditorOperations.MoveBlockDown ()
		{
			using (var undo = TextEditor.OpenUndoGroup ()) {
				TextEditor.RunAction (MiscActions.MoveBlockDown);
				CorrectIndenting ();
			}
		}

		void IMonoDevelopEditorOperations.ToggleBlockSelectionMode ()
		{
			TextEditor.SelectionMode = TextEditor.SelectionMode == MonoDevelop.Ide.Editor.SelectionMode.Normal ? MonoDevelop.Ide.Editor.SelectionMode.Block : MonoDevelop.Ide.Editor.SelectionMode.Normal;
			TextEditor.QueueDraw ();
		}

#endregion

	}
}
