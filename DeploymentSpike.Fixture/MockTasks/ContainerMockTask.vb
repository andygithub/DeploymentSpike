Imports Castle.Core.Logging

Namespace Mocks

    Public Class ContainerMockTask
        Implements ITask

        Private _settings As Interfaces.ISettings
        Private _logger As ILogger

        Sub New(settings As Interfaces.ISettings)
            If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
            _settings = settings
        End Sub

        Public Function Execute() As Interfaces.TaskStatus Implements Interfaces.ITask.Execute
            Logger.Debug("Starting the execution of task: ")
            Dim _locallogger As ILogger = Logger.CreateChildLogger("Task:ContainerMockTask")
            _locallogger.Warn("This is warning one.")
            _locallogger.Debug("This is trace one.")

            Logger.Debug("Ending the execution of task.")
            Return TaskStatus.Completed
        End Function

        Public Property Logger As Castle.Core.Logging.ILogger Implements Interfaces.ITask.Logger
            Get
                If _logger Is Nothing Then Return New Castle.Core.Logging.NullLogger
                Return _logger
            End Get
            Set(value As Castle.Core.Logging.ILogger)
                _logger = value
            End Set
        End Property

        Public Property TaskSettings As ISettings Implements Interfaces.ITask.TaskSettings
            Get
                Return _settings
            End Get
            Set(value As ISettings)
                _settings = value
            End Set
        End Property

        Public Function ValidateSettings() As Boolean Implements Interfaces.ITask.ValidateSettings
            Return True
        End Function

        Public Property MetaInformation As Interfaces.ITaskInformation Implements Interfaces.ITask.MetaInformation
        Public Property MetaInformationList As IEnumerable(Of Interfaces.ITaskInformation) Implements Interfaces.ITask.MetaInformationList
    End Class

End Namespace