//
// Will Strohl (will.strohl@gmail.com)
// http://www.willstrohl.com
//
//Copyright (c) 2009-2015, Will Strohl
//All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, are 
//permitted provided that the following conditions are met:
//
//Redistributions of source code must retain the above copyright notice, this list of 
//conditions and the following disclaimer.
//
//Redistributions in binary form must reproduce the above copyright notice, this list 
//of conditions and the following disclaimer in the documentation and/or other 
//materials provided with the distribution.
//
//Neither the name of Will Strohl, Content Injection, nor the names of its contributors may be 
//used to endorse or promote products derived from this software without specific prior 
//written permission.
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
//EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
//OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
//SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
//INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
//TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR 
//BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
//CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
//ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH 
//DAMAGE.
//

using DotNetNuke.Common.Utilities;
using System;
using System.Data;

namespace WillStrohl.Modules.Injection.Components
{
	public abstract class DataProvider
	{
		#region " Private Members "
        
		private const string c_AssemblyName = "WillStrohl.Modules.Injection.Components.SqlDataProvider, WillStrohl.Modules.Injection";

		#endregion

		#region " Shared/Static Methods "

		// singleton reference to the instantiated object 
        private static DataProvider objProvider = null;
		
        // constructor
		static DataProvider()
		{
			CreateProvider();
		}

		// dynamically create provider
		private static void CreateProvider()
		{
			if (objProvider != null) return;
		    
            var objectType = Type.GetType(c_AssemblyName);

		    objProvider = (DataProvider)Activator.CreateInstance(objectType);

		    DataCache.SetCache(objectType.FullName, objProvider);
		}

		// return the provider
		public static new DataProvider Instance()
		{
			if (objProvider == null) {
				CreateProvider();
			}
			return objProvider;
		}

		#endregion

		#region " Abstract Methods "

		public abstract int AddInjectionContent(int ModuleId, bool InjectTop, string InjectName, string InjectContent, bool IsEnabled, int OrderShown, string CustomProperties);
		public abstract void UpdateInjectionContent(int InjectionId, int ModuleId, bool InjectTop, string InjectName, string InjectContent, bool IsEnabled, int OrderShown, string CustomProperties);
		public abstract void DisableInjectionContent(int InjectionId);
		public abstract void EnableInjectionContent(int InjectionId);
		public abstract void DeleteInjectionContent(int InjectionId);
		public abstract IDataReader GetInjectionContent(int InjectionId);
		public abstract IDataReader GetActiveInjectionContents(int ModuleId);
		public abstract IDataReader GetInjectionContents(int ModuleId);
		public abstract int GetNextOrderNumber(int ModuleId);
		public abstract void ChangeOrder(int InjectionId, string Direction);
		public abstract bool DoesInjectionNameExist(string InjectionName, int ModuleId);

		#endregion
	}
}