Imports Microsoft.Web.Administration

Public Class RemoveDefaultPageFixture

    <Fact()>
    Public Sub null_logging_parameter()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Dim _task As New DeploymentSpike.Tasks.RemoveDefaultPage(Nothing)
                                                End Sub)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_null_setting_dictionary()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Tasks.RemoveDefaultPage(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
        _task.Logger = Nothing
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_valid_server_list()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, ChangeIISDefaultFileFixture.LocalMachineNames)
        Dim _task As New DeploymentSpike.Tasks.RemoveDefaultPage(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_invalid_site_list()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, ChangeIISDefaultFileFixture.LocalMachineNames)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisSites, String.Empty)
        Dim _task As New DeploymentSpike.Tasks.RemoveDefaultPage(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_invalid_site_list_count()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, ChangeIISDefaultFileFixture.LocalMachineNames)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisSites, ChangeIISDefaultFileFixture.DefaultWebSite)
        Dim _task As New DeploymentSpike.Tasks.RemoveDefaultPage(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_invalid_page_list_count()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, ChangeIISDefaultFileFixture.LocalMachineNames)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisSites, ChangeIISDefaultFileFixture.LocalWebSites)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisPages, ChangeIISDefaultFileFixture.DefaultPage)
        Dim _task As New DeploymentSpike.Tasks.RemoveDefaultPage(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_valid_site_list()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, ChangeIISDefaultFileFixture.LocalMachineNames)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisSites, ChangeIISDefaultFileFixture.LocalWebSites)
        Dim _task As New DeploymentSpike.Tasks.RemoveDefaultPage(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_valid()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, ChangeIISDefaultFileFixture.LocalMachineNames)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisSites, ChangeIISDefaultFileFixture.LocalWebSites)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisPages, ChangeIISDefaultFileFixture.LocalDefaultPages)
        Dim _task As New DeploymentSpike.Tasks.RemoveDefaultPage(_defaultEmptySettings)
        Assert.True(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub execute_success_setup_new_default()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, Environment.MachineName)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisSites, ChangeIISDefaultFileFixture.DefaultWebSite)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisPages, ChangeIISDefaultFileFixture.DefaultPage)
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(Of TaskStatus)(_status, TaskStatus.Completed)
        'check to see if the specified page is now the first default.
        Using serverManager As ServerManager = serverManager.OpenRemote(Environment.MachineName)
            Dim config As Microsoft.Web.Administration.Configuration = serverManager.GetWebConfiguration(ChangeIISDefaultFileFixture.DefaultWebSite)
            Dim defaultDocumentSection As ConfigurationSection = config.GetSection(Tasks.Constants.SystemWebServerDefaultDocument)
            Dim filesCollection As ConfigurationElementCollection = defaultDocumentSection.GetCollection(Tasks.Constants.Files)
            Dim _value As String = filesCollection(0).GetAttributeValue(Tasks.Constants.Value).ToString
            Assert.Contains(ChangeIISDefaultFileFixture.DefaultPage, _value)
        End Using
        'remove it with the task
        Dim _removeTask As New DeploymentSpike.Tasks.RemoveDefaultPage(_defaultEmptySettings)
        _status = _removeTask.Execute
        Assert.Equal(Of TaskStatus)(_status, TaskStatus.Completed)
        'check to see if the specified page is not present
        Using serverManager As ServerManager = serverManager.OpenRemote(Environment.MachineName)
            Dim config As Microsoft.Web.Administration.Configuration = serverManager.GetWebConfiguration(ChangeIISDefaultFileFixture.DefaultWebSite)
            Dim defaultDocumentSection As ConfigurationSection = config.GetSection(Tasks.Constants.SystemWebServerDefaultDocument)
            Dim filesCollection As ConfigurationElementCollection = defaultDocumentSection.GetCollection(Tasks.Constants.Files)
            Dim _value As String = filesCollection(0).GetAttributeValue(Tasks.Constants.Value).ToString
            Assert.DoesNotContain(ChangeIISDefaultFileFixture.DefaultPage, _value)
        End Using
    End Sub

    <Fact()>
    Public Sub execute_success_setup_new_virtual_directory()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, Environment.MachineName)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisSites, ChangeIISDefaultFileFixture.DefaultWebSite)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisVirtualDirectories, ChangeIISDefaultFileFixture.DefaultVirtualDirectory)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisPages, ChangeIISDefaultFileFixture.DefaultPage)
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(Of TaskStatus)(_status, TaskStatus.Completed)
        'check to see if the specified page is now the first default.
        Using serverManager As ServerManager = serverManager.OpenRemote(Environment.MachineName)
            Dim config As Microsoft.Web.Administration.Configuration = serverManager.GetWebConfiguration(ChangeIISDefaultFileFixture.DefaultWebSite, ChangeIISDefaultFileFixture.DefaultVirtualDirectory)
            Dim defaultDocumentSection As ConfigurationSection = config.GetSection(Tasks.Constants.SystemWebServerDefaultDocument)
            Dim filesCollection As ConfigurationElementCollection = defaultDocumentSection.GetCollection(Tasks.Constants.Files)
            Dim _value As String = filesCollection(0).GetAttributeValue(Tasks.Constants.Value).ToString
            Assert.Contains(ChangeIISDefaultFileFixture.DefaultPage, _value)
        End Using
        'remove it with the task
        Dim _removeTask As New DeploymentSpike.Tasks.RemoveDefaultPage(_defaultEmptySettings)
        _status = _removeTask.Execute
        Assert.Equal(Of TaskStatus)(_status, TaskStatus.Completed)
        'check to see if the specified page is not present
        Using serverManager As ServerManager = serverManager.OpenRemote(Environment.MachineName)
            Dim config As Microsoft.Web.Administration.Configuration = serverManager.GetWebConfiguration(ChangeIISDefaultFileFixture.DefaultWebSite, ChangeIISDefaultFileFixture.DefaultVirtualDirectory)
            Dim defaultDocumentSection As ConfigurationSection = config.GetSection(Tasks.Constants.SystemWebServerDefaultDocument)
            Dim filesCollection As ConfigurationElementCollection = defaultDocumentSection.GetCollection(Tasks.Constants.Files)
            Dim _value As String = filesCollection(0).GetAttributeValue(Tasks.Constants.Value).ToString
            Assert.DoesNotContain(ChangeIISDefaultFileFixture.DefaultPage, _value)
        End Using
    End Sub

End Class
