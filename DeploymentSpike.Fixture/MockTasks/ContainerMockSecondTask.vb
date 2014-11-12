Imports Castle.Core.Logging

Namespace Mocks

    Public Class ContainerSecondMockTask
        Implements ITask

        Private _settings As Interfaces.ISettings
        Private _logger As ILogger

        Sub New(settings As Interfaces.ISettings)
            If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
            _settings = settings
        End Sub

        Public Property MetaInformation As Interfaces.ITaskInformation Implements Interfaces.ITask.MetaInformation

        Public Function Execute() As Interfaces.TaskStatus Implements Interfaces.ITask.Execute
            Throw New ArgumentNullException
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
        Public Property MetaInformationList As IEnumerable(Of Interfaces.ITaskInformation) Implements Interfaces.ITask.MetaInformationList
    End Class

End Namespace