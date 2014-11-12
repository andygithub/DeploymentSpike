Imports Castle.Windsor
Imports Castle.Core.Resource
Imports Castle.MicroKernel
Imports Castle.MicroKernel.Handlers
Imports Castle.MicroKernel.Registration
Imports Castle.Windsor.Configuration.Interpreters
Imports Castle.DynamicProxy
Imports Castle.Core.Logging
Imports DeploymentSpike.Controller.Utility

Namespace Container

    ''' <summary>
    ''' Interceptor implementation that loads custom settings for the task from the task step settings.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TaskSettingsInjectionInterceptor
        Implements IInterceptor

        Private _logger As ILogger
        Private _settings As Interfaces.IConfigurationSettingsManager

        ''' <summary>
        ''' Default constructor.
        ''' </summary>
        ''' <param name="logger"></param>
        ''' <remarks></remarks>
        Public Sub New(logger As ILogger, settings As Interfaces.IConfigurationSettingsManager)
            If logger Is Nothing Then Throw New ArgumentNullException(DeploymentSpike.Interfaces.Constants.LoggerParameterName)
            _logger = logger
            If settings Is Nothing Then Throw New ArgumentNullException(DeploymentSpike.Interfaces.Constants.SettingsParameterName)
            _settings = settings
        End Sub

        Public Sub Intercept(invocation As Castle.DynamicProxy.IInvocation) Implements Castle.DynamicProxy.IInterceptor.Intercept
            If invocation Is Nothing Then Throw New ArgumentNullException(Utility.Constants.InvocationParameterName)
            If MethodInterceptionFilter.IsMethodIntercepted(invocation) Then
                Dim _task As ITask = invocation.GetPrivateFieldValue(Of ITask)(Constants.InterceptionInstanceInternalPropertyName)
                _logger.Debug(My.Resources.Messages.StartingTaskSettingsInjectionInterceptor)
                'get the meta settings from the task and pass it to the factory.
                'only perform this if meta information is not null and the config property has a value
                'merge the new settings into any existing settings
                If _task.MetaInformation IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(_task.MetaInformation.TaskComponentConfiguration) Then
                    Dim _newsettings As ISettings = Factory.TaskNotificationSettingsFactory.Create(_task.MetaInformation.TaskComponentConfiguration, _logger, _settings)
                    _task.TaskSettings = Factory.TaskNotificationSettingsFactory.MergeTaskSettings(_task.TaskSettings, _newsettings, _settings)
                End If
            End If
            invocation.Proceed()
        End Sub

    End Class

End Namespace