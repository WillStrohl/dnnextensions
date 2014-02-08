'
' Lightbox Gallery Module for DotNetNuke
' Project Contributors - Will Strohl (http://www.WillStrohl.com), Armand Datema (http://www.schwingsoft.com)
'
'Copyright (c) 2009-2012, Will Strohl
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

Imports DotNetNuke.Services.Exceptions
Imports System.Collections.Generic

Imports WillStrohl.Modules.Lightbox.ImageInfoMembers

Namespace WillStrohl.Modules.Lightbox

    Public NotInheritable Class ImageInfoCollection
        Inherits List(Of ImageInfo)

#Region " Constructors "

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal capacity As Integer)
            MyBase.New(capacity)
        End Sub

        Public Sub New(ByVal collection As IEnumerable(Of ImageInfo))
            MyBase.New(collection)
        End Sub

#End Region

        Public Sub Fill(ByVal dr As System.Data.IDataReader)
            Try
                While dr.Read
                    Dim obj As New ImageInfo
                   If Not dr.Item(ImageIdField) Is Nothing Then
                        obj.ImageId = Integer.Parse(dr.Item(ImageIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(LightboxIdField) Is Nothing Then
                        obj.LightboxId = Integer.Parse(dr.Item(LightboxIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(FileNameField) Is Nothing Then
                        obj.FileName = dr.Item(FileNameField).ToString
                    End If
                    If Not dr.Item(TitleField) Is Nothing Then
                        obj.Title = dr.Item(TitleField).ToString
                    End If
                    If Not dr.Item(DescriptionField) Is Nothing Then
                        obj.Description = dr.Item(DescriptionField).ToString
                    End If
                    If Not dr.Item(DisplayOrderField) Is Nothing Then
                        obj.DisplayOrder = Integer.Parse(dr.Item(DisplayOrderField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(LastUpdatedByField) Is Nothing Then
                        Try
                            obj.LastUpdatedBy = Integer.Parse(dr.Item(LastUpdatedByField).ToString, Globalization.NumberStyles.Integer)
                        Catch
                            ' do nothing
                        End Try
                    End If
                    If Not dr.Item(LastUpdatedDateField) Is Nothing Then
                        Try
                            obj.LastUpdatedDate = DateTime.Parse(dr.Item(LastUpdatedDateField).ToString)
                        Catch
                            obj.LastUpdatedDate = DateTime.Now
                        End Try
                    End If
                    Me.Add(obj)
                End While
            Catch ex As Exception
                LogException(ex)
            Finally
                If Not dr.IsClosed Then
                    dr.Close()
                End If
            End Try
        End Sub

    End Class

End Namespace