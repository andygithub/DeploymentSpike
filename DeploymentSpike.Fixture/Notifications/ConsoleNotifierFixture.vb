Public Class ConsoleNotifierFixture

    <Fact()>
    Public Sub null_logging_parameter()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Dim _task As New DeploymentSpike.Notification.ConsoleNotifier(Nothing)
                                                End Sub)

    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_null_setting_dictionary()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Notification.ConsoleNotifier(_defaultEmptySettings)
        Assert.True(_task.ValidateSettings)
        _task.Logger = Nothing
        Dim _name As String = _task.Name
    End Sub

    <Fact()>
    Public Sub NotifyStarted_success_null_meta_property()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, Boolean.TrueString)
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        Dim _notificationsSettings As New Controller.Domain.TaskSettings
        Dim _notificationtask As New DeploymentSpike.Notification.ConsoleNotifier(_notificationsSettings)
        _notificationtask.NotifyStarted(_task)
        Assert.Equal(_notificationtask.NotificationSettings.Settings(Notification.Constants.ConfigNotifyFail), Boolean.TrueString)
    End Sub

    <Fact()>
    Public Sub NotifyStarted_success_populated_meta_property()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, Boolean.TrueString)
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        _task.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.DeploymentTask = "xunit:"}
        Dim _notificationsSettings As New Controller.Domain.TaskSettings
        Dim _notificationtask As New DeploymentSpike.Notification.ConsoleNotifier(_notificationsSettings)
        _notificationtask.NotifyStarted(_task)
        Assert.Equal(_notificationtask.NotificationSettings.Settings(Notification.Constants.ConfigNotifyFail), Boolean.TrueString)
    End Sub

    <Fact()>
    Public Sub NotifyCompleted_success_null_meta_property()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, Boolean.TrueString)
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        Dim _notificationsSettings As New Controller.Domain.TaskSettings
        Dim _notificationtask As New DeploymentSpike.Notification.ConsoleNotifier(_notificationsSettings)
        _notificationtask.NotifyCompletedSuccess(_task)
        Assert.Equal(_notificationtask.NotificationSettings.Settings(Notification.Constants.ConfigNotifyFail), Boolean.TrueString)
    End Sub

    <Fact()>
    Public Sub NotifyCompleted_success_populated_meta_property()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, Boolean.TrueString)
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        _task.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.DeploymentTask = "xunit:"}
        Dim _notificationsSettings As New Controller.Domain.TaskSettings
        Dim _notificationtask As New DeploymentSpike.Notification.ConsoleNotifier(_notificationsSettings)
        _notificationtask.NotifyCompletedSuccess(_task)
        Assert.Equal(_notificationtask.NotificationSettings.Settings(Notification.Constants.ConfigNotifyFail), Boolean.TrueString)
    End Sub

    <Fact()>
    Public Sub NotifyFailed_success_null_meta_property()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, Boolean.TrueString)
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        Dim _notificationsSettings As New Controller.Domain.TaskSettings
        Dim _notificationtask As New DeploymentSpike.Notification.ConsoleNotifier(_notificationsSettings)
        _notificationtask.NotifyCompletedFailure(_task)
        Assert.Equal(_notificationtask.NotificationSettings.Settings(Notification.Constants.ConfigNotifyFail), Boolean.TrueString)
    End Sub

    <Fact()>
    Public Sub NotifyFailed_success_populated_meta_property()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, Boolean.TrueString)
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        _task.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.DeploymentTask = "xunit:"}
        Dim _notificationsSettings As New Controller.Domain.TaskSettings
        Dim _notificationtask As New DeploymentSpike.Notification.ConsoleNotifier(_notificationsSettings)
        _notificationtask.NotifyCompletedFailure(_task)
        Assert.Equal(_notificationtask.NotificationSettings.Settings(Notification.Constants.ConfigNotifyFail), Boolean.TrueString)
    End Sub

End Class
