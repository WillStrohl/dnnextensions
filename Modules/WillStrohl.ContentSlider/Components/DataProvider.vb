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

Imports DotNetNuke

Namespace WillStrohl.Modules.ContentSlider

    Public MustInherit Class DataProvider

#Region " Private Members "

        Private Const c_AssemblyName As String = "WillStrohl.Modules.ContentSlider.SqlDataProvider, WillStrohl.Modules.ContentSlider"

#End Region

#Region " Shared/Static Methods "

        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            If objProvider IsNot Nothing Then
                Return
            End If

            Dim objectType As Type = Type.GetType(c_AssemblyName)

            objProvider = CType(Activator.CreateInstance(objectType), DataProvider)

            DataCache.SetCache(objectType.FullName, objProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region " Abstract Methods "

        Public MustOverride Function AddSlider(ByVal ModuleId As Integer, ByVal SliderName As String, ByVal SliderContent As String, ByVal AlternateText As String, ByVal Link As String, ByVal NewWindow As Boolean, ByVal DisplayOrder As Integer, ByVal LastUpdatedBy As Integer, ByVal StartDate As DateTime, ByVal EndDate As DateTime) As Integer
        Public MustOverride Sub UpdateSlider(ByVal SliderId As Integer, ByVal ModuleId As Integer, ByVal SliderName As String, ByVal SliderContent As String, ByVal AlternateText As String, ByVal Link As String, ByVal NewWindow As Boolean, ByVal DisplayOrder As Integer, ByVal LastUpdatedBy As Integer, ByVal StartDate As DateTime, ByVal EndDate As DateTime)
        Public MustOverride Sub DeleteSlider(ByVal SliderId As Integer)
        Public MustOverride Function GetSliders(ByVal ModuleId As Integer) As IDataReader
        Public MustOverride Function GetSlidersForEdit(ByVal ModuleId As Integer) As IDataReader
        Public MustOverride Function GetSlider(ByVal SliderId As Integer) As IDataReader

#End Region

    End Class

End Namespace