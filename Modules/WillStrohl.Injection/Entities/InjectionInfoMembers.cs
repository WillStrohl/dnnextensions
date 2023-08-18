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

namespace WillStrohl.Modules.Injection.Entities
{
	public sealed class InjectionInfoMembers
	{
		public const string InjectionIdField = "injection_id";
		public const string ModuleIdField = "module_id";
		public const string InjectTopField = "inject_top";
		public const string InjectNameField = "inject_name";
		public const string InjectContentField = "inject_content";
		public const string IsEnabledField = "is_enabled";
		public const string OrderShownField = "order_shown";
        public const string CustomPropertiesField = "custom_properties";

        // custom properties
	    public const string InjectionTypeField = "InjectionType";
        public const string LastUpdatedByField = "LastUpdatedBy";
        public const string LastUpdatedDateField = "LastUpdatedDate";
        public const string CrmPriorityField = "CrmPriority";
        public const string CrmProviderField = "CrmProvider";
	}
}