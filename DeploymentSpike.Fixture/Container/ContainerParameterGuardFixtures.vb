Imports DeploymentSpike.Controller
Imports System.IO
Imports DeploymentSpike.Controller.Configuration
Imports DeploymentSpike.Controller.Factory
Imports Castle.Core.Logging

Public Class ContainerParameterGuardFixtures

    Dim _logger As ILogger
    Dim _appConfiguration As ConfigurationSettingsManager
    Dim _default As ISettings

    Sub New()
        _logger = MockRepository.GenerateMock(Of ILogger)()
        _appConfiguration = New ConfigurationSettingsManager(Nothing, Nothing)
        _appConfiguration.LoadSetting(FixtureConstants.list_Key, FixtureConstants.list_Value)
        _default = TaskNotificationSettingsFactory.GetDefaultSettings(_appConfiguration)
    End Sub

    <Fact()> _
    Public Sub DiagnosticTaskInterceptor_null_intercept()
        Dim _interceptor As New Controller.Container.DiagnosticTaskInterceptor(_logger)
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    _interceptor.Intercept(Nothing)
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub ErrorHandlingInterceptor_null_intercept()
        Dim _interceptor As New Controller.Container.ErrorHandlingInterceptor(_logger)
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    _interceptor.Intercept(Nothing)
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub InvocationLogging_null_logstring()
        Assert.Empty(Controller.Container.InvocationLogging.CreateInvocationLogString(Nothing))
    End Sub

    <Fact()> _
    Public Sub InvocationLogging_null_dumpobject()
        Assert.Empty(Controller.Container.InvocationLogging.DumpObject(Nothing))
    End Sub

    <Fact()> _
    Public Sub LoggingInterceptor_null_intercept()
        Dim _interceptor As New Controller.Container.LoggingInterceptor(_logger)
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    _interceptor.Intercept(Nothing)
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub MethodInterceptionFilter_null_IsMethodIntercepted()
        Assert.False(Controller.Container.MethodInterceptionFilter.IsMethodIntercepted(Nothing))
    End Sub

    <Fact()> _
    Public Sub NotificationInterceptorSelector_null_intercept()
        Dim _interceptor As New Controller.Container.NotificationInterceptorSelector()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Dim _value As Boolean = _interceptor.HasInterceptors(Nothing)
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub NotificationLoggingInterceptor_null_intercept()
        Dim _interceptor As New Controller.Container.NotificationLoggingInterceptor(_logger)
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    _interceptor.Intercept(Nothing)
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub SettingResolver_null_intercept()
        Dim _interceptor As New Controller.Container.SettingResolver(_logger, _appConfiguration)
        Assert.False(_interceptor.CanResolve(Nothing, Nothing, Nothing, Nothing))
    End Sub

    <Fact()> _
    Public Sub SettingResolver_null_resolve()
        Dim _interceptor As New Controller.Container.SettingResolver(_logger, _appConfiguration)
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    _interceptor.Resolve(Nothing, Nothing, Nothing, Nothing)
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub TaskInterceptorSelector_null_intercept()
        Dim _interceptor As New Controller.Container.TaskInterceptorSelector()
        Assert.False(_interceptor.HasInterceptors(Nothing))
    End Sub

    <Fact()> _
    Public Sub TaskMetaInterceptor_null_intercept()
        Dim _interceptor As New Controller.Container.TaskMetaInterceptor(_logger)
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    _interceptor.Intercept(Nothing)
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub TaskSettingsInjectionInterceptor_null_intercept()
        Dim _interceptor As New Controller.Container.TaskSettingsInjectionInterceptor(_logger, _appConfiguration)
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    _interceptor.Intercept(Nothing)
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub TaskSettingValidationInterceptor_null_intercept()
        Dim _interceptor As New Controller.Container.TaskSettingValidationInterceptor(_logger)
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    _interceptor.Intercept(Nothing)
                                                End Sub)
    End Sub

End Class
