'
' Copyright Upendo Ventures, LLC
' https://upendoventures.com
' 
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software
' and associated documentation files (the "Software"), to deal in the Software without restriction,
' including without limitation the rights to use, copy, modify, merge, publish, distribute,
' sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
' 
' The above copyright notice and this permission notice shall be included in all copies or
' substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
' NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF Or IN CONNECTION WITH THE SOFTWARE Or THE USE Or OTHER DEALINGS IN THE SOFTWARE.
'

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Framework.JavaScriptLibraries

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