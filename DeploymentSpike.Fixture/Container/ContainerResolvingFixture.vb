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
Imports DeploymentSpike.Controller.Interfaces
Imports DeploymentSpike.Controller.Utility

Namespace Container

    Public Class ContainerResolvingFixture
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
        Public Sub items_bootstrapped_resolve_tasks()
            Dim _tasks As IEnumerable(Of ITask) = _container.ResolveAll(Of ITask)()
            _container.Kernel.GetAssignableHandlers(GetType(ITask)).ForEach(Sub(x)
                                                                                Debug.WriteLine(x.ComponentModel.Name)
                                                                            End Sub)
            If Not ProxyUtil.IsProxy(_tasks(0)) Then
                Assert.True(_tasks.OfType(Of ContainerMockTask)().Any())
                Assert.True(_tasks.OfType(Of ContainerSecondMockTask)().Any())
            Else
                Assert.IsType(Of ContainerSecondMockTask)(ProxyUtil.GetUnproxiedInstance(_tasks(0)))
                Assert.IsType(Of ContainerMockTask)(ProxyUtil.GetUnproxiedInstance(_tasks(1)))
            End If
        End Sub

        <Fact()> _
        Public Sub items_bootstrapped_resolve_task()
            Dim _task As ITask = _container.Resolve(Of ITask)(FixtureConstants.MockTaskFullName)
            _container.Kernel.GetAssignableHandlers(GetType(ITask)).ForEach(Sub(x)
                                                                                Debug.WriteLine(x.ComponentModel.Name)
                                                                            End Sub)
            If ProxyUtil.IsProxy(_task) Then
                Assert.IsType(Of ContainerMockTask)(ProxyUtil.GetUnproxiedInstance(_task))
            Else
                'check for non-proxied type
                Assert.IsType(Of ContainerMockTask)(_task)
            End If
        End Sub

        <Fact()> _
        Public Sub items_bootstrapped_resolve_task_populate_constructor_parameters()
            Dim _task As ITask = _container.Resolve(Of ITask)(FixtureConstants.MockTaskFullName)
            _container.Kernel.GetAssignableHandlers(GetType(ITask)).ForEach(Sub(x)
                                                                                Debug.WriteLine(x.ComponentModel.Name)
                                                                            End Sub)
            _task.Execute()
            If ProxyUtil.IsProxy(_task) Then
                Assert.IsType(Of ContainerMockTask)(ProxyUtil.GetUnproxiedInstance(_task))
            Else
                'check for non-proxied type
                Assert.IsType(Of ContainerMockTask)(_task)
            End If
        End Sub

        <Fact()> _
        Public Sub items_bootstrapped_resolve_task_populate_constructor_parameters_task_throws_exception_interceptor_handles()
            Dim _task As ITask = _container.Resolve(Of ITask)(FixtureConstants.MockSecondTaskFullName)
            _container.Kernel.GetAssignableHandlers(GetType(ITask)).ForEach(Sub(x)
                                                                                Debug.WriteLine(x.ComponentModel.Name)
                                                                            End Sub)

            _task.Execute()
            If ProxyUtil.IsProxy(_task) Then
                Assert.IsType(Of ContainerSecondMockTask)(ProxyUtil.GetUnproxiedInstance(_task))
            Else
                'check for non-proxied type
                Assert.IsType(Of ContainerSecondMockTask)(_task)
            End If
        End Sub

        <Fact()> _
        Public Sub items_bootstrapped_resolve_logging()
            Dim _task As ILogger = _container.Resolve(Of ILogger)()
            _container.Kernel.GetAssignableHandlers(GetType(ILogger)).ForEach(Sub(x)
                                                                                  Debug.WriteLine(x.ComponentModel.Name)
                                                                              End Sub)
            Assert.IsType(Of Castle.Services.Logging.Log4netIntegration.Log4netLogger)(_task)
        End Sub

        <Fact()> _
        Public Sub container_interface_to_single_task()
            Using container = New WindsorContainer()
                'add resolvers for dependency classes
                container.Kernel.Resolver.AddSubResolver(New SettingResolver(_containerLogger, _appConfiguration))

                container.Register(Component.[For](Of ITask)().ImplementedBy(Of ContainerMockTask)())
                Dim _task As ITask = container.Resolve(Of ITask)()

                Assert.IsAssignableFrom(Of ContainerMockTask)(_task)
            End Using
        End Sub

        <Fact()> _
        Public Sub container_interface_to_multiple_tasks()
            Using container = New WindsorContainer()
                'add resolvers for dependency classes
                container.Kernel.Resolver.AddSubResolver(New SettingResolver(_containerLogger, _appConfiguration))

                container.Register(Component.[For](Of ITask)().ImplementedBy(Of ContainerMockTask)())
                container.Register(Component.[For](Of ITask)().ImplementedBy(Of ContainerSecondMockTask)())

                Dim _tasks As IEnumerable(Of ITask) = container.ResolveAll(Of ITask)()

                Assert.True(_tasks.OfType(Of ContainerMockTask)().Any())
                Assert.True(_tasks.OfType(Of ContainerSecondMockTask)().Any())
            End Using
        End Sub

        <Fact()> _
        Public Sub register_all_assembly_tasks()
            Using container = New WindsorContainer()
                'add resolvers for dependency classes
                container.Kernel.Resolver.AddSubResolver(New SettingResolver(_containerLogger, _appConfiguration))

                container.Register(Classes.FromThisAssembly().BasedOn(Of ITask).WithServiceAllInterfaces)
                container.Kernel.GetAssignableHandlers(GetType(ITask)).ForEach(Sub(x)
                                                                                   Debug.WriteLine(x.ComponentModel.Name)
                                                                               End Sub)
                Dim _tasks As IEnumerable(Of ITask) = container.ResolveAll(Of ITask)()
                Assert.True(_tasks.OfType(Of ContainerMockTask)().Any())
                Assert.True(_tasks.OfType(Of ContainerSecondMockTask)().Any())
            End Using
        End Sub

        <Fact()> _
        Public Sub resolve_specific_task_from_assembly()
            Using container = New WindsorContainer()
                'add resolvers for dependency classes
                container.Kernel.Resolver.AddSubResolver(New SettingResolver(_containerLogger, _appConfiguration))

                container.Register(Classes.FromThisAssembly().BasedOn(Of ITask).WithServiceAllInterfaces)

                container.Kernel.GetAssignableHandlers(GetType(ITask)).ForEach(Sub(x)
                                                                                   Debug.WriteLine(x.ComponentModel.Name)
                                                                               End Sub)

                Dim _tasks As ITask = container.Resolve(Of ITask)(FixtureConstants.MockTaskFullName)
                Assert.IsType(Of ContainerMockTask)(_tasks)
            End Using
        End Sub

        <Fact()> _
        Public Sub resolve_invalid_task_from_assembly()
            Using container = New WindsorContainer()
                container.Register(Classes.FromThisAssembly().BasedOn(Of ITask).WithServiceAllInterfaces)

                Assert.Throws(Of Castle.MicroKernel.ComponentNotFoundException)(Sub()
                                                                                    Dim _tasks As ITask = container.Resolve(Of ITask)(FixtureConstants.DeploymentSpikeFixtureDoesNotExist)
                                                                                End Sub)
            End Using
        End Sub

        <Fact()> _
        Public Sub logging_interceptor_null_constructor()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _interceptor As New LoggingInterceptor(Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub error_interceptor_null_constructor()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _interceptor As New ErrorHandlingInterceptor(Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub notification_interceptor_null_constructor_logging()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _interceptor As New NotificationInterceptor(Nothing, Nothing, Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub null_reflection_argument()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _interceptor As String = Nothing
                                                        _interceptor.GetPrivateFieldValue(Of String)(Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub invalid_propert_reflection_argument()
            Assert.Throws(Of ArgumentOutOfRangeException)(Sub()
                                                              Dim _interceptor As New NotificationLoggingInterceptor(_containerLogger)
                                                              _interceptor.GetPrivateFieldValue(Of String)(FixtureConstants.Initial)
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

End Namespace