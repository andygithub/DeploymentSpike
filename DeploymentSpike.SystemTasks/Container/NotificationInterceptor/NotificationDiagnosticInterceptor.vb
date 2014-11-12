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
    ''' Interception class that checks for notification diagnotics settings and if the flag is present the execution is halted.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NotificationDiagnosticInterceptor
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
            If invocation Is Nothing Then Throw New ArgumentNullException(Constants.InvocationParameterName)
            If MethodInterceptionFilter.IsMethodIntercepted(invocation) Then
                _logger.Debug(My.Resources.Messages.DiagnosticFlagCheck)
                'check the task settings for the diagnostic setting and if it is present then stop the process.
                Dim _taskInstance As ITask = DirectCast(invocation.GetArgumentValue(0), ITask)
                If _taskInstance.TaskSettings Is Nothing OrElse _taskInstance.TaskSettings.GetExecutionFlag <> ExecutionType.Diagnostic Then
                    invocation.Proceed()
                Else
                    _logger.Debug(My.Resources.Messages.DiagnosticFlagSet)
                    invocation.ReturnValue = False
                End If
            Else
                invocation.Proceed()
            End If
        End Sub

    End Class

End Namespace