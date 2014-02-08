'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2011-2013, Will Strohl
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

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions

Imports WillStrohl.Modules.ContentSlider.SliderInfoMembers

Namespace WillStrohl.Modules.ContentSlider

    <Serializable()> _
    Public NotInheritable Class SliderInfo
        Implements ISliderInfo, IHydratable

#Region " Private Members "

        Private p_SliderContent As String = Null.NullString
        Private p_AlternateText As String = Null.NullString
        Private p_SliderName As String = Null.NullString
        Private p_SliderId As Integer = Null.NullInteger
        Private p_ModuleId As Integer = Null.NullInteger
        Private p_DisplayOrder As Integer = Null.NullInteger
        Private p_LastUpdatedBy As Integer = Null.NullInteger
        Private p_LastUpdatedDate As DateTime = Null.NullDate
        Private p_Link As String = Null.NullString
        Private p_NewWindow As Boolean = Null.NullBoolean
        Private p_StartDate As DateTime = Null.NullDate
        Private p_EndDate As DateTime = Null.NullDate

#End Region

#Region " Public Properties "

        Public Property SliderContent() As String Implements ISliderInfo.SliderContent
            Get
                Return Me.p_SliderContent
            End Get
            Set(ByVal value As String)
                Me.p_SliderContent = value
            End Set
        End Property

        Public Property AlternateText() As String Implements ISliderInfo.AlternateText
            Get
                Return Me.p_AlternateText
            End Get
            Set(ByVal value As String)
                Me.p_AlternateText = value
            End Set
        End Property

        Public Property SliderName() As String Implements ISliderInfo.SliderName
            Get
                Return Me.p_SliderName
            End Get
            Set(ByVal value As String)
                Me.p_SliderName = value
            End Set
        End Property

        Public Property SliderId() As Integer Implements ISliderInfo.SliderId
            Get
                Return Me.p_SliderId
            End Get
            Set(ByVal value As Integer)
                Me.p_SliderId = value
            End Set
        End Property

        Public Property ModuleId() As Integer Implements ISliderInfo.ModuleId
            Get
                Return Me.p_ModuleId
            End Get
            Set(ByVal value As Integer)
                Me.p_ModuleId = value
            End Set
        End Property

        Public Property DisplayOrder() As Integer Implements ISliderInfo.DisplayOrder
            Get
                Return Me.p_DisplayOrder
            End Get
            Set(ByVal value As Integer)
                Me.p_DisplayOrder = value
            End Set
        End Property

        Public Property LastUpdatedBy As Integer Implements ISliderInfo.LastUpdatedBy
            Get
                Return Me.p_LastUpdatedBy
            End Get
            Set(ByVal value As Integer)
                Me.p_LastUpdatedBy = value
            End Set
        End Property

        Public Property LastUpdatedDate As DateTime Implements ISliderInfo.LastUpdatedDate
            Get
                Return Me.p_LastUpdatedDate
            End Get
            Set(ByVal value As DateTime)
                Me.p_LastUpdatedDate = value
            End Set
        End Property

        Public Property Link As String Implements ISliderInfo.Link
            Get
                Return Me.p_Link
            End Get
            Set(ByVal value As String)
                Me.p_Link = value
            End Set
        End Property

        Public Property NewWindow As Boolean Implements ISliderInfo.NewWindow
            Get
                Return Me.p_NewWindow
            End Get
            Set(ByVal value As Boolean)
                Me.p_NewWindow = value
            End Set
        End Property

        Public Property StartDate As DateTime Implements ISliderInfo.StartDate
            Get
                Return Me.p_StartDate
            End Get
            Set(value As DateTime)
                Me.p_StartDate = value
            End Set
        End Property

        Public Property EndDate As DateTime Implements ISliderInfo.EndDate
            Get
                Return Me.p_EndDate
            End Get
            Set(value As DateTime)
                Me.p_EndDate = value
            End Set
        End Property

#End Region

        Public Sub New()
            ' do nothing
        End Sub

#Region " IHydratable Implementation "

        Public Property KeyID() As Integer Implements DotNetNuke.Entities.Modules.IHydratable.KeyID
            Get
                Return Me.p_SliderId
            End Get
            Set(ByVal value As Integer)
                Me.p_SliderId = value
            End Set
        End Property

        Public Sub Fill(ByVal dr As System.Data.IDataReader) Implements DotNetNuke.Entities.Modules.IHydratable.Fill
            Try
                While dr.Read
                    If Not dr.Item(SliderIdField) Is Nothing Then
                        Me.p_SliderId = Integer.Parse(dr.Item(SliderIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(ModuleIdField) Is Nothing Then
                        Me.p_ModuleId = Integer.Parse(dr.Item(ModuleIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(SliderNameField) Is Nothing Then
                        Me.p_SliderName = dr.Item(SliderNameField).ToString
                    End If
                    If Not dr.Item(SliderContentField) Is Nothing Then
                        Me.p_SliderContent = dr.Item(SliderContentField).ToString
                    End If
                    If Not dr.Item(AlternateTextField) Is Nothing Then
                        Me.p_AlternateText = dr.Item(AlternateTextField).ToString
                    End If
                    If Not dr.Item(LinkField) Is Nothing Then
                        Me.p_Link = dr.Item(LinkField).ToString
                    End If
                    If Not dr.Item(NewWindowField) Is Nothing Then
                        Me.p_NewWindow = Boolean.Parse(dr.Item(NewWindowField).ToString)
                    End If
                    If Not dr.Item(DisplayOrderField) Is Nothing Then
                        Me.p_DisplayOrder = Integer.Parse(dr.Item(DisplayOrderField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(LastUpdatedByField) Is Nothing Then
                        Me.p_LastUpdatedBy = Integer.Parse(dr.Item(LastUpdatedByField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(LastUpdatedDateField) Is Nothing Then
                        Me.p_LastUpdatedDate = DateTime.Parse(dr.Item(LastUpdatedDateField).ToString)
                    End If
                    If Not dr.Item(StartDateField) Is Nothing Then
                        Me.p_StartDate = DateTime.Parse(dr.Item(StartDateField).ToString)
                    End If
                    If Not dr.Item(EndDateField) Is Nothing Then
                        Dim strDate As String = dr.Item(EndDateField).ToString
                        If Not String.IsNullOrEmpty(strDate) Then
                            Me.p_EndDate = DateTime.Parse(strDate)
                        End If
                    End If
                End While
            Catch ex As Exception
                LogException(ex)
            Finally
                If Not dr.IsClosed Then
                    dr.Close()
                End If
            End Try
        End Sub

#End Region

    End Class

End Namespace