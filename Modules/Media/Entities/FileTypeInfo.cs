//
// DNN Corp - http://www.dnnsoftware.com
// Copyright (c) 2002-2014
// by DNN Corp
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

using System;
using DotNetNuke.Common.Utilities;

namespace DotNetNuke.Modules.Media
{

	/// <summary>
	/// Represents a File Type object.
	/// </summary>
	[Serializable]
	public class FileTypeInfo : IFileTypeInfo
	{

#region  Fields 

		private string p_FileType = Null.NullString;
		private bool p_HostSupport = Null.NullBoolean;
		private bool p_ModuleSupport = Null.NullBoolean;

#endregion

#region  Initialization 

		public FileTypeInfo()
		{
			// do nothing
		}

		public FileTypeInfo(string FileType, bool ModuleSupport, bool HostSupport)
		{
			this.p_FileType = FileType;
			this.p_ModuleSupport = ModuleSupport;
			this.p_HostSupport = HostSupport;
		}

#endregion

#region  Properties 

		public string FileType
		{
			get
			{
				return this.p_FileType;
			}
			set
			{
				this.p_FileType = value;
			}
		}

		public bool HostSupport
		{
			get
			{
				return this.p_HostSupport;
			}
			set
			{
				this.p_HostSupport = value;
			}
		}

		public bool ModuleSupport
		{
			get
			{
				return this.p_ModuleSupport;
			}
			set
			{
				this.p_ModuleSupport = value;
			}
		}

#endregion

	}

}
