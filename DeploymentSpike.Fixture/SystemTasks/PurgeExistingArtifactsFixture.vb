
Public Class PurgeExistingArtifactsFixture

    Public Const Temp As String = "temp"
    Public Const Singlefolder As String = "singlefolder"
    Public Const Singlefiletxt As String = "singlefile.txt"
    Private Const ExtensionLog As String = ".log"
    Private Const ExtensionZip As String = ".zip"

    <Fact()> _
    Public Sub null_logging_parameter()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Dim _task As New DeploymentSpike.Tasks.PurgeExistingArtifacts(Nothing)
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub valid_settings_get_settings()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Dim _setttings As ISettings = _task.TaskSettings
        Assert.Equal(_setttings, _defaultEmptySettings)
    End Sub

    <Fact()>
    Public Sub logger_property()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigBackupFileName, String.Empty)
        Dim _task As New DeploymentSpike.Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Assert.IsType(Of Castle.Core.Logging.NullLogger)(_task.Logger)
        _task.Logger = Nothing
        _task.TaskSettings = Nothing
    End Sub

    <Fact()> _
    Public Sub valid_settings_parameter_null_setting_dictionary()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub valid_settings_invalid_purge_folder()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigPurgeFolder, GetInvalidTempFolder)
        Dim _task As New DeploymentSpike.Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub valid_settings_valid_purge_folder()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigPurgeFolder, GetValidTempFolder)
        Dim _task As New DeploymentSpike.Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Assert.True(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub valid_settings_invalid_purge_folders()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigPurgeFolder, GetInvalidTempFolders)
        Dim _task As New DeploymentSpike.Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub valid_settings_valid_purge_folders()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigPurgeFolder, GetValidTempFolders)
        Dim _task As New DeploymentSpike.Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Assert.True(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub execute_task_delete_one_folder_in_use()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _tempRoot As String = GetValidTempFolder()
        IO.Directory.CreateDirectory(_tempRoot & IO.Path.DirectorySeparatorChar & Temp)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigPurgeFolder, _tempRoot)
        Dim _task As New DeploymentSpike.Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Assert.Throws(Of System.IO.IOException)(Sub()
                                                    Dim _status As TaskStatus = _task.Execute
                                                End Sub)

    End Sub

    <Fact()> _
    Public Sub execute_task_delete_one_folder()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _tempRoot As String = GetValidTempFolder() & Temp
        IO.Directory.CreateDirectory(_tempRoot & IO.Path.DirectorySeparatorChar & Temp)
        IO.Directory.CreateDirectory(_tempRoot & IO.Path.DirectorySeparatorChar & Singlefolder)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigPurgeFolder, _tempRoot)
        Dim _task As New DeploymentSpike.Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(TaskStatus.Completed, _status)
    End Sub

    <Fact()> _
    Public Sub execute_task_delete_one_folder_one_file()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _tempRoot As String = GetValidTempFolder() & Temp
        IO.Directory.CreateDirectory(_tempRoot & IO.Path.DirectorySeparatorChar & Temp)
        IO.Directory.CreateDirectory(_tempRoot & IO.Path.DirectorySeparatorChar & Singlefolder)
        IO.File.AppendAllText(_tempRoot & IO.Path.DirectorySeparatorChar & Singlefiletxt, Temp)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigPurgeFolder, _tempRoot)
        Dim _task As New DeploymentSpike.Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(TaskStatus.Completed, _status)
    End Sub

    Public Shared Function GetValidTempFolder() As String
        Return IO.Path.GetTempPath
    End Function

    Public Shared Function GetInvalidTempFolder() As String
        Return GetValidTempFolder() & "_"
    End Function

    Public Shared Function GetValidTempFolders() As String
        Return GetValidTempFolder() & Controller.Utility.Constants.Comma & GetValidTempFolder()
    End Function

    Public Shared Function GetInvalidTempFolders() As String
        Return GetInvalidTempFolder() & Controller.Utility.Constants.Comma & GetInvalidTempFolder()
    End Function

    Public Shared Function GetValidTempZipFile() As String
        Dim _temp As String = IO.Path.GetTempPath & IO.Path.DirectorySeparatorChar & Temp & ExtensionZip
        IO.File.AppendAllText(_temp, Temp)
        Return _temp
    End Function

    Public Shared Function GetValidTempZipFiles() As String
        Return GetValidTempZipFile() & Controller.Utility.Constants.Comma & GetValidTempZipFile()
    End Function

    Public Shared Function GetInvalidTempTextFile() As String
        Return IO.Path.GetTempFileName & ExtensionLog
    End Function

    Public Shared Function GetValidTempTextFile() As String
        Dim _temp As String = IO.Path.GetTempPath & IO.Path.DirectorySeparatorChar & Temp & ExtensionLog
        IO.File.WriteAllText(_temp, Temp)
        Return _temp
    End Function

    Public Shared Function GetValidTempTextFile(content As String) As String
        Dim _temp As String = IO.Path.GetTempPath & IO.Path.DirectorySeparatorChar & Temp & ExtensionLog
        IO.File.WriteAllText(_temp, content)
        Return _temp
    End Function


End Class
