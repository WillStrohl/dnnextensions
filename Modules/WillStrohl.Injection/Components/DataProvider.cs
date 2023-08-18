/*
Copyright © Upendo Ventures, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial 
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT 
NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES 
OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

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