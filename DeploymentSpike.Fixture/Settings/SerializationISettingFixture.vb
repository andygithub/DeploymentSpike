Imports DeploymentSpike.Interfaces
Imports DeploymentSpike.Controller
Imports System.Runtime.Serialization
Imports System.IO
Imports System.Xml
Imports DeploymentSpike.Controller.Domain
Imports DeploymentSpike.Fixture.Configuration

Namespace Settings

    Public Class SerializationISettingFixture

        <Fact()> _
        Public Sub Serialize_object()
            Dim _settings As ISettings = New TaskSettings
            _settings.SettingsLocation = AssemblyManageFixture.GetCurrentAssemblyNameFullPath
            _settings.Settings.Add(FixtureConstants.list_Keya, FixtureConstants.list_Valuea)
            _settings.Settings.Add(FixtureConstants.list_Key1, FixtureConstants.list_Value1)
            Dim _file As String = IO.Path.GetTempFileName()
            Using _writer As New FileStream(_file, FileMode.Create)
                ServiceStack.Text.JsonSerializer.SerializeToStream(Of ISettings)(_settings, _writer)
                Assert.True(IO.File.Exists(_file))
            End Using
            IO.File.Delete(_file)
        End Sub

        <Fact()> _
        Public Sub Load_object_from_file()
            Dim _settings As ISettings = New TaskSettings
            _settings.SettingsLocation = AssemblyManageFixture.GetCurrentAssemblyNameFullPath
            _settings.Settings.Add(FixtureConstants.list_Keya, FixtureConstants.list_Valuea)
            _settings.Settings.Add(FixtureConstants.list_Key1, FixtureConstants.list_Value1)
            Dim _file As String = IO.Path.GetTempFileName()
            Using _writer1 As New FileStream(_file, FileMode.Create)
                ServiceStack.Text.JsonSerializer.SerializeToStream(Of ISettings)(_settings, _writer1)
            End Using
            Using _writer As New FileStream(_file, FileMode.Open)
                Dim _settingsloaded As ISettings = ServiceStack.Text.JsonSerializer.DeserializeFromStream(Of ISettings)(_writer)
                Assert.Equal(_settings.Settings.Keys, _settingsloaded.Settings.Keys)
            End Using
            IO.File.Delete(_file)
        End Sub

    End Class

End Namespace