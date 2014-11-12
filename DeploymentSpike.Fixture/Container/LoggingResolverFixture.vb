'Imports DeploymentSpike.Controller
'Imports Castle.Windsor
'Imports Castle.Core
'Imports Castle.Core.Internal
'Imports Castle.Core.Resource
'Imports Castle.MicroKernel
'Imports Castle.MicroKernel.Handlers
'Imports Castle.MicroKernel.Registration
'Imports Castle.Windsor.Configuration.Interpreters
'Imports DeploymentSpike.Controller.Container
'Imports System.Reflection
'Imports Castle.DynamicProxy
'Imports DeploymentSpike.Controller.Configuration
'Imports DeploymentSpike.Controller.DefaultInstance


'Public Class LoggingResolverFixture
'    Private Const InvalidFileLoggingtest As String = "InvalidFileLogging.test"

'    Dim _logger As Interfaces.ILogging
'    Dim _ComponentRegistration As Container.ComponentRegistration
'    Dim _appConfiguration As ConfigurationSettingsManager

'    Sub New()
'        _logger = MockRepository.GenerateMock(Of Interfaces.ILogging)()
'        _appConfiguration = New ConfigurationSettingsManager(Nothing, Nothing)
'        _appConfiguration.LoadSetting(FixtureConstants.list_Key, FixtureConstants.list_Value)
'    End Sub

'    '<Fact()> _
'    'Public Sub resolve_logging_null_settings_null_kernel()
'    '    Using container = New WindsorContainer()
'    '        Assert.Throws(Of ArgumentNullException)(Sub()
'    '                                                    container.Kernel.Resolver.AddSubResolver(New LoggingResolver(Nothing, Nothing))
'    '                                                End Sub)
'    '    End Using
'    'End Sub

'    '<Fact()> _
'    'Public Sub resolve_logging_null_settings()
'    '    Using container = New WindsorContainer()
'    '        Assert.Throws(Of ArgumentNullException)(Sub()
'    '                                                    container.Kernel.Resolver.AddSubResolver(New LoggingResolver(Nothing, container.Kernel))
'    '                                                End Sub)
'    '    End Using
'    'End Sub

'    '<Fact()> _
'    'Public Sub resolve_logging_null_kernel()
'    '    Using container = New WindsorContainer()
'    '        Assert.Throws(Of ArgumentNullException)(Sub()
'    '                                                    container.Kernel.Resolver.AddSubResolver(New LoggingResolver(_appConfiguration, Nothing))
'    '                                                End Sub)
'    '    End Using
'    'End Sub

'    Private Function GetLoggerInstance(task As ITask) As IMemoryLogging
'        If ProxyUtil.IsProxy(task) Then
'            Dim _proxytask As ITask = CType(ProxyUtil.GetUnproxiedInstance(task), ITask)
'            Return _proxytask.GetPrivateFieldValue(Of IMemoryLogging)(FixtureConstants.loggerFieldName)
'        Else
'            Return task.GetPrivateFieldValue(Of IMemoryLogging)(FixtureConstants.loggerFieldName)
'        End If
'    End Function

'    <Fact()> _
'    Public Sub All_tasks_multiple_container_logging()

'        Using _compreg As New Container.ComponentRegistration(_logger, _appConfiguration)

'            Using _seperateContainer = CType(_compreg.BootstrapContainer(New Generic.List(Of String)(New String() {FixtureConstants.MultipleLogging})), WindsorContainer)
'                Dim _itask As ITask = _seperateContainer.Resolve(Of ITask)()
'                Dim _list As New List(Of String)
'                Array.ForEach(_seperateContainer.Kernel.GetAssignableHandlers(GetType(IMemoryLogging)), Sub(x)
'                                                                                                            _list.Add(x.ComponentModel.Name.ToString)
'                                                                                                            Debug.WriteLine(x.ComponentModel.Name)
'                                                                                                        End Sub)
'                Dim _t As IMemoryLogging = GetLoggerInstance(_itask)

'                Assert.Contains(Of String)(_t.GetType.ToString, _list)
'                Assert.IsType(Of DeploymentSpike.Fixture.MultipleLogging.ConsoleLog)(_t)
'            End Using
'        End Using
'    End Sub

'    <Fact()> _
'    Public Sub All_tasks_multiple_container_configured_logging_empty()
'        Dim _localappConfiguration As New ConfigurationSettingsManager(Nothing, Nothing)
'        _localappConfiguration.LoadSetting(Controller.Constants.ConfigLogClass, String.Empty)
'        Using _compreg As New Container.ComponentRegistration(_logger, _localappConfiguration)

'            Using _seperateContainer = CType(_compreg.BootstrapContainer(New Generic.List(Of String)(New String() {FixtureConstants.MultipleLogging})), WindsorContainer)
'                Dim _itask As ITask = _seperateContainer.Resolve(Of ITask)()
'                Dim _list As New List(Of String)
'                Array.ForEach(_seperateContainer.Kernel.GetAssignableHandlers(GetType(IMemoryLogging)), Sub(x)
'                                                                                                            _list.Add(x.ComponentModel.Name.ToString)
'                                                                                                            Debug.WriteLine(x.ComponentModel.Name)
'                                                                                                        End Sub)
'                Dim _t As IMemoryLogging = GetLoggerInstance(_itask)
'                Assert.Contains(Of String)(_t.GetType.ToString, _list)
'                Assert.IsType(Of DeploymentSpike.Fixture.MultipleLogging.ConsoleLog)(_t)
'            End Using
'        End Using
'    End Sub

'    <Fact()> _
'    Public Sub All_tasks_multiple_container_configured_logging_valid()
'        Dim _localappConfiguration As New ConfigurationSettingsManager(Nothing, Nothing)
'        _localappConfiguration.LoadSetting(Controller.Constants.ConfigLogClass, FixtureConstants.MemoryLogClassName)
'        Using _compreg As New Container.ComponentRegistration(_logger, _localappConfiguration)

'            Using _seperateContainer = CType(_compreg.BootstrapContainer(New Generic.List(Of String)(New String() {FixtureConstants.MultipleLogging})), WindsorContainer)
'                Dim _itask As ITask = _seperateContainer.Resolve(Of ITask)()
'                Dim _list As New List(Of String)
'                Array.ForEach(_seperateContainer.Kernel.GetAssignableHandlers(GetType(IMemoryLogging)), Sub(x)
'                                                                                                            _list.Add(x.ComponentModel.Name.ToString)
'                                                                                                            Debug.WriteLine(x.ComponentModel.Name)
'                                                                                                        End Sub)
'                Dim _t As IMemoryLogging = GetLoggerInstance(_itask)
'                Assert.Contains(Of String)(_t.GetType.ToString, _list)
'                Assert.IsType(Of DeploymentSpike.Fixture.MultipleLogging.MemoryLog)(_t)
'            End Using
'        End Using
'    End Sub

'    <Fact()> _
'    Public Sub All_tasks_multiple_container_invalid_configuration_logging_valid()
'        Dim _localappConfiguration As New ConfigurationSettingsManager(Nothing, Nothing)
'        _localappConfiguration.LoadSetting(Controller.Constants.ConfigLogClass, InvalidFileLoggingtest)
'        Using _compreg As New Container.ComponentRegistration(_logger, _localappConfiguration)

'            Using _seperateContainer = CType(_compreg.BootstrapContainer(New Generic.List(Of String)(New String() {FixtureConstants.MultipleLogging})), WindsorContainer)
'                Assert.Throws(Of Castle.MicroKernel.ComponentNotFoundException)(Sub()
'                                                                                    Dim _itask As ITask = _seperateContainer.Resolve(Of ITask)()
'                                                                                End Sub)
'            End Using
'        End Using
'    End Sub

'    <Fact()> _
'    Public Sub All_tasks_multiple_container_configured_logging_empty_assembly()
'        Dim _localappConfiguration As New ConfigurationSettingsManager(Nothing, Nothing)
'        _localappConfiguration.LoadSetting(Controller.Constants.ConfigLogClass, FixtureConstants.MemoryLogClassName)
'        Using _compreg As New Container.ComponentRegistration(_logger, _localappConfiguration)

'            Using _seperateContainer = CType(_compreg.BootstrapContainer(New Generic.List(Of String)(New String() {FixtureConstants.NoLogging})), WindsorContainer)
'                Dim _itask As ITask = _seperateContainer.Resolve(Of ITask)()
'                Dim _list As New List(Of String)
'                Array.ForEach(_seperateContainer.Kernel.GetAssignableHandlers(GetType(IMemoryLogging)), Sub(x)
'                                                                                                            _list.Add(x.ComponentModel.Name.ToString)
'                                                                                                            Debug.WriteLine(x.ComponentModel.Name)
'                                                                                                        End Sub)
'                Dim _t As IMemoryLogging = GetLoggerInstance(_itask)
'                Assert.Empty(_list)
'                Assert.IsType(Of ConsoleLogging)(_t)
'            End Using
'        End Using
'    End Sub

'    <Fact()> _
'    Public Sub All_tasks_multiple_container_empty_configuration_logging_invalid()
'        Dim _localappConfiguration As New ConfigurationSettingsManager(Nothing, Nothing)
'        _localappConfiguration.LoadSetting(Controller.Constants.ConfigLogClass, String.Empty)
'        Using _compreg As New Container.ComponentRegistration(_logger, _localappConfiguration)

'            Using _seperateContainer = CType(_compreg.BootstrapContainer(New Generic.List(Of String)(New String() {FixtureConstants.NoLogging})), WindsorContainer)
'                Dim _itask As ITask = _seperateContainer.Resolve(Of ITask)()
'                Dim _list As New List(Of String)
'                Array.ForEach(_seperateContainer.Kernel.GetAssignableHandlers(GetType(IMemoryLogging)), Sub(x)
'                                                                                                            _list.Add(x.ComponentModel.Name.ToString)
'                                                                                                            Debug.WriteLine(x.ComponentModel.Name)
'                                                                                                        End Sub)
'                Dim _t As IMemoryLogging = GetLoggerInstance(_itask)
'                Assert.Empty(_list)
'                Assert.IsType(Of ConsoleLogging)(_t)
'            End Using
'        End Using
'    End Sub

'End Class
