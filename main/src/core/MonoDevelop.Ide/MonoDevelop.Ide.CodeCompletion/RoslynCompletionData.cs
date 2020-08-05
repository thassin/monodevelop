﻿//
// RoslynCompletionData.cs
//
// Author:
//       Mike Krüger <mikkrg@microsoft.com>
//
// Copyright (c) 2017 Microsoft Corporation
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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.Text;
using MonoDevelop.Core;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Ide.Editor;
using MonoDevelop.Ide.Editor.Extension;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
//using Microsoft.VisualStudio.Platform; oe removed...
//using Microsoft.VisualStudio.Text; oe removed...
using MonoDevelop.Ide.CodeTemplates;
using System.Linq;
using System.Text;
using MonoDevelop.Ide.Editor.Highlighting;
using MonoDevelop.Ide.Fonts;

namespace MonoDevelop.Ide.CodeCompletion
{
	public abstract class RoslynCompletionData : CompletionData
	{
		protected readonly Microsoft.CodeAnalysis.Document doc;
	//oe	protected readonly ITextSnapshot triggerSnapshot; removed...
		protected readonly CompletionService completionService;

		public CompletionItem CompletionItem { get; private set; }

		public override CompletionItemRules Rules { get { return CompletionItem.Rules; } }

		public override string DisplayText {
			get {
				return CompletionItem.DisplayText;
			}
		}

		public override string Description {
			get {
				var description = completionService.GetDescriptionAsync (doc, CompletionItem).Result;
				return description.Text;
			}
		}

		public override string CompletionText {
			get {
				return CompletionItem.DisplayText;
			}
		}

		public abstract CompletionProvider Provider { get; }

		protected abstract string MimeType { get; }

		public override IconId Icon {
			get {
				if (CompletionItem.Tags.Contains ("Snippet")) {
					var template = CodeTemplateService.GetCodeTemplates (MimeType).FirstOrDefault (t => t.Shortcut == CompletionItem.DisplayText);
					if (template != null)
						return template.Icon;
				}
				var modifier = GetItemModifier ();
				var type = GetItemType ();
				var hash = CalculateHashCode (modifier, type);
				if (!IconIdCache.ContainsKey (hash))
					IconIdCache [hash] = "md-" + modifier + type;
				return IconIdCache [hash];
			}
		}

		internal static int CalculateHashCode (string modifier, string type)
		{
			return modifier.GetHashCode () ^ type.GetHashCode ();
		}

		static Dictionary<int, string> IconIdCache = new Dictionary<int, string>();

	// oe TODO parameter removed -- need to replace it??? also see CSharpCompletionData.
	//oe	public RoslynCompletionData (Microsoft.CodeAnalysis.Document document, ITextSnapshot triggerSnapshot, CompletionService completionService, CompletionItem completionItem)
		public RoslynCompletionData (Microsoft.CodeAnalysis.Document document, /* ITextSnapshot triggerSnapshot, */ CompletionService completionService, CompletionItem completionItem)
		{
			this.doc = document;
		//oe	this.triggerSnapshot = triggerSnapshot;
			this.completionService = completionService;
			CompletionItem = completionItem;
		}

		public override string GetDisplayDescription (bool isSelected)
		{
			if (CompletionItem.Properties.TryGetValue ("DescriptionMarkup", out string result))
				return result;
			return base.GetDisplayDescription (isSelected);
		}

		public override string GetRightSideDescription (bool isSelected)
		{
			if (CompletionItem.Properties.TryGetValue ("RightSideMarkup", out string result))
				return result;
			return null;
		}

		internal static Dictionary<string, string> roslynCompletionTypeTable = new Dictionary<string, string> {
			{ "Field", "field" },
			{ "Alias", "field" },
			{ "ArrayType", "field" },
			{ "Assembly", "field" },
			{ "DynamicType", "field" },
			{ "ErrorType", "field" },
			{ "Label", "field" },
			{ "NetModule", "field" },
			{ "PointerType", "field" },
			{ "RangeVariable", "field" },
			{ "TypeParameter", "field" },
			{ "Preprocessing", "field" },

			{ "Constant", "literal" },

			{ "Parameter", "variable" },
			{ "Local", "variable" },

			{ "Method", "method" },

			{ "Namespace", "name-space" },

			{ "Property", "property" },

			{ "Event", "event" },

			{ "Class", "class" },

			{ "Delegate", "delegate" },

			{ "Enum", "enum" },

			{ "Interface", "interface" },

			{ "Struct", "struct" },
			{ "Structure", "struct" },

			{ "Keyword", "keyword" },

			{ "Snippet", "template"},

			{ "EnumMember", "literal" },

			{ "NewMethod", "newmethod" },

			{ "ExtensionMethod", "extensionmethod" }
		};

		string GetItemType ()
		{
			foreach (var tag in CompletionItem.Tags) {
				if (roslynCompletionTypeTable.TryGetValue (tag, out string result))
					return result;
			}
			LoggingService.LogWarning ("RoslynCompletionData: Can't find item type '" + string.Join (",", CompletionItem.Tags) + "'");
			return "literal";
		}

		internal static Dictionary<string, string> modifierTypeTable = new Dictionary<string, string> {
			{ "Private", "private-" },
			{ "ProtectedAndInternal", "ProtectedOrInternal-" },
			{ "Protected", "protected-" },
			{ "Internal", "internal-" },
			{ "ProtectedOrInternal", "ProtectedOrInternal-" }
		};

		string GetItemModifier ()
		{
			foreach (var tag in CompletionItem.Tags) {
				if (modifierTypeTable.TryGetValue (tag, out string result))
					return result;
			}
			return "";
		}

		public override DisplayFlags DisplayFlags {
			get {
				DisplayFlags result = DisplayFlags.None;

				if (CompletionItem.Tags.Contains ("bold")) {
					result = DisplayFlags.MarkedBold;
				}

				return result;
			}
		}

		public override void InsertCompletionText (CompletionListWindow window, ref KeyActions ka, KeyDescriptor descriptor)
		{

Console.WriteLine("oe-TODO :: RoslynCompletionData.InsertCompletionText");

			var document = IdeApp.Workbench.ActiveDocument;
			var editor = document?.Editor;
			if (editor == null || Provider == null) {
				base.InsertCompletionText (window, ref ka, descriptor);
				return;
			}
			var completionChange = Provider.GetChangeAsync (doc, CompletionItem, null, default (CancellationToken)).WaitAndGetResult (default (CancellationToken));
			var textChange = completionChange.TextChange;

			// oe add...
			int start = textChange.Span.Start;
			int length = textChange.Span.Length;
			string newText = textChange.NewText;
// tommih 20200721 : usually "length" is too small (always 1), even though multiple chars have been typed since the completion event launch.
// -> perhaps one could simply use the current caret-position to fix that in most cases? there is no option to change line or go back???
			int length2 = editor.CaretOffset - start;
Console.WriteLine("RoslynCompletionData.InsertCompletionText :: debug l=" + length + " l2=" + length2);
			if (length2 > length) textChange = new TextChange( new TextSpan( start, length2 ), newText );
Console.WriteLine("RoslynCompletionData.InsertCompletionText :: textChange s=" + textChange.Span.Start + " l=" + textChange.Span.Length + " nt='" + textChange.NewText + "'");
			editor.ApplyTextChanges ( new [] { textChange } );

		//oe	var currentBuffer = editor.GetPlatformTextBuffer ();
		//oe	var triggerSnapshotSpan = new SnapshotSpan (triggerSnapshot, new Span (textChange.Span.Start, textChange.Span.Length));
		//oe	var mappedSpan = triggerSnapshotSpan.TranslateTo (currentBuffer.CurrentSnapshot, SpanTrackingMode.EdgeInclusive);
		//oe	using (var undo = editor.OpenUndoGroup ()) {
		//oe		// Work around for https://github.com/dotnet/roslyn/issues/22885
		//oe		if (editor.GetCharAt (mappedSpan.Start) == '@') {
		//oe			editor.ReplaceText (mappedSpan.Start + 1, mappedSpan.Length - 1, completionChange.TextChange.NewText);
		//oe		} else
		//oe			editor.ReplaceText (mappedSpan.Start, mappedSpan.Length, completionChange.TextChange.NewText);

				if (completionChange.NewPosition.HasValue)
					editor.CaretOffset = completionChange.NewPosition.Value;

		//oe		if (CompletionItem.Rules.FormatOnCommit) {
		//oe			var endOffset = mappedSpan.Start.Position + completionChange.TextChange.NewText.Length;
		//oe			Format (editor, document, mappedSpan.Start, endOffset);
		//oe		}
		//oe	}

		}

		protected abstract void Format (TextEditor editor, Gui.Document document, int start, int end);

		public override async Task<TooltipInformation> CreateTooltipInformation (bool smartWrap, CancellationToken cancelToken)
		{
			var description = await completionService.GetDescriptionAsync (doc, CompletionItem);
			var markup = new StringBuilder ();
			var theme = SyntaxHighlightingService.GetIdeFittingTheme (DefaultSourceEditorOptions.Instance.GetEditorTheme ());
			var taggedParts = description.TaggedParts;
			int i = 0;
			while (i < taggedParts.Length) {
				if (taggedParts [i].Tag == "LineBreak")
					break;
				i++;
			}
			if (i + 1 >= taggedParts.Length) {
				markup.AppendTaggedText (theme, taggedParts);
			} else {
				markup.AppendTaggedText (theme, taggedParts.Take (i));
				markup.Append ("<span font='" + FontService.SansFontName + "' size='small'>");
				markup.AppendLine ();
				markup.AppendLine ();
				markup.AppendTaggedText (theme, taggedParts.Skip (i + 1));
				markup.Append ("</span>");
			}
			return new TooltipInformation {
				SignatureMarkup = markup.ToString ()
			};
		}
	}

}
