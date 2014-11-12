Imports Castle.Core.Logging

''' <summary>
''' A notification implementation that will write information to the implemented log instance.
''' </summary>
''' <remarks></remarks>
Public Class ConsoleNotifier
    Implements INotification

    Private _logger As ILogger

    ''' <summary>
    ''' Default constructor to set the settings.
    ''' </summary>
    ''' <param name="settings"></param>
    ''' <remarks></remarks>
    Public Sub New(settings As Interfaces.ISettings)
        If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
        NotificationSettings = settings
        SetupDefaults()
    End Sub

    ''' <summary>
    ''' Method to set the defaults for the notifications.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetupDefaults()
        'there does not need to be a flag to determine which of the notificiations should be fired.
        'default them all to true.
        If Not NotificationSettings.Settings.ContainsKey(Constants.ConfigNotifyStart) Then
            NotificationSettings.Settings.Add(Constants.ConfigNotifyStart, Boolean.TrueString)
        End If
        If Not NotificationSettings.Settings.ContainsKey(Constants.ConfigNotifySuccess) Then
            NotificationSettings.Settings.Add(Constants.ConfigNotifySuccess, Boolean.TrueString)
        End If
        If Not NotificationSettings.Settings.ContainsKey(Constants.ConfigNotifyFail) Then
            NotificationSettings.Settings.Add(Constants.ConfigNotifyFail, Boolean.TrueString)
        End If
    End Sub

    Public Function NotifyCompletedFailure(task As Interfaces.ITask) As Boolean Implements Interfaces.INotification.NotifyCompletedFailure
        If task.MetaInformation Is Nothing Then
            Logger.Debug(My.Resources.Messages.Task & My.Resources.Messages.CompletedFailure)
        Else
            Logger.Debug(task.MetaInformation.DeploymentTask & My.Resources.Messages.Space & My.Resources.Messages.CompletedFailure)
        End If
        Return True
    End Function

    Public Function NotifyCompletedSuccess(task As Interfaces.ITask) As Boolean Implements Interfaces.INotification.NotifyCompletedSuccess
        If task.MetaInformation Is Nothing Then
            Logger.Debug(My.Resources.Messages.Task & My.Resources.Messages.CompletedSuccess)
        Else
            Logger.Debug(task.MetaInformation.DeploymentTask & My.Resources.Messages.Space & My.Resources.Messages.CompletedSuccess)
        End If
        Return True
    End Function

    Public Function NotifyStarted(task As Interfaces.ITask) As Boolean Implements Interfaces.INotification.NotifyStarted
        If task.MetaInformation Is Nothing Then
            Logger.Debug(My.Resources.Messages.Task & My.Resources.Messages.Started)
        Else
            Logger.Debug(task.MetaInformation.DeploymentTask & My.Resources.Messages.Space & My.Resources.Messages.Started)
        End If
        Return True
    End Function

    ''' <summary>
    ''' Logger instance for the notification.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Logger As Castle.Core.Logging.ILogger Implements Interfaces.INotification.Logger
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

    ''' <summary>
    ''' Settings for the notification.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NotificationSettings As Interfaces.ISettings Implements Interfaces.INotification.Settings

    ''' <summary>
    ''' Logged name of the implementation of INotification.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Name As String Implements Interfaces.INotification.Name
        Get
            Return "ConsoleNotifier"
        End Get
    End Property

    ''' <summary>
    ''' This class doesn't have any settings so this method is not complicated.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateSettings() As Boolean Implements Interfaces.INotification.ValidateSettings
        Return True
    End Function

End Class
