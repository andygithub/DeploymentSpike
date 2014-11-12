Imports Castle.Windsor
Imports Castle.Core.Resource
Imports Castle.MicroKernel
Imports Castle.MicroKernel.Handlers
Imports Castle.MicroKernel.Registration
Imports Castle.Windsor.Configuration.Interpreters
Imports System.Reflection
Imports DeploymentSpike.Controller.Configuration
Imports Castle.Core.Logging

Namespace Container

    ''' <summary>
    ''' Class to build the container wtih internal and external components.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ComponentRegistration
        Implements IDisposable


        Private _logger As Controller.Interfaces.IMemoryLogging
        Dim _container As IWindsorContainer
        Dim _appConfiguration As Controller.Interfaces.IConfigurationSettingsManager

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="logger"></param>
        ''' <remarks></remarks>
        Public Sub New(logger As Controller.Interfaces.IMemoryLogging, settings As Controller.Interfaces.IConfigurationSettingsManager)
            If logger Is Nothing Then Throw New ArgumentNullException(DeploymentSpike.Interfaces.Constants.LoggerParameterName)
            _logger = logger
            If settings Is Nothing Then Throw New ArgumentNullException(DeploymentSpike.Interfaces.Constants.SettingsParameterName)
            _appConfiguration = settings
            _container = New WindsorContainer
            InitializeFacilities()
            SetupContainerEvents()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="mappingAssemblies"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function BootstrapContainer(mappingAssemblies As IEnumerable(Of String)) As IWindsorContainer
            If mappingAssemblies Is Nothing OrElse mappingAssemblies.Count = 0 Then Throw New ArgumentNullException(Utility.Constants.MappingAssembliesParameterName)
            _logger.Trace(My.Resources.Messages.StartingAssemblyScan)
            For Each item In mappingAssemblies
                _logger.Trace(My.Resources.Messages.ScanningAssembly & item)
                _container.Register(
                             Classes.FromAssemblyNamed(item).BasedOn(Of ITask).WithServiceAllInterfaces.LifestyleTransient,
                             Classes.FromAssemblyNamed(item).BasedOn(Of INotification).WithServiceAllInterfaces.LifestyleTransient
                             )
            Next
            _logger.Trace(My.Resources.Messages.EndingAssemblyScan)
            _logger.Trace(My.Resources.Messages.CompleetedLoadingTaskAssemblies)
            Return _container
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitializeFacilities()
            If IO.File.Exists(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & _appConfiguration.LogConfigurationFile) Then
                _container.AddFacility(Of Castle.Facilities.Logging.LoggingFacility)(Function(f) f.UseLog4Net())
            Else
                _container.AddFacility(Of Castle.Facilities.Logging.LoggingFacility)(Function(f) f.LogUsing(Of Factory.LoggingFactory)())
            End If

            _container.Kernel.Resolver.AddSubResolver(New SettingResolver(_container.Resolve(Of ILogger)(), _appConfiguration))
            _container.Kernel.Resolver.AddSubResolver(New Resolvers.SpecializedResolvers.ListResolver(_container.Kernel))
            '_container.Register(Component.For(Of ErrorHandlingInterceptor))
            '_container.Register(Component.For(Of LoggingInterceptor))
            _container.Register(Component.For(Of TaskSettingsInjectionInterceptor).DependsOn(New With {.settings = _appConfiguration}))
            '_container.Register(Component.For(Of TaskSettingValidationInterceptor))
            '_container.Register(Component.For(Of DiagnosticTaskInterceptor))
            '_container.Register(Component.For(Of TaskMetaInterceptor))
            _container.Register(Component.For(Of NotificationInterceptor).DependsOn(New With {.settings = _appConfiguration}))
            '_container.Register(Component.For(Of NotificationErrorHandlingInterceptor))
            '_container.Register(Component.For(Of NotificationLoggingInterceptor))
            '_container.Register(Component.For(Of NotificationSettingValidationInterceptor))
            '_container.Register(Component.For(Of NotificationDiagnosticInterceptor))
            _container.Register(Classes.FromThisAssembly().BasedOn(Of Castle.DynamicProxy.IInterceptor)().LifestyleTransient())
            _container.Register(Component.For(Of ITaskInformation).ImplementedBy(Of Domain.TaskMetaInformation))
            _container.Kernel.ProxyFactory.AddInterceptorSelector(New TaskInterceptorSelector)
            _container.Kernel.ProxyFactory.AddInterceptorSelector(New NotificationInterceptorSelector)

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetupContainerEvents()
            AddHandler _container.Kernel.ComponentRegistered, (Sub(k, h)
                                                                   _logger.Trace(String.Format(System.Globalization.CultureInfo.InvariantCulture, My.Resources.Messages.RegisteredComponent, k, h.ComponentModel))
                                                               End Sub)
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    _container.Dispose()
                End If


            End If
            Me.disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

End Namespace