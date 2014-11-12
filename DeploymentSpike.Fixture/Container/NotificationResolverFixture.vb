Imports DeploymentSpike.Controller
Imports Castle.Windsor
Imports Castle.Core
Imports Castle.Core.Internal
Imports Castle.Core.Resource
Imports Castle.MicroKernel
Imports Castle.MicroKernel.Handlers
Imports Castle.MicroKernel.Registration
Imports Castle.Windsor.Configuration.Interpreters
Imports DeploymentSpike.Controller.Container
Imports DeploymentSpike.Controller.Configuration
Imports Castle.Core.Logging

'Public Class NotificationResolverFixture

'    Dim _logger As ILogger
'    Dim _ComponentRegistration As Container.ComponentRegistration
'    Dim _appConfiguration As ConfigurationSettingsManager

'    Sub New()
'        _logger = MockRepository.GenerateMock(Of ILogger)()
'        _appConfiguration = New ConfigurationSettingsManager(Nothing, Nothing)
'        _appConfiguration.LoadSetting(FixtureConstants.list_Key, FixtureConstants.list_Value)
'    End Sub

'    <Fact()> _
'    Public Sub resolve_notification_null_settings_null_kernel()
'        Using container = New WindsorContainer()
'            Assert.Throws(Of ArgumentNullException)(Sub()
'                                                        container.Kernel.Resolver.AddSubResolver(New NotificationResolver(Nothing, Nothing))
'                                                    End Sub)
'        End Using
'    End Sub

'    <Fact()> _
'    Public Sub resolve_notification_null_settings()
'        Using container = New WindsorContainer()
'            Assert.Throws(Of ArgumentNullException)(Sub()
'                                                        container.Kernel.Resolver.AddSubResolver(New NotificationResolver(Nothing, container.Kernel))
'                                                    End Sub)
'        End Using
'    End Sub

'    <Fact()> _
'    Public Sub resolve_notification_null_kernel()
'        Using container = New WindsorContainer()
'            Assert.Throws(Of ArgumentNullException)(Sub()
'                                                        container.Kernel.Resolver.AddSubResolver(New NotificationResolver(_appConfiguration, Nothing))
'                                                    End Sub)
'        End Using
'    End Sub


'End Class
