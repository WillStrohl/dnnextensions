'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2011-2016, Will Strohl
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
'Neither the name of Will Strohl, Content Slider, nor the names of its contributors may be 
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

Imports WillStrohl.Modules.ContentSlider.SliderInfoMembers

Namespace WillStrohl.Modules.ContentSlider

    <Serializable()> _
    Public NotInheritable Class SliderInfoCollection
        Inherits List(Of SliderInfo)

#Region " Constructors "

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal capacity As Integer)
            MyBase.New(capacity)
        End Sub

        Public Sub New(ByVal collection As IEnumerable(Of SliderInfo))
            MyBase.New(collection)
        End Sub

#End Region

        Public Sub Fill(ByVal dr As System.Data.IDataReader)
            Try
                While dr.Read
                    Dim obj As New SliderInfo
                    If Not dr.Item(SliderIdField) Is Nothing Then
                        obj.SliderId = Integer.Parse(dr.Item(SliderIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(ModuleIdField) Is Nothing Then
                        obj.ModuleId = Integer.Parse(dr.Item(ModuleIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(SliderNameField) Is Nothing Then
                        obj.SliderName = dr.Item(SliderNameField).ToString
                    End If
                    If Not dr.Item(SliderContentField) Is Nothing Then
                        obj.SliderContent = dr.Item(SliderContentField).ToString
                    End If
                    If Not dr.Item(AlternateTextField) Is Nothing Then
                        obj.AlternateText = dr.Item(AlternateTextField).ToString
                    End If
                    If Not dr.Item(LinkField) Is Nothing Then
                        obj.Link = dr.Item(LinkField).ToString
                    End If
                    If Not dr.Item(NewWindowField) Is Nothing Then
                        obj.NewWindow = Boolean.Parse(dr.Item(NewWindowField).ToString)
                    End If
                    If Not dr.Item(DisplayOrderField) Is Nothing Then
                        obj.DisplayOrder = Integer.Parse(dr.Item(DisplayOrderField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(LastUpdatedByField) Is Nothing Then
                        obj.LastUpdatedBy = Integer.Parse(dr.Item(LastUpdatedByField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(LastUpdatedDateField) Is Nothing Then
                        obj.LastUpdatedDate = DateTime.Parse(dr.Item(LastUpdatedDateField).ToString)
                    End If
                    If Not dr.Item(StartDateField) Is Nothing Then
                        obj.StartDate = DateTime.Parse(dr.Item(StartDateField).ToString)
                    End If
                    If Not dr.Item(EndDateField) Is Nothing Then
                        Dim strDate As String = dr.Item(EndDateField).ToString
                        If Not String.IsNullOrEmpty(strDate) Then
                            obj.EndDate = DateTime.Parse(dr.Item(EndDateField).ToString)
                        End If
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