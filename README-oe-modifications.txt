
this version is based on commit 3761b7424abc985823ad57ddc5d7e2b53c068851 dated 2017-07-07.

Makefile 
	recursive operations fail sometimes, add a patch from later MD versions (some quotes added).

main/src/core/MonoDevelop.Core/MonoDevelop.Projects.MSBuild/MSBuildProjectService.cs 
	some problems to find MSBuild.exe file(s) as required.
	apparently the toolsVersion value can be higher as expected.
	add a fallback mechanism (similar existed in MD v5.10) to switch 15.0 -> 14.0 toolsVersion.
	also added extra logging for GetExeLocation() method.

main/src/core/MonoDevelop.Core/packages.config 
	remove all dependencies <package id="Microsoft.VisualStudio...

main/src/core/Mono.TextEditor.Platform 
	remove the directory.

main/src/core/Mono.TextEditor.Shared/Mono.TextEditor/Actions/MiscActions.cs 
	remove line "using Microsoft.VisualStudio.Text.Implementation;" (unused).

main/src/core/Mono.TextEditor.Shared/Mono.TextEditor/Document/TextDocument.cs 
	remove the Microsoft.VisualStudio... references and revert the ones providing the old editor.
	TODO the method ApplyTextChanges() not yet implemented (use cases unknown so far).

main/src/core/Mono.TextEditor.Shared/Mono.TextEditor/TextLinkEditMode.cs
	revert the code related to old editor.

main/src/addins/MonoDevelop.SourceEditor2/MonoDevelop.SourceEditor.csproj 
	remove the Microsoft.VisualStudio... references and project items.

main/src/addins/MonoDevelop.SourceEditor2/MonoDevelop.SourceEditor/SourceEditorView.cs
	revert the code related to old editor.

main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Editor/TextEditor.cs
	remove a TextView property related to the new editor.

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

	NOTICE! the process seems to hang and keep running after closing the app.

