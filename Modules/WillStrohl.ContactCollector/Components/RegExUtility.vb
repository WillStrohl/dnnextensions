'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2015, Will Strohl
'All rights reserved.
'
'Redistribution and use in source and binary forms, with or without modification, are 
'permitted provided that the following conditions are met:
'
'Redistributions of source code must retain the above copyright notice, this list of 
'conditions and the following disclaimer.
'
'Redistributions in binary form must reproduce the above copyright notice, this list 
'of conditions and the following disclaimer in the documentation and/or other 
'materials provided with the distribution.
'
'Neither the name of Will Strohl, Contact Collector, nor the names of its contributors may be 
'used to endorse or promote products derived from this software without specific prior 
'written permission.
'
'THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
'EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
'OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
'SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
'INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
'TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR 
'BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
'CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
'ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH 
'DAMAGE.
'

Imports System.Text.RegularExpressions

Namespace WillStrohl.Modules.ContactCollector

    Public NotInheritable Class RegExUtility

#Region " Constants "

        Private Const POSITIVE_ONLY_PATTERN As String = "^\d+(\.\d+)*?$"
        Private Const NEGATIVE_ALLOWED_PATTERN As String = "^\-*\d+(\.\d+)*?$"
        Private Const BOOLEAN_PATTERN As String = "^(1|0|true|false)$"

#End Region

        ''' <summary>
        ''' IsNumber - this method uses a regular expression to determine if the value object is in a valid numeric format.
        ''' </summary>
        ''' <param name="Value">Object - the object to parse to see if it's a number</param>
        ''' <returns>If true, the Value object was in a valid numeric format</returns>
        ''' <remarks>
        ''' This method does not consider commas (,) to be a valid character. This overload defaults PositiveOnly to True.
        ''' </remarks>
        ''' <history>
        ''' [wstrohl] - 20100130 - created
        ''' </history>
        Public Overloads Shared Function IsNumber(ByVal Value As Object) As Boolean
            Return IsNumber(Value, True)
        End Function

        ''' <summary>
        ''' IsNumber - this method uses a regular expression to determine if the value object is in a valid numeric format.
        ''' </summary>
        ''' <param name="Value">Object - the object to parse to see if it's a number</param>
        ''' <param name="PositiveOnly">Boolean - if true, a negative number will be considered valid</param>
        ''' <returns>If true, the Value object was in a valid numeric format</returns>
        ''' <remarks>
        ''' This method does not consider commas (,) to be a valid character.
        ''' </remarks>
        ''' <history>
        ''' [wstrohl] - 20100130 - created
        ''' </history>
        Public Overloads Shared Function IsNumber(ByVal Value As Object, ByVal PositiveOnly As Boolean) As Boolean

            If Value Is Nothing Then
                Return False
            End If

            If PositiveOnly Then
                Return Regex.IsMatch(Value.ToString, POSITIVE_ONLY_PATTERN)
            Else
                Return Regex.IsMatch(Value.ToString, NEGATIVE_ALLOWED_PATTERN)
            End If

        End Function

        ''' <summary>
        ''' IsBoolean - this method uses a regular expression to determine if the value object is in a valid boolean format.
        ''' </summary>
        ''' <param name="Value">Object - the object to parse to see if it is in a boolean fomat</param>
        ''' <returns>If true, the Value object was in a valid boolean format</returns>
        ''' <remarks>
        ''' This method looks for one of the following: 1, 0, true, false (case insensitive)
        ''' </remarks>
        ''' <history>
        ''' [wstrohl] - 20100130 - created
        ''' </history>
        Public Overloads Shared Function IsBoolean(ByVal Value As Object) As Boolean

            If Value Is Nothing Then
                Return False
            End If

            Return Regex.IsMatch(Value.ToString, BOOLEAN_PATTERN, RegexOptions.IgnoreCase)

        End Function

    End Class

End Namespace