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
    ''' Interceptor implementation that handles the validation of the task settings.  If the validation fails that the task process is halted.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TaskSettingValidationInterceptor
        Implements IInterceptor

        Private _logger As ILogger

        ''' <summary>
        ''' Default constructor.
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
                'first validate the task settings
                Dim _taskInstance As ITask = invocation.GetPrivateFieldValue(Of ITask)(Constants.InterceptionInstanceInternalPropertyName)
                If _taskInstance.ValidateSettings Then
                    _logger.Debug(My.Resources.Messages.ValidateTaskSettingsSucceeded)
                    invocation.Proceed()
                Else
                    _logger.Fatal(My.Resources.Messages.ValidateTaskSettingsFailed)
                    invocation.ReturnValue = TaskStatus.Failed
                End If
            Else
                invocation.Proceed()
            End If
        End Sub

    End Class

End Namespace