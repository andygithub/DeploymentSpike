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
    ''' Interception class that handles error catching for the task process.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ErrorHandlingInterceptor
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

        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
        Public Sub Intercept(invocation As Castle.DynamicProxy.IInvocation) Implements Castle.DynamicProxy.IInterceptor.Intercept
            If invocation Is Nothing Then Throw New ArgumentNullException(Utility.Constants.InvocationParameterName)
            'handle all exceptions from tasks by logging the exception to the log and setting the task status to failed.
            If MethodInterceptionFilter.IsMethodIntercepted(invocation) Then
                _logger.Debug(My.Resources.Messages.StartingErrorHandling)
                Try
                    invocation.Proceed()
                Catch ex As Exception
                    invocation.ReturnValue = TaskStatus.Failed
                    _logger.Error(My.Resources.Messages.TaskException & ex.Message  & ex.StackTrace)
                End Try
            Else
                invocation.Proceed()
            End If

        End Sub


    End Class

End Namespace