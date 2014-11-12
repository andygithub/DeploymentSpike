Public Class ExecuteFileFixture
    Private Const PingExe As String = "ping.exe"
    Private Const _wwwgooglecom As String = "www.google.com"

    <Fact()>
    Public Sub null_logging_parameter()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Dim _task As New DeploymentSpike.Tasks.ExecuteFile(Nothing)
                                                End Sub)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_null_setting_dictionary()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Tasks.ExecuteFile(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
        _task.Logger = Nothing
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_valid_executing_file_name()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFileName, PingExe)
        Dim _task As New DeploymentSpike.Tasks.ExecuteFile(_defaultEmptySettings)
        Assert.True(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub execute_task_success()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFileName, PingExe)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingArguments, _wwwgooglecom)
        Dim _task As New DeploymentSpike.Tasks.ExecuteFile(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(Of TaskStatus)(_status, TaskStatus.Completed)
    End Sub

    <Fact()>
    Public Sub execute_task_success_redirect_false()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFileName, PingExe)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingArguments, _wwwgooglecom)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingRedirect, "False")
        Dim _task As New DeploymentSpike.Tasks.ExecuteFile(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(Of TaskStatus)(_status, TaskStatus.Completed)
    End Sub

    <Fact()>
    Public Sub execute_task_failure()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _file As String = ExecuteFolderFixture.GetValidErrorBatchFile()
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFileName, _file)
        Dim _task As New DeploymentSpike.Tasks.ExecuteFile(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(Of TaskStatus)(_status, TaskStatus.Failed)
        IO.File.Delete(_file)
    End Sub

    <Fact()>
    Public Sub execute_task_failed()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFileName, "111" & PingExe)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingArguments, _wwwgooglecom)
        Dim _task As New DeploymentSpike.Tasks.ExecuteFile(_defaultEmptySettings)
        Assert.Throws(Of System.ComponentModel.Win32Exception)(Sub()
                                                                   Dim _status As TaskStatus = _task.Execute
                                                               End Sub)
    End Sub



    <Fact()>
    Public Sub WasCommandSuccessful_null_parameter()
        Assert.True(DeploymentSpike.Tasks.ExecuteFile.WasCommandSuccessful(Nothing))
    End Sub

    <Fact()>
    Public Sub WasCommandSuccessful_true_parameter()
        Assert.True(DeploymentSpike.Tasks.ExecuteFile.WasCommandSuccessful("just a simple string"))
    End Sub

    <Fact()>
    Public Sub WasCommandSuccessful_false_parameter()
        Assert.False(DeploymentSpike.Tasks.ExecuteFile.WasCommandSuccessful("just a simple string" & Tasks.Constants.AccessDenied))
    End Sub



End Class
