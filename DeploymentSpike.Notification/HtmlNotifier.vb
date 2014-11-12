Imports Castle.Core.Logging

''' <summary>
''' A notification implementation that will write information to the configured html file instances.
''' </summary>
''' <remarks></remarks>
Public Class HtmlNotifier
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

    Public Function NotifyCompletedFailure(task As Interfaces.ITask) As Boolean Implements Interfaces.INotification.NotifyCompletedFailure
        If task.MetaInformation Is Nothing Then
            Logger.Debug(My.Resources.Messages.Task & My.Resources.Messages.CompletedFailure)
        Else
            Logger.Debug(task.MetaInformation.DeploymentTask & My.Resources.Messages.Space & My.Resources.Messages.CompletedFailure)
        End If
        BuildHtmlFile(task, NotificationType.CompletedFailure)
        Return True
    End Function

    Public Function NotifyCompletedSuccess(task As Interfaces.ITask) As Boolean Implements Interfaces.INotification.NotifyCompletedSuccess
        If task.MetaInformation Is Nothing Then
            Logger.Debug(My.Resources.Messages.Task & My.Resources.Messages.CompletedSuccess)
        Else
            Logger.Debug(task.MetaInformation.DeploymentTask & My.Resources.Messages.Space & My.Resources.Messages.CompletedSuccess)
        End If
        BuildHtmlFile(task, NotificationType.CompletedSuccess)
        Return True
    End Function

    Public Function NotifyStarted(task As Interfaces.ITask) As Boolean Implements Interfaces.INotification.NotifyStarted
        If task.MetaInformation Is Nothing Then
            Logger.Debug(My.Resources.Messages.Task & My.Resources.Messages.Started)
        Else
            Logger.Debug(task.MetaInformation.DeploymentTask & My.Resources.Messages.Space & My.Resources.Messages.Started)
        End If
        BuildHtmlFile(task, NotificationType.Started)
        Return True
    End Function

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

    Public ReadOnly Property Name As String Implements Interfaces.INotification.Name
        Get
            Return "HtmlNotifier"
        End Get
    End Property

    Public Function ValidateSettings() As Boolean Implements INotification.ValidateSettings
        'need the file setting to be populated and valid locations
        'require valid accessible folder.
        If Not NotificationSettings.Settings.ContainsKey(Constants.ConfigHtmlFile) OrElse String.IsNullOrWhiteSpace(NotificationSettings.Settings(Constants.ConfigHtmlFile)) Then
            Logger.Fatal(My.Resources.Messages.MissingHtmlFile)
            Return False
        End If
        'validate that all of the folders exist
        For Each item In NotificationSettings.Settings(Constants.ConfigHtmlFile).Split(Constants.Comma)
            Dim _t As String = IO.Path.GetDirectoryName(item)
            If IO.Directory.Exists(IO.Path.GetDirectoryName(item)) Then
                Logger.Debug(My.Resources.Messages.SpecifiedFolderFound & item)
            Else
                Logger.Fatal(My.Resources.Messages.SpecifiedFolderNotFound & item)
                Return False
            End If
        Next
        Return True
    End Function

    Public Sub BuildHtmlFile(task As Interfaces.ITask, notifytype As NotificationType)
        Dim _output As String = task.MetaInformationList.ToHtmlReport
        For Each item In NotificationSettings.Settings(Constants.ConfigHtmlFile).Split(Constants.Comma)
            IO.File.WriteAllText(item, _output)
        Next
    End Sub

End Class
