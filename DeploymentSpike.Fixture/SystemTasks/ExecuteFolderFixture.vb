
Public Class ExecuteFolderFixture

    <Fact()>
    Public Sub null_logging_parameter()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Dim _task As New DeploymentSpike.Tasks.ExecuteFolder(Nothing)
                                                End Sub)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_null_setting_dictionary()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Tasks.ExecuteFolder(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
        _task.Logger = Nothing
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_invalid_folder()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFolder, PurgeExistingArtifactsFixture.GetInvalidTempFolder)
        Dim _task As New DeploymentSpike.Tasks.ExecuteFolder(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_valid_folder()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFolder, IO.Directory.GetCurrentDirectory)
        Dim _task As New DeploymentSpike.Tasks.ExecuteFolder(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_valid_extension()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFolder, IO.Directory.GetCurrentDirectory)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFileExtension, "*.bin")
        Dim _task As New DeploymentSpike.Tasks.ExecuteFolder(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_valid()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFolder, IO.Directory.GetCurrentDirectory)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFileExtension, "*.bin")
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingArguments, "{0}")
        Dim _task As New DeploymentSpike.Tasks.ExecuteFolder(_defaultEmptySettings)
        Assert.True(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub execute_task_folder_does_not_have_extension()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFolder, IO.Directory.GetCurrentDirectory)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFileExtension, "*.bat")
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingArguments, "{0}")
        Dim _task As New DeploymentSpike.Tasks.ExecuteFolder(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(Of TaskStatus)(_status, TaskStatus.Completed)
    End Sub

    <Fact()>
    Public Sub execute_task_folder_has_one_file()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _file As String = GetValidBatchFile()
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFolder, IO.Directory.GetCurrentDirectory)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFileExtension, "*.bat")
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingArguments, "{0}")
        Dim _task As New DeploymentSpike.Tasks.ExecuteFolder(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(Of TaskStatus)(_status, TaskStatus.Completed)
        IO.File.Delete(_file)
    End Sub

    <Fact()>
    Public Sub execute_task_folder_has_one_sql_file()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _file As String = GetValidSQLFile()
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFolder, IO.Directory.GetCurrentDirectory)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFileName, "sqlcmd")
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFileExtension, "*.sql")
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingArguments, "-s (local) -i ""{0}""")
        Dim _task As New DeploymentSpike.Tasks.ExecuteFolder(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(Of TaskStatus)(_status, TaskStatus.Completed)
        IO.File.Delete(_file)
    End Sub

    <Fact()>
    Public Sub execute_task_folder_has_one_file_redirect_false()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _file As String = GetValidBatchFile()
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFolder, IO.Directory.GetCurrentDirectory)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFileExtension, "*.bat")
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingArguments, "{0}")
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingRedirect, "False")
        Dim _task As New DeploymentSpike.Tasks.ExecuteFolder(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(Of TaskStatus)(_status, TaskStatus.Completed)
        IO.File.Delete(_file)
    End Sub

    <Fact()>
    Public Sub execute_task_folder_has_one_file_fails()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _file As String = GetValidErrorBatchFile()
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFolder, IO.Directory.GetCurrentDirectory)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingFileExtension, "*.bat")
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExecutingArguments, "{0}")
        Dim _task As New DeploymentSpike.Tasks.ExecuteFolder(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(Of TaskStatus)(_status, TaskStatus.Failed)
        IO.File.Delete(_file)
    End Sub

    <Fact()>
    Public Sub WasCommandSuccessful_null_parameter()
        Assert.True(DeploymentSpike.Tasks.ExecuteFolder.WasCommandSuccessful(Nothing))
    End Sub

    <Fact()>
    Public Sub WasCommandSuccessful_true_parameter()
        Assert.True(DeploymentSpike.Tasks.ExecuteFolder.WasCommandSuccessful("just a simple string"))
    End Sub

    <Fact()>
    Public Sub WasCommandSuccessful_false_parameter()
        Assert.False(DeploymentSpike.Tasks.ExecuteFolder.WasCommandSuccessful("just a simple string" & Tasks.Constants.AccessDenied))
    End Sub

    Public Shared Function GetValidBatchFile() As String
        Dim _temp As String = IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & "simple.bat"
        IO.File.AppendAllText(_temp, "dir")
        Return _temp
    End Function

    Public Shared Function GetValidErrorBatchFile() As String
        Dim _temp As String = IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & "simple.bat"
        IO.File.AppendAllText(_temp, "echo error")
        Return _temp
    End Function

    Public Shared Function GetValidSQLFile() As String
        Dim _temp As String = IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & "simple.sql"
        IO.File.AppendAllText(_temp, "sp_who")
        Return _temp
    End Function

End Class
