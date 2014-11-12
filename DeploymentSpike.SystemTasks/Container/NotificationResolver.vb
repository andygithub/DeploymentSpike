Imports Castle.Windsor
Imports Castle.Core.Resource
Imports Castle.MicroKernel
Imports Castle.MicroKernel.Handlers
Imports Castle.MicroKernel.Registration
Imports Castle.MicroKernel.Facilities
Imports DeploymentSpike.Controller.Configuration


Namespace Container

    'Public Class NotificationResolver
    '    Implements ISubDependencyResolver

    '    Dim _settings As ConfigurationSettingsManager
    '    ReadOnly _kernel As IKernel

    '    Public Sub New(settings As ConfigurationSettingsManager, kernel As IKernel)
    '        If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
    '        _settings = settings
    '        If kernel Is Nothing Then Throw New ArgumentNullException
    '        _kernel = kernel
    '    End Sub

    '    Public Function CanResolve(context As Castle.MicroKernel.Context.CreationContext, contextHandlerResolver As Castle.MicroKernel.ISubDependencyResolver, model As Castle.Core.ComponentModel, dependency As Castle.Core.DependencyModel) As Boolean Implements Castle.MicroKernel.ISubDependencyResolver.CanResolve
    '        If dependency.TargetType = GetType(INotification) Then Return True
    '        Return False
    '    End Function

    '    Public Function Resolve(context As Castle.MicroKernel.Context.CreationContext, contextHandlerResolver As Castle.MicroKernel.ISubDependencyResolver, model As Castle.Core.ComponentModel, dependency As Castle.Core.DependencyModel) As Object Implements Castle.MicroKernel.ISubDependencyResolver.Resolve
    '        Dim _count As Integer = _kernel.GetAssignableHandlers(GetType(INotification)).Count
    '        'use the factory if a class has not been specified
    '        If String.IsNullOrWhiteSpace(_settings.NotificationClass) AndAlso _count = 0 Then Return Factory.NotificationFactory.Create()
    '        'if only one component is registered then return it.
    '        If _count = 1 Then Return _kernel.Resolve(Of INotification)()
    '        'return default component
    '        If _count > 1 AndAlso String.IsNullOrWhiteSpace(_settings.NotificationClass) Then Return _kernel.Resolve(Of INotification)()
    '        'return specified component by configuration
    '        If _count > 1 AndAlso Not String.IsNullOrWhiteSpace(_settings.NotificationClass) Then Return _kernel.Resolve(Of INotification)(_settings.NotificationClass)
    '        'return factory if it gets through all of the conditionals
    '        Return Factory.NotificationFactory.Create()
    '    End Function

    'End Class

End Namespace