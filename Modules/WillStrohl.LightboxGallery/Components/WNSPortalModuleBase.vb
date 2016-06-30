'
' Lightbox Gallery Module for DotNetNuke
' Project Contributors - Will Strohl (http://www.WillStrohl.com), Armand Datema (http://www.schwingsoft.com)
'
'Copyright (c) 2009-2016, Will Strohl
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
'Neither the name of Will Strohl, Armand Datema, Lightbox Gallery, nor the names of its contributors may be 
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
Imports DotNetNuke.Framework.JavaScriptLibraries
Imports DotNetNuke.Services.Localization

Namespace WillStrohl.Modules.Lightbox

    Public MustInherit Class WnsPortalModuleBase
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

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            ' request that the DNN framework load the jQuery script into the markup
            JavaScript.RequestRegistration(CommonJs.DnnPlugins)

        End Sub

    End Class

End Namespace