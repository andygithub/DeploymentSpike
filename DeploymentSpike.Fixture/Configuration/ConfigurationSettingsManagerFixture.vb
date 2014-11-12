Imports DeploymentSpike.Controller
Imports DeploymentSpike.Controller.Configuration

Namespace Configuration

    Public Class ConfigurationSettingsManagerFixture


        Sub New()

        End Sub

        <Fact()>
        Public Sub null_filesettings_null_commandline()
            Dim _settingsmanager As New ConfigurationSettingsManager(Nothing, Nothing)
            Assert.NotNull(_settingsmanager.ApplicationSettings)
        End Sub

        <Fact()>
        Public Sub one_filesettings_null_commandline()
            Dim _list As New Dictionary(Of String, String)
            _list.Add(FixtureConstants.list_Key1, FixtureConstants.list_Value1)
            Dim _settingsmanager As New ConfigurationSettingsManager(_list, Nothing)
            Assert.Equal(1, _settingsmanager.ApplicationSettings.Count)
            Assert.Equal(FixtureConstants.list_Value1, _settingsmanager.ApplicationSettings.Item(FixtureConstants.list_Key1))
        End Sub

        <Fact()>
        Public Sub one_filesettings_one_commandline()
            Dim _list As New Dictionary(Of String, String)
            _list.Add(FixtureConstants.list_Key1, FixtureConstants.list_Value1)
            Dim _cmdlist As New Dictionary(Of String, String)
            _cmdlist.Add(FixtureConstants.list_Key2, FixtureConstants.list_Value2)
            Dim _settingsmanager As New ConfigurationSettingsManager(_list, _cmdlist)
            Assert.Equal(2, _settingsmanager.ApplicationSettings.Count)
            Assert.Equal(FixtureConstants.list_Value1, _settingsmanager.ApplicationSettings.Item(FixtureConstants.list_Key1))
            Assert.Equal(FixtureConstants.list_Value2, _settingsmanager.ApplicationSettings.Item(FixtureConstants.list_Key2))
        End Sub

        <Fact()>
        Public Sub one_filesettings_one_commandline_samekey_overwrite_constructor()
            Dim _list As New Dictionary(Of String, String)
            _list.Add(FixtureConstants.list_Key1, FixtureConstants.list_Value1)
            Dim _cmdlist As New Dictionary(Of String, String)
            _cmdlist.Add(FixtureConstants.list_Key1, FixtureConstants.list_Value2)
            Dim _settingsmanager As New ConfigurationSettingsManager(_list, _cmdlist)
            Assert.Equal(1, _settingsmanager.ApplicationSettings.Count)
            Assert.Equal(FixtureConstants.list_Value2, _settingsmanager.ApplicationSettings.Item(FixtureConstants.list_Key1))
        End Sub

        <Fact()>
        Public Sub one_filesettings_one_commandline_samekey_overwrite_method()
            Dim _list As New Dictionary(Of String, String)
            _list.Add(FixtureConstants.list_Key1, FixtureConstants.list_Value1)
            Dim _cmdlist As New Dictionary(Of String, String)
            _cmdlist.Add(FixtureConstants.list_Key1, FixtureConstants.list_Value2)
            Dim _settingsmanager As New ConfigurationSettingsManager(_list, Nothing)
            _settingsmanager.LoadSetting(_cmdlist)
            Assert.Equal(1, _settingsmanager.ApplicationSettings.Count)
            Assert.Equal(FixtureConstants.list_Value2, _settingsmanager.ApplicationSettings.Item(FixtureConstants.list_Key1))
            _settingsmanager.LoadSetting(Nothing, Nothing)
            Assert.Equal(1, _settingsmanager.ApplicationSettings.Count)
            Assert.Equal(FixtureConstants.list_Value2, _settingsmanager.ApplicationSettings.Item(FixtureConstants.list_Key1))
        End Sub

        <Fact()>
        Public Sub property_logic_assemblylist()
            Dim _settingsmanager As New ConfigurationSettingsManager(Nothing, Nothing)
            Assert.True(_settingsmanager.AssemblyList = String.Empty)
            _settingsmanager.LoadSetting(Utility.Constants.ConfigAssemblyNamesKey, FixtureConstants.testValue)
            Assert.True(_settingsmanager.AssemblyList = FixtureConstants.testValue)
        End Sub

        <Fact()>
        Public Sub property_logic_configfilepath()
            Dim _settingsmanager As New ConfigurationSettingsManager(Nothing, Nothing)
            Assert.True(_settingsmanager.ConfigFilePath = Utility.Constants.Config)
            _settingsmanager.LoadSetting(Utility.Constants.ConfigConfigFilePath, FixtureConstants.testValue)
            Assert.True(_settingsmanager.ConfigFilePath = FixtureConstants.testValue)
        End Sub

        <Fact()>
        Public Sub property_logic_logclass()
            Dim _settingsmanager As New ConfigurationSettingsManager(Nothing, Nothing)
            Assert.True(_settingsmanager.LogConfigurationFile = String.Empty)
            _settingsmanager.LoadSetting(Utility.Constants.ConfigLogConfigurationFile, FixtureConstants.testValue)
            Assert.True(_settingsmanager.LogConfigurationFile = FixtureConstants.testValue)
        End Sub

        <Fact()>
        Public Sub property_logic_notificationclass()
            Dim _settingsmanager As New ConfigurationSettingsManager(Nothing, Nothing)
            Assert.True(_settingsmanager.NotificationClass = String.Empty)
            _settingsmanager.LoadSetting(Utility.Constants.ConfigNotificationClass, FixtureConstants.testValue)
            Assert.True(_settingsmanager.NotificationClass = FixtureConstants.testValue)
        End Sub

        <Fact()>
        Public Sub property_logic_tasklist()
            Dim _settingsmanager As New ConfigurationSettingsManager(Nothing, Nothing)
            Assert.True(_settingsmanager.TaskList = String.Empty)
            _settingsmanager.LoadSetting(Utility.Constants.ConfigTaskList, FixtureConstants.testValue)
            Assert.True(_settingsmanager.TaskList = FixtureConstants.testValue)
        End Sub

        <Fact()>
        Public Sub property_logic_executiontype()
            Dim _settingsmanager As New ConfigurationSettingsManager(Nothing, Nothing)
            Assert.True(_settingsmanager.ConfigFilePath = Utility.Constants.Config)
            _settingsmanager.LoadSetting(Utility.Constants.ConfigExecutionSwitch, CStr(ExecutionType.Release))
            Assert.True(_settingsmanager.ExecutionSwitch = ExecutionType.Release)
        End Sub

        <Fact()>
        Public Sub property_logic_executiontype_invalidvalue()
            Dim _settingsmanager As New ConfigurationSettingsManager(Nothing, Nothing)
            Assert.True(_settingsmanager.ConfigFilePath = Utility.Constants.Config)
            _settingsmanager.LoadSetting(Utility.Constants.ConfigExecutionSwitch, FixtureConstants.testValue)
            Assert.True(_settingsmanager.ExecutionSwitch = ExecutionType.NotSet)
        End Sub

        <Fact()>
        Public Sub property_logic_executiontype_notset()
            Dim _settingsmanager As New ConfigurationSettingsManager(Nothing, Nothing)
            Assert.True(_settingsmanager.ConfigFilePath = Utility.Constants.Config)
            Assert.True(_settingsmanager.ExecutionSwitch = ExecutionType.NotSet)
        End Sub

    End Class

End Namespace