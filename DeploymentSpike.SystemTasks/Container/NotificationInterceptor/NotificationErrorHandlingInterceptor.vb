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
    ''' Interception class that handles error catching for the notification process.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NotificationErrorHandlingInterceptor
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
            If MethodInterceptionFilter.IsMethodIntercepted(invocation) Then
                _logger.Debug(My.Resources.Messages.StartingErrorHandlingNotification)
                Try
                    invocation.Proceed()
                Catch ex As Exception
                    _logger.Warn(My.Resources.Messages.NotificationException)
                    _logger.WarnFormat(System.Globalization.CultureInfo.InvariantCulture, My.Resources.Messages.LoggedException, ex.Message, ex.StackTrace)
                    'set a return parmeter values since the method didn't set it, this would get complicated if we were intercepting anything other than boolean functions
                    invocation.ReturnValue = False
                End Try
            Else
                invocation.Proceed()
            End If
        End Sub

    End Class

End Namespace