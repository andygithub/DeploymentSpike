
'Imports Castle.Windsor
'Imports Castle.Core
'Imports Castle.Core.Internal
'Imports Castle.Core.Resource
'Imports Castle.MicroKernel
'Imports Castle.MicroKernel.Handlers
'Imports Castle.MicroKernel.Registration
'Imports Castle.Windsor.Configuration.Interpreters

'Imports System.Reflection
'Imports DeploymentSpike.Interfaces
'Imports DeploymentSpike.Controller
'Imports DeploymentSpike.Controller.Configuration
'Imports Castle.Core.Logging

'Public Class LoggingMultipleCountsFixture
'    Implements IDisposable


'    Private _logger As IMemoryLogging
'    Dim _seperateContainer As New WindsorContainer
'    Dim _appConfiguration As ConfigurationSettingsManager

'    Sub New()
'        _logger = MockRepository.GenerateMock(Of IMemoryLogging)()
'        _appConfiguration = New ConfigurationSettingsManager(Nothing, Nothing)
'        _appConfiguration.LoadSetting(FixtureConstants.list_Key, FixtureConstants.list_Value)
'    End Sub

'    <Fact()> _
'    Public Sub All_tasks_multiple_container_logging()
'        Using _compreg As New Container.ComponentRegistration(_logger, _appConfiguration)

'            Using _seperateContainer = CType(_compreg.BootstrapContainer(New Generic.List(Of String)(New String() {FixtureConstants.MultipleLogging})), WindsorContainer)
'                Array.ForEach(_seperateContainer.Kernel.GetAssignableHandlers(GetType(ILogger)), Sub(x)
'                                                                                                     Debug.WriteLine(x.ComponentModel.Name)
'                                                                                                 End Sub)
'                Assert.Equal(5, _seperateContainer.Kernel.GetAssignableHandlers(GetType(ILogger)).Count)
'            End Using
'        End Using
'    End Sub

'    <Fact()> _
'    Public Sub All_tasks_no_container_logging()
'        Using _compreg As New Container.ComponentRegistration(_logger, _appConfiguration)

'            Using _seperateContainer = CType(_compreg.BootstrapContainer(New Generic.List(Of String)(New String() {FixtureConstants.NoLogging})), WindsorContainer)
'                Dim _count As Integer = _seperateContainer.Kernel.GetAssignableHandlers(GetType(ILogger)).Count
'                Array.ForEach(_seperateContainer.Kernel.GetAssignableHandlers(GetType(ILogger)), Sub(x)
'                                                                                                     Debug.WriteLine(x.ComponentModel.Name)
'                                                                                                 End Sub)
'                Assert.Equal(0, _seperateContainer.Kernel.GetAssignableHandlers(GetType(ILogger)).Count)
'            End Using
'        End Using
'    End Sub


'#Region "IDisposable Support"
'    Private disposedValue As Boolean ' To detect redundant calls

'    ' IDisposable
'    Protected Overridable Sub Dispose(disposing As Boolean)
'        If Not Me.disposedValue Then
'            If disposing Then
'                _seperateContainer.Dispose()
'            End If

'        End If
'        Me.disposedValue = True
'    End Sub

'    ' This code added by Visual Basic to correctly implement the disposable pattern.
'    Public Sub Dispose() Implements IDisposable.Dispose
'        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
'        Dispose(True)
'        GC.SuppressFinalize(Me)
'    End Sub
'#End Region

'End Class
