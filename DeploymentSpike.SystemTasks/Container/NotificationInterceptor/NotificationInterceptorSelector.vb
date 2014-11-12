Imports Castle.Windsor
Imports Castle.Core
Imports Castle.Core.Resource
Imports Castle.MicroKernel
Imports Castle.MicroKernel.Handlers
Imports Castle.MicroKernel.Registration
Imports Castle.Windsor.Configuration.Interpreters
Imports Castle.DynamicProxy
Imports Castle.MicroKernel.Proxy

Imports DeploymentSpike.Interfaces

Namespace Container

    ''' <summary>
    ''' Implementation of IModelInterceptorsSelector that specifies which interceptor classes are attached to the INotificaiton implementations.  Note that the order of types returned by selectinterceptors is the order that the interceptors will be fired.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NotificationInterceptorSelector
        Implements IModelInterceptorsSelector


        Public Function HasInterceptors(model As Castle.Core.ComponentModel) As Boolean Implements IModelInterceptorsSelector.HasInterceptors
            If model Is Nothing Then Throw New ArgumentNullException(Utility.Constants.ModelParameterName)
            Return model.Services.Any(Function(x)
                                          Return GetType(INotification).IsAssignableFrom(x)
                                      End Function)
        End Function

        Public Function SelectInterceptors(model As Castle.Core.ComponentModel, interceptors() As Castle.Core.InterceptorReference) As InterceptorReference() Implements IModelInterceptorsSelector.SelectInterceptors
            Return New InterceptorReference() {New InterceptorReference(GetType(NotificationErrorHandlingInterceptor)),
                                    New InterceptorReference(GetType(NotificationLoggingInterceptor)),
                                   New InterceptorReference(GetType(NotificationSettingValidationInterceptor)),
                                    New InterceptorReference(GetType(NotificationDiagnosticInterceptor))
                                  }
        End Function

    End Class

End Namespace