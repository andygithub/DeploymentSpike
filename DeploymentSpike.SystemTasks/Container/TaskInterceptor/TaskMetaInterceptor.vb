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
    ''' Interceptor implementation that updates the task step values.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TaskMetaInterceptor
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
                _logger.Debug(My.Resources.Messages.TaskMetaInterceptor)
                Dim _taskInstance As ITask = invocation.GetPrivateFieldValue(Of ITask)(Constants.InterceptionInstanceInternalPropertyName)
                _taskInstance.MetaInformation.ActualDate = Controller.Provider.TimeProvider.Current.UtcNow
                _taskInstance.MetaInformation.ActualStartTime = Controller.Provider.TimeProvider.Current.UtcNow
                invocation.Proceed()
                _taskInstance.MetaInformation.ActualEndTime = Controller.Provider.TimeProvider.Current.UtcNow
                _taskInstance.MetaInformation.ActualDuration = _taskInstance.MetaInformation.ActualEndTime - _taskInstance.MetaInformation.ActualStartTime
            Else
                invocation.Proceed()
            End If
        End Sub

    End Class

End Namespace