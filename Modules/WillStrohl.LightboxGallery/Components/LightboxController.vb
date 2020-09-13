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
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Services.Search
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml.Linq

Imports WillStrohl.Modules.Lightbox.LightboxInfoMembers

Namespace WillStrohl.Modules.Lightbox

    Public NotInheritable Class LightboxController
        Implements IPortable, ISearchable
        
#Region " Constants "

        Public Const SCRIPT_TAG_FORMAT As String = "<script language=""javascript"" type=""text/javascript"" src=""{0}""></script>"
        Public Const STYLESHEET_TAG_FORMAT As String = "<link rel=""stylesheet"" type=""text/css"" href=""{0}"" />"
        Public Const IMAGE_THUMBNAIL_PATTERN As String = "-thumb$0"
        Public Const IMAGE_FILE_PATTERN As String = "\.(png|gif|jpg)$"
        Public Const FOLDER_NAME_MATCH_PATTERN As String = "^[\/]"
        Public Const FOLDER_NAME_REPLACE_PATTERN As String = "[\/](.*)"
        Public Const FOLDER_NAME_REPLACEMENT_PATTERN As String = "$1"
        Public Const THUMBNAIL_MATCH_PATTERN As String = "-thumb\.(png|gif|jpg)$"

        Private Const STANDARD_FOLDER_PROVIDER As String = "StandardFolderProvider"

#End Region

#Region " Data Access "

        Public Function AddLightbox(ByVal objLightbox As LightboxInfo) As Integer
            Return DataProvider.Instance().AddLightbox(objLightbox.ModuleId, objLightbox.GalleryName, objLightbox.GalleryDescription, objLightbox.GalleryFolder, objLightbox.DisplayOrder, objLightbox.HideTitleDescription, objLightbox.LastUpdatedBy)
        End Function
        Public Sub UpdateLightbox(ByVal objLightbox As LightboxInfo)
            DataProvider.Instance().UpdateLightbox(objLightbox.LightboxId, objLightbox.ModuleId, objLightbox.GalleryName, objLightbox.GalleryDescription, objLightbox.GalleryFolder, objLightbox.DisplayOrder, objLightbox.HideTitleDescription, objLightbox.LastUpdatedBy)
        End Sub
        Public Sub DeleteLightbox(ByVal LightboxId As Integer)
            DataProvider.Instance().DeleteLightbox(LightboxId)
        End Sub
        Public Function GetLightbox(ByVal LightboxId As Integer) As LightboxInfo

            Dim objInj As New LightboxInfo
            objInj.Fill(DataProvider.Instance().GetLightbox(LightboxId))
            Return objInj

        End Function
        Public Function GetLightboxes(ByVal ModuleId As Integer) As LightboxInfoCollection

            Dim collInj As New LightboxInfoCollection
            collInj.Fill(DataProvider.Instance().GetLightboxes(ModuleId))
            Return collInj

        End Function
        Public Function GetLightboxIds(ByVal ModuleId As Integer) As List(Of Integer)

            Dim arrId As New List(Of Integer)
            Dim rdr As IDataReader = DataProvider.Instance().GetLightboxIds(ModuleId)

            While rdr.Read
                arrId.Add(Integer.Parse(rdr.Item(LightboxIdField).ToString, Globalization.NumberStyles.Integer))
            End While

            Return arrId

        End Function
        ''' <summary>
        ''' GetLightboxCount - this method returns a total count of the albums per the given ModuleId.
        ''' </summary>
        ''' <param name="ModuleId">Integer - the ID number for the module</param>
        ''' <returns>Integer - the total number of albums for the module</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 20101013 - wstrohl - Added error handling for 0-albums use case
        ''' </history>
        Public Function GetLightboxCount(ByVal ModuleId As Integer) As Integer

            Dim intReturn As Integer = 0

            Try
                intReturn = DataProvider.Instance().GetLightboxCount(ModuleId)
            Catch ex As NullReferenceException
                ' do nothing - this happened because there were no albums
            End Try

            Return intReturn

        End Function
        Public Function DoesDisplayOrderNeedUpdate() As Boolean
            Return DataProvider.Instance().DoesDisplayOrderNeedUpdate
        End Function
        Public Sub UpdateDisplayOrder()
            DataProvider.Instance().UpdateDisplayOrder()
        End Sub


        Public Overloads Function AddSetting(ByVal Settings As SettingInfo) As Integer
            Return DataProvider.Instance().AddSetting(Settings.LightboxId, Settings.Padding, Settings.Margin, Settings.Opacity, Settings.Modal, Settings.Cyclic, Settings.OverlayShow, Settings.OverlayOpacity, Settings.OverlayColor, Settings.TitleShow, Settings.TitlePosition, Settings.Transition, Settings.Speed, Settings.ChangeSpeed, Settings.ShowCloseButton, Settings.ShowNavArrows, Settings.EnableEscapeButton, Settings.OnStart, Settings.OnCancel, Settings.OnComplete, Settings.OnCleanup, Settings.OnClosed)
        End Function
        Public Sub UpdateSetting(ByVal Settings As SettingInfo)
            DataProvider.Instance().UpdateSetting(Settings.SettingId, Settings.LightboxId, Settings.Padding, Settings.Margin, Settings.Opacity, Settings.Modal, Settings.Cyclic, Settings.OverlayShow, Settings.OverlayOpacity, Settings.OverlayColor, Settings.TitleShow, Settings.TitlePosition, Settings.Transition, Settings.Speed, Settings.ChangeSpeed, Settings.ShowCloseButton, Settings.ShowNavArrows, Settings.EnableEscapeButton, Settings.OnStart, Settings.OnCancel, Settings.OnComplete, Settings.OnCleanup, Settings.OnClosed)
        End Sub
        Public Sub DeleteSetting(ByVal SettingId As Integer)
            DataProvider.Instance().DeleteSetting(SettingId)
        End Sub
        Public Function GetSettings(ByVal LightboxId As Integer) As SettingInfo
            Dim objSetting As New SettingInfo
            objSetting.Fill(DataProvider.Instance().GetSettings(LightboxId))

            If Not objSetting Is Nothing AndAlso objSetting.SettingId > 0 Then
                Return objSetting
            Else
                Return Nothing
            End If
        End Function


        Public Function GetImages(ByVal LightboxId As Integer) As ImageInfoCollection
            Dim collImage As New ImageInfoCollection
            collImage.Fill(DataProvider.Instance().GetImages(LightboxId))
            Return collImage
        End Function
        Public Function GetImageById(ByVal ImageId As Integer) As ImageInfo
            Dim objImage As New ImageInfo
            objImage.Fill(DataProvider.Instance().GetImageById(ImageId))

            If Not objImage Is Nothing AndAlso objImage.ImageId > 0 Then
                Return objImage
            Else
                Return Nothing
            End If
        End Function
        Public Function GetImageByFileName(ByVal LightboxId As Integer, ByVal FileName As String) As ImageInfo
            Dim objImage As New ImageInfo
            objImage.Fill(DataProvider.Instance().GetImageByFileName(LightboxId, FileName))

            If Not objImage Is Nothing AndAlso objImage.ImageId > 0 Then
                Return objImage
            Else
                Return Nothing
            End If
        End Function
        Public Overloads Function AddImage(ByVal Image As ImageInfo) As Integer
            Return DataProvider.Instance().AddImage(Image.LightboxId, Image.FileName, Image.Title, Image.Description, Image.DisplayOrder, Image.LastUpdatedBy)
        End Function
        Public Overloads Function AddImage(ByVal LightboxId As Integer, ByVal FileName As String, ByVal Title As String, ByVal Description As String, ByVal DisplayOrder As Integer, ByVal LastUpdatedBy As Integer) As Integer
            Return DataProvider.Instance().AddImage(LightboxId, FileName, Title, Description, DisplayOrder, LastUpdatedBy)
        End Function
        Public Overloads Sub UpdateImage(ByVal Image As ImageInfo)
            DataProvider.Instance().UpdateImage(Image.ImageId, Image.LightboxId, Image.FileName, Image.Title, Image.Description, Image.DisplayOrder, Image.LastUpdatedBy)
        End Sub
        Public Overloads Sub UpdateImage(ByVal ImageId As Integer, ByVal LightboxId As Integer, ByVal FileName As String, ByVal Title As String, ByVal Description As String, ByVal DisplayOrder As Integer, ByVal LastUpdatedBy As Integer)
            DataProvider.Instance().UpdateImage(ImageId, LightboxId, FileName, Title, Description, DisplayOrder, LastUpdatedBy)
        End Sub
        Public Sub DeleteImageByFileName(ByVal LightboxId As Integer, ByVal FileName As String)
            DataProvider.Instance().DeleteImageByFileName(LightboxId, FileName)
        End Sub
        Public Sub DeleteImageById(ByVal ImageId As Integer)
            DataProvider.Instance().DeleteImageById(ImageId)
        End Sub
        Public Function GetImageCount(ByVal LightboxId As Integer) As Integer
            DataProvider.Instance().GetImageCount(LightboxId)
        End Function

#End Region

#Region " Path Helpers "

        ''' <summary>
        ''' ReformatUrlForWeb - this method performs the steps necessary to translate a local path to a virtual path
        ''' </summary>
        ''' <param name="Path">Path to parse</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ReformatUrlForWeb(ByVal Path As String) As String

            Return Path.Replace("\", "/")

        End Function

        ''' <summary>
        ''' ReformatPathForLocal - this method performs the steps necessary to translate a virtual path to a local path
        ''' </summary>
        ''' <param name="Path">Path to parse</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ReformatPathForLocal(ByVal Path As String) As String

            Return Path.Replace("/", "\")

        End Function

#End Region

#Region " Image Helpers "

        Public Function GetImageFromProvider(ByVal PortalId As Integer, ByVal FolderName As String, ByVal FileName As String) As IFileInfo

            Dim oFolder As IFolderInfo = FolderManager.Instance().GetFolder(PortalId, FolderName)
            Dim oImage As IFileInfo = FileManager.Instance().GetFile(oFolder, FileName)

            Return oImage

        End Function

        Public Function GetImageFromProvider(ByVal PortalId As Integer, ByVal Folder As IFolderInfo, ByVal FileName As String) As IFileInfo

            Dim oImage As IFileInfo = FileManager.Instance().GetFile(Folder, FileName)
            Return oImage

        End Function

        ''' <summary>
        ''' GetThumbnailImageName - Takes the original file name and returns the thumbnail equivalent
        ''' </summary>
        ''' <param name="File">Full or partial path, or just the file name that you want to translate</param>
        ''' <param name="Settings">Current portal settings with HttpContext</param>
        ''' <param name="ReplaceSlash">True, if you want to switch the black slashes to forward slashes</param>
        ''' <returns>String - the translated file name</returns>
        ''' <remarks></remarks>
        Public Shared Function GetThumbnailImageName(ByVal File As String, ByVal Settings As DotNetNuke.Entities.Portals.PortalSettings, ByVal ReplaceSlash As Boolean) As String
            If ReplaceSlash Then
                Return ReformatUrlForWeb(Regex.Replace(File, IMAGE_FILE_PATTERN, IMAGE_THUMBNAIL_PATTERN, RegexOptions.IgnoreCase).Replace(Settings.HomeDirectoryMapPath, String.Empty))
            Else
                Return Regex.Replace(File, IMAGE_FILE_PATTERN, IMAGE_THUMBNAIL_PATTERN, RegexOptions.IgnoreCase)
            End If
        End Function

        ''' <summary>
        ''' VerifiedFilesList - returns a string array of file names that match the modules supported file types
        ''' </summary>
        ''' <param name="Files">An array object of the file names</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function VerifiedFileList(ByVal Files As IEnumerable(Of IFileInfo)) As ArrayList
            Return VerifiedFileList(Files, False)
        End Function

        ''' <summary>
        ''' VerifiedFilesList - returns a string array of file names that match the modules supported file types
        ''' </summary>
        ''' <param name="Files">An array object of the file names</param>
        ''' <param name="ThumbnailsOnly">If true, only the thumbnails will be returned</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function VerifiedFileList(ByVal Files As IEnumerable(Of IFileInfo), ByVal ThumbnailsOnly As Boolean) As ArrayList
            Dim arrFiles As New ArrayList

            If Not ThumbnailsOnly Then

                For Each oFile As IFileInfo In Files
                    ' do not include non-image files in the list, also exclude thumbnails that have already been generated
                    If Regex.IsMatch(oFile.FileName, IMAGE_FILE_PATTERN, RegexOptions.IgnoreCase) AndAlso Not Regex.IsMatch(oFile.FileName, THUMBNAIL_MATCH_PATTERN, RegexOptions.IgnoreCase) Then
                        arrFiles.Add(oFile)
                    End If
                Next

            Else

                For Each oFile As IFileInfo In Files
                    ' do not include non-image files in the list, and only includes thumbnails
                    If Regex.IsMatch(oFile.FileName, THUMBNAIL_MATCH_PATTERN, RegexOptions.IgnoreCase) Then
                        arrFiles.Add(oFile)
                    End If
                Next

            End If

            Return arrFiles
        End Function

        ''' <summary>
        ''' GetImageFileUrl - this method returns the valid URL for any file, regardless to folder or folder provider in use
        ''' </summary>
        ''' <param name="Image">Fully loaded IFileInfo object</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' WARNING!!! This method can return exceptions. They should be caught and processed in the UI though.
        ''' </remarks>
        Public Function GetImageFileUrl(ByVal Image As IFileInfo) As String

            '*******************************************************'
            ' WARNING!!!
            ' This method can return exceptions. They should be 
            ' caught and processed in the UI though.
            '*******************************************************'
            Dim mapFolder As FolderMappingInfo = FolderMappingController.Instance.GetFolderMapping(Image.FolderMappingID)
            Dim basicUrl = FolderProvider.Instance(mapFolder.FolderProviderType).GetFileUrl(Image)
            '
            'Add to the URL a parameter containing the server side timestamp of the file.  This is used purely 
            'as a cache buster.  Makes it safe to use long duration cache headers with these files.
            '
            Dim lastModificationTime = FolderProvider.Instance(mapFolder.FolderProviderType).GetLastModificationTime(Image)
            Dim urlWithTimeStamp = basicUrl & "?" & "rev=" & lastModificationTime.ToString("yyyy-MM-dd-HH-mm-ss")

            Return urlWithTimeStamp

        End Function

        ''' <summary>
        ''' IsLocalFolder - this method allows you to determine if the file is in a local or remote (cloud) folder
        ''' </summary>
        ''' <param name="FileObject">Fully loaded IFileInfo object</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        Public Function IsLocalFolder(ByVal FileObject As IFileInfo) As Boolean

            '*******************************************************'
            ' WARNING!!!
            ' This method can return exceptions. They should be 
            ' caught and processed in the UI though.
            '*******************************************************'
            Dim mapFolder As FolderMappingInfo = FolderMappingController.Instance.GetFolderMapping(FileObject.FolderMappingID)
            Return String.Equals(mapFolder.FolderProviderType, STANDARD_FOLDER_PROVIDER)

        End Function

#End Region

#Region " IPortable Implementation "

        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements DotNetNuke.Entities.Modules.IPortable.ExportModule
            Dim sb As New StringBuilder(150)
            Dim collInj As New LightboxInfoCollection
            collInj = GetLightboxes(ModuleID)
            Dim objSetting As New SettingInfo

            sb.Append("<WillStrohl><Lightboxes>")
            For Each obj As LightboxInfo In collInj
                sb.Append("<Lightbox>")

                sb.AppendFormat("<LightboxId>{0}</LightboxId>", obj.LightboxId)
                sb.AppendFormat("<ModuleId>{0}</ModuleId>", obj.ModuleId)
                sb.AppendFormat("<GalleryName>{0}</GalleryName>", XmlUtils.XMLEncode(obj.GalleryName))
                sb.AppendFormat("<GalleryDescription>{0}</GalleryDescription>", XmlUtils.XMLEncode(obj.GalleryDescription))
                sb.AppendFormat("<GalleryFolder>{0}</GalleryFolder>", XmlUtils.XMLEncode(obj.GalleryFolder))
                sb.AppendFormat("<DisplayOrder>{0}</DisplayOrder>", obj.DisplayOrder)
                ' 20101013 - Add HideTitleDescription
                sb.AppendFormat("<HideTitleDescription>{0}</HideTitleDescription>", obj.HideTitleDescription)
                ' 20120214 - Added auditing
                sb.AppendFormat("<LastUpdatedBy>{0}</LastUpdatedBy>", obj.LastUpdatedBy)
                sb.AppendFormat("<LastUpdatedDate>{0}</LastUpdatedDate>", obj.LastUpdatedDate)

                objSetting = GetSettings(obj.LightboxId)
                sb.Append("<Settings>")
                sb.AppendFormat("<ChangeSpeed>{0}</ChangeSpeed>", objSetting.ChangeSpeed)
                sb.AppendFormat("<Cyclic>{0}</Cyclic>", objSetting.Cyclic)
                sb.AppendFormat("<EnableEscapeButton>{0}</EnableEscapeButton>", objSetting.EnableEscapeButton)
                sb.AppendFormat("<Margin>{0}</Margin>", objSetting.Margin)
                sb.AppendFormat("<Modal>{0}</Modal>", objSetting.Modal)
                sb.AppendFormat("<OnCancel>{0}</OnCancel>", XmlUtils.XMLEncode(objSetting.OnCancel))
                sb.AppendFormat("<OnCleanup>{0}</OnCleanup>", XmlUtils.XMLEncode(objSetting.OnCleanup))
                sb.AppendFormat("<OnClosed>{0}</OnClosed>", XmlUtils.XMLEncode(objSetting.OnClosed))
                sb.AppendFormat("<OnComplete>{0}</OnComplete>", XmlUtils.XMLEncode(objSetting.OnComplete))
                sb.AppendFormat("<OnStart>{0}</OnStart>", XmlUtils.XMLEncode(objSetting.OnStart))
                sb.AppendFormat("<Opacity>{0}</Opacity>", objSetting.Opacity)
                sb.AppendFormat("<OverlayColor>{0}</OverlayColor>", XmlUtils.XMLEncode(objSetting.OverlayColor))
                sb.AppendFormat("<OverlayOpacity>{0}</OverlayOpacity>", objSetting.OverlayOpacity)
                sb.AppendFormat("<OverlayShow>{0}</OverlayShow>", objSetting.OverlayShow)
                sb.AppendFormat("<Padding>{0}</Padding>", objSetting.Padding)
                sb.AppendFormat("<ShowCloseButton>{0}</ShowCloseButton>", objSetting.ShowCloseButton)
                sb.AppendFormat("<ShowNavArrows>{0}</ShowNavArrows>", objSetting.ShowNavArrows)
                sb.AppendFormat("<Speed>{0}</Speed>", objSetting.Speed)
                sb.AppendFormat("<TitlePosition>{0}</TitlePosition>", XmlUtils.XMLEncode(objSetting.TitlePosition))
                sb.AppendFormat("<TitleShow>{0}</TitleShow>", objSetting.TitleShow)
                sb.AppendFormat("<Transition>{0}</Transition>", XmlUtils.XMLEncode(objSetting.Transition))
                sb.Append("</Settings>")

                Dim collImage As ImageInfoCollection = GetImages(obj.LightboxId)
                sb.Append("<Images>")
                For Each img As ImageInfo In collImage
                    sb.Append("<Image>")
                    sb.AppendFormat("<ImageId>{0}</ImageId>", img.ImageId)
                    sb.AppendFormat("<LightboxId>{0}</LightboxId>", img.LightboxId)
                    sb.AppendFormat("<FileName>{0}</FileName>", XmlUtils.XMLEncode(img.FileName))
                    sb.AppendFormat("<Title>{0}</Title>", XmlUtils.XMLEncode(img.Title))
                    sb.AppendFormat("<Description>{0}</Description>", XmlUtils.XMLEncode(img.Description))
                    sb.AppendFormat("<DisplayOrder>{0}</DisplayOrder>", img.DisplayOrder)
                    ' 20120214 - Added auditing
                    sb.AppendFormat("<LastUpdatedBy>{0}</LastUpdatedBy>", img.LastUpdatedBy)
                    sb.AppendFormat("<LastUpdatedDate>{0}</LastUpdatedDate>", img.LastUpdatedDate)
                    sb.Append("</Image>")
                Next
                sb.Append("</Images>")

                sb.Append("</Lightbox>")
            Next
            sb.Append("</Lightboxes>")
            ' later on, will probably need to add module settings here
            sb.Append("</WillStrohl>")

            Return sb.ToString
        End Function

        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserID As Integer) Implements DotNetNuke.Entities.Modules.IPortable.ImportModule

            Try
                Dim xContents As XDocument = XDocument.Parse(Content)

                For Each xLightbox As XElement In xContents.Descendants("Lightbox")

                    Dim objLightbox As New LightboxInfo
                    objLightbox.ModuleId = ModuleID
                    objLightbox.GalleryName = xLightbox.Element("GalleryName").Value
                    objLightbox.GalleryDescription = xLightbox.Element("GalleryDescription").Value
                    objLightbox.GalleryFolder = xLightbox.Element("GalleryFolder").Value
                    objLightbox.DisplayOrder = Integer.Parse(xLightbox.Element("DisplayOrder").Value, Globalization.NumberStyles.Integer)

                    If Not xLightbox.Element("HideTitleDescription") Is Nothing Then
                        objLightbox.HideTitleDescription = Boolean.Parse(xLightbox.Element("HideTitleDescription").Value)
                    Else
                        objLightbox.HideTitleDescription = False
                    End If

                    objLightbox.LastUpdatedBy = UserID
                    objLightbox.LastUpdatedDate = DateTime.Now

                    objLightbox.LightboxId = AddLightbox(objLightbox)

                    For Each xSetting As XElement In xLightbox.Descendants("Settings")

                        Dim objSetting As New SettingInfo
                        objSetting.ChangeSpeed = Integer.Parse(xSetting.Element("ChangeSpeed").Value, Globalization.NumberStyles.Integer)
                        objSetting.Cyclic = Boolean.Parse(xSetting.Element("Cyclic").Value)
                        objSetting.EnableEscapeButton = Boolean.Parse(xSetting.Element("EnableEscapeButton").Value)
                        objSetting.LightboxId = objLightbox.LightboxId
                        objSetting.Margin = Integer.Parse(xSetting.Element("Margin").Value, Globalization.NumberStyles.Integer)
                        objSetting.Modal = Boolean.Parse(xSetting.Element("Modal").Value)
                        objSetting.OnCancel = xSetting.Element("OnCancel").Value
                        objSetting.OnCleanup = xSetting.Element("OnCleanup").Value
                        objSetting.OnClosed = xSetting.Element("OnClosed").Value
                        objSetting.OnComplete = xSetting.Element("OnComplete").Value
                        objSetting.OnStart = xSetting.Element("OnStart").Value
                        objSetting.Opacity = Boolean.Parse(xSetting.Element("Opacity").Value)
                        objSetting.OverlayColor = xSetting.Element("OverlayColor").Value
                        objSetting.OverlayOpacity = Decimal.Parse(xSetting.Element("OverlayOpacity").Value, Globalization.NumberStyles.Float)
                        objSetting.OverlayShow = Boolean.Parse(xSetting.Element("OverlayShow").Value)
                        objSetting.Padding = Integer.Parse(xSetting.Element("Padding").Value, Globalization.NumberStyles.Integer)
                        objSetting.ShowCloseButton = Boolean.Parse(xSetting.Element("ShowCloseButton").Value)
                        objSetting.ShowNavArrows = Boolean.Parse(xSetting.Element("ShowNavArrows").Value)
                        objSetting.Speed = Integer.Parse(xSetting.Element("Speed").Value, Globalization.NumberStyles.Integer)
                        objSetting.TitlePosition = xSetting.Element("TitlePosition").Value
                        objSetting.TitleShow = Boolean.Parse(xSetting.Element("TitleShow").Value)
                        objSetting.Transition = xSetting.Element("Transition").Value

                        AddSetting(objSetting)
                    Next

                    For Each xImage As XElement In xLightbox.Descendants("Image")

                        Dim objImage As New ImageInfo
                        objImage.LightboxId = objLightbox.LightboxId
                        objImage.FileName = xImage.Element("FileName").Value
                        objImage.Title = xImage.Element("Title").Value
                        objImage.Description = xImage.Element("Description").Value
                        objImage.DisplayOrder = Integer.Parse(xImage.Element("DisplayOrder").Value, Globalization.NumberStyles.Integer)

                        objImage.LastUpdatedBy = UserID
                        objImage.LastUpdatedDate = DateTime.Now

                        AddImage(objImage)

                    Next

                Next

            Catch ex As Exception
                LogException(ex)
            End Try

        End Sub

#End Region

#Region " ISearchable Implementation "

        Public Function GetSearchItems(ModInfo As ModuleInfo) As SearchItemInfoCollection Implements ISearchable.GetSearchItems

            Dim searchItems As New SearchItemInfoCollection
            Dim collLightbox As LightboxInfoCollection = GetLightboxes(ModInfo.ModuleID)

            If Not collLightbox Is Nothing Then

                ' iterate through each album
                For Each oLightbox As LightboxInfo In collLightbox

                    Dim siLightbox As New SearchItemInfo

                    With siLightbox
                        .Author = oLightbox.LastUpdatedBy
                        .Content = oLightbox.GalleryName
                        .Description = oLightbox.GalleryDescription
                        .ModuleId = oLightbox.ModuleId
                        .PubDate = oLightbox.LastUpdatedDate
                        .SearchItemId = oLightbox.LightboxId
                        .SearchKey = oLightbox.LightboxId.ToString()
                        .Title = oLightbox.GalleryName
                    End With
                    searchItems.Add(siLightbox)

                    ' iterate through each image title & description
                    Dim collImage As ImageInfoCollection = GetImages(oLightbox.LightboxId)

                    If Not collImage Is Nothing AndAlso collImage.Count > 0 Then

                        For Each oImage As ImageInfo In collImage

                            Dim siImage As New SearchItemInfo

                            With siImage
                                .Author = oImage.LastUpdatedBy
                                .Content = oImage.Title
                                .Description = oImage.Description
                                .ModuleId = oLightbox.ModuleId
                                .PubDate = oImage.LastUpdatedDate
                                .SearchItemId = oImage.ImageId
                                .SearchKey = oImage.LightboxId.ToString()
                                .Title = oImage.Title
                            End With
                            searchItems.Add(siImage)

                        Next

                    End If

                Next

            End If

            Return searchItems

        End Function

#End Region

        End Class

End Namespace