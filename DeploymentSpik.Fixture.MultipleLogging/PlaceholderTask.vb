Public Class PlaceholderTask
    Implements ITask

    Dim _settings As Interfaces.ISettings
    Dim _logger As Interfaces.ILogging

    Sub New(settings As Interfaces.ISettings, logger As Interfaces.ILogging)
        If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
        If logger Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.LoggerParameterName)
        _settings = settings
        _logger = logger
    End Sub

    Public Function Execute() As Interfaces.TaskStatus Implements Interfaces.ITask.Execute

        Return TaskStatus.Completed
    End Function

    Public ReadOnly Property Name As String Implements Interfaces.ITask.Name
        Get
            Return Nothing
        End Get
    End Property


End Class
