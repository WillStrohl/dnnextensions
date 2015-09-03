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

Imports DotNetNuke

Namespace WillStrohl.Modules.Injection

    Public MustInherit Class DataProvider

#Region " Private Members "

        Private Const c_AssemblyName As String = "WillStrohl.Modules.Injection.SqlDataProvider, WillStrohl.Modules.Injection"

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
            'objProvider = CType(Framework.Reflection.CreateObject(c_Object, c_ObjectType, c_AssemblyName), DataProvider)

            If objProvider Is Nothing Then
                Dim objectType As Type = Type.GetType(c_AssemblyName)

                objProvider = DirectCast(Activator.CreateInstance(objectType), DataProvider)
                DataCache.SetCache(objectType.FullName, objProvider)
            End If
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            If objProvider Is Nothing Then
                CreateProvider()
            End If
            Return objProvider
        End Function

#End Region

#Region " Abstract Methods "

        Public MustOverride Function AddInjectionContent(ByVal ModuleId As Integer, ByVal InjectTop As Boolean, ByVal InjectName As String, ByVal InjectContent As String, ByVal IsEnabled As Boolean, ByVal OrderShown As Integer) As Integer
        Public MustOverride Sub UpdateInjectionContent(ByVal InjectionId As Integer, ByVal ModuleId As Integer, ByVal InjectTop As Boolean, ByVal InjectName As String, ByVal InjectContent As String, ByVal IsEnabled As Boolean, ByVal OrderShown As Integer)
        Public MustOverride Sub DisableInjectionContent(ByVal InjectionId As Integer)
        Public MustOverride Sub EnableInjectionContent(ByVal InjectionId As Integer)
        Public MustOverride Sub DeleteInjectionContent(ByVal InjectionId As Integer)
        Public MustOverride Function GetInjectionContent(ByVal InjectionId As Integer) As IDataReader
        Public MustOverride Function GetActiveInjectionContents(ByVal ModuleId As Integer) As IDataReader
        Public MustOverride Function GetInjectionContents(ByVal ModuleId As Integer) As IDataReader
        Public MustOverride Function GetNextOrderNumber(ByVal ModuleId As Integer) As Integer
        Public MustOverride Sub ChangeOrder(ByVal InjectionId As Integer, ByVal Direction As String)
        Public MustOverride Function DoesInjectionNameExist(ByVal InjectionName As String, ByVal ModuleId As Integer) As Boolean

#End Region

    End Class

End Namespace