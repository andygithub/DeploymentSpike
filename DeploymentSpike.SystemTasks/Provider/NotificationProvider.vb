Imports Castle.Core.Logging
Imports DeploymentSpike.Controller.Interfaces
Imports DeploymentSpike.Controller.Utility


Namespace Provider

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NotificationProvider

        Private _logger As ILogger
        Private _notifications As IList(Of INotification)
        Private _settings As IConfigurationSettingsManager
        Private _task As ITask

        Sub New(logger As ILogger, task As ITask, notifications As IList(Of INotification), settings As IConfigurationSettingsManager)
            If logger Is Nothing Then Throw New ArgumentNullException(DeploymentSpike.Interfaces.Constants.LoggerParameterName)
            _logger = logger
            If settings Is Nothing Then Throw New ArgumentNullException(DeploymentSpike.Interfaces.Constants.SettingsParameterName)
            _settings = settings
            If task Is Nothing Then Throw New ArgumentNullException(Utility.Constants.Task)
            _task = task
            _notifications = notifications
            LoadMetaSettingsIntoNotifications(_task)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub NotifyStarted()
            ExecuteNotifications(Constants.ConfigNotifyStart)
        End Sub

        Public Sub NotifyCompleted()
            ExecuteNotifications(Constants.ConfigNotifySuccess)
        End Sub

        Public Sub NotifyCompletedFailure()
            ExecuteNotifications(Constants.ConfigNotifyFail)
        End Sub

        Private Sub ExecuteNotifications(notificationType As String)
            'get the list of a notifications
            'if the notification throws an exception log and continue on with the process.
            'loop through them all 
            'execute the success method and pass the task
            'order of notifications executed should not matter.
            Dim _configuredNotifications As List(Of String) = GetConfiguredNotifications()
            _notifications.ToList.ForEach(Sub(item)
                                              If _configuredNotifications.Count = 0 OrElse _configuredNotifications.Contains(item.GetType.ToString) Then
                                                  RunExecution(item, _task, notificationType)
                                              End If
                                          End Sub)
        End Sub

        Private Function GetConfiguredNotifications() As List(Of String)
            If String.IsNullOrWhiteSpace(_settings.NotificationClass) Then Return New List(Of String)
            Dim _configuredItems As List(Of String) = _settings.NotificationClass.Split(Constants.Comma).ToList
            'remove all items from the configured list that are not setup in the handler
            Dim _handlerList As New List(Of String)
            _notifications.ToList.ForEach(Sub(item)
                                              _handlerList.Add(item.GetType.ToString())
                                          End Sub)
            _configuredItems.RemoveAll(Function(x)
                                           Return Not _handlerList.Contains(x)
                                       End Function)
            Return _configuredItems
        End Function

        Private Sub LoadMetaSettingsIntoNotifications(task As ITask)
            'get the meta settings from the task and pass it to the factory.
            'only perform this if meta information is not null and the config property has a value
            If task.MetaInformation IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(task.MetaInformation.NotificationComponentConfiguration) Then Exit Sub
            Dim _customNotificationSettings As ISettings = Factory.TaskNotificationSettingsFactory.Create(task.MetaInformation.NotificationComponentConfiguration, _logger, _settings)
            'merge these settings into the existing settings
            _notifications.ToList.ForEach(Sub(item)
                                              item.Settings = Factory.TaskNotificationSettingsFactory.MergeTaskSettings(item.Settings, _customNotificationSettings, _settings)
                                          End Sub)
        End Sub

        Private Sub RunExecution(item As INotification, currentTask As ITask, notificationType As String)
            _logger.DebugFormat(System.Globalization.CultureInfo.InvariantCulture, My.Resources.Messages.NotificationType, item.Name)
            Select Case notificationType
                Case Constants.ConfigNotifyStart
                    If item.Settings.Settings.ContainsKey(Constants.ConfigNotifyStart) AndAlso item.Settings.Settings(Constants.ConfigNotifyStart) <> Boolean.TrueString Then
                        _logger.Debug(My.Resources.Messages.StartedNotFired)
                    Else
                        LogNotificationStatus(item.NotifyStarted(currentTask))
                    End If
                Case Constants.ConfigNotifySuccess
                    If item.Settings.Settings.ContainsKey(Constants.ConfigNotifySuccess) AndAlso item.Settings.Settings(Constants.ConfigNotifySuccess) <> Boolean.TrueString Then
                        _logger.Debug(My.Resources.Messages.SuccessNotFired)
                    Else
                        LogNotificationStatus(item.NotifyCompletedSuccess(currentTask))
                    End If
                Case Else
                    If item.Settings.Settings.ContainsKey(Constants.ConfigNotifyFail) AndAlso item.Settings.Settings(Constants.ConfigNotifyFail) <> Boolean.TrueString Then
                        _logger.Debug(My.Resources.Messages.FailedNotFired)
                    Else
                        LogNotificationStatus(item.NotifyCompletedFailure(currentTask))
                    End If
            End Select

        End Sub

        Private Sub LogNotificationStatus(value As Boolean)
            If value Then
                _logger.Debug(My.Resources.Messages.NotificationSuccess)
            Else
                _logger.Debug(My.Resources.Messages.NotificationFailure)
            End If
        End Sub

    End Class

End Namespace