'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2013, Will Strohl
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

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Controllers
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml

Namespace WillStrohl.Modules.ContactCollector

    Public NotInheritable Class ContactCollectorController
        Implements Entities.Modules.IPortable

#Region " Constants "

        Public Const SETTING_INCLUDE_COMMENT As String = "IncludeComment"
        Public Const SETTING_USECAPTCHA As String = "UseCaptcha"
        Public Const SETTING_SENDEMAILTOCONTACT As String = "SendEmailToContact"
        Public Const SETTING_SENDEMAILTOADMIN As String = "SendEmailToAdmin"
        Public Const SETTING_ADMINEMAIL As String = "AdministratorEmail"

#End Region

        Public Sub SendMail(ByVal FromEmail As String, ByVal ToEmail As String, ByVal Subject As String, ByVal Body As String)

            Dim settingsDictionary As Dictionary(Of String, String)
            settingsDictionary = HostController.Instance().GetSettingsDictionary

            Dim blnSsl As Boolean = String.Equals(settingsDictionary("SMTPEnableSSL").ToString, "Y")

            Services.Mail.Mail.SendMail(FromEmail, ToEmail, Null.NullString, _
                Null.NullString, Services.Mail.MailPriority.Normal, _
                Subject, Services.Mail.MailFormat.Html, _
                System.Text.Encoding.UTF8, Body, New String() {}, settingsDictionary("SMTPServer").ToString, _
                settingsDictionary("SMTPAuthentication").ToString, settingsDictionary("SMTPUsername").ToString, _
                settingsDictionary("SMTPPassword").ToString, blnSsl)

        End Sub

#Region " Export Data "

        Public Sub ExportData(ByVal ModuleId As Integer, ByVal ObjectName As String, ByVal objExportType As ExportType)
            Dim dtResult As DataTable = GetContactsDataTable(ModuleId)
            Dim ResultName As String = String.Format("{0}_{1}", ObjectName, DateTime.Now.Ticks)

            Select Case objExportType
                Case ExportType.Excel
                    ExportToExcel(dtResult, ResultName)
                Case ExportType.XML
                    ExportToXML(dtResult, ResultName)
                Case ExportType.CSV
                    ExportToCSV(dtResult, ResultName)
            End Select
        End Sub

        Private Sub ExportToExcel(ByVal dtSource As DataTable, ByVal FileName As String)
            Dim objDoc As XmlDocument = New XmlDocument()

            Dim objInstructionXml As XmlProcessingInstruction = objDoc.CreateProcessingInstruction("xml", " version=""1.0"" encoding=""utf-8""")
            objDoc.AppendChild(objInstructionXml)

            Dim objInstruction As XmlProcessingInstruction = objDoc.CreateProcessingInstruction("mso-application", "progid=""Excel.Sheet""")
            objDoc.AppendChild(objInstruction)

            Dim objRoot As XmlElement = objDoc.CreateElement("Workbook", "urn:schemas-microsoft-com:office:spreadsheet")

            Dim objHtml As XmlAttribute = objDoc.CreateAttribute("xmlns:html")
            objHtml.Value = "http://www.w3.org/TR/REC-html40"
            objRoot.Attributes.Append(objHtml)

            Dim objO As XmlAttribute = objDoc.CreateAttribute("xmlns:o")
            objO.Value = "urn:schemas-microsoft-com:office:office"
            objRoot.Attributes.Append(objO)

            Dim objX As XmlAttribute = objDoc.CreateAttribute("xmlns:x")
            objX.Value = "urn:schemas-microsoft-com:office:excel"
            objRoot.Attributes.Append(objX)

            Dim objMs As XmlAttribute = objDoc.CreateAttribute("xmlns:ms")
            objMs.Value = "urn:schemas-microsoft-com:xslt"
            objRoot.Attributes.Append(objMs)

            Dim objSs As XmlAttribute = objDoc.CreateAttribute("xmlns:ss")
            objSs.Value = "urn:schemas-microsoft-com:office:spreadsheet"
            objRoot.Attributes.Append(objSs)

            Dim objWorksheet As XmlElement = objDoc.CreateElement("Worksheet", "urn:schemas-microsoft-com:office:spreadsheet")
            Dim objWorksheetName As XmlAttribute = objDoc.CreateAttribute(Nothing, "Name", "urn:schemas-microsoft-com:office:spreadsheet")
            If (FileName.Length > 30) Then
                objWorksheetName.Value = FileName.Substring(0, 30)
            Else
                objWorksheetName.Value = FileName
            End If
            objWorksheet.Attributes.Append(objWorksheetName)

            Dim objTable As XmlElement = objDoc.CreateElement("Table", "urn:schemas-microsoft-com:office:spreadsheet")

            ' Header
            Dim objTitleRow As XmlElement = objDoc.CreateElement("Row", "urn:schemas-microsoft-com:office:spreadsheet")
            For Each dc As DataColumn In dtSource.Columns
                Dim objTitleCell As XmlElement = objDoc.CreateElement("Cell", "urn:schemas-microsoft-com:office:spreadsheet")

                Dim objTitleData As XmlElement = objDoc.CreateElement("Data", "urn:schemas-microsoft-com:office:spreadsheet")
                Dim objTitleDataType As XmlAttribute = objDoc.CreateAttribute(Nothing, "Type", "urn:schemas-microsoft-com:office:spreadsheet")
                objTitleDataType.Value = "String"
                objTitleData.InnerXml = dc.ColumnName
                objTitleData.Attributes.Append(objTitleDataType)

                objTitleCell.AppendChild(objTitleData)
                objTitleRow.AppendChild(objTitleCell)
            Next
            objTable.AppendChild(objTitleRow)

            ' Data
            For Each dr As DataRow In dtSource.Rows
                Try
                    Dim objRow As XmlElement = objDoc.CreateElement("Row", "urn:schemas-microsoft-com:office:spreadsheet")
                    For Each dc As DataColumn In dtSource.Columns
                        Dim objCell As XmlElement = objDoc.CreateElement("Cell", "urn:schemas-microsoft-com:office:spreadsheet")
                        Dim objData As XmlElement = objDoc.CreateElement("Data", "urn:schemas-microsoft-com:office:spreadsheet")
                        Dim objDataType As XmlAttribute = objDoc.CreateAttribute(Nothing, "Type", "urn:schemas-microsoft-com:office:spreadsheet")
                        objDataType.Value = "String"
                        objData.InnerXml = String.Format("{0}", dr(dc.ColumnName))
                        objData.Attributes.Append(objDataType)

                        objCell.AppendChild(objData)
                        objRow.AppendChild(objCell)
                    Next
                    objTable.AppendChild(objRow)
                Catch
                    ' do nothing
                End Try
            Next

            objWorksheet.AppendChild(objTable)
            objRoot.AppendChild(objWorksheet)
            objDoc.AppendChild(objRoot)

            ResponseWrite(objDoc.InnerXml, String.Format("{0}.xls", FileName), "application/excel")
        End Sub

        Private Sub ExportToXML(ByVal dtSource As DataTable, ByVal FileName As String)
            Dim objDoc As XmlDocument = New XmlDocument
            Dim objRoot As XmlElement = objDoc.CreateElement("root")

            For i As Integer = 0 To dtSource.Rows.Count - 1
                Try
                    Dim objItem As XmlElement = objDoc.CreateElement(FileName)
                    Dim dr As DataRow = dtSource.Rows(i)
                    For j As Integer = 0 To dtSource.Columns.Count - 1
                        Dim objAttr As XmlAttribute = objDoc.CreateAttribute(dtSource.Columns(j).ColumnName)
                        objAttr.Value = String.Format("{0}", dr(j))
                        objItem.Attributes.Append(objAttr)
                    Next
                    objRoot.AppendChild(objItem)
                Catch
                    ' do nothing
                End Try
            Next
            objDoc.AppendChild(objRoot)

            ResponseWrite(objDoc.InnerXml, String.Format("{0}.xml", FileName), "text/xml")
        End Sub

        Private Function FixCSVString(ByVal Source As String) As String
            'return Source.Replace("\"", "\"\"");
            Return Source.Replace("""", """""")
        End Function

        Private Sub ExportToCSV(ByVal dtSource As DataTable, ByVal FileName As String)
            Dim Row As String = String.Empty
            For Each dc As DataColumn In dtSource.Columns
                If String.IsNullOrEmpty(Row) Then
                    Row = String.Format("{0}{2}""{1}""", _
                        Row, _
                        FixCSVString(dc.ColumnName), _
                        String.Empty)
                Else
                    Row = String.Format("{0}{2}""{1}""", _
                        Row, _
                        FixCSVString(dc.ColumnName), _
                        ",")
                End If
            Next
            Dim Result As String = String.Format("{0}{1}", Row, vbCrLf)

            For j As Integer = 0 To dtSource.Columns.Count - 1
                Try
                    Dim dr As DataRow = dtSource.Rows(j)
                    Row = String.Empty
                    For k As Integer = 0 To dtSource.Columns.Count - 1
                        If String.IsNullOrEmpty(Row) Then
                            Row = String.Format("{0}{2}""{1}""", _
                                Row, _
                                FixCSVString(String.Format("{0}", dr(k))), _
                                String.Empty)
                        Else
                            Row = String.Format("{0}{2}""{1}""", _
                               Row, _
                               FixCSVString(String.Format("{0}", dr(k))), _
                               ",")
                        End If
                    Next
                    If Not String.IsNullOrEmpty(Row.Trim) Then
                        If Row.Contains(vbCrLf) Then
                            Result = String.Concat(Result, String.Format("{0}", Row))
                        Else
                            Result = String.Concat(Result, String.Format("{0}{1}", Row, vbCrLf))
                        End If
                    End If
                Catch
                    ' do nothing
                End Try
            Next

            ResponseWrite(Result, String.Format("{0}.csv", FileName), "text/csv")
        End Sub

        Private Sub ResponseWrite(ByVal Result As String, ByVal FileName As String, ByVal ContentType As String)
            Dim Response As System.Web.HttpResponse = System.Web.HttpContext.Current.Response

            Dim lstByte As Byte() = System.Text.Encoding.UTF8.GetBytes(Result)

            Response.ClearHeaders()
            Response.ClearContent()
            Response.ContentType = String.Format("{0}; charset=utf-8", ContentType)
            Response.AppendHeader("Content-disposition", String.Format("attachment; filename=""{0}""", FileName))
            Response.AppendHeader("Content-Length", lstByte.Length.ToString)
            Response.BinaryWrite(lstByte)
            Response.Flush()
            Response.End()
        End Sub

        Public Enum SourceOfData
            Table = 0
            View = 1
            Query = 2
        End Enum

        Public Enum ExportType
            Excel = 0
            XML = 1
            CSV = 2
        End Enum

#End Region

#Region " DAL Methods "

        Public Overloads Function AddContact(ByVal ModuleId As Integer, ByVal FirstName As String, ByVal LastName As String, ByVal EmailAddress As String, ByVal IsActive As Boolean, ByVal Comment As String) As Integer
            Return DataProvider.Instance().AddContact(ModuleId, FirstName, LastName, EmailAddress, IsActive, Comment)
        End Function

        Public Overloads Function AddContact(ByVal Contact As ContactInfo) As Integer
            Return AddContact(Contact.ModuleId, Contact.FirstName, Contact.LastName, _
                Contact.EmailAddress, Contact.IsActive, Contact.Comment)
        End Function

        Public Sub DeleteContact(ByVal ContactId As Integer)
            DataProvider.Instance().DeleteContact(ContactId)
        End Sub

        Public Function GetContact(ByVal ContactId As Integer) As ContactInfo
            Dim collContact As New ContactInfoCollection
            collContact.Fill(DataProvider.Instance().GetContact(ContactId))

            If collContact.Count > 0 Then
                Return collContact(0)
            Else
                Return Nothing
            End If
        End Function

        Public Overloads Function GetContacts(ByVal ModuleId As Integer) As ContactInfoCollection
            Dim collContact As New ContactInfoCollection
            collContact.Fill(DataProvider.Instance().GetContacts(ModuleId))

            If collContact.Count > 0 Then
                Return collContact
            Else
                Return Nothing
            End If
        End Function

        Public Overloads Function GetContactsDataTable(ByVal ModuleId As Integer) As DataTable
            Dim dtContact As New DataTable
            dtContact.Load(DataProvider.Instance().GetContacts(ModuleId))

            If dtContact.Rows.Count > 0 Then
                Return dtContact
            Else
                Return Nothing
            End If
        End Function

        Public Sub UpdateContact(ByVal ContactId As Integer, ByVal ModuleId As Integer, ByVal FirstName As String, ByVal LastName As String, ByVal EmailAddress As String, ByVal IsActive As Boolean, ByVal Comment As String)
            DataProvider.Instance().UpdateContact(ContactId, ModuleId, FirstName, LastName, EmailAddress, IsActive, Comment)
        End Sub

        Public Function GetEmailTemplate(ByVal ModuleId As Integer) As EmailTemplateInfo
            Dim objTemplate As New EmailTemplateInfo
            objTemplate.Fill(DataProvider.Instance().GetEmailTemplate(ModuleId))
            Return objTemplate
        End Function

        Public Sub UpdateEmailTemplate(ByVal EmailId As Integer, ByVal ModuleId As Integer, ByVal ContactSubject As String, ByVal ContactTemplate As String, ByVal AdminSubject As String, ByVal AdminTemplate As String)
            DataProvider.Instance().UpdateEmailTemplate(EmailId, ModuleId, ContactSubject, ContactTemplate, AdminSubject, AdminTemplate)
        End Sub

#End Region

#Region " IPortable Implementation "

        ''' <summary>
        ''' This method generates and returns XML output matching the module data for the module instance
        ''' </summary>
        ''' <param name="ModuleID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements DotNetNuke.Entities.Modules.IPortable.ExportModule
            Dim sb As New StringBuilder(100)
            Dim collContacts As ContactInfoCollection
            collContacts = GetContacts(ModuleID)
            Dim objEmail As New EmailTemplateInfo
            objEmail = GetEmailTemplate(ModuleID)
            Dim ctlModule As New Entities.Modules.ModuleController
            Dim mSettings As Hashtable = ctlModule.GetModuleSettings(ModuleID)
            Dim eItem As IDictionaryEnumerator = mSettings.GetEnumerator

            With sb
                .Append("<WillStrohl.ContactCollector>")
                ' iterate through the settings and add them to the output XML
                If mSettings.Count > 0 Then
                    .Append("<settings>")
                    While eItem.MoveNext
                        .Append("<setting>")
                        .AppendFormat("<key>{0}</key>", XmlUtils.XMLEncode(eItem.Key.ToString))
                        .AppendFormat("<value>{0}</value>", XmlUtils.XMLEncode(eItem.Value.ToString))
                        .Append("</setting>")
                    End While
                    .Append("</settings>")
                End If
                ' iterate through the collected contacts and add them to the output XML
                If collContacts.Count > 0 Then
                    .Append("<contacts>")
                    For Each item As ContactInfo In collContacts
                        .Append("<contact>")
                        .AppendFormat("<contactId>{0}</contactId>", XmlUtils.XMLEncode(item.ContactId.ToString))
                        .AppendFormat("<firstName>{0}</firstName>", XmlUtils.XMLEncode(item.FirstName))
                        .AppendFormat("<lastName>{0}</lastName>", XmlUtils.XMLEncode(item.LastName))
                        .AppendFormat("<emailAddress>{0}</emailAddress>", XmlUtils.XMLEncode(item.EmailAddress))
                        .AppendFormat("<comment>{0}</comment>", XmlUtils.XMLEncode(item.Comment))
                        .AppendFormat("<isActive>{0}</isActive>", XmlUtils.XMLEncode(item.IsActive.ToString))
                        .Append("</contact>")
                    Next
                    .Append("</contacts>")
                End If
                ' iterate through the collected contacts and add them to the output XML
                If Not objEmail Is Nothing AndAlso objEmail.EmailId > Null.NullInteger Then
                    .Append("<emailTemplate>")
                    .AppendFormat("<AdminSubject>{0}</AdminSubject>", XmlUtils.XMLEncode(objEmail.AdminSubject))
                    .AppendFormat("<AdminTemplate>{0}</AdminTemplate>", XmlUtils.XMLEncode(objEmail.AdminTemplate))
                    .AppendFormat("<ContactSubject>{0}</ContactSubject>", XmlUtils.XMLEncode(objEmail.ContactSubject))
                    .AppendFormat("<ContactTemplate>{0}</ContactTemplate>", XmlUtils.XMLEncode(objEmail.ContactTemplate))
                    .AppendFormat("<EmailId>{0}</EmailId>", XmlUtils.XMLEncode(objEmail.EmailId.ToString))
                    .AppendFormat("<ModuleId>{0}</ModuleId>", XmlUtils.XMLEncode(objEmail.ModuleId.ToString))
                    .Append("</emailTemplate>")
                End If
                .Append("</WillStrohl.ContactCollector>")
            End With

            Return sb.ToString
        End Function

        ''' <summary>
        ''' This module imports module content using the XML passed to it
        ''' </summary>
        ''' <param name="ModuleID"></param>
        ''' <param name="Content"></param>
        ''' <param name="Version"></param>
        ''' <param name="UserID"></param>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserID As Integer) Implements DotNetNuke.Entities.Modules.IPortable.ImportModule
            Try
                Dim doc As New XmlDocument
                doc.LoadXml(Content)
                Dim cNode As XmlNode = Nothing
                Dim ctlModule As New Entities.Modules.ModuleController

                For Each cNode In doc.SelectNodes("//setting")
                    ctlModule.UpdateModuleSetting(ModuleID, cNode.SelectSingleNode("key").InnerText, cNode.SelectSingleNode("value").InnerText)
                Next
                Entities.Modules.ModuleController.SynchronizeModule(ModuleID)

                For Each cNode In doc.SelectNodes("//contact")
                    Dim iContact As New ContactInfo
                    With iContact
                        .ModuleId = ModuleID
                        .FirstName = cNode.SelectSingleNode("firstName").InnerText
                        .LastName = cNode.SelectSingleNode("lastName").InnerText
                        .EmailAddress = cNode.SelectSingleNode("emailAddress").InnerText

                        If Not cNode.SelectSingleNode("comment") Is Nothing Then
                            .Comment = cNode.SelectSingleNode("comment").InnerText
                        End If

                        .IsActive = Boolean.Parse(cNode.SelectSingleNode("isActive").InnerText)
                    End With
                    AddContact(iContact)
                Next

                cNode = doc.SelectSingleNode("/emailTemplate")
                If Not cNode Is Nothing Then
                    Dim objEmail As New EmailTemplateInfo
                    Dim child As XmlNode

                    child = cNode.SelectSingleNode("/AdminSubject")
                    If Not child Is Nothing Then
                        objEmail.AdminSubject = child.InnerText
                    End If

                    child = cNode.SelectSingleNode("/AdminTemplate")
                    If Not child Is Nothing Then
                        objEmail.AdminTemplate = child.InnerText
                    End If

                    child = cNode.SelectSingleNode("/ContactSubject")
                    If Not child Is Nothing Then
                        objEmail.ContactSubject = child.InnerText
                    End If

                    child = cNode.SelectSingleNode("/ContactTemplate")
                    If Not child Is Nothing Then
                        objEmail.ContactTemplate = child.InnerText
                    End If

                    objEmail.ModuleId = ModuleID

                    UpdateEmailTemplate(Null.NullInteger, objEmail.ModuleId, objEmail.ContactSubject, _
                        objEmail.ContactSubject, objEmail.AdminSubject, objEmail.AdminTemplate)
                End If

            Catch exc As Exception
                Services.Exceptions.LogException(exc)
            End Try
        End Sub

#End Region

    End Class

End Namespace