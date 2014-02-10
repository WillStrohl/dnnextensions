'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2009-2014, Will Strohl
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
'Neither the name of Will Strohl nor the names of its contributors may be 
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

Imports Newtonsoft.Json

Namespace WillStrohl.API.oEmbed

    ''' <summary>
    ''' ApplicationError - this class is used to help report errors that occur during oEmbed provider requests
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class ApplicationError

        ''' <summary>
        ''' GetAppliationErrorObject - this object is used to report errors from oEmbed requests
        ''' </summary>
        ''' <param name="ErrorMessage">String - the error message to relay to the calling function</param>
        ''' <returns>String - the error object, in the form of a nearly empty oEmbed object</returns>
        ''' <remarks>
        ''' Look in the "title" parameter of the oEmbed object to see the error.
        ''' </remarks>
        Public Shared Function GetAppliationErrorObject(ByVal ErrorMessage As String) As String
            Dim objOEmbed As New oEmbedInfo
            objOEmbed.Title = ErrorMessage
            Return JsonConvert.SerializeObject(objOEmbed)
        End Function

    End Class

End Namespace