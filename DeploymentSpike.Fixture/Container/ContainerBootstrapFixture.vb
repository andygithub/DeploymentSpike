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
Imports DeploymentSpike.Controller.Configuration
Imports Castle.Core.Logging
Imports DeploymentSpike.Controller.Interfaces

Namespace Container

    Public Class ContainerBootstrapFixture
        Implements IDisposable


        Dim _container As New WindsorContainer
        Dim _invalidcontainer As New WindsorContainer
        Dim _seperateContainer As New WindsorContainer
        Dim _logger As IMemoryLogging
        Dim _ComponentRegistration As Controller.Container.ComponentRegistration
        Dim _appConfiguration As ConfigurationSettingsManager
        Dim _currentAssembly As New Generic.List(Of String)(New String() {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name})

        Sub New()
            _logger = MockRepository.GenerateMock(Of IMemoryLogging)()
            _appConfiguration = New ConfigurationSettingsManager(Nothing, Nothing)
            _appConfiguration.LoadSetting(FixtureConstants.list_Key, FixtureConstants.list_Value)
            _ComponentRegistration = New Controller.Container.ComponentRegistration(_logger, _appConfiguration)
            _container = CType(_ComponentRegistration.BootstrapContainer(_currentAssembly), WindsorContainer)
        End Sub

        <Fact()> _
        Public Sub Null_logging_parameter()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Using _ComponentRegistration As New Controller.Container.ComponentRegistration(Nothing, _appConfiguration)
                                                            _ComponentRegistration.ToString()
                                                        End Using
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub Null_settings_parameter()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Using _ComponentRegistration As New Controller.Container.ComponentRegistration(_logger, Nothing)
                                                            _ComponentRegistration.ToString()
                                                        End Using
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub Null_assembly_bootstrap_parameter()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        _invalidcontainer = CType(_ComponentRegistration.BootstrapContainer(Nothing), WindsorContainer)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub All_logging_are_undefined()
            Dim nonTransient As IEnumerable(Of IHandler) = _container.Kernel.GetAssignableHandlers(GetType(ILogger)).Where(Function(controller)
                                                                                                                               Return controller.ComponentModel.LifestyleType <> LifestyleType.Undefined
                                                                                                                           End Function).ToList()
            Assert.Empty(nonTransient)
        End Sub

        <Fact()> _
        Public Sub All_notifications_are_transient()
            Dim nonTransient As IEnumerable(Of IHandler) = _container.Kernel.GetAssignableHandlers(GetType(INotification)).Where(Function(controller)
                                                                                                                                     Return controller.ComponentModel.LifestyleType <> LifestyleType.Transient
                                                                                                                                 End Function).ToList()
            Assert.Empty(nonTransient)
        End Sub

        <Fact()> _
        Public Sub All_tasks_are_transient()
            Dim nonTransient As IEnumerable(Of IHandler) = _container.Kernel.GetAssignableHandlers(GetType(ITask)).Where(Function(controller)
                                                                                                                             Return controller.ComponentModel.LifestyleType <> LifestyleType.Transient
                                                                                                                         End Function).ToList()
            Assert.Empty(nonTransient)
        End Sub

        <Fact()> _
        Public Sub All_tasks_no_container_logging()
            Using _compreg As New Controller.Container.ComponentRegistration(_logger, _appConfiguration)

                Using _seperateContainer = CType(_ComponentRegistration.BootstrapContainer(New Generic.List(Of String)(New String() {FixtureConstants.DeploymentSpikeTasks})), WindsorContainer)
                    Dim _count As Integer = _seperateContainer.Kernel.GetAssignableHandlers(GetType(ILogger)).Count
                    Assert.True(1 = _seperateContainer.Kernel.GetAssignableHandlers(GetType(ILogger)).Count)
                End Using
            End Using
        End Sub

        '<Fact()> _
        'Public Sub All_tasks_expose_themselves_as_service()
        '    Dim tasksWithWrongName As IEnumerable(Of IHandler) = _container.Kernel.GetAssignableHandlers(GetType(ITask)).Where(Function(controller) controller.ComponentModel.Services.[Single]() <> controller.ComponentModel.Implementation).ToList()

        '    Assert.Empty(tasksWithWrongName)
        'End Sub

        '<Fact()> _
        'Public Sub All_tasks_are_registered()
        '    ' Is<TType> is an helper, extension method from Windsor in the Castle.Core.Internal namespace
        '    ' which behaves like 'is' keyword in C# but at a Type, not instance level
        '    Dim allControllers = GetPublicClassesFromApplicationAssembly(Function(c) c.[Is](Of ITask)())
        '    Dim registeredControllers = GetImplementationTypesFor(GetType(ITask), _container)
        '    Assert.Equal(allControllers, registeredControllers)
        'End Sub

        '<Fact()> _
        'Public Sub All_and_only_tasks_have_Controllers_suffix()
        '    Dim allItems = GetPublicClassesFromApplicationAssembly(Function(c) c.Name.EndsWith("Task"))
        '    Dim registeredTasks = GetImplementationTypesFor(GetType(ITask), _container)
        '    Assert.Equal(allItems, registeredTasks)
        'End Sub

        '<Fact()> _
        'Public Sub All_and_only_controllers_live_in_task_namespace()
        '    Dim allControllers = GetPublicClassesFromApplicationAssembly(Function(c) c.[Namespace].Contains("Task"))
        '    Dim registeredControllers = GetImplementationTypesFor(GetType(ITask), _container)
        '    Assert.Equal(allControllers, registeredControllers)

        'End Sub

        'Private Function GetImplementationTypesFor(type As Type, container As IWindsorContainer) As Type()
        '    Return GetHandlersFor(type, container).[Select](Function(h) h.ComponentModel.Implementation).OrderBy(Function(t) t.Name).ToArray()
        'End Function

        'Private Function GetPublicClassesFromApplicationAssembly(whereCriteria As Predicate(Of Type)) As Type()
        '    Return GetType(ContainerBootstrapFixture).Assembly _
        '    .GetExportedTypes() _
        '    .Where(Function(t) t.IsClass) _
        '    .Where(Function(t) t.IsAbstract = False) _
        '    .Where(whereCriteria.Invoke) _
        '    .OrderBy(Function(t) t.Name) _
        '    .ToArray()
        'End Function

        'Private Function GetAllHandlers(container As IWindsorContainer) As IHandler()
        '    Return GetHandlersFor(GetType(Object), container)
        'End Function

        'Private Function GetHandlersFor(type As Type, container As IWindsorContainer) As IHandler()
        '    Return container.Kernel.GetAssignableHandlers(type)
        'End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then

                    _container.Dispose()
                    _invalidcontainer.Dispose()
                    _seperateContainer.Dispose()
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