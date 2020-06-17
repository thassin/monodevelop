
this version is based on commit 24703023b6036b87005daa33e0397608f45679b0 dated 2017-02-28.

Makefile 
	recursive operations fail sometimes, add a patch from later MD versions (some quotes added).

main/src/core/MonoDevelop.Core/MonoDevelop.Projects.MSBuild/MSBuildProjectService.cs 
	some problems to find MSBuild.exe file(s) as required.
	apparently the toolsVersion value can be higher as expected.
	add a fallback mechanism (similar existed in MD v5.10) to switch 15.0 -> 14.0 toolsVersion.
	also added extra logging for GetExeLocation() method.

