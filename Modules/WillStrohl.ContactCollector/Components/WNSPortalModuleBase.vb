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

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Framework.JavaScriptLibraries

Namespace WillStrohl.Modules.ContactCollector

    Public MustInherit Class WNSPortalModuleBase
        Inherits PortalModuleBase

        Protected Overloads Function GetLocalizedString(ByVal LocalizationKey As String) As String
            If Not String.IsNullOrEmpty(LocalizationKey) Then
                Return Localization.GetString(LocalizationKey, Me.LocalResourceFile)
            Else
                Return String.Empty
            End If
        End Function

        Protected Overloads Function GetLocalizedString(ByVal LocalizationKey As String, ByVal LocalResourceFilePath As String) As String
            If Not String.IsNullOrEmpty(LocalizationKey) Then
                Return Localization.GetString(LocalizationKey, LocalResourceFilePath)
            Else
                Return String.Empty
            End If
        End Function

        Protected Function GetClientScriptBlock(ByVal Script As String) As String

            Dim strScriptBlock As String = "<script language=""javascript"" type=""text/javascript"">/*<![CDATA[*/ {0} /*]]*/></script>"

            If Not String.IsNullOrEmpty(Script) Then
                Return String.Format(strScriptBlock, Script)
            Else
                Return String.Empty
            End If

        End Function

        Protected Function GetClientScript(ByVal ScriptPath As String) As String

            Dim strScript As String = "<script language=""javascript"" type=""text/javascript"" src=""{0}""></script>"

            If Not String.IsNullOrEmpty(ScriptPath) Then
                Return String.Format(strScript, ScriptPath)
            Else
                Return String.Empty
            End If

        End Function

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            ' request that the DNN framework load the jQuery script into the markup
            JavaScript.RequestRegistration(CommonJs.DnnPlugins)

        End Sub

    End Class

End Namespace