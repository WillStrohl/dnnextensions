
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions
Imports System.Collections.Generic

Imports WillStrohl.Modules.Injection.InjectionInfoMembers

Namespace WillStrohl.Modules.Injection

    <Serializable()> _
    Public NotInheritable Class InjectionInfoCollection
        Inherits List(Of InjectionInfo)

#Region " Constructors "

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal capacity As Integer)
            MyBase.New(capacity)
        End Sub

        Public Sub New(ByVal collection As IEnumerable(Of InjectionInfo))
            MyBase.New(collection)
        End Sub

#End Region

        Public Sub Fill(ByVal dr As System.Data.IDataReader)
            Try
                While dr.Read
                    Dim obj As New InjectionInfo
                    If Not dr.Item(InjectionIdField) Is Nothing Then
                        obj.InjectionId = Integer.Parse(dr.Item(InjectionIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(InjectNameField) Is Nothing Then
                        obj.InjectName = dr.Item(InjectNameField).ToString
                    End If
                    If Not dr.Item(InjectContentField) Is Nothing Then
                        obj.InjectContent = dr.Item(InjectContentField).ToString
                    End If
                    If Not dr.Item(InjectTopField) Is Nothing Then
                        obj.InjectTop = Boolean.Parse(dr.Item(InjectTopField).ToString)
                    End If
                    If Not dr.Item(IsEnabledField) Is Nothing Then
                        obj.IsEnabled = Boolean.Parse(dr.Item(IsEnabledField).ToString)
                    End If
                    If Not dr.Item(ModuleIdField) Is Nothing Then
                        obj.ModuleId = Integer.Parse(dr.Item(ModuleIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(OrderShownField) Is Nothing Then
                        obj.OrderShown = Integer.Parse(dr.Item(OrderShownField).ToString, Globalization.NumberStyles.Integer)
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