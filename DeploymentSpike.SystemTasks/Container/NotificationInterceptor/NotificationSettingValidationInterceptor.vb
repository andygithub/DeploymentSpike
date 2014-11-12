Imports Castle.Windsor
Imports Castle.Core.Resource
Imports Castle.MicroKernel
Imports Castle.MicroKernel.Handlers
Imports Castle.MicroKernel.Registration
Imports Castle.Windsor.Configuration.Interpreters
Imports Castle.DynamicProxy
Imports Castle.Core.Logging
Imports Castle.Core.Interceptor
Imports DeploymentSpike.Controller.Utility

Namespace Container

    ''' <summary>
    ''' Interceptor class that validates the settings of the notification implementation.  If the validation fails then the notificaion process is halted.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NotificationSettingValidationInterceptor
        Implements IInterceptor

        Private _logger As ILogger

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="logger"></param>
        ''' <remarks></remarks>
        Public Sub New(logger As ILogger)
            If logger Is Nothing Then Throw New ArgumentNullException(DeploymentSpike.Interfaces.Constants.LoggerParameterName)
            _logger = logger
        End Sub

        Public Sub Intercept(invocation As Castle.DynamicProxy.IInvocation) Implements Castle.DynamicProxy.IInterceptor.Intercept
            If invocation Is Nothing Then Throw New ArgumentNullException(Utility.Constants.InvocationParameterName)
            If MethodInterceptionFilter.IsMethodIntercepted(invocation) Then
                _logger.Debug(My.Resources.Messages.ValidateTaskSettings)
                'validate the notification settings.  the rest of the steps shouldn't fire if the settings are valid.
                Dim _taskInstance As INotification = invocation.GetPrivateFieldValue(Of INotification)(Constants.InterceptionInstanceInternalPropertyName)
                If _taskInstance.ValidateSettings Then
                    _logger.Debug(My.Resources.Messages.ValidateNotificationSettingsSucceeded)
                    invocation.Proceed()
                Else
                    _logger.Fatal(My.Resources.Messages.ValidateNotificationSettingsFailed)
                    invocation.ReturnValue = TaskStatus.Failed
                End If
            Else
                invocation.Proceed()
            End If
        End Sub

    End Class

End Namespace