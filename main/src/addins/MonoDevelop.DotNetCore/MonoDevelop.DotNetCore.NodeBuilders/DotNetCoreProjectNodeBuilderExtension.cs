﻿//
// DotNetCoreProjectNodeBuilderExtension.cs
//
// Author:
//       Matt Ward <matt.ward@xamarin.com>
//
// Copyright (c) 2017 Xamarin Inc. (http://xamarin.com)
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
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Tasks;
using MonoDevelop.Projects;

namespace MonoDevelop.DotNetCore.NodeBuilders
{
	class DotNetCoreProjectNodeBuilderExtension : NodeBuilderExtension
	{
		public override bool CanBuildNode (Type dataType)
		{
			return typeof (DotNetProject).IsAssignableFrom (dataType);
		}

		public override void BuildNode (ITreeBuilder treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			var project = dataObject as DotNetProject;
			if (project == null)
				return;

			var dotNetCoreProject = project.GetFlavor<DotNetCoreProjectExtension> ();
			if (dotNetCoreProject == null)
				return;

			if (dotNetCoreProject.HasSdk && !dotNetCoreProject.IsDotNetCoreSdkInstalled ()) {
				nodeInfo.StatusSeverity = TaskSeverity.Error;
				nodeInfo.StatusMessage = GetMessage (dotNetCoreProject);
			}
		}

		string GetMessage (DotNetCoreProjectExtension dotNetCoreProject)
		{
			if (dotNetCoreProject.IsUnsupportedDotNetCoreSdkInstalled ())
				return GettextCatalog.GetString ("The .NET Core SDK installed is not supported. Please install a more recent version. {0}", DotNetCoreNotInstalledDialog.DotNetCoreDownloadUrl);

			return GettextCatalog.GetString (".NET Core SDK is not installed. This is required to build .NET Core projects. {0}", DotNetCoreNotInstalledDialog.DotNetCoreDownloadUrl);
		}
	}
}
