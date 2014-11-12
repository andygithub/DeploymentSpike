''' <summary>
''' This interface is used for a settings container on tasks and notifications.
''' </summary>
''' <remarks></remarks>
Public Interface ISettings

    Property SettingsLocation As String
    Function GetExecutionFlag() As ExecutionType
    Property Settings As Dictionary(Of String, String)

End Interface
