Public NotInheritable Class Constants

    Public Const ConfigNotifyStart As String = "NotifyStart"
    Public Const ConfigNotifyFail As String = "NotifyFail"
    Public Const ConfigNotifySuccess As String = "NotifySuccess"

    Public Const Comma As Char = ","c

    Public Const ConfigEmailFile As String = "EmailFile"
    Public Const ConfigEmailFromAddress As String = "EmailFromAddres"
    Public Const ConfigEmailFromName As String = "EmailFromName"
    Public Const ConfigEmailHost As String = "EmailHost"
    Public Const ConfigEmailPort As String = "EmailPort"

    Public Const ConfigHtmlFile As String = "HtmlFile"

End Class

Public Enum NotificationType
    Started
    CompletedSuccess
    CompletedFailure
End Enum