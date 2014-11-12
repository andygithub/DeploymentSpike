Imports DeploymentSpike.Controller
Imports Castle.Windsor
Imports Castle.Core
Imports Castle.Core.Internal
Imports Castle.Core.Resource
Imports Castle.MicroKernel
Imports Castle.MicroKernel.Handlers
Imports Castle.MicroKernel.Registration
Imports Castle.Windsor.Configuration.Interpreters

Imports System.Reflection
Imports DeploymentSpike.Interfaces
Imports DeploymentSpike.Controller.Container
Imports Castle.DynamicProxy
Imports DeploymentSpike.Controller.Configuration
Imports Castle.Core.Logging
Imports DeploymentSpike.Fixture.Mocks
Imports System.IO
Imports DeploymentSpike.Controller.Interfaces

Public Class TaskSettingsInjectionInterceptorFixture
    Implements IDisposable

    Dim _container As New WindsorContainer
    Dim _logger As IMemoryLogging
    Dim _containerLogger As ILogger
    Dim _ComponentRegistration As Controller.Container.ComponentRegistration
    Dim _appConfiguration As ConfigurationSettingsManager
    Dim _currentAssembly As New Generic.List(Of String)(New String() {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name})



    Sub New()
        _logger = MockRepository.GenerateMock(Of IMemoryLogging)()
        _containerLogger = MockRepository.GenerateMock(Of ILogger)()
        _appConfiguration = New ConfigurationSettingsManager(Nothing, Nothing)
        _appConfiguration.LoadSetting(FixtureConstants.list_Key, FixtureConstants.list_Value)
        _appConfiguration.LoadSetting(Controller.Utility.Constants.ConfigNotificationClass, FixtureConstants.list_Value1)
        _ComponentRegistration = New Controller.Container.ComponentRegistration(_logger, _appConfiguration)
        _container = CType(_ComponentRegistration.BootstrapContainer(_currentAssembly), WindsorContainer)
    End Sub

    <Fact()> _
    Public Sub items_bootstrapped_resolve_task_populate_meta_settings_valid_configuration_file()
        Dim _task As ITask = _container.Resolve(Of ITask)(FixtureConstants.MockTaskFullName)
        'setup folder
        Dim _componentFileName As String = FixtureConstants.TestFileComponentTest
        IO.Directory.CreateDirectory(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config)
        Dim _file As String = IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config & IO.Path.DirectorySeparatorChar & _componentFileName & Controller.Utility.Constants.ValidSettingsExtension
        Dim _defaultSettings As New Controller.Domain.TaskSettings
        _defaultSettings.SettingsLocation = FixtureConstants.NoLogging
        _defaultSettings.Settings.Add(FixtureConstants.list_Key, FixtureConstants.list_Value)
        SerializeObject(_file, _defaultSettings)
        Assert.True(IO.File.Exists(_file))

        _task.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.TaskComponentConfiguration = IO.Path.GetFileName(_file)}
        _task.Execute()
        Assert.Equal(_task.TaskSettings.SettingsLocation, _file)
        Assert.True(_task.TaskSettings.Settings.ContainsKey(FixtureConstants.list_Key))

        IO.File.Delete(_file)
    End Sub

    <Fact()> _
    Public Sub items_bootstrapped_resolve_task_populate_meta_settings_valid_configuration_file_keep_existing_diagnostic_setting()
        Dim _task As ITask = _container.Resolve(Of ITask)(FixtureConstants.MockTaskFullName)
        'setup folder
        Dim _componentFileName As String = FixtureConstants.TestFileComponentTest
        IO.Directory.CreateDirectory(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config)
        Dim _file As String = IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config & IO.Path.DirectorySeparatorChar & _componentFileName & Controller.Utility.Constants.ValidSettingsExtension
        Dim _defaultSettings As New Controller.Domain.TaskSettings
        _defaultSettings.SettingsLocation = FixtureConstants.NoLogging
        _defaultSettings.Settings.Add(FixtureConstants.list_Key, FixtureConstants.list_Value)
        _defaultSettings.Settings.Add(Utility.Constants.ConfigExecutionSwitch, CStr(ExecutionType.Diagnostic))
        SerializeObject(_file, _defaultSettings)
        Assert.True(IO.File.Exists(_file))

        _task.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.TaskComponentConfiguration = IO.Path.GetFileName(_file)}
        _task.Execute()
        Assert.Equal(_task.TaskSettings.SettingsLocation, _file)
        Assert.True(_task.TaskSettings.Settings.ContainsKey(FixtureConstants.list_Key))
        Assert.True(_task.TaskSettings.GetExecutionFlag = ExecutionType.Diagnostic)

        IO.File.Delete(_file)
    End Sub

    <Fact()> _
    Public Sub items_bootstrapped_resolve_task_populate_meta_settings_valid_configuration_setting_fail_execution()
        Dim _task As ITask = _container.Resolve(Of ITask)(FixtureConstants.MockTaskFailExecuteFullName)
        'setup folder
        Dim _componentFileName As String = FixtureConstants.TestFileComponentTest
        IO.Directory.CreateDirectory(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config)
        Dim _file As String = IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config & IO.Path.DirectorySeparatorChar & _componentFileName & Controller.Utility.Constants.ValidSettingsExtension
        Dim _defaultSettings As New Controller.Domain.TaskSettings
        _defaultSettings.SettingsLocation = FixtureConstants.NoLogging
        _defaultSettings.Settings.Add(FixtureConstants.list_Key, FixtureConstants.list_Value)
        SerializeObject(_file, _defaultSettings)
        Assert.True(IO.File.Exists(_file))

        _task.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.TaskComponentConfiguration = IO.Path.GetFileName(_file)}
        Dim _status As TaskStatus = _task.Execute
        Assert.True(_status = TaskStatus.Failed)
        Assert.Equal(_task.TaskSettings.SettingsLocation, _file)
        Assert.True(_task.TaskSettings.Settings.ContainsKey(FixtureConstants.list_Key))

        IO.File.Delete(_file)
    End Sub

    <Fact()> _
    Public Sub items_bootstrapped_resolve_task_populate_meta_settings_valid_configuration_setting_fail_validate()
        Dim _task As ITask = _container.Resolve(Of ITask)(FixtureConstants.MockTaskFailValidateFullName)
        'setup folder
        Dim _componentFileName As String = FixtureConstants.TestFileComponentTest
        IO.Directory.CreateDirectory(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config)
        Dim _file As String = IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config & IO.Path.DirectorySeparatorChar & _componentFileName & Controller.Utility.Constants.ValidSettingsExtension
        Dim _defaultSettings As New Controller.Domain.TaskSettings
        _defaultSettings.SettingsLocation = FixtureConstants.NoLogging
        _defaultSettings.Settings.Add(FixtureConstants.list_Key, FixtureConstants.list_Value)
        SerializeObject(_file, _defaultSettings)
        Assert.True(IO.File.Exists(_file))

        _task.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.TaskComponentConfiguration = IO.Path.GetFileName(_file)}
        Dim _status As TaskStatus = _task.Execute
        Assert.True(_status = TaskStatus.Failed)
        Assert.Equal(_task.TaskSettings.SettingsLocation, _file)
        Assert.True(_task.TaskSettings.Settings.ContainsKey(FixtureConstants.list_Key))

        IO.File.Delete(_file)
    End Sub

    <Fact()> _
    Public Sub items_bootstrapped_resolve_task_populate_meta_settings_valid_configuration_setting_merge_component_settings()
        Dim _componentFileName As String = FixtureConstants.TestFileComponentTest
        IO.Directory.CreateDirectory(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config)
        Dim _file As String = IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config & IO.Path.DirectorySeparatorChar & _componentFileName & Controller.Utility.Constants.ValidSettingsExtension
        Dim _taskConfigfile As String = IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config & IO.Path.DirectorySeparatorChar & FixtureConstants.MockTaskFailValidateFullName & Controller.Utility.Constants.ValidSettingsExtension
        Dim _defaultSettings As New Controller.Domain.TaskSettings
        _defaultSettings.SettingsLocation = FixtureConstants.NoLogging
        _defaultSettings.Settings.Add(FixtureConstants.list_Key, FixtureConstants.list_Value)
        SerializeObject(_file, _defaultSettings)
        Assert.True(IO.File.Exists(_file))
        Dim _componentdefaultSettings As New Controller.Domain.TaskSettings
        _componentdefaultSettings.Settings.Add(FixtureConstants.list_Keya, FixtureConstants.list_Valuea)
        SerializeObject(_taskConfigfile, _componentdefaultSettings)
        Assert.True(IO.File.Exists(_taskConfigfile))

        Dim _task As ITask = _container.Resolve(Of ITask)(FixtureConstants.MockTaskFailValidateFullName)

        _task.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.TaskComponentConfiguration = IO.Path.GetFileName(_file)}
        Dim _status As TaskStatus = _task.Execute
        Assert.True(_status = TaskStatus.Failed)
        Assert.Equal(_task.TaskSettings.SettingsLocation, _file)
        Assert.True(_task.TaskSettings.Settings.ContainsKey(FixtureConstants.list_Key))
        Assert.True(_task.TaskSettings.Settings.ContainsKey(FixtureConstants.list_Keya))

        IO.File.Delete(_file)
        IO.File.Delete(_taskConfigfile)
    End Sub

    <Fact()> _
    Public Sub items_bootstrapped_resolve_task_populate_meta_settings_valid_configuration_setting_merge_notification_component_settings()
        Dim _componentFileName As String = FixtureConstants.TestFileComponentTest
        IO.Directory.CreateDirectory(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config)
        Dim _file As String = IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config & IO.Path.DirectorySeparatorChar & _componentFileName & Controller.Utility.Constants.ValidSettingsExtension
        Dim _taskConfigfile As String = IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config & IO.Path.DirectorySeparatorChar & FixtureConstants.MockNotificationFullName & Controller.Utility.Constants.ValidSettingsExtension
        Dim _defaultSettings As New Controller.Domain.TaskSettings
        _defaultSettings.SettingsLocation = FixtureConstants.NoLogging
        _defaultSettings.Settings.Add(FixtureConstants.list_Key, FixtureConstants.list_Value)
        SerializeObject(_file, _defaultSettings)
        Assert.True(IO.File.Exists(_file))
        Dim _componentdefaultSettings As New Controller.Domain.TaskSettings
        _componentdefaultSettings.Settings.Add(FixtureConstants.list_Keya, FixtureConstants.list_Valuea)
        SerializeObject(_taskConfigfile, _componentdefaultSettings)
        Assert.True(IO.File.Exists(_taskConfigfile))

        Dim _task As ITask = _container.Resolve(Of ITask)(FixtureConstants.MockTaskFailValidateFullName)

        _task.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.TaskComponentConfiguration = IO.Path.GetFileName(_file)}
        Dim _status As TaskStatus = _task.Execute
        Assert.True(_status = TaskStatus.Failed)
        Assert.Equal(_task.TaskSettings.SettingsLocation, _file)
        Assert.True(_task.TaskSettings.Settings.ContainsKey(FixtureConstants.list_Key))

        IO.File.Delete(_file)
        IO.File.Delete(_taskConfigfile)
    End Sub

    <Fact()> _
    Public Sub items_bootstrapped_resolve_task_populate_meta_settings_valid_configuration_invalid_mocked_notification()
        _container.Register(Component.For(Of INotification)().Instance(GetNotificationMock))
        Dim _task As ITask = _container.Resolve(Of ITask)(FixtureConstants.MockTaskFailValidateFullName)
        'setup folder
        Dim _componentFileName As String = FixtureConstants.TestFileComponentTest
        IO.Directory.CreateDirectory(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config)
        Dim _file As String = IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config & IO.Path.DirectorySeparatorChar & _componentFileName & Controller.Utility.Constants.ValidSettingsExtension
        Dim _defaultSettings As New Controller.Domain.TaskSettings
        _defaultSettings.SettingsLocation = FixtureConstants.NoLogging
        _defaultSettings.Settings.Add(FixtureConstants.list_Key, FixtureConstants.list_Value)
        SerializeObject(_file, _defaultSettings)
        Assert.True(IO.File.Exists(_file))

        _task.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.TaskComponentConfiguration = IO.Path.GetFileName(_file)}
        Dim _status As TaskStatus = _task.Execute
        Assert.True(_status = TaskStatus.Failed)
        Assert.Equal(_task.TaskSettings.SettingsLocation, _file)
        Assert.True(_task.TaskSettings.Settings.ContainsKey(FixtureConstants.list_Key))

        IO.File.Delete(_file)
    End Sub

    <Fact()> _
    Public Sub items_bootstrapped_resolve_task_populate_meta_settings_valid_configuration_valid_mocked_notification()
        _container.Register(Component.For(Of INotification)().Instance(GetNotificationMock))
        Dim _task As ITask = _container.Resolve(Of ITask)(FixtureConstants.MockTaskFullName)
        'setup folder
        Dim _componentFileName As String = FixtureConstants.TestFileComponentTest
        IO.Directory.CreateDirectory(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config)
        Dim _file As String = IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.Config & IO.Path.DirectorySeparatorChar & _componentFileName & Controller.Utility.Constants.ValidSettingsExtension
        Dim _defaultSettings As New Controller.Domain.TaskSettings
        _defaultSettings.SettingsLocation = FixtureConstants.NoLogging
        _defaultSettings.Settings.Add(FixtureConstants.list_Key, FixtureConstants.list_Value)
        SerializeObject(_file, _defaultSettings)
        Assert.True(IO.File.Exists(_file))

        _task.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.TaskComponentConfiguration = IO.Path.GetFileName(_file)}
        Dim _status As TaskStatus = _task.Execute
        Assert.True(_status = TaskStatus.Completed)
        Assert.Equal(_task.TaskSettings.SettingsLocation, _file)
        Assert.True(_task.TaskSettings.Settings.ContainsKey(FixtureConstants.list_Key))

        IO.File.Delete(_file)
    End Sub

    Private Sub SerializeObject(settingsFile As String, value As ISettings)
        Using _writer As New FileStream(settingsFile, FileMode.Create)
            ServiceStack.Text.JsonSerializer.SerializeToStream(Of ISettings)(value, _writer)
        End Using
    End Sub

    Private Function GetNotificationMock() As INotification
        Dim _item As INotification = MockRepository.GenerateStub(Of INotification)()
        _item.Stub(Function(x) As String
                       Return x.Name
                   End Function).Return("StubName")
        _item.Stub(Function(x) As Boolean
                       Return x.NotifyStarted(Nothing)
                   End Function).IgnoreArguments.Return(True)
        _item.Stub(Function(x) As Boolean
                       Return x.NotifyCompletedSuccess(Nothing)
                   End Function).IgnoreArguments.Return(True)
        _item.Stub(Function(x) As Boolean
                       Return x.NotifyCompletedFailure(Nothing)
                   End Function).IgnoreArguments.Return(True)
        _item.Stub(Function(x) As Boolean
                       Return x.ValidateSettings()
                   End Function).Return(True)
        _item.Settings = New Controller.Domain.TaskSettings
        _item.Settings.Settings.Add(Controller.Utility.Constants.ConfigNotifyStart, Boolean.FalseString)
        _item.Settings.Settings.Add(Controller.Utility.Constants.ConfigNotifySuccess, Boolean.FalseString)
        _item.Settings.Settings.Add(Controller.Utility.Constants.ConfigNotifyFail, Boolean.FalseString)
        Return _item
    End Function

    <Fact()> _
    Public Sub task_setting_validation_interceptor_null_constructor()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Dim _interceptor As New TaskSettingsInjectionInterceptor(Nothing, Nothing)
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub task_setting_validation_interceptor_null_settings_constructor()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Dim _interceptor As New TaskSettingsInjectionInterceptor(_containerLogger, Nothing)
                                                End Sub)
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                _container.Dispose()
                _ComponentRegistration.Dispose()
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
