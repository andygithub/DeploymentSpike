Imports Castle.Core.Logging
''' <summary>
''' The interface for the notification events.
''' </summary>
''' <remarks>This interface needs to be implemented for a notification to take place in the deployment process.</remarks>
Public Interface INotification

    Function NotifyStarted(task As ITask) As Boolean
    Function NotifyCompletedSuccess(task As ITask) As Boolean
    Function NotifyCompletedFailure(task As ITask) As Boolean
    Function ValidateSettings() As Boolean

    ReadOnly Property Name As String
    Property Logger As ILogger
    Property Settings As ISettings

End Interface
