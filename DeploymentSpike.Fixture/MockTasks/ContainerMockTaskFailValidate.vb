Imports Castle.Core.Logging

Namespace Mocks

    Public Class ContainerMockTaskFailValidate
        Implements ITask

        Private _logger As ILogger

        Sub New(settings As Interfaces.ISettings)
            If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
            TaskSettings = settings
        End Sub

        Public Function Execute() As Interfaces.TaskStatus Implements Interfaces.ITask.Execute
            Return TaskStatus.Failed
        End Function

        Public Property Logger As Castle.Core.Logging.ILogger Implements Interfaces.ITask.Logger
            Get
                If _logger Is Nothing Then
                    _logger = New Castle.Core.Logging.NullLogger
                End If
                Return _logger
            End Get
            Set(value As Castle.Core.Logging.ILogger)
                _logger = value
            End Set
        End Property

        Public Property TaskSettings As ISettings Implements Interfaces.ITask.TaskSettings

        Public Function ValidateSettings() As Boolean Implements Interfaces.ITask.ValidateSettings
            Return False
        End Function

        Public Property MetaInformation As Interfaces.ITaskInformation Implements Interfaces.ITask.MetaInformation
        Public Property MetaInformationList As IEnumerable(Of Interfaces.ITaskInformation) Implements Interfaces.ITask.MetaInformationList
    End Class

End Namespace