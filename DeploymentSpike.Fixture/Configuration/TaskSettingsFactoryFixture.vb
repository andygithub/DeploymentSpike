Imports DeploymentSpike.Controller
Imports System.IO
Imports DeploymentSpike.Controller.Configuration
Imports DeploymentSpike.Controller.Factory
Imports Castle.Core.Logging

Namespace Configuration

    Public Class TaskSettingsFactoryFixture

        Dim _logger As ILogger
        Dim _appConfiguration As ConfigurationSettingsManager
        Dim _default As ISettings

        Sub New()
            _logger = MockRepository.GenerateMock(Of ILogger)()
            _appConfiguration = New ConfigurationSettingsManager(Nothing, Nothing)
            _appConfiguration.LoadSetting(FixtureConstants.list_Key, FixtureConstants.list_Value)
            _default = TaskNotificationSettingsFactory.GetDefaultSettings(_appConfiguration)
        End Sub

        <Fact()> _
        Public Sub factory_null_logger_parameter()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _settings As ISettings = TaskNotificationSettingsFactory.Create(Nothing, Nothing, Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub factory_null_settings_parameter()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _settings As ISettings = TaskNotificationSettingsFactory.Create(Nothing, _logger, Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub factory_null_componentName_parameter()
            Dim _settings As ISettings = TaskNotificationSettingsFactory.Create(Nothing, _logger, _appConfiguration)
            Assert.Equal(Of String)(_default.SettingsLocation, _settings.SettingsLocation)
        End Sub

        <Fact()> _
        Public Sub factory_merge_null_default_null_loaded()
            Dim _merge As ISettings = TaskNotificationSettingsFactory.MergeTaskSettings(Nothing, Nothing, Nothing)
            Assert.Equal(Of String)(_default.SettingsLocation, _merge.SettingsLocation)
        End Sub

        <Fact()> _
        Public Sub factory_merge_null_loaded()
            Dim _merge As ISettings = TaskNotificationSettingsFactory.MergeTaskSettings(_default, Nothing, Nothing)
            Assert.Equal(Of String)(_default.SettingsLocation, _merge.SettingsLocation)
        End Sub

        <Fact()> _
        Public Sub factory_merge_null_default()
            Dim _merge As ISettings = TaskNotificationSettingsFactory.MergeTaskSettings(Nothing, _default, Nothing)
            Assert.Equal(Of String)(_default.SettingsLocation, _merge.SettingsLocation)
        End Sub

        <Fact()> _
        Public Sub factory_merge_no_collisions()
            Dim _defaultsettings As ISettings = TaskNotificationSettingsFactory.GetDefaultSettings(Nothing)
            Dim _loadedsettings As ISettings = TaskNotificationSettingsFactory.GetDefaultSettings(Nothing)
            _defaultsettings.SettingsLocation = FixtureConstants.Initial
            _loadedsettings.SettingsLocation = FixtureConstants.Loaded
            _defaultsettings.Settings.Add(FixtureConstants.list_Keyi, FixtureConstants.list_Valuei)
            _loadedsettings.Settings.Add(FixtureConstants.list_Keyl, FixtureConstants.list_Valuel)
            Dim _merge As ISettings = TaskNotificationSettingsFactory.MergeTaskSettings(_defaultsettings, _loadedsettings, Nothing)
            Assert.Equal(Of String)(_merge.SettingsLocation, FixtureConstants.Loaded)
            Assert.Equal(3, _merge.Settings.Count)
            Assert.True(_merge.Settings.ContainsKey(FixtureConstants.list_Keyi))
        End Sub

        <Fact()> _
        Public Sub factory_merge_collisions()
            Dim _defaultsettings As ISettings = TaskNotificationSettingsFactory.GetDefaultSettings(Nothing)
            Dim _loadedsettings As ISettings = TaskNotificationSettingsFactory.GetDefaultSettings(Nothing)
            _defaultsettings.SettingsLocation = FixtureConstants.Initial
            _loadedsettings.SettingsLocation = FixtureConstants.Loaded
            _defaultsettings.Settings.Add(FixtureConstants.list_Keyi, FixtureConstants.list_Valuei)
            _loadedsettings.Settings.Add(FixtureConstants.list_Keyi, FixtureConstants.list_Valuel)
            Dim _merge As ISettings = TaskNotificationSettingsFactory.MergeTaskSettings(_defaultsettings, _loadedsettings, Nothing)
            Assert.Equal(Of String)(_merge.SettingsLocation, FixtureConstants.Loaded)
            Assert.Equal(2, _merge.Settings.Count)
            Assert.True(_merge.Settings.ContainsKey(FixtureConstants.list_Keyi))
            Assert.True(_merge.Settings(FixtureConstants.list_Keyi) = FixtureConstants.list_Valuel)
        End Sub

        <Fact()> _
        Public Sub factory_folder_name_null()
            Dim _value As String = TaskNotificationSettingsFactory.GetSettingsFolderName(Nothing)
            Assert.Equal(_value, Utility.Constants.Config)
        End Sub

        <Fact()> _
        Public Sub factory_load_settings_null()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _value As ISettings = TaskNotificationSettingsFactory.LoadSettings(Nothing, Nothing, Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub factory_load_settings_validfile()
            Dim _settings As ISettings = New Controller.Domain.TaskSettings
            _settings.SettingsLocation = AssemblyManageFixture.GetCurrentAssemblyNameFullPath
            _settings.Settings.Add(FixtureConstants.list_Key, FixtureConstants.list_Value)
            _settings.Settings.Add(FixtureConstants.list_Key1, FixtureConstants.list_Value1)
            Dim _file As String = IO.Path.GetTempFileName()
            Using _writer As New FileStream(_file, FileMode.Create)
                ServiceStack.Text.JsonSerializer.SerializeToStream(Of ISettings)(_settings, _writer)
                Assert.True(IO.File.Exists(_file))
            End Using
            Dim _value As ISettings = TaskNotificationSettingsFactory.LoadSettings(_file, Nothing, _logger)
            Assert.Equal(_value.SettingsLocation, _file)
            Assert.Equal(_value.Settings.Count, _settings.Settings.Count + 1)
            IO.File.Delete(_file)
        End Sub

        <Fact()> _
        Public Sub factory_create_validfile()
            Dim _settings As ISettings = New Controller.Domain.TaskSettings
            _settings.SettingsLocation = AssemblyManageFixture.GetCurrentAssemblyNameFullPath
            _settings.Settings.Add(FixtureConstants.list_Keya, FixtureConstants.list_Valuea)
            _settings.Settings.Add(FixtureConstants.list_Key1, FixtureConstants.list_Value1)
            'setup folder
            IO.Directory.CreateDirectory(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Utility.Constants.Config)
            Dim _file As String = IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Utility.Constants.Config & IO.Path.DirectorySeparatorChar & FixtureConstants.TestFileComponentTest & Utility.Constants.ValidSettingsExtension
            Using _writer As New FileStream(_file, FileMode.Create)
                ServiceStack.Text.JsonSerializer.SerializeToStream(Of ISettings)(_settings, _writer)
                Assert.True(IO.File.Exists(_file))
            End Using

            Dim _value As ISettings = TaskNotificationSettingsFactory.Create(FixtureConstants.TestFileComponentTest & Utility.Constants.ValidSettingsExtension, _logger, _appConfiguration)
            Assert.Equal(_value.SettingsLocation, _file)
            Assert.Equal(_value.Settings.Count, _settings.Settings.Count + 1)
            IO.File.Delete(_file)
        End Sub

        <Fact()> _
        Public Sub factory_create_invalidfile()
            Dim _settings As ISettings = New Controller.Domain.TaskSettings
            _settings.SettingsLocation = AssemblyManageFixture.GetCurrentAssemblyNameFullPath
            _settings.Settings.Add(FixtureConstants.list_Keya, FixtureConstants.list_Valuea)
            _settings.Settings.Add(FixtureConstants.list_Key1, FixtureConstants.list_Value1)
            Dim _componentFileName As String = FixtureConstants.TestFileComponentTest
            'setup folder
            IO.Directory.CreateDirectory(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Utility.Constants.Config)
            Dim _file As String = IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Utility.Constants.Config & IO.Path.DirectorySeparatorChar & _componentFileName & Utility.Constants.ValidSettingsExtension
            IO.File.AppendAllText(_file, FixtureConstants.Loaded)
            Assert.True(IO.File.Exists(_file))

            Dim _value As ISettings = TaskNotificationSettingsFactory.Create(_componentFileName, _logger, _appConfiguration)
            Assert.Equal(_value.SettingsLocation, _default.SettingsLocation)
            Assert.Equal(_value.Settings.Count, 1)
            IO.File.Delete(_file)
        End Sub

        '<Fact()> _
        'Public Sub factory_meta_load_null_file()
        '    Dim _metasettings As New Controller.Domain.TaskMetaInformation
        '    Dim _value As ISettings = TaskNotificationSettingsFactory.CreateFromMetaSettings(_metasettings, _logger, _appConfiguration)
        '    Assert.Equal(_value.SettingsLocation, Controller.My.Resources.Messages.NoLocationUsed)
        'End Sub

        <Fact()> _
        Public Sub test_json_library()
            Dim _settings As ISettings = New Controller.Domain.TaskSettings
            _settings.SettingsLocation = AssemblyManageFixture.GetCurrentAssemblyNameFullPath
            _settings.Settings.Add(FixtureConstants.list_Keya, FixtureConstants.list_Valuea)
            _settings.Settings.Add(FixtureConstants.list_Key1, FixtureConstants.list_Value1)
            Dim _json As String = ServiceStack.Text.JsonSerializer.SerializeToString(_settings)
            Dim _fromJson As ISettings = ServiceStack.Text.JsonSerializer.DeserializeFromString(Of ISettings)(_json)
            Assert.Contains(FixtureConstants.list_Keya, _fromJson.Settings.Keys)
        End Sub

        <Fact()> _
        Public Sub test_json_library_invalidtype()
            Dim _json As String = ServiceStack.Text.JsonSerializer.SerializeToString(_appConfiguration)
            Dim _fromJson As ISettings = ServiceStack.Text.JsonSerializer.DeserializeFromString(Of ISettings)(_json)
            Assert.Null(_fromJson)
        End Sub

    End Class

End Namespace