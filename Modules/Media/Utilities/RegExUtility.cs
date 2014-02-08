//
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2013
// by DotNetNuke Corporation
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

//INSTANT C# NOTE: Formerly VB project-level imports:
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;

using System.Text.RegularExpressions;

namespace DotNetNuke.Modules.Media
{

	public sealed class RegExUtility
	{

#region  Constants 

		private const string POSITIVE_ONLY_PATTERN = "^\\d+(\\.\\d+)*?$";
		private const string NEGATIVE_ALLOWED_PATTERN = "^\\-*\\d+(\\.\\d+)*?$";
		private const string BOOLEAN_PATTERN = "^(1|0|true|false)$";

#endregion

		/// <summary>
		/// IsNumber - this method uses a regular expression to determine if the value object is in a valid numeric format.
		/// </summary>
		/// <param name="Value">Object - the object to parse to see if it's a number</param>
		/// <returns>If true, the Value object was in a valid numeric format</returns>
		/// <remarks>
		/// This method does not consider commas (,) to be a valid character. This overload defaults PositiveOnly to True.
		/// </remarks>
		/// <history>
		/// [wstrohl] - 20100130 - created
		/// </history>
		public static bool IsNumber(object Value)
		{
			return IsNumber(Value, true);
		}

		/// <summary>
		/// IsNumber - this method uses a regular expression to determine if the value object is in a valid numeric format.
		/// </summary>
		/// <param name="Value">Object - the object to parse to see if it's a number</param>
		/// <param name="PositiveOnly">Boolean - if true, a negative number will be considered valid</param>
		/// <returns>If true, the Value object was in a valid numeric format</returns>
		/// <remarks>
		/// This method does not consider commas (,) to be a valid character.
		/// </remarks>
		/// <history>
		/// [wstrohl] - 20100130 - created
		/// </history>
		public static bool IsNumber(object Value, bool PositiveOnly)
		{

			if (Value == null)
			{
				return false;
			}

			if (PositiveOnly)
			{
				return Regex.IsMatch(Value.ToString(), POSITIVE_ONLY_PATTERN);
			}
			else
			{
				return Regex.IsMatch(Value.ToString(), NEGATIVE_ALLOWED_PATTERN);
			}

		}

		/// <summary>
		/// IsBoolean - this method uses a regular expression to determine if the value object is in a valid boolean format.
		/// </summary>
		/// <param name="Value">Object - the object to parse to see if it is in a boolean fomat</param>
		/// <returns>If true, the Value object was in a valid boolean format</returns>
		/// <remarks>
		/// This method looks for one of the following: 1, 0, true, false (case insensitive)
		/// </remarks>
		/// <history>
		/// [wstrohl] - 20100130 - created
		/// </history>
		public static bool IsBoolean(object Value)
		{

			if (Value == null)
			{
				return false;
			}

			return Regex.IsMatch(Value.ToString(), BOOLEAN_PATTERN, RegexOptions.IgnoreCase);

		}

	}

}