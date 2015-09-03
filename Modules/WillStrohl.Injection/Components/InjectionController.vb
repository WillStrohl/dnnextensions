'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2009-2015, Will Strohl
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
'Neither the name of Will Strohl, Content Injection, nor the names of its contributors may be 
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

Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules

Namespace WillStrohl.Modules.Injection

    Public NotInheritable Class InjectionController
        Implements IPortable

#Region " Data Access "

        Public Function AddInjectionContent(ByVal objInjection As InjectionInfo) As Integer
            Return CType(DataProvider.Instance().AddInjectionContent(objInjection.ModuleId, objInjection.InjectTop, objInjection.InjectName, objInjection.InjectContent, objInjection.IsEnabled, objInjection.OrderShown), Integer)
        End Function
        Public Sub UpdateInjectionContent(ByVal objInjection As InjectionInfo)
            DataProvider.Instance().UpdateInjectionContent(objInjection.InjectionId, objInjection.ModuleId, objInjection.InjectTop, objInjection.InjectName, objInjection.InjectContent, objInjection.IsEnabled, objInjection.OrderShown)
        End Sub
        Public Sub DisableInjectionContent(ByVal InjectionContentId As Integer)
            DataProvider.Instance().DisableInjectionContent(InjectionContentId)
        End Sub
        Public Sub EnableInjectionContent(ByVal InjectionContentId As Integer)
            DataProvider.Instance().EnableInjectionContent(InjectionContentId)
        End Sub
        Public Sub DeleteInjectionContent(ByVal InjectionContentId As Integer)
            DataProvider.Instance().DeleteInjectionContent(InjectionContentId)
        End Sub
        Public Function GetInjectionContent(ByVal InjectionContentId As Integer) As InjectionInfo
            Dim objInj As New InjectionInfo
            objInj.Fill(DataProvider.Instance().GetInjectionContent(InjectionContentId))
            Return objInj
        End Function
        Public Function GetActiveInjectionContents(ByVal ModuleId As Integer) As InjectionInfoCollection
            Dim collInj As New InjectionInfoCollection
            collInj.Fill(DataProvider.Instance().GetActiveInjectionContents(ModuleId))
            Return collInj
        End Function
        Public Function GetInjectionContents(ByVal ModuleId As Integer) As InjectionInfoCollection
            Dim collInj As New InjectionInfoCollection
            collInj.Fill(DataProvider.Instance().GetInjectionContents(ModuleId))
            Return collInj
        End Function
        Public Function GetNextOrderNumber(ByVal ModuleId As Integer) As Integer
            Return DataProvider.Instance().GetNextOrderNumber(ModuleId)
        End Function
        Public Sub ChangeOrder(ByVal InjectionId As Integer, ByVal Direction As String)
            If Not Regex.IsMatch(Direction, "^(moveup|movedown)$", RegexOptions.IgnoreCase) Then
                Throw New ArgumentOutOfRangeException("Direction")
            End If

            DataProvider.Instance().ChangeOrder(InjectionId, Direction)
        End Sub
        Public Function DoesInjectionNameExist(ByVal InjectionName As String, ByVal ModuleId As Integer) As Boolean
            Return DataProvider.Instance().DoesInjectionNameExist(InjectionName, ModuleId)
        End Function

#End Region

#Region " IPortable Implementation "

        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements DotNetNuke.Entities.Modules.IPortable.ExportModule
            Dim sb As New StringBuilder(150)
            Dim collInj As New InjectionInfoCollection
            collInj = GetInjectionContents(ModuleID)

            sb.Append("<WillStrohl><injectionContents>")
            For Each obj As InjectionInfo In collInj
                sb.Append("<injectionContent>")
                sb.AppendFormat("<injectionId>{0}</injectionId>", XmlUtils.XMLEncode(obj.InjectionId.ToString))
                sb.AppendFormat("<moduleId>{0}</moduleId>", XmlUtils.XMLEncode(obj.ModuleId.ToString))
                sb.AppendFormat("<injectTop>{0}</injectTop>", XmlUtils.XMLEncode(obj.InjectTop.ToString))
                sb.AppendFormat("<injectName>{0}</injectName>", XmlUtils.XMLEncode(obj.InjectName))
                sb.AppendFormat("<injectContent>{0}</injectContent>", XmlUtils.XMLEncode(obj.InjectContent))
                sb.AppendFormat("<isEnabled>{0}</isEnabled>", XmlUtils.XMLEncode(obj.IsEnabled.ToString))
                sb.AppendFormat("<orderShown>{0}</orderShown>", XmlUtils.XMLEncode(obj.OrderShown.ToString))
                sb.Append("</injectionContent>")
            Next
            sb.Append("</injectionContents>")
            ' later on, will probably need to add module settings here
            sb.Append("</WillStrohl>")

            Return sb.ToString
        End Function

        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserID As Integer) Implements DotNetNuke.Entities.Modules.IPortable.ImportModule

            Try

                Dim injContents As XmlNode = DotNetNuke.Common.Globals.GetContent(Content, "//injectionContents")

                For Each injContent As XmlNode In injContents.SelectNodes("//injectionContent")
                    Dim objInj As New InjectionInfo
                    objInj.ModuleId = ModuleID
                    If Not injContent.SelectSingleNode("injectTop") Is Nothing Then
                        objInj.InjectTop = Boolean.Parse(injContent.SelectSingleNode("injectTop").InnerText)
                    End If
                    If Not injContent.SelectSingleNode("injectName") Is Nothing Then
                        objInj.InjectName = injContent.SelectSingleNode("injectName").InnerText
                    End If
                    If Not injContent.SelectSingleNode("injectContent") Is Nothing Then
                        objInj.InjectContent = injContent.SelectSingleNode("injectContent").InnerText
                    End If
                    If Not injContent.SelectSingleNode("isEnabled") Is Nothing Then
                        objInj.IsEnabled = Boolean.Parse(injContent.SelectSingleNode("isEnabled").InnerText)
                    End If
                    If Not injContent.SelectSingleNode("orderShown") Is Nothing Then
                        objInj.OrderShown = Integer.Parse(injContent.SelectSingleNode("orderShown").InnerText, Globalization.NumberStyles.Integer)
                    End If
                    AddInjectionContent(objInj)
                Next

            Catch ex As Exception
                DotNetNuke.Services.Exceptions.LogException(ex)
            End Try

        End Sub

#End Region

    End Class

End Namespace