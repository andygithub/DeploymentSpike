Imports Castle.Core.Logging

Namespace Mocks

    Public Class ContainerMockNotifications
        Implements INotification

        Private _logger As ILogger

        Public Function NotifyCompletedFailure(task As Interfaces.ITask) As Boolean Implements Interfaces.INotification.NotifyCompletedFailure
            Throw New NotImplementedException
        End Function

        Public Function NotifyCompletedSuccess(task As Interfaces.ITask) As Boolean Implements Interfaces.INotification.NotifyCompletedSuccess
            Return True
        End Function

        Public Function NotifyStarted(task As Interfaces.ITask) As Boolean Implements Interfaces.INotification.NotifyStarted
            Throw New NotImplementedException
        End Function

        Public Property Logger As Castle.Core.Logging.ILogger Implements Interfaces.INotification.Logger
            Get
                If _logger Is Nothing Then Return New Castle.Core.Logging.NullLogger
                Return _logger
            End Get
            Set(value As Castle.Core.Logging.ILogger)
                _logger = value
            End Set
        End Property

        Public Property Settings As Interfaces.ISettings Implements Interfaces.INotification.Settings

        Public ReadOnly Property Name As String Implements Interfaces.INotification.Name
            Get
                Return "ContainerMockNotifications"
            End Get
        End Property

        Public Function ValidateSettings() As Boolean Implements Interfaces.INotification.ValidateSettings
            Return True
        End Function

    End Class

End Namespace