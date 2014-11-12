Imports DeploymentSpike.Controller
Imports DeploymentSpike.Controller.DefaultInstance
Imports DeploymentSpike.Controller.Interfaces

Namespace Configuration
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LoggingFactoryFixture

        Sub New()
            _castleLogFactory = New Factory.LoggingFactory
        End Sub

        Dim _castleLogFactory As Factory.LoggingFactory

        <Fact()> _
        Public Sub logging_factory_returns_interface_type()
            Dim _logging As IMemoryLogging = Factory.MemoryLoggingFactory.Create
            Assert.IsType(Of ConsoleLogging)(_logging)
        End Sub

        <Xunit.Extensions.Theory()>
        <Xunit.Extensions.InlineData(1, 1, 2)>
        <Xunit.Extensions.InlineData(1, 2, 3)>
        <Xunit.Extensions.InlineData(1, 5, 2)>
        Public Sub Add(x As Integer, y As Integer, result As Integer)
            Assert.True(x + y = result)
        End Sub

        <Fact()> _
        Public Sub logging_factory_name_level_parameter()
            Dim _logging As Castle.Core.Logging.ILogger = _castleLogFactory.Create(String.Empty, Castle.Core.Logging.LoggerLevel.Info)
            Assert.IsType(Of Castle.Services.Logging.Log4netIntegration.Log4netLogger)(_logging)
        End Sub

        <Fact()> _
        Public Sub logging_factory_type_level_parameter()
            Dim _logging As Castle.Core.Logging.ILogger = _castleLogFactory.Create(GetType(String), Castle.Core.Logging.LoggerLevel.Warn)
            Assert.IsType(Of Castle.Services.Logging.Log4netIntegration.Log4netLogger)(_logging)
        End Sub

    End Class

End Namespace