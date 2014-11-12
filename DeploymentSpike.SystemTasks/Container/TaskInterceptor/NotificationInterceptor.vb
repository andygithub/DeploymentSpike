Imports Castle.Windsor
Imports Castle.Core.Resource
Imports Castle.MicroKernel
Imports Castle.MicroKernel.Handlers
Imports Castle.MicroKernel.Registration
Imports Castle.Windsor.Configuration.Interpreters
Imports Castle.DynamicProxy
Imports Castle.Core.Logging
Imports DeploymentSpike.Controller.Interfaces
Imports DeploymentSpike.Controller.Utility

Namespace Container

    ''' <summary>
    ''' Interceptor implementation that handles firing the notification events from the task process.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NotificationInterceptor
        Implements IInterceptor

        Private Const _NotifyStarted As String = "NotifyStarted"
        Private Const _NotifyCompletedSuccess As String = "NotifyCompletedSuccess"
        Private Const _NotifyCompletedFailure As String = "NotifyCompletedFailure"

        Private _logger As ILogger
        Private _notifications As IList(Of INotification)
        Private _settings As IConfigurationSettingsManager

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="logger"></param>
        ''' <remarks></remarks>
        Public Sub New(logger As ILogger, notification As IList(Of INotification), settings As IConfigurationSettingsManager)
            If logger Is Nothing Then Throw New ArgumentNullException(DeploymentSpike.Interfaces.Constants.LoggerParameterName)
            _logger = logger
            If settings Is Nothing Then Throw New ArgumentNullException(DeploymentSpike.Interfaces.Constants.SettingsParameterName)
            _settings = settings
            _notifications = notification
        End Sub

        Public Sub Intercept(invocation As Castle.DynamicProxy.IInvocation) Implements Castle.DynamicProxy.IInterceptor.Intercept
            If invocation Is Nothing Then Throw New ArgumentNullException(Utility.Constants.InvocationParameterName)
            If MethodInterceptionFilter.IsMethodIntercepted(invocation) Then
                _logger.Debug(My.Resources.Messages.StartingNotificationInterception)
                Dim _task As ITask = invocation.GetPrivateFieldValue(Of ITask)(Constants.InterceptionInstanceInternalPropertyName)
                Dim _notificationFacade As New Provider.NotificationProvider(_logger, _task, _notifications, _settings)
                _notificationFacade.NotifyStarted()
                invocation.Proceed()
                If Convert.ToInt32(invocation.ReturnValue, System.Globalization.CultureInfo.InvariantCulture) = TaskStatus.Failed Then
                    _notificationFacade.NotifyCompletedFailure()
                Else
                    _notificationFacade.NotifyCompleted()
                End If
            Else
                invocation.Proceed()
            End If
        End Sub

    End Class

End Namespace