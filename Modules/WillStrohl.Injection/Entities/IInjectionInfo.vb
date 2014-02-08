
Namespace WillStrohl.Modules.Injection

    Public Interface IInjectionInfo
        Property InjectionId() As Integer
        Property ModuleId() As Integer
        Property InjectTop() As Boolean
        Property InjectName() As String
        Property InjectContent() As String
        Property IsEnabled() As Boolean
        Property OrderShown() As Integer
    End Interface

End Namespace