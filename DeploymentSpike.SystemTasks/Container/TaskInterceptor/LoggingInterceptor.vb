Imports Castle.Windsor
Imports Castle.Core.Resource
Imports Castle.MicroKernel
Imports Castle.MicroKernel.Handlers
Imports Castle.MicroKernel.Registration
Imports Castle.Windsor.Configuration.Interpreters
Imports Castle.DynamicProxy
Imports Castle.Core.Logging

Namespace Container

    ''' <summary>
    ''' Interceptor class that handles logging for the task process.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LoggingInterceptor
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
                _logger.Debug(InvocationLogging.CreateInvocationLogString(invocation))
                invocation.Proceed()
                _logger.Debug(Controller.My.Resources.Messages.ExecutionCompleted)
                If Convert.ToInt32(invocation.ReturnValue, System.Globalization.CultureInfo.InvariantCulture) = TaskStatus.Failed Then
                    _logger.Debug(Controller.My.Resources.Messages.TaskFailure)
                    _logger.Debug(Controller.My.Resources.Messages.ProcessHaltedFailedTask)
                End If
            Else
                invocation.Proceed()
            End If
        End Sub

    End Class

End Namespace