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

Namespace Container

    Public Class SettingResolverFixture

        Dim _logger As ILogger
        Dim _ComponentRegistration As Controller.Container.ComponentRegistration
        Dim _appConfiguration As ConfigurationSettingsManager

        Sub New()
            _logger = MockRepository.GenerateMock(Of ILogger)()
            _appConfiguration = New ConfigurationSettingsManager(Nothing, Nothing)
            _appConfiguration.LoadSetting(FixtureConstants.list_Key, FixtureConstants.list_Value)
            _appConfiguration.LoadSetting(Controller.Utility.Constants.ConfigNotificationClass, String.Empty)
        End Sub

        <Fact()> _
        Public Sub resolve_setting_null_settings_null_kernel()
            Using container = New WindsorContainer()
                Assert.Throws(Of ArgumentNullException)(Sub()
                                                            container.Kernel.Resolver.AddSubResolver(New SettingResolver(Nothing, Nothing))
                                                        End Sub)
            End Using
        End Sub

        <Fact()> _
        Public Sub resolve_setting_null_settings()
            Using container = New WindsorContainer()
                Assert.Throws(Of ArgumentNullException)(Sub()
                                                            container.Kernel.Resolver.AddSubResolver(New SettingResolver(Nothing, _appConfiguration))
                                                        End Sub)
            End Using
        End Sub

        <Fact()> _
        Public Sub resolve_setting_null_kernel()
            Using container = New WindsorContainer()
                Assert.Throws(Of ArgumentNullException)(Sub()
                                                            container.Kernel.Resolver.AddSubResolver(New SettingResolver(_logger, Nothing))
                                                        End Sub)
            End Using
        End Sub


    End Class

End Namespace
