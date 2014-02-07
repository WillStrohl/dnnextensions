//----------------------------------------------------------------------------------------
//	Copyright © 2003 - 2011 Tangible Software Solutions Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class simulates the behavior of the classic VB 'IsDate' function.
//----------------------------------------------------------------------------------------
public static class SimulateIsDate
{
	public static bool IsDate(object expression)
	{
		if (expression == null)
			return false;

		System.DateTime testDate;
		return System.DateTime.TryParse(expression.ToString(), out testDate);
	}
}