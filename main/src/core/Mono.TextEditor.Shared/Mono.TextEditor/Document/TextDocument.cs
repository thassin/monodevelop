﻿// 
// TextDocument.cs
//  
// Author:
//       Mike Krüger <mkrueger@xamarin.com>
// 
// Copyright (c) 2007 Novell, Inc (http://www.novell.com)
// Copyright (c) 2012 Xamarin Inc. (http://xamarin.com)
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
using System.Collections.Generic;
using System.Diagnostics;
using Mono.TextEditor.Highlighting;
using Mono.TextEditor.Utils;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MonoDevelop.Core.Text;
using MonoDevelop.Ide.Editor;
using MonoDevelop.Core;
using System.IO;
using MonoDevelop.Ide.Editor.Highlighting;

namespace Mono.TextEditor
{
	class TextDocument : ITextDocument, IDisposable
	{
		ImmutableText buffer;
		ILineSplitter splitter;

		ISyntaxHighlighting syntaxMode = null;

		TextSourceVersionProvider versionProvider = new TextSourceVersionProvider ();

		bool   readOnly;
		ReadOnlyCheckDelegate readOnlyCheckDelegate;

		string mimeType;
		public string MimeType {
			get {
				return mimeType;
			}
			set {
				if (mimeType != value) {
					mimeType = value;
					UpdateSyntaxMode ();
					OnMimeTypeChanged (EventArgs.Empty);
				}
			}
		}

	// REMOVED...
	//	private static Microsoft.VisualStudio.Utilities.IContentType GetContentTypeFromMimeType(string mimeType)

		public event EventHandler MimeTypeChanged;

		protected virtual void OnMimeTypeChanged (EventArgs e)
		{
			EventHandler handler = this.MimeTypeChanged;
			if (handler != null)
				handler (this, e);
		}

		FilePath fileName;
		public FilePath FileName {
			get {
				return fileName;
			}
			set {
				if (fileName != value) {
					fileName = value;
					UpdateSyntaxMode ();
					OnFileNameChanged (EventArgs.Empty);
				}
			}
		}

		public event EventHandler FileNameChanged;

		protected virtual void OnFileNameChanged (EventArgs e)
		{
			EventHandler handler = this.FileNameChanged;
			if (handler != null)
				handler (this, e);
		}

		public System.Text.Encoding Encoding {
			get {
				return buffer.Encoding;
			}
			set {
				buffer.Encoding = value ?? MonoDevelop.Core.Text.TextFileUtility.DefaultEncoding;
			}
		}

		internal ILineSplitter Splitter {
			get {
				return splitter;
			}
		}

		internal ISyntaxHighlighting SyntaxMode {
			get {
				if (syntaxMode == null) {
					lock (syncObject) {
						InitializeSyntaxMode ();
					}
				}
				return syntaxMode;
			}
			set {
				ISyntaxHighlighting old;
				lock (syncObject) {
					old = syntaxMode;
					if (old != null && old != DefaultSyntaxHighlighting.Instance) {
						old.HighlightingStateChanged -= SyntaxMode_HighlightingStateChanged;
						old.Dispose ();
					}

					syntaxMode = value;
					if (syntaxMode != null && syntaxMode != DefaultSyntaxHighlighting.Instance)
						syntaxMode.HighlightingStateChanged += SyntaxMode_HighlightingStateChanged;
				}
				OnSyntaxModeChanged (new SyntaxModeChangeEventArgs (old, syntaxMode));
			}
		}

		void SyntaxMode_HighlightingStateChanged (object sender, MonoDevelop.Ide.Editor.LineEventArgs e)
		{
			CommitDocumentUpdate ();
		}

		void OnSyntaxModeChanged (SyntaxModeChangeEventArgs e)
		{
			var handler = SyntaxModeChanged;
			if (handler != null)
				handler (this, e);
		}

		string syntaxModeFileName, syntaxModeMimeType;

		void InitializeSyntaxMode ()
		{
			var def = SyntaxHighlightingService.GetSyntaxHighlightingDefinition (FileName, this.MimeType);
			if (def != null) {
				SyntaxMode = new SyntaxHighlighting (def, this);
			} else {
#if false
		is_not_used	SyntaxMode = TagBasedSyntaxHighlighting.CreateSyntaxHighlighting(this.TextBuffer);
#else
				SyntaxMode = DefaultSyntaxHighlighting.Instance;
#endif
			}
		}

		void UpdateSyntaxMode ()
		{
			//never been initialized, don't need to update
			if (syntaxMode == null) {
				return;
			}

			//already up to date
			if (syntaxModeFileName == this.FileName && syntaxModeMimeType == this.MimeType) {
				return;
			}
			syntaxModeFileName = this.FileName;
			syntaxModeMimeType = this.MimeType;

			InitializeSyntaxMode ();
		}

		internal event EventHandler<SyntaxModeChangeEventArgs> SyntaxModeChanged;

		public object Tag {
			get;
			set;
		}

		public bool HasLineEndingMismatchOnTextSet {
			get {
				return splitter.LineEndingMismatch;
			}
			set {
				splitter.LineEndingMismatch = value;
			}
		}

		protected void Initialize(Encoding encoding, string fileName, ImmutableText buffer, ILineSplitter splitter)
		{
Console.WriteLine( "debug Initialize :: '" + fileName + "' enc=" + encoding + " len=" +  buffer.Length);

			this.buffer = buffer;
			this.splitter = splitter;

			this.Encoding = encoding;
			this.fileName = fileName;

			TextChanging += HandleSplitterLineSegmentTreeLineRemoved;

			foldSegmentTree.tree.NodeRemoved += HandleFoldSegmentTreetreeNodeRemoved;

			this.diffTracker.SetTrackDocument(this);

Console.WriteLine( "debug Initialize completed" );
		}

		public void Dispose()
		{
		// REMOVED new-editor stuff...
		//	this.TextBuffer.Changed -= this.OnTextBufferChanged;
		//	this.TextBuffer.ContentTypeChanged -= this.OnTextBufferContentTypeChanged;
		//	this.TextBuffer.Properties.RemoveProperty(typeof(ITextDocument));
		//	this.VsTextDocument.FileActionOccurred -= this.OnTextDocumentFileActionOccured;

// TODO what to do here??? is this needed at all???
Console.WriteLine( "tommih : TextDocument Disposed" );

			SyntaxMode = null;
		}

	// REMOVED...
	//	void OnTextBufferChanged(object sender, Microsoft.VisualStudio.Text.TextContentChangedEventArgs args)
	//	void OnTextBufferContentTypeChanged(object sender, Microsoft.VisualStudio.Text.ContentTypeChangedEventArgs args)
	//	void OnTextDocumentFileActionOccured(object sender, Microsoft.VisualStudio.Text.TextDocumentFileActionEventArgs args)

		void HandleFoldSegmentTreetreeNodeRemoved (object sender, RedBlackTree<FoldSegment>.RedBlackTreeNodeEventArgs e)
		{
			if (e.Node.IsCollapsed)
				foldedSegments.Remove (e.Node);
		}

		public TextDocument (string fileName, string mimeType)
		{
Console.WriteLine( "debug TextDocument ctor1 :: " + fileName + " : " + mimeType );

			Encoding enc;
			var text = TextFileUtility.GetText (fileName, out enc);

ImmutableText buffer = new ImmutableText (text);
ILineSplitter splitter = new LineSplitter();

			this.Initialize( enc, fileName, buffer, splitter );

Text = text;	// must set the Text property...

		}

		public TextDocument (string text = null)
		{
string info = "<null>";
if ( text != null ) info = "len=" + text.Length;
Console.WriteLine( "debug TextDocument ctor2 :: " + info );

// TODO how to detect the encoding from string???
Encoding enc = MonoDevelop.Core.Text.TextFileUtility.DefaultEncoding;
string fileName = null;

ImmutableText buffer = new ImmutableText (text);
splitter = new LineSplitter();

			this.Initialize( enc, fileName, buffer, splitter ); // PURA POIS VAAN... ????

Text = text;	// must set the Text property...

		}

		public TextDocument (string text, ILineSplitter splitter)
		{
Console.WriteLine( "debug TextDocument ctor3 SNAPSHOT :: len=" + text.Length );

// TODO how to detect the encoding from string???
Encoding enc = MonoDevelop.Core.Text.TextFileUtility.DefaultEncoding;
string fileName = null;

ImmutableText buffer = new ImmutableText (text);

			this.Initialize( enc, fileName, buffer, splitter ); // PURA POIS VAAN... ????

// this constructor is used only for "snapshot document" generation.
// -> the Text property must NOT be set here!!!

		}

		public static TextDocument CreateImmutableDocument (string text, bool suppressHighlighting = true)
		{
			return new TextDocument (text) {
				SuppressHighlightUpdate = suppressHighlighting,
				Text = text,
				IsReadOnly = true
			};
		}

#region Buffer implementation

		public int Length {
			get {
				return buffer.Length;
			}
		}

		public bool SuppressHighlightUpdate { get; set; }
		internal DocumentLine longestLineAtTextSet;
		WeakReference cachedText;

		public string Text {
			get {
				string completeText = cachedText != null ? (cachedText.Target as string) : null;
				if (completeText == null) {
					completeText = buffer.ToString();
					cachedText = new WeakReference(completeText);
				}
				return completeText;
			}
			set {
Console.WriteLine( "Text setter called" );
				var tmp = IsReadOnly;
				IsReadOnly = false;

				if (value == null)
					value = "";
				var args = new TextChangeEventArgs (0, Text, value);
				textSegmentMarkerTree.Clear ();
				OnTextReplacing (args);
				cachedText = null;
				buffer = new ImmutableText (value);
				extendingTextMarkers = new List<TextLineMarker> ();
				splitter.Initalize (value, out longestLineAtTextSet);
				ClearFoldSegments ();
				OnTextReplaced (args);
				versionProvider = new TextSourceVersionProvider ();
				buffer.Version = Version;
				OnTextSet (EventArgs.Empty);
				CommitUpdateAll ();

				ClearUndoBuffer ();

				IsReadOnly = tmp;
				IsTextSet = true;
			}
		}

		internal bool IsTextSet { get; set; }

		public void InsertText (int offset, string text)
		{
			ReplaceText (offset, 0, text);
		}

		public void InsertText (int offset, ITextSource text)
		{
			ReplaceText (offset, 0, text);
		}

		public void RemoveText (int offset, int count)
		{
			ReplaceText (offset, count, (string)null);
		}
		
		public void RemoveText (ISegment segment)
		{
			RemoveText (segment.Offset, segment.Length);
		}

		public void ReplaceText (int offset, int count, ITextSource value)
		{
			ReplaceText (offset, count, value?.Text);
		}

		public void ReplaceText (int offset, int count, string value)
		{
			if (offset < 0)
				throw new ArgumentOutOfRangeException (nameof (offset), "must be > 0, was: " + offset);
			if (offset > Length)
				throw new ArgumentOutOfRangeException (nameof (offset), "must be <= TextLength(" + Length +"), was: " + offset);
			if (count < 0)
				throw new ArgumentOutOfRangeException (nameof (count), "must be > 0, was: " + count);
			if (IsReadOnly)
				return;

			InterruptFoldWorker ();

			//int oldLineCount = LineCount;
			var args = new TextChangeEventArgs (offset, count > 0 ? GetTextAt (offset, count) : "", value);

			UndoOperation operation = null;
			bool endUndo = false;
			if (!isInUndo) {
				operation = new UndoOperation (args);
				if (currentAtomicOperation != null) {
					currentAtomicOperation.Add (operation);
				} else {
					OnBeginUndo ();
					undoStack.Push (operation);
					endUndo = true;
				}
				redoStack.Clear ();
			}

			if (value != null)
				EnsureSegmentIsUnfolded (offset, value.Length);
			
			OnTextReplacing (args);

Console.WriteLine( "ReplaceText :: args.TextChanges.Count=" + args.TextChanges.Count );

// tommih 20200617 : TextChangeEventArgs has been splitted to TextChangeEventArgs + TextChange...
if ( args.TextChanges.Count != 1 ) throw new InvalidOperationException("args.TextChanges.Count is not valid : " + args.TextChanges.Count);

			value = args.TextChanges[0].InsertedText.Text;

			cachedText = null;
			buffer = buffer.RemoveText(offset, count);
			if (!string.IsNullOrEmpty (value))
				buffer = buffer.InsertText (offset, value);
			foldSegmentTree.UpdateOnTextReplace (this, args);
			splitter.TextReplaced (this, args);
			versionProvider.AppendChange (args);
			buffer.Version = Version;
			OnTextReplaced (args);
			if (endUndo)
				OnEndUndo (new UndoOperationEventArgs (operation));
		}

		public void ApplyTextChanges (IEnumerable<Microsoft.CodeAnalysis.Text.TextChange> changes)
		{

Console.WriteLine( "TextDocument.ApplyTextChanges() start" );

			if (changes == null)
				throw new ArgumentNullException(nameof(changes));

			// apply the changes in reverse order, to keep the positions from begin corrupted.
			// TODO is "changes" always ordered like this? ever need to sort it???

			int? prevStart = null;
			foreach (var change in changes.Reverse())
			{
				int start = change.Span.Start;
				int length = change.Span.Length;

				if ( prevStart.HasValue && start >= prevStart.Value ) {
					Console.WriteLine( "TextDocument.ApplyTextChanges() changes has BAD ordering : prevStart=" + prevStart.Value + " start=" + start );
				}

				prevStart = start;
				string oldtext = GetTextAt (start, length);

				string newtext = change.NewText;
				if ( newtext == null ) newtext = "";

				Console.WriteLine ("TextDocument.ApplyTextChanges() :: s=" + start + " l=" + length + " ot='" + oldtext + "' nt='" + newtext + "' l2=" + newtext.Length);

			/*	if (length > 0) {	// this works but requires 2 operations...
					RemoveText (start, length);
				}

				if (newtext.Length > 0) {
					InsertText (start, newtext);
				}	*/

				if ( newtext.Length < 1 ) newtext = null;
				ReplaceText(start, length, newtext);
			}
		}

		public string GetTextBetween (int startOffset, int endOffset)
		{
			if (startOffset < 0)
				throw new ArgumentException ("startOffset < 0");
			if (startOffset > Length)
				throw new ArgumentException ("startOffset > Length");
			if (endOffset < 0)
				throw new ArgumentException ("startOffset < 0");
			if (endOffset > Length)
				throw new ArgumentException ("endOffset > Length");

			return buffer.ToString (startOffset, endOffset - startOffset);
		}

		public string GetTextBetween (DocumentLocation start, DocumentLocation end)
		{
			return GetTextBetween (LocationToOffset (start), LocationToOffset (end));
		}
		
		public string GetTextBetween (int startLine, int startColumn, int endLine, int endColumn)
		{
			return GetTextBetween (LocationToOffset (startLine, startColumn), LocationToOffset (endLine, endColumn));
		}
		
		public string GetTextAt (int offset, int count)
		{
			if (offset < 0)
				throw new ArgumentException ("startOffset < 0");
			if (offset > Length)
				throw new ArgumentException ("startOffset > Length");
			if (count < 0)
				throw new ArgumentException ("count < 0");
			if (offset + count > Length)
				throw new ArgumentException ("offset + count is beyond EOF");

			return buffer.ToString (offset, count);
		}
		
		public string GetTextAt (DocumentRegion region)
		{
			return GetTextAt (region.GetSegment (this));
		}

		public string GetTextAt (ISegment segment)
		{
			return GetTextAt (segment.Offset, segment.Length);
		}

		/// <summary>
		/// Gets the line text without the delimiter.
		/// </summary>
		/// <returns>
		/// The line text.
		/// </returns>
		/// <param name='line'>
		/// The line number.
		/// </param>
		public string GetLineText (int line)
		{
			var lineSegment = GetLine (line);
			return lineSegment != null ? GetTextAt (lineSegment.Offset, lineSegment.Length) : null;
		}
		
		public string GetLineText (int line, bool includeDelimiter)
		{
			var lineSegment = GetLine (line);
			return GetTextAt (lineSegment.Offset, includeDelimiter ? lineSegment.LengthIncludingDelimiter : lineSegment.Length);
		}
		
		public char GetCharAt (int offset)
		{
			return buffer [offset];
		}

		public char GetCharAt (DocumentLocation location)
		{
			return buffer [LocationToOffset (location)];
		}

		public char GetCharAt (int line, int column)
		{
			return buffer [LocationToOffset (line, column)];
		}

		/// <summary>
		/// Gets the index of the first occurrence of the character in the specified array.
		/// </summary>
		/// <param name="c">Character to search for</param>
		/// <param name="startIndex">Start index of the area to search.</param>
		/// <param name="count">Length of the area to search.</param>
		/// <returns>The first index where the character was found; or -1 if no occurrence was found.</returns>
		public int IndexOf (char c, int startIndex, int count)
		{
			// REPLACED by an old version...
			return Text.IndexOf (c, startIndex, count);
		}

	// REMOVED...
	//	public int IndexOf(string searchText, int startIndex, int count, StringComparison comparisonType)

		/// <summary>
		/// Gets the index of the first occurrence of any character in the specified array.
		/// </summary>
		/// <param name="anyOf">Characters to search for</param>
		/// <param name="startIndex">Start index of the area to search.</param>
		/// <param name="count">Length of the area to search.</param>
		/// <returns>The first index where any character was found; or -1 if no occurrence was found.</returns>
		public int IndexOfAny (char[] anyOf, int startIndex, int count)
		{
			return Text.IndexOfAny (anyOf, startIndex, count);
		}
		
		/// <summary>
		/// Gets the index of the first occurrence of the specified search text in this text source.
		/// </summary>
		/// <param name="searchText">The search text</param>
		/// <param name="startIndex">Start index of the area to search.</param>
		/// <param name="count">Length of the area to search.</param>
		/// <param name="comparisonType">String comparison to use.</param>
		/// <returns>The first index where the search term was found; or -1 if no occurrence was found.</returns>
		public int IndexOf (string searchText, int startIndex, int count, StringComparison comparisonType)
		{
			return Text.IndexOf (searchText, startIndex, count, comparisonType);
		}
		
		/// <summary>
		/// Gets the index of the last occurrence of the specified character in this text source.
		/// </summary>
		/// <param name="c">The search character</param>
		/// <param name="startIndex">Start index of the area to search.</param>
		/// <param name="count">Length of the area to search.</param>
		/// <returns>The last index where the search term was found; or -1 if no occurrence was found.</returns>
		/// <remarks>The search proceeds backwards from (startIndex+count) to startIndex.
		/// This is different than the meaning of the parameters on string.LastIndexOf!</remarks>
		public int LastIndexOf (char c, int startIndex, int count)
		{
			return Text.LastIndexOf (c, startIndex, count);
		}
		
		/// <summary>
		/// Gets the index of the last occurrence of the specified search text in this text source.
		/// </summary>
		/// <param name="searchText">The search text</param>
		/// <param name="startIndex">Start index of the area to search.</param>
		/// <param name="count">Length of the area to search.</param>
		/// <param name="comparisonType">String comparison to use.</param>
		/// <returns>The last index where the search term was found; or -1 if no occurrence was found.</returns>
		/// <remarks>The search proceeds backwards from (startIndex+count) to startIndex.
		/// This is different than the meaning of the parameters on string.LastIndexOf!</remarks>
		public int LastIndexOf (string searchText, int startIndex, int count, StringComparison comparisonType)
		{
			return Text.LastIndexOf (searchText, startIndex, count, comparisonType);
		}

		protected virtual void OnTextReplaced (TextChangeEventArgs args)
		{
			if (TextChanged != null)
				TextChanged (this, args);
		}

		public event EventHandler<TextChangeEventArgs> TextChanged;

		protected virtual void OnTextReplacing (TextChangeEventArgs args)
		{
			if (TextChanging != null)
				TextChanging (this, args);
		}

		public event EventHandler<TextChangeEventArgs> TextChanging;

		protected virtual void OnTextSet (EventArgs e)
		{
			EventHandler handler = this.TextSet;
			if (handler != null)
				handler (this, e);
		}

		public event EventHandler TextSet;

#endregion

#region Line Splitter operations

		public IEnumerable<DocumentLine> Lines {
			get {
				return splitter.Lines;
			}
		}

		public int LineCount {
			get {
				return splitter.Count;
			}
		}

		public IEnumerable<DocumentLine> GetLinesBetween (int startLine, int endLine)
		{
			return splitter.GetLinesBetween (startLine, endLine);
		}

		public IEnumerable<DocumentLine> GetLinesStartingAt (int startLine)
		{
			return splitter.GetLinesStartingAt (startLine);
		}

		public IEnumerable<DocumentLine> GetLinesReverseStartingAt (int startLine)
		{
			return splitter.GetLinesReverseStartingAt (startLine);
		}

		// 20200703 tommih : the above method(s) from old, the next (LocationToOffset) can be a new one...

		public int LocationToOffset (int line, int column)
		{
			if (line > this.LineCount || line < DocumentLocation.MinLine)
				return -1;
			DocumentLine documentLine = GetLine(line);
			return System.Math.Min(Length, documentLine.Offset + System.Math.Max(0, System.Math.Min(documentLine.Length, column - 1)));
		}
		
		public int LocationToOffset (DocumentLocation location)
		{
			return LocationToOffset(location.Line, location.Column);
		}
		
		public DocumentLocation OffsetToLocation (int offset)
		{
			IDocumentLine line = this.GetLineByOffset(offset);
			if (line == null)
				return DocumentLocation.Empty;

			var col = System.Math.Max(1, System.Math.Min(line.LengthIncludingDelimiter, offset - line.Offset) + 1);
			return new DocumentLocation(line.LineNumber, col);
		}

		public string GetLineIndent (int lineNumber)
		{
			return GetLineIndent (GetLine (lineNumber));
		}
		
		public string GetLineIndent (DocumentLine segment)
		{
			if (segment == null)
				return "";
			return segment.GetIndentation (this);
		}
		
		public DocumentLine GetLine (int lineNumber)
		{
			if (lineNumber < DocumentLocation.MinLine)
				return null;
			
			return splitter.Get (lineNumber);
		}

		IDocumentLine IReadonlyTextDocument.GetLine (int lineNumber)
		{
			return GetLine (lineNumber);
		}

	//	DocumentLine cachedLine;
	//	int cachedLineNumber = -1;
	//	DocumentLine cachedLineFromLineNumber;
	//	void ClearLineCache ()
	//	{
	//		cachedLine = null;
	//		cachedLineNumber = -1;
	//	}

		public DocumentLine GetLineByOffset (int offset)
		{
			return splitter.GetLineByOffset (offset);
		}

		IDocumentLine IReadonlyTextDocument.GetLineByOffset (int offset)
		{
			return GetLineByOffset (offset);
		}

		public int OffsetToLineNumber (int offset)
		{
			return splitter.OffsetToLineNumber (offset);
		}

#endregion

#region Undo/Redo operations

		internal class UndoOperation
		{
// tommih 20200703 property name change "Changes" -> "Args" and revert the old type.
// tommih 20200703 property name change "Changes" -> "Args" and revert the old type.
// tommih 20200703 property name change "Changes" -> "Args" and revert the old type.
			TextChangeEventArgs args;
			public virtual TextChangeEventArgs Args {
				get {
					return args;
				}
			}
			
			public object Tag {
				get;
				set;
			}
			
			protected UndoOperation()
			{
			}

			public UndoOperation (TextChangeEventArgs args)
			{
				this.args = args;
			}

			public virtual void Undo (TextDocument doc, bool fireEvent = true)
			{
			//	var changes = new List<TextChange>();
				var changes = new List<Microsoft.CodeAnalysis.Text.TextChange>();
			//TODO	foreach (var change in args.TextChanges)
			//TODO		changes.Add(new TextChange(change.NewOffset, change.InsertedText, change.RemovedText));

Console.WriteLine( "TODO TextDocument.Undo not implemented!" );

				doc.ApplyTextChanges(changes);
				if (fireEvent)
					OnUndoDone ();
			}
			
			public virtual void Redo (TextDocument doc, bool fireEvent = true)
			{
			//	var changes = new List<TextChange>();
				var changes = new List<Microsoft.CodeAnalysis.Text.TextChange>();
			//TODO	foreach (var change in args.TextChanges.Reverse())
			//TODO		changes.Add(new TextChange(change.Offset, change.RemovedText, change.InsertedText));

Console.WriteLine( "TODO TextDocument.Redo not implemented!" );

				doc.ApplyTextChanges(changes);
				if (fireEvent)
					OnRedoDone ();
			}
			
			protected virtual void OnUndoDone ()
			{
				if (UndoDone != null)
					UndoDone (this, EventArgs.Empty);
			}
			public event EventHandler UndoDone;
			
			protected virtual void OnRedoDone ()
			{
				if (RedoDone != null)
					RedoDone (this, EventArgs.Empty);
			}
			public event EventHandler RedoDone;
		}
		
		class AtomicUndoOperation : UndoOperation
		{
			OperationType operationType;
			protected List<UndoOperation> operations = new List<UndoOperation> ();

			public OperationType OperationType {
				get {
					return operationType;
				}
			}
			
			public List<UndoOperation> Operations {
				get {
					return operations;
				}
			}

// tommih 20200703 method name change "Changes" -> "Args" and revert the old type.
// tommih 20200703 method name change "Changes" -> "Args" and revert the old type.
// tommih 20200703 method name change "Changes" -> "Args" and revert the old type.
		//	public virtual Microsoft.VisualStudio.Text.INormalizedTextChangeCollection Changes {
			public override TextChangeEventArgs Args {
				get {
					return null;
				}
			}

			public AtomicUndoOperation (OperationType operationType = OperationType.Undefined)
			{
				this.operationType = operationType;
			}
		

			public void Insert (int index, UndoOperation operation)
			{
				operations.Insert (index, operation);
			}
			
			public void Add (UndoOperation operation)
			{
				operations.Add (operation);
			}
			
			public override void Undo (TextDocument doc, bool fireEvent = true)
			{
				doc.BeginAtomicUndo (operationType);
				try {
					for (int i = operations.Count - 1; i >= 0; i--) {
						operations [i].Undo (doc, false);
						doc.OnUndone (new UndoOperationEventArgs (operations [i]));
					}
				} finally {
					doc.EndAtomicUndo ();
				}
				if (fireEvent)
					OnUndoDone ();
			}
			
			public override void Redo (TextDocument doc, bool fireEvent = true)
			{
				doc.BeginAtomicUndo (operationType);
				try {
					foreach (UndoOperation operation in this.operations) {
						operation.Redo (doc, false);
						doc.OnRedone (new UndoOperationEventArgs (operation));
					}
				} finally {
					doc.EndAtomicUndo ();
				}
				if (fireEvent)
					OnRedoDone ();
			}
		}
		
		class KeyboardStackUndo : AtomicUndoOperation
		{
			bool isClosed = false;
			
			public bool IsClosed {
				get {
					return isClosed;
				}
				set {
					isClosed = value;
				}
			}

// tommih 20200703 method name change "Changes" -> "Args".
// tommih 20200703 method name change "Changes" -> "Args".
// tommih 20200703 method name change "Changes" -> "Args".
		//	public virtual Microsoft.VisualStudio.Text.INormalizedTextChangeCollection Changes {
			public override TextChangeEventArgs Args {
				get {
					return operations.Count > 0 ? operations [operations.Count - 1].Args : null;
				}
			}
		}
		
		bool isInUndo = false;
		Stack<UndoOperation> undoStack = new Stack<UndoOperation> ();
		Stack<UndoOperation> redoStack = new Stack<UndoOperation> ();
		AtomicUndoOperation currentAtomicOperation = null;

		internal int UndoBeginOffset {
			get {
				if (undoStack.Count == 0)
					return -1;
				var op = undoStack.Peek ();
				while (op is AtomicUndoOperation)
					op = ((AtomicUndoOperation)op).Operations.FirstOrDefault ();
				if (op == null)
					return -1;
			//	return ((UndoOperation)op).Changes[0].OldPosition;
				return ((UndoOperation)op).Args.TextChanges.Select (c => c.Offset).Min ();
			}
		}

		internal int RedoBeginOffset {
			get {
				if (redoStack.Count == 0)
					return -1;
				var op = redoStack.Peek ();
				while (op is AtomicUndoOperation)
					op = ((AtomicUndoOperation)op).Operations.FirstOrDefault ();
				if (op == null)
					return -1;
			//	return ((UndoOperation)op).Changes[0].NewPosition;
				return ((UndoOperation)op).Args.TextChanges.Select (c => c.Offset).Min ();
			}
		}

		public bool CanUndo {
			get {
				return this.undoStack.Count > 0 || currentAtomicOperation != null;
			}
		}
		
		UndoOperation[] savePoint = null;
		public bool IsDirty {
			get {
				if (this.currentAtomicOperation != null)
					return true;
				if (savePoint == null)
					return CanUndo;
				if (undoStack.Count != savePoint.Length)
					return true;
				UndoOperation[] currentStack = undoStack.ToArray ();
				for (int i = 0; i < currentStack.Length; i++) {
					if (savePoint[i] != currentStack[i])
						return true;
				}
				return false;
			}
		}
		
		public enum LineState {
			Unchanged,
			Dirty,
			Changed
		}

		public DiffTracker diffTracker = new DiffTracker ();

		public DiffTracker DiffTracker {
			get {
				return diffTracker;
			}
			set {
				diffTracker = value;
			}
		}
		
		public LineState GetLineState (DocumentLine line)
		{
			return diffTracker.GetLineState (line);
		}

		/// <summary>
		/// Marks the document not dirty at this point (should be called after save).
		/// </summary>
		public void SetNotDirtyState ()
		{
			OptimizeTypedUndo ();
			if (undoStack.Count > 0 && undoStack.Peek () is KeyboardStackUndo)
				((KeyboardStackUndo)undoStack.Peek ()).IsClosed = true;

			savePoint = undoStack.ToArray ();
			this.CommitUpdateAll ();
			DiffTracker.SetBaseDocument (CreateDocumentSnapshot ());
		}
		
		public void OptimizeTypedUndo ()
		{
			if (undoStack.Count == 0)
				return;
			UndoOperation top = undoStack.Pop ();
		//	if (top.Changes == null || top.Changes.Count > 1) {
			if (top.Args == null) {
				undoStack.Push (top);
				return;
			}
		//	foreach (var change in top.Changes) {
		//		if (change.NewLength != 1 || (top is KeyboardStackUndo && ((KeyboardStackUndo)top).IsClosed)) {
			foreach (var change in top.Args.TextChanges) {
				if (change == null || change.InsertedText == null || change.InsertionLength != 1 || (top is KeyboardStackUndo && ((KeyboardStackUndo)top).IsClosed)) {
					undoStack.Push (top);
					continue;
				}
				if (undoStack.Count == 0 || !(undoStack.Peek () is KeyboardStackUndo))
					undoStack.Push (new KeyboardStackUndo ());
				var keyUndo = (KeyboardStackUndo)undoStack.Pop ();
				if (keyUndo.IsClosed) {
					undoStack.Push (keyUndo);
					keyUndo = new KeyboardStackUndo ();
				}
			//	if (keyUndo.Changes != null) {
			//		foreach (var kchange in keyUndo.Changes) {
			//			if (kchange.OldPosition + 1 != change.OldPosition || change.NewLength == 0 || !char.IsLetterOrDigit (change.NewText [0])) {
				if (keyUndo.Args != null) {
					foreach (var kchange in keyUndo.Args.TextChanges) {
						if (kchange.Offset + 1 != change.Offset || !char.IsLetterOrDigit (change.InsertedText [0])) {
							keyUndo.IsClosed = true;
							undoStack.Push (keyUndo);
							keyUndo = new KeyboardStackUndo ();
						}
					}
				}
				keyUndo.Add (top);
				undoStack.Push (keyUndo);
			}
		}
		
		public int GetCurrentUndoDepth ()
		{
			return undoStack.Count;
		}
		
		public void StackUndoToDepth (int depth)
		{
			if (undoStack.Count == depth)
				return;
			var atomicUndo = new AtomicUndoOperation ();
			while (undoStack.Count > depth) {
				atomicUndo.Operations.Insert (0, undoStack.Pop ());
			}
			undoStack.Push (atomicUndo);
		}
		
		public void MergeUndoOperations (int number)
		{
			number = System.Math.Min (number, undoStack.Count);
			var atomicUndo = new AtomicUndoOperation ();
			while (number-- > 0) {
				atomicUndo.Insert (0, undoStack.Pop ());
			}
			undoStack.Push (atomicUndo);
		}
		
		public void Undo ()
		{
			if (undoStack.Count <= 0)
				return;
			OnBeforeUndoOperation (EventArgs.Empty);
			isInUndo = true;
			var operation = undoStack.Pop ();
			redoStack.Push (operation);
			operation.Undo (this);
			isInUndo = false;
			OnUndone (new UndoOperationEventArgs (operation));
		}

		public void RollbackTo (ITextSourceVersion version)
		{
			var steps = Version.CompareAge (version);
			if (steps < 0)
				throw new InvalidOperationException ("Invalid version");
			while (steps-- > 0) {
				undoStack.Pop ().Undo (this);
			}
		}

		internal protected virtual void OnUndone (UndoOperationEventArgs e)
		{
			EventHandler<UndoOperationEventArgs> handler = this.Undone;
			if (handler != null)
				handler (this, e);
		}

		public event EventHandler<UndoOperationEventArgs> Undone;
		
		internal protected virtual void OnBeforeUndoOperation (EventArgs e)
		{
			var handler = this.BeforeUndoOperation;
			if (handler != null)
				handler (this, e);
		}

		public event EventHandler BeforeUndoOperation;

		public bool CanRedo {
			get {
				return this.redoStack.Count > 0;
			}
		}
		
		public void Redo ()
		{
			if (redoStack.Count <= 0)
				return;
			isInUndo = true;
			UndoOperation operation = redoStack.Pop ();
			undoStack.Push (operation);
			operation.Redo (this);
			isInUndo = false;
			OnRedone (new UndoOperationEventArgs (operation));
		}
		
		internal protected virtual void OnRedone (UndoOperationEventArgs e)
		{
			EventHandler<UndoOperationEventArgs> handler = this.Redone;
			if (handler != null)
				handler (this, e);
		}
		
		public event EventHandler<UndoOperationEventArgs> Redone;
		 
		Stack<OperationType> currentAtomicUndoOperationType =  new Stack<OperationType> ();
		int atomicUndoLevel;

		public bool IsInAtomicUndo {
			get {
				return atomicUndoLevel > 0;
			}
		}

		public OperationType CurrentAtomicUndoOperationType {
			get {
				return currentAtomicUndoOperationType.Count > 0 ?  currentAtomicUndoOperationType.Peek () : OperationType.Undefined;
			}
		}
		
		class UndoGroup : IDisposable
		{
			TextDocument doc;
			
			public UndoGroup (TextDocument doc, OperationType operationType)
			{
				if (doc == null)
					throw new ArgumentNullException ("doc");
				doc.BeginAtomicUndo (operationType);
				this.doc = doc;
			}

			public void Dispose ()
			{
				if (doc != null) {
					doc.EndAtomicUndo ();
					doc = null;
				}
			}
		}
		
		public IDisposable OpenUndoGroup()
		{
			return OpenUndoGroup(OperationType.Undefined);
		}

		public IDisposable OpenUndoGroup(OperationType operationType)
		{
			return new UndoGroup (this, operationType);
		}

		internal void BeginAtomicUndo (OperationType operationType = OperationType.Undefined)
		{
			currentAtomicUndoOperationType.Push (operationType);
			if (currentAtomicOperation == null) {
				Debug.Assert (atomicUndoLevel == 0); 
				currentAtomicOperation = new AtomicUndoOperation (operationType);
				OnBeginUndo ();
			}
			atomicUndoLevel++;
		}

		internal void EndAtomicUndo ()
		{
			if (atomicUndoLevel <= 0)
				throw new InvalidOperationException ("There is no atomic undo operation running.");
			atomicUndoLevel--;
			Debug.Assert (atomicUndoLevel >= 0); 
			
			if (atomicUndoLevel == 0 && currentAtomicOperation != null) {
				var cuao = currentAtomicOperation;
				currentAtomicOperation = null;

				if (cuao.Operations.Count > 1) {
					undoStack.Push (cuao);
					OnEndUndo (new UndoOperationEventArgs (cuao));
				} else {
					if (cuao.Operations.Count > 0) {
						undoStack.Push (cuao.Operations [0]);
						OnEndUndo (new UndoOperationEventArgs (cuao.Operations [0]));
					} else {
						OnEndUndo (null);
					}
				}
			}
			currentAtomicUndoOperationType.Pop ();
		}
		
		protected virtual void OnBeginUndo ()
		{
			if (BeginUndo != null) 
				BeginUndo (this, EventArgs.Empty);
		}
		
		public void ClearUndoBuffer ()
		{
			undoStack.Clear ();
			redoStack.Clear ();
		}
		
		[Serializable]
		public sealed class UndoOperationEventArgs : EventArgs
		{
			public UndoOperation Operation { get; private set; }

			public UndoOperationEventArgs (UndoOperation operation)
			{
				this.Operation = operation;
			}
			
		}
		
		protected virtual void OnEndUndo (UndoOperationEventArgs e)
		{
			EventHandler<UndoOperationEventArgs> handler = this.EndUndo;
			if (handler != null)
				handler (this, e);
		}
		
		public event EventHandler                         BeginUndo;
		public event EventHandler<UndoOperationEventArgs> EndUndo;

#endregion
		
#region Folding
		
		SegmentTree<FoldSegment> foldSegmentTree = new SegmentTree<FoldSegment> ();
		
		public bool IgnoreFoldings {
			get;
			set;
		}
		
		public bool HasFoldSegments {
			get {
				return FoldSegments.Any ();
			}
		}
		
		public IEnumerable<FoldSegment> FoldSegments {
			get {
				return foldSegmentTree.Segments;
			}
		}
		
		readonly object syncObject = new object();

		CancellationTokenSource foldSegmentSrc;
		object foldSegmentTaskLock = new object ();
		Task foldSegmentTask;

		public void UpdateFoldSegments (IEnumerable<IFoldSegment> newSegments, bool startTask = false, bool useApplicationInvoke = false, CancellationToken masterToken = default(CancellationToken))
		{
			if (newSegments == null) {
				return;
			}
			lock (foldSegmentTaskLock) {
				InterruptFoldWorker ();
				bool update;
				if (!startTask) {
					var newFoldedSegments = UpdateFoldSegmentWorker (newSegments, out update);
					if (useApplicationInvoke) {
						Gtk.Application.Invoke ((o, args) => {
							foldedSegments = newFoldedSegments;
							InformFoldTreeUpdated ();
						});
					} else {
						foldedSegments = newFoldedSegments;
						InformFoldTreeUpdated ();
					}
					return;
				}
				foldSegmentSrc = new CancellationTokenSource ();
				masterToken.Register (InterruptFoldWorker);
				var token = foldSegmentSrc.Token;
				foldSegmentTask = Task.Factory.StartNew (delegate {
					var segments = UpdateFoldSegmentWorker (newSegments, out update, token);
					if (token.IsCancellationRequested)
						return;
					foldedSegments = segments;
					Gtk.Application.Invoke ((o, args) => {
						if (token.IsCancellationRequested)
							return;
						InformFoldTreeUpdated ();
						if (update)
							CommitUpdateAll ();
					});
				}, token);
			}
		}
		
		void RemoveFolding (FoldSegment folding)
		{
			folding.isAttached = false;
			if (folding.isFolded)
				foldedSegments.Remove (folding);
			foldSegmentTree.Remove (folding);
		}
		
		/// <summary>
		/// Updates the fold segments in a background worker thread. Don't call this method outside of a background worker.
		/// Use UpdateFoldSegments instead.
		/// </summary>
		HashSet<FoldSegment> UpdateFoldSegmentWorker (IEnumerable<IFoldSegment> segments, out bool update, CancellationToken token = default(CancellationToken))
		{
			var oldSegments = new List<FoldSegment> (FoldSegments);
			int oldIndex = 0;
			bool foldedSegmentAdded = false;
			var newSegments = segments.ToList ();
			newSegments.Sort ();
			var newFoldedSegments = new HashSet<FoldSegment> ();
			foreach (var fs in newSegments) {
				FoldSegment newFoldSegment = (fs as FoldSegment) ?? new FoldSegment (fs);
				if (token.IsCancellationRequested) {
					update = false;
					return null;
				}
				int offset = newFoldSegment.Offset;
				while (oldIndex < oldSegments.Count && offset > oldSegments [oldIndex].Offset) {
					RemoveFolding (oldSegments [oldIndex]);
					oldIndex++;
				}

				if (oldIndex < oldSegments.Count && offset == oldSegments [oldIndex].Offset) {
					FoldSegment curSegment = oldSegments [oldIndex];
					if (curSegment.IsCollapsed && newFoldSegment.Length != curSegment.Length)
						curSegment.IsCollapsed = newFoldSegment.IsCollapsed = false;
					curSegment.Length = newFoldSegment.Length;
					curSegment.CollapsedText = newFoldSegment.CollapsedText;

					if (newFoldSegment.IsCollapsed) {
						foldedSegmentAdded |= !curSegment.IsCollapsed;
						curSegment.isFolded = true;
					}
					if (curSegment.isFolded)
						newFoldedSegments.Add (curSegment);
					oldIndex++;
				} else {
					newFoldSegment.isAttached = true;
					foldedSegmentAdded |= newFoldSegment.IsCollapsed;
					if (oldIndex < oldSegments.Count && newFoldSegment.Length == oldSegments [oldIndex].Length) {
						newFoldSegment.isFolded = oldSegments [oldIndex].IsCollapsed;
					}
					if (newFoldSegment.IsCollapsed)
						newFoldedSegments.Add (newFoldSegment);
					foldSegmentTree.Add (newFoldSegment);
				}
			}
			while (oldIndex < oldSegments.Count) {
				if (token.IsCancellationRequested) {
					update = false;
					return null;
				}
				RemoveFolding (oldSegments [oldIndex]);
				oldIndex++;
			}
			bool countChanged = foldedSegments.Count != newFoldedSegments.Count;
			update = foldedSegmentAdded || countChanged;
			return newFoldedSegments;
		}
		
		public void WaitForFoldUpdateFinished ()
		{
			if (foldSegmentTask != null) {
				try {
					foldSegmentTask.Wait (5000);
				} catch (AggregateException e) {
					e.Flatten ().Handle (x => x is OperationCanceledException);
				} catch (OperationCanceledException) {
					
				}
				foldSegmentTask = null;
			}
		}
		
		internal void InterruptFoldWorker ()
		{
			if (foldSegmentSrc == null)
				return;
			foldSegmentSrc.Cancel ();
			WaitForFoldUpdateFinished ();
			foldSegmentSrc = null;
		}
		
		public void ClearFoldSegments ()
		{
			InterruptFoldWorker ();
			foldSegmentTree = new SegmentTree<FoldSegment> ();
			foldSegmentTree.tree.NodeRemoved += HandleFoldSegmentTreetreeNodeRemoved; 
			foldedSegments.Clear ();
			InformFoldTreeUpdated ();
		}
		
		public IEnumerable<FoldSegment> GetFoldingsFromOffset (int offset)
		{
			if (offset < 0 || offset >= Length)
				return new FoldSegment[0];
			return foldSegmentTree.GetSegmentsAt (offset);
		}
		
		public IEnumerable<FoldSegment> GetFoldingContaining (int lineNumber)
		{
			return GetFoldingContaining(this.GetLine (lineNumber));
		}
				
		public IEnumerable<FoldSegment> GetFoldingContaining (DocumentLine line)
		{
			if (line == null)
				return new FoldSegment[0];
			return foldSegmentTree.GetSegmentsOverlapping (line.Offset, line.Length);
		}

		public IEnumerable<FoldSegment> GetFoldingContaining (int offset, int length)
		{
			return foldSegmentTree.GetSegmentsOverlapping (offset, length);
		}

		public IEnumerable<FoldSegment> GetStartFoldings (int lineNumber)
		{
			return GetStartFoldings (this.GetLine (lineNumber));
		}
		
		public IEnumerable<FoldSegment> GetStartFoldings (DocumentLine line)
		{
			if (line == null)
				yield break;
			var lineOffset = line.Offset;
			foreach (var fold in GetFoldingContaining (line))
				if (fold.GetStartLine (this).Offset == lineOffset)
					yield return fold;
		}

		public IEnumerable<FoldSegment> GetStartFoldings (int offset, int length)
		{
			return GetFoldingContaining (offset, length).Where (fold => offset <= fold.GetStartLine (this).Offset && fold.GetStartLine (this).Offset < offset + length);
		}

		public IEnumerable<FoldSegment> GetEndFoldings (int lineNumber)
		{
			return GetStartFoldings (this.GetLine (lineNumber));
		}
		
		public IEnumerable<FoldSegment> GetEndFoldings (DocumentLine line)
		{
			var lineOffset = line.Offset;
			foreach (FoldSegment segment in GetFoldingContaining (line)) {
				if (segment.GetEndLine (this).Offset == lineOffset)
					yield return segment;
			}
		}

		public IEnumerable<FoldSegment> GetEndFoldings (int offset, int length)
		{
			return GetFoldingContaining (offset, length).Where (fold => offset <= fold.GetEndLine (this).Offset && fold.GetEndLine (this).Offset < offset + length);
		}

		public int GetLineCount (FoldSegment segment)
		{
			return segment.GetEndLine (this).LineNumber - segment.GetStartLine (this).LineNumber;
		}
		
		public void EnsureOffsetIsUnfolded (int offset)
		{
			bool needUpdate = false;
			foreach (FoldSegment fold in GetFoldingsFromOffset (offset).Where (f => f.IsCollapsed && f.Offset < offset && offset < f.EndOffset)) {
				needUpdate = true;
				fold.IsCollapsed = false;
				InformFoldChanged(new FoldSegmentEventArgs(fold));
			}
		}

		public void EnsureSegmentIsUnfolded (int offset, int length)
		{
			bool needUpdate = false;
			foreach (var fold in GetFoldingContaining (offset, length).Where (f => f.IsCollapsed)) {
				needUpdate = true;
				fold.IsCollapsed = false;
				InformFoldChanged(new FoldSegmentEventArgs(fold));
			}
		}

		internal void InformFoldTreeUpdated ()
		{
			var handler = FoldTreeUpdated;
			if (handler != null)
				handler (this, EventArgs.Empty);
		}
		public event EventHandler FoldTreeUpdated;
		
		HashSet<FoldSegment> foldedSegments = new HashSet<FoldSegment> ();

		public IEnumerable<FoldSegment> FoldedSegments {
			get {
				return foldedSegments;
			}
		}

		internal void InformFoldChanged (FoldSegmentEventArgs args)
		{
			if (args.FoldSegment.IsCollapsed) {
				foldedSegments.Add (args.FoldSegment);
			} else {
				foldedSegments.Remove (args.FoldSegment);
			}
			var handler = Folded;
			if (handler != null)
				handler (this, args);
		}

		public event EventHandler<FoldSegmentEventArgs> Folded;

#endregion

#region Text line markers

		public event EventHandler<TextMarkerEvent> MarkerAdded;
		protected virtual void OnMarkerAdded (TextMarkerEvent e)
		{
			EventHandler<TextMarkerEvent> handler = this.MarkerAdded;
			if (handler != null)
				handler (this, e);
		}

		public event EventHandler<TextMarkerEvent> MarkerRemoved;
		protected virtual void OnMarkerRemoved (TextMarkerEvent e)
		{
			EventHandler<TextMarkerEvent> handler = this.MarkerRemoved;
			if (handler != null)
				handler (this, e);
		}

		
		List<TextLineMarker> extendingTextMarkers = new List<TextLineMarker> ();
		public IEnumerable<DocumentLine> LinesWithExtendingTextMarkers {
			get {
				foreach (var marker in extendingTextMarkers) {
					var line = marker.LineSegment;
					if (line != null)
						yield return line;
				} 
			}
		}
		
		public void AddMarker (int lineNumber, TextLineMarker marker)
		{
			AddMarker (this.GetLine (lineNumber), marker);
		}
		
		public void AddMarker (DocumentLine line, TextLineMarker marker)
		{
			AddMarker (line, marker, true);
		}

		internal class DocumentLineTextSegmentMarker : TextSegmentMarker
		{
			readonly TextDocument doc;
			public TextLineMarker Marker { get; }

			internal DocumentLine LineSegment {
				get {
					return doc.GetLineByOffset (Offset);
				}
			}

			public DocumentLineTextSegmentMarker (TextDocument doc, DocumentLine line, TextLineMarker marker) : base (line.Offset, line.Length)
			{
				if (marker == null)
					throw new ArgumentNullException (nameof (marker));
				this.doc = doc;
				this.Marker = marker;
				this.Marker.parent = this;
			}
		}

		public bool IsBookmarked (DocumentLine line)
		{
			return GetMarkers (line).Any(m => m == BookmarkMarker.Instance);
		}

		public void SetIsBookmarked (DocumentLine line, bool isBookmarked)
		{
			if (IsBookmarked (line) != isBookmarked) {
				if (isBookmarked) {
					AddMarker (line, BookmarkMarker.Instance);
				} else {
					RemoveMarker (line, typeof (BookmarkMarker));
				}
				RequestUpdate (new LineUpdate (line.LineNumber));
				CommitDocumentUpdate ();
			}
		}

		public IEnumerable<TextLineMarker> GetMarkers (DocumentLine line)
		{
			if (line == null)
				return Enumerable.Empty<TextLineMarker> ();
			return GetTextSegmentMarkersAt (line).OfType<DocumentLineTextSegmentMarker> ().Select (m => m.Marker);
		}

		public IEnumerable<TextLineMarker> GetMarkersOrderedByInsertion (DocumentLine line)
		{
			if (line == null)
				return Enumerable.Empty<TextLineMarker> ();
			return OrderTextSegmentMarkersByInsertion(GetTextSegmentMarkersAt (line)).OfType<DocumentLineTextSegmentMarker> ().Select (m => m.Marker);
		}

		public void ClearMarkers (DocumentLine line)
		{
			if (line == null)
				return;
			foreach (var marker in GetTextSegmentMarkersAt (line).OfType<DocumentLineTextSegmentMarker> ())
				RemoveMarker (marker); 
		}

		public void AddMarker (DocumentLine line, TextLineMarker marker, bool commitUpdate, int idx = -1)
		{
			if (line == null || marker == null)
				return;
			AddMarker (new DocumentLineTextSegmentMarker (this, line, marker));
			OnMarkerAdded (new TextMarkerEvent (line, marker));
			if (marker is IExtendingTextLineMarker) {
				lock (extendingTextMarkers) {
					extendingTextMarkers.Add (marker);
					extendingTextMarkers.Sort (CompareMarkers);
					OnHeightChanged (EventArgs.Empty);
				}
			}
			if (commitUpdate)
				this.CommitLineUpdate (line);
		}
		
		static int CompareMarkers (TextLineMarker left, TextLineMarker right)
		{
			if (left.LineSegment == null || right.LineSegment == null)
				return 0;
			return left.LineSegment.Offset.CompareTo (right.LineSegment.Offset);
		}
		
		public void RemoveMarker (TextLineMarker marker)
		{
			RemoveMarker (marker, true);
		}
		
		public void RemoveMarker (TextLineMarker marker, bool updateLine)
		{
			if (marker == null)
				return;
			var line = marker.LineSegment;
			if (line == null)
				return;
			if (marker is IDisposable)
				((IDisposable)marker).Dispose ();

			foreach (var m in GetTextSegmentMarkersAt (line).OfType<DocumentLineTextSegmentMarker> ()) {
				if (m.Marker == marker) {
					RemoveMarker (m);
					break;
				}
			}

			OnMarkerRemoved (new TextMarkerEvent (line, marker));
			if (marker is IExtendingTextLineMarker) {
				lock (extendingTextMarkers) {
					extendingTextMarkers.Remove (marker);
					OnHeightChanged (EventArgs.Empty);
				}
			}
			if (updateLine)
				this.CommitLineUpdate (line);
		}
		
		public void RemoveMarker (int lineNumber, Type type)
		{
			RemoveMarker (this.GetLine (lineNumber), type);
		}
		
		public void RemoveMarker (DocumentLine line, Type type)
		{
			RemoveMarker (line, type, true);
		}
		
		public void RemoveMarker (DocumentLine line, Type type, bool updateLine)
		{
			if (line == null || type == null)
				return;
			foreach (var m in GetTextSegmentMarkersAt (line).OfType<DocumentLineTextSegmentMarker> ()) {
				if (m.Marker.GetType () == type) {
					RemoveMarker (m);
				}
			}

			if (typeof (IExtendingTextLineMarker).IsAssignableFrom (type)) {
				lock (extendingTextMarkers) {
					foreach (TextLineMarker marker in GetMarkers (line).Where (marker => marker is IExtendingTextLineMarker)) {
						extendingTextMarkers.Remove (marker);
					}
					OnHeightChanged (EventArgs.Empty);
				}
			}
			if (updateLine)
				this.CommitLineUpdate (line);
		}

#endregion

#region Text segment markers

		int textSegmentInsertId = 0;
		SegmentTree<TextSegmentMarker> textSegmentMarkerTree = new SegmentTree<TextSegmentMarker> ();

		public static IEnumerable<TextSegmentMarker> OrderTextSegmentMarkersByInsertion (IEnumerable<TextSegmentMarker> enumerable)
		{
			return enumerable.OrderBy (m => m.insertId);
		}

		public IEnumerable<TextSegmentMarker> GetTextSegmentMarkersAt (DocumentLine line)
		{
			return GetTextSegmentMarkersAt (line.Segment);
		}

		internal IEnumerable<TextSegmentMarker> GetVisibleTextSegmentMarkersAt (DocumentLine line)
		{
			foreach (var marker in textSegmentMarkerTree.GetSegmentsOverlapping (line.Segment))
				if (marker.IsVisible)
					yield return marker;
		}

		int textSegmentCacheOffset = -1, textSegmentCacheLength;
		List<TextSegmentMarker> textSegmentCache;

		int textMarkerCacheOffset = -1;
		List<TextSegmentMarker> textMarkerSegmentCache;

		void ClearTextMarkerCache ()
		{
			textSegmentCacheOffset = textMarkerCacheOffset = -1;
		}

		public IEnumerable<TextSegmentMarker> GetTextSegmentMarkersAt (ISegment segment)
		{
			if (segment.Offset == textSegmentCacheOffset && segment.Length == textSegmentCacheLength)
				return textSegmentCache;
			textSegmentCacheOffset = segment.Offset;
			textSegmentCacheLength = segment.Length;
			return textSegmentCache = textSegmentMarkerTree.GetSegmentsOverlapping (segment).ToList ();
		}

		public IEnumerable<TextSegmentMarker> GetTextSegmentMarkersAt (int offset)
		{
			if (textMarkerCacheOffset == offset)
				return textMarkerSegmentCache;
			textMarkerCacheOffset = offset;
			return textMarkerSegmentCache = textSegmentMarkerTree.GetSegmentsAt (offset).ToList ();
		}
		

		public void AddMarker (TextSegmentMarker marker)
		{
			ClearTextMarkerCache ();
			marker.insertId = textSegmentInsertId++;
			textSegmentMarkerTree.Add (marker);
			var startLine = OffsetToLineNumber (marker.Offset);
			var endLine = OffsetToLineNumber (marker.EndOffset);
			CommitMultipleLineUpdate (startLine, endLine);
		}

		/// <summary>
		/// Removes a marker from the document.
		/// </summary>
		/// <returns><c>true</c>, if marker was removed, <c>false</c> otherwise.</returns>
		/// <param name="marker">Marker.</param>
		public bool RemoveMarker (TextSegmentMarker marker)
		{
			ClearTextMarkerCache ();
			bool wasRemoved = textSegmentMarkerTree.Remove (marker);
			if (wasRemoved) {
				var startLine = OffsetToLineNumber (marker.Offset);
				var endLine = OffsetToLineNumber (marker.EndOffset);
				CommitMultipleLineUpdate (startLine, endLine);
			}
			return wasRemoved;
		}

#endregion

		void HandleSplitterLineSegmentTreeLineRemoved (object sender, TextChangeEventArgs e)
		{
			for (int i = 0; i < e.TextChanges.Count; ++i) {
				var change = e.TextChanges[i];
				var line = GetLineByOffset (change.Offset);
				if (line == null)
					continue;
				var endOffset = change.Offset + change.RemovalLength;
				var offset = line.Offset;
				do {
					foreach (TextLineMarker marker in GetMarkers (line)) {
						if (marker is IExtendingTextLineMarker) {
						//	UnRegisterVirtualTextMarker ((IExtendingTextLineMarker)marker); // old code, virtual markers not added anymore...
							lock (extendingTextMarkers) {
								extendingTextMarkers.Remove (marker);
								OnHeightChanged (EventArgs.Empty);
							}
						}
					}
					offset += line.LengthIncludingDelimiter;
					line = line.NextLine;
				} while (line != null && offset < endOffset);
			}
		}
		
		public bool Contains (int offset)
		{
			return new TextSegment (0, Length).Contains (offset);
		}
		
		public bool Contains (ISegment segment)
		{
			return new TextSegment (0, Length).Contains (segment);
		}

#region Update logic

		List<DocumentUpdateRequest> updateRequests = new List<DocumentUpdateRequest> ();
		
		public IEnumerable<DocumentUpdateRequest> UpdateRequests {
			get {
				return updateRequests;
			}
		}
		// Use CanEdit (int lineNumber) instead for getting a request
		// if a part of a document can be read. ReadOnly should generally not be used
		// for deciding, if a document is readonly or not.
		public bool IsReadOnly {
			get {
				return readOnly;
			}
			set {
				readOnly = value;
			}
		}
		
		public ReadOnlyCheckDelegate ReadOnlyCheckDelegate {
			get { return readOnlyCheckDelegate; }
			set { readOnlyCheckDelegate = value; }
		}


		public void RequestUpdate (DocumentUpdateRequest request)
		{
			lock (syncObject) {
				updateRequests.Add (request);
			}
		}
		
		public void CommitDocumentUpdate ()
		{
			lock (syncObject) {
				if (DocumentUpdated != null)
					DocumentUpdated (this, EventArgs.Empty);
				updateRequests.Clear ();
			}
		}
		
		public void CommitLineUpdate (int line)
		{
			RequestUpdate (new LineUpdate (line));
			CommitDocumentUpdate ();
		}
		
		public void CommitLineUpdate (DocumentLine line)
		{
			CommitLineUpdate (line.LineNumber);
		}

		public void CommitUpdateAll ()
		{
			RequestUpdate (new UpdateAll ());
			CommitDocumentUpdate ();
		}

		public void CommitMultipleLineUpdate (int start, int end)
		{
			RequestUpdate (new MultipleLineUpdate (start, end));
			CommitDocumentUpdate ();
		}
		
		public event EventHandler DocumentUpdated;

#endregion

#region Helper functions

		public const string openBrackets    = "([{<";
		public const string closingBrackets = ")]}>";
		
		public static bool IsBracket (char ch)
		{
			return (openBrackets + closingBrackets).IndexOf (ch) >= 0;
		}
		
		public static bool IsWordSeparator (char ch)
		{
			return !(char.IsLetterOrDigit (ch) || ch == '_');
		}

		public bool IsWholeWordAt (int offset, int length)
		{
			return (offset == 0 || IsWordSeparator (GetCharAt (offset - 1))) &&
				   (offset + length == Length || IsWordSeparator (GetCharAt (offset + length)));
		}
		
		public bool IsEmptyLine (DocumentLine line)
		{
			for (int i = 0; i < line.Length; i++) {
				char ch = GetCharAt (line.Offset + i);
				if (!Char.IsWhiteSpace (ch)) 
					return false;
			}
			return true;
		}

		public enum CharacterClass {
			Unknown,

			Whitespace,

			IdentifierPart

		}

		public static CharacterClass GetCharacterClass (char ch)
		{
			if (Char.IsWhiteSpace (ch))
				return CharacterClass.Whitespace;
			if (Char.IsLetterOrDigit (ch) || ch == '_')
				return CharacterClass.IdentifierPart;

			return CharacterClass.Unknown;
		}
		
		public static void RemoveTrailingWhitespaces (TextEditorData data, DocumentLine line)
		{
			if (line == null)
				return;
			int whitespaces = 0;
			for (int i = line.Length - 1; i >= 0; i--) {
				if (Char.IsWhiteSpace (data.Document.GetCharAt (line.Offset + i))) {
					whitespaces++;
				} else {
					break;
				}
			}
			
			if (whitespaces > 0) {
				var removeOffset = line.Offset + line.Length - whitespaces;
				data.Remove (removeOffset, whitespaces);
			}
		}

#endregion

		public bool IsInUndo {
			get {
				return isInUndo;
			}
		}
		
#region Diff

		int[] GetDiffCodes (ref int codeCounter, Dictionary<string, int> codeDictionary, bool includeEol)
		{
			int i = 0;
			var result = new int[LineCount];
			foreach (DocumentLine line in Lines) {
				string lineText = buffer.ToString (line.Offset, includeEol ? line.LengthIncludingDelimiter : line.Length);
				int curCode;
				if (!codeDictionary.TryGetValue (lineText, out curCode)) {
					codeDictionary[lineText] = curCode = ++codeCounter;
				}
				result[i] = curCode;
				i++;
			}
			return result;
		}
		
		public IEnumerable<Hunk> Diff (TextDocument changedDocument, bool includeEol = true)
		{
			var codeDictionary = new Dictionary<string, int> ();
			int codeCounter = 0;
			return Mono.TextEditor.Utils.Diff.GetDiff<int> (this.GetDiffCodes (ref codeCounter, codeDictionary, includeEol),
				changedDocument.GetDiffCodes (ref codeCounter, codeDictionary, includeEol));
		}

#endregion

#region ContentLoaded

		// The problem: Action to perform on a newly opened text editor, but content didn't get loaded because autosave file exist.
		//              At this point the document is open, but the content didn't yet have loaded - therefore the action on the conent can't be perfomed.
		// Solution: Perform the action after the user did choose load autosave or not. 
		//           This is done by the RunWhenLoaded method. Text editors should call the InformLoadComplete () when the content has successfully been loaded
		//           at that point the outstanding actions are run.
		bool isLoaded;
		List<Action> loadedActions = new List<Action> ();
		List<Action> realizedActions = new List<Action> ();
		
		/// <summary>
		/// Gets a value indicating whether this instance is loaded.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is loaded; otherwise, <c>false</c>.
		/// </value>
		public bool IsLoaded {
			get { return isLoaded; }
		}

		public bool IsRealized {
			get;
			private set;
		}
		
		/// <summary>
		/// Informs the document when the content is loaded. All outstanding actions are executed.
		/// </summary>
		public void InformLoadComplete ()
		{
			if (isLoaded)
				return;
			isLoaded = true;
			loadedActions.ForEach (act => act ());
			loadedActions = null;
		}

		public void InformRealizedComplete ()
		{
			if (IsRealized)
				return;

			IsRealized = true;
			realizedActions.ForEach (act => act ());
			realizedActions = null;
		}
		
		/// <summary>
		/// Performs an action when the content is loaded.
		/// </summary>
		/// <param name='action'>
		/// The action to run.
		/// </param>
		public void RunWhenLoaded (Action action)
		{
			if (IsLoaded) {
				action ();
				return;
			}
			loadedActions.Add (action);
		}

		public void RunWhenRealized (Action action)
		{
			if (IsRealized) {
				action ();
				return;
			}
			realizedActions.Add (action);
		}

#endregion

#region ITextSource implementation

		public System.IO.TextReader CreateReader ()
		{
			return new ImmutableTextTextReader (buffer);
		}

		public System.IO.TextReader CreateReader (int offset, int length)
		{
			return new ImmutableTextTextReader(buffer.GetText(offset, length));
		}

		public virtual ITextSourceVersion Version {
			get {
				return versionProvider.CurrentVersion;
			}
		}

		public char this [int offset] {
			get {
				return GetCharAt (offset);
			}
			set {
				ReplaceText (offset, 1, value.ToString ());
			}
		}

		internal class SnapshotDocument : TextDocument
		{
			readonly ITextSourceVersion version;
			public override ITextSourceVersion Version  {
				get {
					return version;
				}
			}

		//	public SnapshotDocument (TextDocument doc) : base (doc.useBOM, doc.encoding, doc.fileName, CreateBufferFromTextDocument(doc))
		//	public SnapshotDocument (TextDocument doc) : base (doc.UseBOM, doc.Encoding, doc.fileName, doc.buffer, new LazyLineSplitter (doc.LineCount))
			public SnapshotDocument (TextDocument doc) : base (doc.buffer.ToString(), new LazyLineSplitter (doc.LineCount))
			{
Console.WriteLine( "SnapshotDocument ctor" );
				this.version = doc.Version;
				((LazyLineSplitter)splitter).src = this;
				fileName = doc.fileName;
				Encoding = doc.Encoding;
				mimeType = doc.mimeType;

				IsReadOnly = true;
			}
		}

		public TextDocument CreateDocumentSnapshot ()
		{
			return new SnapshotDocument (this);
		}

		public void CopyTo (int sourceIndex, char [] destination, int destinationIndex, int count)
		{
			buffer.CopyTo (sourceIndex, destination, destinationIndex, count); 
		}

		public ImmutableText GetImmutableText ()
		{
			return buffer;
		}

		public ImmutableText GetImmutableText (int offset, int count)
		{
			return buffer.GetText (offset, count);
		}

		ITextSource ITextSource.CreateSnapshot ()
		{
			return GetImmutableText ();
		}

		ITextSource ITextSource.CreateSnapshot (int offset, int length)
		{
			return GetImmutableText (offset, length);
		}

// 20200703 tommih :: the method below is kind-of-duplicate to CreateDocumentSnapshot().
// required by ITextDocument (or similar), cannot have "public" definition.

		IReadonlyTextDocument ITextDocument.CreateDocumentSnapshot ()
		{
			return CreateDocumentSnapshot ();
		}

		public void WriteTextTo (TextWriter writer)
		{
			if (writer == null)
				throw new ArgumentNullException ("writer");
			writer.Write (Text);
		}

		public void WriteTextTo (TextWriter writer, int offset, int length)
		{
			if (writer == null)
				throw new ArgumentNullException ("writer");
			writer.Write (GetTextAt (offset, length));
		}

#endregion

		void OnHeightChanged (EventArgs e)
		{
			HeightChanged?.Invoke (this, e);
		}

		internal event EventHandler HeightChanged;

	// REMOVED...
	//	private DocumentLine Get(int number)
	//	internal sealed class DocumentLineFromTextSnapshotLine : DocumentLine
	//	class SnapshotToReadonlyTextDocument : SnapshotSpanToTextSource, IReadonlyTextDocument
	//	class SnapshotSpanToTextSource : ITextSource
	//	sealed class SnapshotSpanToTextReader : TextReader
	//	public class TextVersionToTextSourceVersion : ITextSourceVersion

	}

	delegate bool ReadOnlyCheckDelegate (int line);
}
