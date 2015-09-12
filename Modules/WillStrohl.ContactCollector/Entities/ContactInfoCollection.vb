'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2015, Will Strohl
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

Imports DotNetNuke.Entities.Modules
Imports System.Collections.Generic
Imports WillStrohl.Modules.ContactCollector.ContactInfoMembers

Namespace WillStrohl.Modules.ContactCollector

    <Serializable()> _
    Public NotInheritable Class ContactInfoCollection
        Inherits List(Of ContactInfo)

#Region " Constructors "

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal capacity As Integer)
            MyBase.New(capacity)
        End Sub

        Public Sub New(ByVal collection As IEnumerable(Of ContactInfo))
            MyBase.New(collection)
        End Sub

#End Region

#Region " IHydratable Implementation "

        ''' <summary>
        ''' Fill - this method fill a current instance of the collection
        ''' </summary>
        ''' <param name="dr"></param>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Sub Fill(ByVal dr As System.Data.IDataReader) 
            Try
                While dr.Read
                    Dim obj As New ContactInfo
                    If Not dr.Item(ContactIdField) Is Nothing Then
                        obj.ContactId = Integer.Parse(dr.Item(ContactIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(ModuleIdField) Is Nothing Then
                        obj.ModuleId = Integer.Parse(dr.Item(ModuleIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(FirstNameField) Is Nothing Then
                        obj.FirstName = dr.Item(FirstNameField).ToString
                    End If
                    If Not dr.Item(LastNameField) Is Nothing Then
                        obj.LastName = dr.Item(LastNameField).ToString
                    End If
                    If Not dr.Item(EmailAddressField) Is Nothing Then
                        obj.EmailAddress = dr.Item(EmailAddressField).ToString
                    End If
                    If Not dr.Item(IsActiveField) Is Nothing Then
                        obj.IsActive = Boolean.Parse(dr.Item(IsActiveField).ToString)
                    End If
                    If Not dr.Item(CommentField) Is Nothing Then
                        obj.Comment = dr.Item(CommentField).ToString
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

#End Region

    End Class

End Namespace