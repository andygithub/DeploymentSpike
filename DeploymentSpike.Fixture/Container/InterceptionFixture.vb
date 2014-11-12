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
Imports DeploymentSpike.Controller.Interfaces
Imports Castle.Core.Logging
Imports DeploymentSpike.Fixture.Mocks


Namespace Container

    Public Class InterceptionFixture
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
            _appConfiguration.LoadSetting(Controller.Utility.Constants.ConfigExecutionSwitch, ExecutionType.Diagnostic.ToString)
            _appConfiguration.LoadSetting(Controller.Utility.Constants.ConfigNotificationClass, FixtureConstants.list_Value1)
            _ComponentRegistration = New Controller.Container.ComponentRegistration(_logger, _appConfiguration)
            _container = CType(_ComponentRegistration.BootstrapContainer(_currentAssembly), WindsorContainer)
        End Sub

        <Fact()> _
        Public Sub diagnostic_flag_set_in_setting_task_exited()
            Dim _task As ITask = _container.Resolve(Of ITask)(FixtureConstants.MockTaskFullName)
            _task.Execute()
            If ProxyUtil.IsProxy(_task) Then
                Assert.IsType(Of ContainerMockTask)(ProxyUtil.GetUnproxiedInstance(_task))
            Else
                'check for non-proxied type
                Assert.IsType(Of ContainerMockTask)(_task)
            End If
        End Sub

        <Fact()> _
        Public Sub invalid_settings_in_setting_task_exited()
            Dim _task As ITask = _container.Resolve(Of ITask)(FixtureConstants.MockThirdTaskFullName)
            _task.Execute()
            If ProxyUtil.IsProxy(_task) Then
                Assert.IsType(Of ContainerThirdMockTask)(ProxyUtil.GetUnproxiedInstance(_task))
            Else
                'check for non-proxied type
                Assert.IsType(Of ContainerThirdMockTask)(_task)
            End If
        End Sub

        <Fact()> _
        Public Sub notification_interceptor_null_constructor_settings()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _interceptor As New NotificationInterceptor(_containerLogger, Nothing, Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub notificationlogging_interceptor_null_constructor()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _interceptor As New NotificationLoggingInterceptor(Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub diagnostics_task_interceptor_null_constructor()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _interceptor As New DiagnosticTaskInterceptor(Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub task_meta_interceptor_null_constructor()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _interceptor As New TaskMetaInterceptor(Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub task_setting_validation_interceptor_null_constructor()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _interceptor As New TaskSettingValidationInterceptor(Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub NotificationDiagnosticInterceptor_interceptor_null_constructor()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _interceptor As New NotificationDiagnosticInterceptor(Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub NotificationSettingValidationInterceptor_interceptor_null_constructor()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _interceptor As New NotificationSettingValidationInterceptor(Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub NotificationErrorHandlingInterceptor_interceptor_null_constructor()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _interceptor As New NotificationErrorHandlingInterceptor(Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub notification_interceptor_null_constructor()
            Dim _interceptor As New NotificationInterceptor(_containerLogger, Nothing, _appConfiguration)
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        _interceptor.Intercept(Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub NotificationDiagnosticInterceptor_interceptor_null_intercept()
            Dim _interceptor As New NotificationDiagnosticInterceptor(_containerLogger)
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        _interceptor.Intercept(Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub NotificationSettingValidationInterceptor_interceptor_null_intercept()
            Dim _interceptor As New NotificationSettingValidationInterceptor(_containerLogger)
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        _interceptor.Intercept(Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub NotificationErrorHandlingInterceptor_interceptor_null_intercept()
            Dim _interceptor As New NotificationErrorHandlingInterceptor(_containerLogger)
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        _interceptor.Intercept(Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub NotificationProvider_interceptor_null_logger_constructor()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _provider As New Provider.NotificationProvider(Nothing, Nothing, Nothing, Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub NotificationProvider_interceptor_null_settings_constructor()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _provider As New Provider.NotificationProvider(_containerLogger, Nothing, Nothing, Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub NotificationProvider_interceptor_null_task_constructor()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _provider As New Provider.NotificationProvider(_containerLogger, Nothing, Nothing, _appConfiguration)
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