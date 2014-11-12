Imports DeploymentSpike.Controller
Imports System.IO
Imports DeploymentSpike.Controller.Configuration
Imports DeploymentSpike.Controller.Factory
Imports DeploymentSpike.Controller.Utility
Imports Castle.Core.Logging

Public Class TaskStepsFactoryFixture
    Private Const _TestStepsFileName As String = "test_steps"
    Private Const _TestStepsConfigFileName As String = "test_steps_config"

    Dim _logger As ILogger
    Dim _appConfiguration As ConfigurationSettingsManager

    Sub New()
        _logger = MockRepository.GenerateMock(Of ILogger)()
        _appConfiguration = New ConfigurationSettingsManager(Nothing, Nothing)
        _appConfiguration.LoadSetting(FixtureConstants.list_Key, FixtureConstants.list_Value)
    End Sub

    <Fact()> _
    Public Sub factory_null_logger_parameter()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Dim _steps As IEnumerable(Of Interfaces.ITaskInformation) = TaskStepsFactory.Create(Nothing, Nothing, Nothing)
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub factory_null_settings_parameter()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Dim _steps As IEnumerable(Of Interfaces.ITaskInformation) = TaskStepsFactory.Create(Nothing, _logger, Nothing)
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub factory_null_filename_parameter()
        Dim _steps As IEnumerable(Of Interfaces.ITaskInformation) = TaskStepsFactory.Create(Nothing, _logger, _appConfiguration)
        Assert.Empty(_steps)
    End Sub

    <Fact()> _
    Public Sub factory_filename_doesnotexist_parameter()
        Dim _file As String = PurgeExistingArtifactsFixture.GetValidTempFolder() & "filenoexist.json"
        Dim _steps As IEnumerable(Of Interfaces.ITaskInformation) = TaskStepsFactory.Create(_file, _logger, _appConfiguration)
        Assert.Empty(_steps)
    End Sub

    <Fact()> _
    Public Sub loadtasksteps_filename_doesnotexist_parameter()
        Dim _steps As IEnumerable(Of Interfaces.ITaskInformation) = TaskStepsFactory.LoadTaskSteps(String.Empty)
        Assert.Empty(_steps)
    End Sub

    <Fact()> _
    Public Sub factory_filename_exists_parameter()
        'setup folder
        IO.Directory.CreateDirectory(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Constants.Config)
        Dim _file As String = _TestStepsFileName & Constants.ValidSettingsExtension
        Dim _taskConfigfile As String = _TestStepsConfigFileName & Constants.ValidSettingsExtension
        Dim _stepsInit As New List(Of Controller.Domain.TaskMetaInformation)
        _stepsInit.Add(New Controller.Domain.TaskMetaInformation() With {.StepNumber = 1, .Owners = "unknown", .TaskComponentName = FixtureConstants.MockTaskFullName, .TaskComponentConfiguration = _taskConfigfile})
        _stepsInit.Add(New Controller.Domain.TaskMetaInformation() With {.StepNumber = 2, .Owners = "unknown", .TaskComponentName = FixtureConstants.MockSecondTaskFullName, .TaskComponentConfiguration = _taskConfigfile})
        Using _writer1 As New FileStream(GetFullPathForFile(_TestStepsFileName), FileMode.Create)
            ServiceStack.Text.JsonSerializer.SerializeToStream(Of IEnumerable(Of Controller.Domain.TaskMetaInformation))(_stepsInit, _writer1)
        End Using
        Dim _steps As IEnumerable(Of Interfaces.ITaskInformation) = TaskStepsFactory.Create(_file, _logger, _appConfiguration)
        Assert.NotEmpty(_steps)
        Assert.True(_steps.Count = _stepsInit.Count)
        Assert.True(_steps(0).TaskComponentName = _stepsInit(0).TaskComponentName)
        IO.File.Delete(_file)
        IO.File.Delete(_taskConfigfile)
    End Sub

    <Fact()> _
    Public Sub loadtasksteps_filename_exists_empty_list()
        'setup folder
        IO.Directory.CreateDirectory(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Constants.Config)
        Dim _file As String = _TestStepsFileName & Constants.ValidSettingsExtension
        Dim _taskConfigfile As String = _TestStepsConfigFileName & Constants.ValidSettingsExtension
        Dim _stepsInit As New List(Of Controller.Domain.TaskMetaInformation)
        Using _writer1 As New FileStream(GetFullPathForFile(_TestStepsFileName), FileMode.Create)
            ServiceStack.Text.JsonSerializer.SerializeToStream(Of IEnumerable(Of Controller.Domain.TaskMetaInformation))(_stepsInit, _writer1)
        End Using
        Dim _steps As IEnumerable(Of Interfaces.ITaskInformation) = TaskStepsFactory.LoadTaskSteps(GetFullPathForFile(_TestStepsFileName))
        Assert.Empty(_steps)
        IO.File.Delete(_file)
        IO.File.Delete(_taskConfigfile)
    End Sub

    <Fact()> _
    Public Sub SaveTaskSteps_filename_exists_null_settings()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    TaskStepsFactory.SaveTaskSteps(Of ITaskInformation)(Nothing, Nothing, GetFullPathForFile(_TestStepsFileName))
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub SaveTaskSteps_filename_exists_null_list()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    TaskStepsFactory.SaveTaskSteps(Of ITaskInformation)(_appConfiguration, Nothing, GetFullPathForFile(_TestStepsFileName))
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub SaveTaskSteps_filename_exists_null_file()
        Dim _stepsInit As New List(Of Controller.Domain.TaskMetaInformation)
        _stepsInit.Add(New Controller.Domain.TaskMetaInformation() With {.StepNumber = 1, .Owners = "unknown", .TaskComponentName = FixtureConstants.MockTaskFullName, .TaskComponentConfiguration = String.Empty})
        _stepsInit.Add(New Controller.Domain.TaskMetaInformation() With {.StepNumber = 2, .Owners = "unknown", .TaskComponentName = FixtureConstants.MockSecondTaskFullName, .TaskComponentConfiguration = String.Empty})
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    TaskStepsFactory.SaveTaskSteps(Of ITaskInformation)(_appConfiguration, _stepsInit, Nothing)
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub SaveTaskSteps_valid_parameters()
        Dim _stepsInit As New List(Of Controller.Domain.TaskMetaInformation)
        _stepsInit.Add(New Controller.Domain.TaskMetaInformation() With {.StepNumber = 1, .Owners = "unknown", .TaskComponentName = FixtureConstants.MockTaskFullName, .TaskComponentConfiguration = String.Empty})
        _stepsInit.Add(New Controller.Domain.TaskMetaInformation() With {.StepNumber = 2, .Owners = "unknown", .TaskComponentName = FixtureConstants.MockSecondTaskFullName, .TaskComponentConfiguration = String.Empty})
        TaskStepsFactory.SaveTaskSteps(Of ITaskInformation)(_appConfiguration, _stepsInit, GetFullPathForFile(_TestStepsFileName))
        Assert.True(IO.File.Exists(GetFullPathForFile(_TestStepsFileName)))
        IO.File.Delete(GetFullPathForFile(_TestStepsFileName))
    End Sub

    Private Function GetFullPathForFile(filename As String) As String
        IO.Directory.CreateDirectory(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Constants.Config)
        Return IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Constants.Config & IO.Path.DirectorySeparatorChar & filename & Constants.ValidSettingsExtension
    End Function


End Class
