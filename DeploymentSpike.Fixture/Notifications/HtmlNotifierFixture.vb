Imports DeploymentSpike.Notification

Public Class HtmlNotifierFixture
    Private Const _Noticehtml As String = "notice.html"

    <Fact()>
    Public Sub null_logging_parameter()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Dim _task As New DeploymentSpike.Notification.HtmlNotifier(Nothing)
                                                End Sub)

    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_null_setting_dictionary()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Notification.HtmlNotifier(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
        _task.Logger = Nothing
        Dim _name As String = _task.Name
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_invalid_path()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigHtmlFile, PurgeExistingArtifactsFixture.GetInvalidTempFolder & IO.Path.DirectorySeparatorChar & _Noticehtml)
        Dim _task As New DeploymentSpike.Notification.HtmlNotifier(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_valid_path()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigHtmlFile, PurgeExistingArtifactsFixture.GetValidTempFolder & _Noticehtml)
        Dim _task As New DeploymentSpike.Notification.HtmlNotifier(_defaultEmptySettings)
        Assert.True(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub htmltable_extension_method()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        _task.MetaInformationList = New List(Of ITaskInformation)
        Dim _line As String = _task.MetaInformationList.ToHtmlTable
        Assert.Contains("</table>", _line)
    End Sub

    <Fact()>
    Public Sub htmlreport_extension_method()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        _task.MetaInformationList = GenerateTaskReportFixture.GenerateTaskMetaSteps
        Dim _line As String = _task.MetaInformationList.ToHtmlReport
        Assert.Contains("Maintenance", _line)
    End Sub

    <Fact()>
    Public Sub NotifyStarted_null_metainformation()
        Dim _file As String = PurgeExistingArtifactsFixture.GetValidTempFolder & _Noticehtml
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigHtmlFile, _file)
        Dim _purgetask As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Dim _task As New DeploymentSpike.Notification.HtmlNotifier(_defaultEmptySettings)
        Assert.True(_task.NotifyStarted(_purgetask))
        Assert.True(IO.File.Exists(_file))
        IO.File.Delete(_file)
    End Sub

    <Fact()>
    Public Sub NotifyStarted_populated_metainformation()
        Dim _file As String = PurgeExistingArtifactsFixture.GetValidTempFolder & _Noticehtml
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigHtmlFile, _file)
        Dim _purgetask As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        _purgetask.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.DeploymentTask = EmailNotifierFixture.XunitTask}
        _purgetask.MetaInformationList = GenerateTaskReportFixture.GenerateTaskMetaSteps
        Dim _task As New DeploymentSpike.Notification.HtmlNotifier(_defaultEmptySettings)
        Assert.True(_task.NotifyStarted(_purgetask))
        Assert.True(IO.File.Exists(_file))
        IO.File.Delete(_file)
    End Sub

    <Fact()>
    Public Sub NotifyCompletedFailure_null_metainformation()
        Dim _file As String = PurgeExistingArtifactsFixture.GetValidTempFolder & _Noticehtml
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigHtmlFile, _file)
        Dim _purgetask As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Dim _task As New DeploymentSpike.Notification.HtmlNotifier(_defaultEmptySettings)
        Assert.True(_task.NotifyCompletedFailure(_purgetask))
        Assert.True(IO.File.Exists(_file))
        IO.File.Delete(_file)
    End Sub

    <Fact()>
    Public Sub NotifyCompletedFailure_populated_metainformation()
        Dim _file As String = PurgeExistingArtifactsFixture.GetValidTempFolder & _Noticehtml
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigHtmlFile, _file)
        Dim _purgetask As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        _purgetask.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.DeploymentTask = EmailNotifierFixture.XunitTask}
        _purgetask.MetaInformationList = GenerateTaskReportFixture.GenerateTaskMetaSteps
        Dim _task As New DeploymentSpike.Notification.HtmlNotifier(_defaultEmptySettings)
        Assert.True(_task.NotifyCompletedFailure(_purgetask))
        Assert.True(IO.File.Exists(_file))
        IO.File.Delete(_file)
    End Sub

    <Fact()>
    Public Sub NotifyCompletedSuccess_null_metainformation()
        Dim _file As String = PurgeExistingArtifactsFixture.GetValidTempFolder & _Noticehtml
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigHtmlFile, _file)
        Dim _purgetask As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Dim _task As New DeploymentSpike.Notification.HtmlNotifier(_defaultEmptySettings)
        Assert.True(_task.NotifyCompletedSuccess(_purgetask))
        Assert.True(IO.File.Exists(_file))
        IO.File.Delete(_file)
    End Sub

    <Fact()>
    Public Sub NotifyCompletedSuccess_populated_metainformation()
        Dim _file As String = PurgeExistingArtifactsFixture.GetValidTempFolder & _Noticehtml
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigHtmlFile, _file)
        Dim _purgetask As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        _purgetask.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.DeploymentTask = EmailNotifierFixture.XunitTask}
        _purgetask.MetaInformationList = GenerateTaskReportFixture.GenerateTaskMetaSteps
        Dim _task As New DeploymentSpike.Notification.HtmlNotifier(_defaultEmptySettings)
        Assert.True(_task.NotifyCompletedSuccess(_purgetask))
        Assert.True(IO.File.Exists(_file))
        IO.File.Delete(_file)
    End Sub

End Class
