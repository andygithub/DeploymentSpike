Imports DeploymentSpike.Controller
Imports DeploymentSpike.Controller.Configuration
Imports DeploymentSpike.Controller.Interfaces

Namespace Configuration

    Public Class AssemblyManageFixture

        Private _logger As IMemoryLogging


        Sub New()
            _logger = MockRepository.GenerateMock(Of IMemoryLogging)()
        End Sub

        <Fact()>
        Public Sub null_configuration_null_exclusions_null_logged_assembly_list()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _asm As New AssemblyManger(Nothing, Nothing, Nothing)
                                                    End Sub)
        End Sub

        <Fact()>
        Public Sub null_configuration_null_exclusions_assembly_list()
            Dim _asm As New AssemblyManger(Nothing, Nothing, _logger)
            Assert.Contains(GetCurrentAssemblyNameFullPath, _asm.GetValidAssemblies())
        End Sub

        <Fact()>
        Public Sub null_configuration_assembly_list()
            Dim _asm As New AssemblyManger(Nothing, New List(Of String)(New String() {FixtureConstants.excludeditem}), _logger)
            Assert.DoesNotContain(FixtureConstants.excludeditem, _asm.GetValidAssemblies())
        End Sub

        <Fact()>
        Public Sub file_does_not_exist_configuration_assembly_list()
            Const _configuredAssemblies As String = "file does not exist app.config"
            _logger.Expect(Sub(x)
                               x.Warning(DeploymentSpike.Controller.My.Resources.Messages.InvalidAssembly & _configuredAssemblies)
                           End Sub)
            Dim _asm As New AssemblyManger(_configuredAssemblies, New List(Of String)(New String() {FixtureConstants.excludeditem}), _logger)
            Assert.DoesNotContain(_configuredAssemblies, _asm.GetValidAssemblies())
            _logger.VerifyAllExpectations()
        End Sub

        <Fact()>
        Public Sub wrong_file_extension_configuration_assembly_list()
            Const _configuredAssemblies As String = "Castle.Core.xml"
            Dim _asm As New AssemblyManger(_configuredAssemblies, New List(Of String)(New String() {FixtureConstants.excludeditem}), _logger)
            Assert.DoesNotContain(_configuredAssemblies, _asm.GetValidAssemblies())
        End Sub

        <Fact()>
        Public Sub one_wrong_file_extension_one_valid_configuration_assembly_list()
            Const _configuredAssemblies As String = "Castle.Core.xml"
            Dim _asm As New AssemblyManger(_configuredAssemblies & "," & FixtureConstants.excludeditem, Nothing, _logger)
            Assert.Contains(FixtureConstants.excludeditem, _asm.GetValidAssemblies())
        End Sub

        <Fact()>
        Public Sub one_wrong_file_extension_one_valid_but_exclued_configuration_assembly_list()
            Const _configuredAssemblies As String = "Castle.Core.xml"
            Dim _asm As New AssemblyManger(_configuredAssemblies & "," & FixtureConstants.excludeditem, New List(Of String)(New String() {FixtureConstants.excludeditem}), _logger)
            Dim _list As IEnumerable(Of String) = _asm.GetValidAssemblies()
            Assert.DoesNotContain(FixtureConstants.excludeditem, _asm.GetValidAssemblies())
        End Sub

        Public Shared Function GetCurrentAssemblyNameFullPath() As String
            Return IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & System.Reflection.Assembly.GetExecutingAssembly().GetName().Name & Controller.Utility.Constants.ValidExtension
        End Function

    End Class

End Namespace