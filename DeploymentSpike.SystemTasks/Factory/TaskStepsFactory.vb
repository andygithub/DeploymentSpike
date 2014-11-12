Imports DeploymentSpike.Interfaces
Imports System.IO
Imports DeploymentSpike.Controller.Configuration
Imports Castle.Core.Logging
Imports DeploymentSpike.Controller.Interfaces

Namespace Factory

    ''' <summary>
    ''' Factory to load task steps from a file.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class TaskStepsFactory

        Private Sub New()

        End Sub

        Shared Function Create(fileName As String, logger As ILogger, settings As IConfigurationSettingsManager) As IEnumerable(Of ITaskInformation)
            If logger Is Nothing Then Throw New ArgumentNullException(DeploymentSpike.Interfaces.Constants.LoggerParameterName)
            If settings Is Nothing Then Throw New ArgumentNullException(DeploymentSpike.Interfaces.Constants.SettingsParameterName)
            If String.IsNullOrWhiteSpace(fileName) Then
                logger.Debug(My.Resources.Messages.FileNotFound)
                Return GetDefaultTaskSteps()
            End If
            'determine the task to get the settings
            Dim _loadfile As String = TaskNotificationSettingsFactory.GetFullPathName(settings, fileName)

            'check settings to see if settings files has been specified
            If Not IO.File.Exists(_loadfile) Then
                logger.Debug(My.Resources.Messages.FileNotFound & _loadfile & My.Resources.Messages.ForComponent & fileName)
                Return GetDefaultTaskSteps()
            End If
            'attempt to load task steps
            Return LoadTaskSteps(_loadfile)
        End Function

        Public Shared ReadOnly Property GetDefaultTaskSteps() As IEnumerable(Of ITaskInformation)
            Get
                Return New List(Of Domain.TaskMetaInformation)
            End Get
        End Property

        Public Shared Function LoadTaskSteps(settingsFile As String) As IEnumerable(Of ITaskInformation)
            If String.IsNullOrWhiteSpace(settingsFile) Then Return GetDefaultTaskSteps()
            Using _writer As New FileStream(settingsFile, FileMode.Open)
                Dim _loadedSetting As IEnumerable(Of ITaskInformation) = ServiceStack.Text.JsonSerializer.DeserializeFromStream(Of IEnumerable(Of ITaskInformation))(_writer)
                If _loadedSetting Is Nothing Then
                    Return GetDefaultTaskSteps()
                Else
                    Return _loadedSetting
                End If
            End Using
        End Function

        Public Shared Sub SaveTaskSteps(Of T)(settings As IConfigurationSettingsManager, tasks As IEnumerable(Of T), fileName As String)
            If settings Is Nothing Then Throw New ArgumentNullException(DeploymentSpike.Interfaces.Constants.SettingsParameterName)
            If tasks Is Nothing Then Throw New ArgumentNullException(Utility.Constants.Tasks)
            If String.IsNullOrWhiteSpace(fileName) Then Throw New ArgumentNullException(Utility.Constants.FileName)
            Using _writer1 As New FileStream(TaskNotificationSettingsFactory.GetFullPathName(settings, fileName), FileMode.Create)
                ServiceStack.Text.JsonSerializer.SerializeToStream(Of IEnumerable(Of T))(tasks, _writer1)
            End Using
        End Sub

    End Class

End Namespace