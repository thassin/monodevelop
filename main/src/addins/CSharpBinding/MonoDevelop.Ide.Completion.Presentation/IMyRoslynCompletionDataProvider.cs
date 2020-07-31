using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
//using Microsoft.VisualStudio.Text; oe removed...

namespace MonoDevelop.Ide.Completion.Presentation
{
	public interface IMyRoslynCompletionDataProvider
	{
	// oe TODO parameter removed -- need to to replace it??? also see RoslynCompletionData.
	//oe	MyRoslynCompletionData CreateCompletionData (Document document, ITextSnapshot textSnapshtot, CompletionService completionService, CompletionItem item);
		MyRoslynCompletionData CreateCompletionData (Document document, /* ITextSnapshot textSnapshtot, */ CompletionService completionService, CompletionItem item);
	}
}
