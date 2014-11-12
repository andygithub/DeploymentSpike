Imports Ionic.Zip
Imports System.Reflection

Public Class CopyArtifactsFromZipFixture
    Private Const _ZipFileName As String = "tempzip.zip"
    Private Const _CoalesceFilter As String = "CoalesceFilter"

    <Fact()> _
    Public Sub null_settings_parameter()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Dim _task As New DeploymentSpike.Tasks.CopyArtifactsFromZip(Nothing)
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub valid_settings_get_settings()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Tasks.CopyArtifactsFromZip(_defaultEmptySettings)
        Dim _setttings As ISettings = _task.TaskSettings
        Assert.Equal(_setttings, _defaultEmptySettings)
    End Sub

    <Fact()>
    Public Sub logger_property()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigBackupFileName, String.Empty)
        Dim _task As New DeploymentSpike.Tasks.CopyArtifactsFromZip(_defaultEmptySettings)
        Assert.IsType(Of Castle.Core.Logging.NullLogger)(_task.Logger)
        _task.Logger = Nothing
        _task.TaskSettings = Nothing
    End Sub

    <Fact()> _
    Public Sub valid_settings_parameter_null_setting_dictionary()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Tasks.CopyArtifactsFromZip(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub valid_settings_invalid_zip_file()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigSourceZip, String.Empty)
        Dim _task As New DeploymentSpike.Tasks.CopyArtifactsFromZip(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub valid_settings_invalid_destination_folder()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigSourceZip, PurgeExistingArtifactsFixture.GetValidTempZipFile)
        Dim _task As New DeploymentSpike.Tasks.CopyArtifactsFromZip(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub valid_settings_invalid_zip_exist()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigSourceZip, PurgeExistingArtifactsFixture.GetInvalidTempFolder)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDestinationFolder, PurgeExistingArtifactsFixture.GetInvalidTempFolder)
        Dim _task As New DeploymentSpike.Tasks.CopyArtifactsFromZip(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub valid_settings_invalid_destination_folder_exist()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigSourceZip, PurgeExistingArtifactsFixture.GetValidTempZipFile)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDestinationFolder, PurgeExistingArtifactsFixture.GetInvalidTempFolder)
        Dim _task As New DeploymentSpike.Tasks.CopyArtifactsFromZip(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub valid_settings()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigSourceZip, PurgeExistingArtifactsFixture.GetValidTempZipFiles)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDestinationFolder, PurgeExistingArtifactsFixture.GetValidTempFolders)
        Dim _task As New DeploymentSpike.Tasks.CopyArtifactsFromZip(_defaultEmptySettings)
        Assert.True(_task.ValidateSettings)
    End Sub

    <Fact()> _
    Public Sub execute_task_unzip_all_to_destinations()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _tempSource As String = PurgeExistingArtifactsFixture.GetValidTempFolder() & CopyNewArtifactsFixture.Temp_source
        Dim _tempSourceZip As String = _tempSource & IO.Path.DirectorySeparatorChar & _ZipFileName
        Dim _tempDest As String = PurgeExistingArtifactsFixture.GetValidTempFolder() & CopyNewArtifactsFixture.Temp_dest
        IO.Directory.CreateDirectory(_tempSource & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Temp)
        IO.Directory.CreateDirectory(_tempDest)
        IO.Directory.CreateDirectory(_tempSource & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefolder)
        IO.File.AppendAllText(_tempSource & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefiletxt, _tempDest)
        IO.File.AppendAllText(_tempSource & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefolder & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefiletxt, _tempDest)
        Using zip1 As ZipFile = New ZipFile
            zip1.AddDirectory(_tempSource)
            zip1.Save(_tempSourceZip)
        End Using
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigSourceZip, _tempSourceZip)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDestinationFolder, _tempDest)
        Dim _task As New DeploymentSpike.Tasks.CopyArtifactsFromZip(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(TaskStatus.Completed, _status)
        Assert.True(IO.Directory.Exists(_tempDest & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefolder))
        Assert.True(IO.File.Exists(_tempDest & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefiletxt))
        IO.Directory.Delete(_tempDest, True)
        IO.Directory.Delete(_tempSource, True)
    End Sub

    <Fact()> _
    Public Sub execute_task_unzip_subset_to_destinations()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _tempSource As String = PurgeExistingArtifactsFixture.GetValidTempFolder() & CopyNewArtifactsFixture.Temp_source
        Dim _tempSourceZip As String = _tempSource & IO.Path.DirectorySeparatorChar & _ZipFileName
        Dim _tempDest As String = PurgeExistingArtifactsFixture.GetValidTempFolder() & CopyNewArtifactsFixture.Temp_dest
        IO.Directory.CreateDirectory(_tempSource & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Temp)
        IO.Directory.CreateDirectory(_tempDest)
        IO.Directory.CreateDirectory(_tempSource & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefolder)
        IO.File.AppendAllText(_tempSource & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefiletxt, _tempDest)
        IO.File.AppendAllText(_tempSource & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefolder & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefiletxt, _tempDest)
        Using zip1 As ZipFile = New ZipFile
            zip1.AddDirectory(_tempSource)
            zip1.Save(_tempSourceZip)
        End Using
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigSourceZip, _tempSourceZip)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDestinationFolder, _tempDest)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigZipFilter, PurgeExistingArtifactsFixture.Singlefolder & IO.Path.DirectorySeparatorChar)
        Dim _task As New DeploymentSpike.Tasks.CopyArtifactsFromZip(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(TaskStatus.Completed, _status)
        Assert.True(IO.Directory.Exists(_tempDest & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefolder))
        Assert.True(IO.File.Exists(_tempSource & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefolder & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefiletxt))
        Assert.False(IO.File.Exists(_tempDest & IO.Path.DirectorySeparatorChar & PurgeExistingArtifactsFixture.Singlefiletxt))
        IO.Directory.Delete(_tempDest, True)
        IO.Directory.Delete(_tempSource, True)
    End Sub

    <Fact()>
    Public Sub CoalesceFilter_index_exceeds_filterlist()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigZipFilter, PurgeExistingArtifactsFixture.Singlefolder & IO.Path.DirectorySeparatorChar)
        Dim _task As New DeploymentSpike.Tasks.CopyArtifactsFromZip(_defaultEmptySettings)
        Dim dynMethod As MethodInfo = _task.GetType().GetMethod(_CoalesceFilter, BindingFlags.NonPublic Or BindingFlags.Instance)
        Dim result As Object = dynMethod.Invoke(_task, New Object() {5})
        Assert.Null(result)
    End Sub

End Class
