
this version is based on commit 24703023b6036b87005daa33e0397608f45679b0 dated 2017-02-28.

Makefile 
	recursive operations fail sometimes, add a patch from later MD versions (some quotes added).

main/src/core/MonoDevelop.Core/MonoDevelop.Projects.MSBuild/MSBuildProjectService.cs 
	some problems to find MSBuild.exe file(s) as required.
	apparently the toolsVersion value can be higher as expected.
	add a fallback mechanism (similar existed in MD v5.10) to switch 15.0 -> 14.0 toolsVersion.
	also added extra logging for GetExeLocation() method.

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

