
Public Class CopyNewArtifactsFixture

    Public Const Temp_source As String = "temp_source"
    Public Const Temp_dest As String = "temp_dest"

    <Fact()> _
    Public Sub null_settings_parameter()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Dim _task As New DeploymentSpike.Tasks.CopyNewArtifacts(Nothing)
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub valid_settings_get_settings()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Tasks.CopyNewArtifacts(_defaultEmptySettings)
        Dim _setttings As ISettings = _task.TaskSettings
        Assert.Equal(_setttings, _defaultEmptySettings)
    End Sub

    <Fact()>
    Public Sub logger_property()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigBackupFileName, String.Empty)
        Dim _task As New DeploymentSpike.Tasks.CopyNewArtifacts(_defaultEmptySettings)
        Assert.IsType(Of Castle.Core.Logging.NullLogger)(_task.Logger)
        _task.Logger = Nothing
        _task.TaskSettings = Nothing
    End Sub

    <Fact()> _
    Public Sub valid_settings_parameter_null_setting_dictionary()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Tasks.CopyNewArtifacts(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub valid_settings_invalid_source_folder()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigSourceFolder, String.Empty)
        Dim _task As New DeploymentSpike.Tasks.CopyNewArtifacts(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub valid_settings_invalid_destination_folder()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigSourceFolder, PurgeExistingArtifactsFixture.GetValidTempFolder)
        Dim _task As New DeploymentSpike.Tasks.CopyNewArtifacts(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub valid_settings_invalid_folder_counts()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigSourceFolder, PurgeExistingArtifactsFixture.GetValidTempFolders)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDestinationFolder, PurgeExistingArtifactsFixture.GetValidTempFolder)
        Dim _task As New DeploymentSpike.Tasks.CopyNewArtifacts(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub valid_settings_invalid_folder_source()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigSourceFolder, PurgeExistingArtifactsFixture.GetInvalidTempFolder)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDestinationFolder, PurgeExistingArtifactsFixture.GetInvalidTempFolder)
        Dim _task As New DeploymentSpike.Tasks.CopyNewArtifacts(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub valid_settings_invalid_folder_destination()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigSourceFolder, PurgeExistingArtifactsFixture.GetValidTempFolder)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDestinationFolder, PurgeExistingArtifactsFixture.GetInvalidTempFolder)
        Dim _task As New DeploymentSpike.Tasks.CopyNewArtifacts(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub valid_settings()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigSourceFolder, PurgeExistingArtifactsFixture.GetValidTempFolders)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDestinationFolder, PurgeExistingArtifactsFixture.GetValidTempFolders)
        Dim _task As New DeploymentSpike.Tasks.CopyNewArtifacts(_defaultEmptySettings)
        Assert.True(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub execute_task_copy_one_folder()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _tempSource As String = PurgeExistingArtifactsFixture.GetValidTempFolder() & Temp_source
        Dim _tempDest As String = PurgeExistingArtifactsFixture.GetValidTempFolder() & Temp_dest
        IO.Directory.CreateDirectory(_tempSource & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Temp)
        IO.Directory.CreateDirectory(_tempDest)
        IO.Directory.CreateDirectory(_tempSource & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefolder)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigSourceFolder, _tempSource)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDestinationFolder, _tempDest)
        Dim _task As New DeploymentSpike.Tasks.CopyNewArtifacts(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(TaskStatus.Completed, _status)
        Assert.True(IO.Directory.Exists(_tempDest & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefolder))
        IO.Directory.Delete(_tempDest, True)
        IO.Directory.Delete(_tempSource, True)
    End Sub

    <Fact()> _
    Public Sub execute_task_copy_one_folder_one_file()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _tempSource As String = PurgeExistingArtifactsFixture.GetValidTempFolder() & Temp_source
        Dim _tempDest As String = PurgeExistingArtifactsFixture.GetValidTempFolder() & Temp_dest
        IO.Directory.CreateDirectory(_tempSource & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Temp)
        IO.Directory.CreateDirectory(_tempDest)
        IO.Directory.CreateDirectory(_tempSource & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefolder)
        IO.File.AppendAllText(_tempSource & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefiletxt, _tempDest)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigSourceFolder, _tempSource)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDestinationFolder, _tempDest)
        Dim _task As New DeploymentSpike.Tasks.CopyNewArtifacts(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(TaskStatus.Completed, _status)
        Assert.True(IO.Directory.Exists(_tempDest & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefolder))
        Assert.True(IO.File.Exists(_tempDest & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefiletxt))
        IO.Directory.Delete(_tempDest, True)
        IO.Directory.Delete(_tempSource, True)
    End Sub

End Class
