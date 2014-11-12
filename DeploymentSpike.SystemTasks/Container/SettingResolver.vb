Imports Castle.Windsor
Imports Castle.Core.Resource
Imports Castle.MicroKernel
Imports Castle.MicroKernel.Handlers
Imports Castle.MicroKernel.Registration
Imports Castle.MicroKernel.Facilities
Imports DeploymentSpike.Controller.Configuration
Imports Castle.Core.Logging
Imports DeploymentSpike.Controller.Interfaces
Imports DeploymentSpike.Interfaces

Namespace Container
    ''' <summary>
    ''' Implementation of IsubDependencyResolver to use a factory to get the settings values for ISettings.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SettingResolver
        Implements ISubDependencyResolver

        Dim _settings As IConfigurationSettingsManager
        Dim _logger As ILogger

        Public Sub New(logger As ILogger, settings As IConfigurationSettingsManager)
            If logger Is Nothing Then Throw New ArgumentNullException(Constants.LoggerParameterName)
            _logger = logger
            If settings Is Nothing Then Throw New ArgumentNullException(Constants.SettingsParameterName)
            _settings = settings
        End Sub

        Public Function CanResolve(context As Castle.MicroKernel.Context.CreationContext, contextHandlerResolver As Castle.MicroKernel.ISubDependencyResolver, model As Castle.Core.ComponentModel, dependency As Castle.Core.DependencyModel) As Boolean Implements Castle.MicroKernel.ISubDependencyResolver.CanResolve
            If dependency Is Nothing Then Return False
            If dependency.TargetType = GetType(ISettings) Then
                Return True
            End If
            Return False
        End Function

        Public Function Resolve(context As Castle.MicroKernel.Context.CreationContext, contextHandlerResolver As Castle.MicroKernel.ISubDependencyResolver, model As Castle.Core.ComponentModel, dependency As Castle.Core.DependencyModel) As Object Implements Castle.MicroKernel.ISubDependencyResolver.Resolve
            If model Is Nothing Then Throw New ArgumentNullException(Utility.Constants.ModelParameterName)
            _logger.Debug(My.Resources.Messages.ResolveSettingsAttempt & model.ComponentName.Name)
            Return Factory.TaskNotificationSettingsFactory.Create(model.ComponentName.Name & Utility.Constants.ValidSettingsExtension, _logger, _settings)
        End Function

    End Class

End Namespace