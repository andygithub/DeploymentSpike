Imports Microsoft.Web.Administration

Public Class ChangeIISDefaultFileFixture

    Public Const DefaultWebSite As String = "Default Web Site"
    Public Const DefaultVirtualDirectory As String = "Ping"
    Public Const DefaultPage As String = "DefaultNunit.html"

    Public Shared Function LocalMachineNames() As String
        Return Environment.MachineName & Controller.Utility.Constants.Comma & Environment.MachineName
    End Function

    Public Shared Function LocalWebSites() As String
        Return DefaultWebSite & Controller.Utility.Constants.Comma & DefaultWebSite
    End Function

    Public Shared Function LocalDefaultPages() As String
        Return DefaultPage & Controller.Utility.Constants.Comma & DefaultPage
    End Function

    <Fact()>
    Public Sub null_logging_parameter()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(Nothing)
                                                End Sub)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_null_setting_dictionary()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
        _task.Logger = Nothing
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_valid_server_list()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, LocalMachineNames)
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_invalid_site_list()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, LocalMachineNames)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisSites, String.Empty)
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_invalid_site_list_count()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, LocalMachineNames)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisSites, DefaultWebSite)
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_invalid_page_list_count()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, LocalMachineNames)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisSites, LocalWebSites)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisPages, DefaultPage)
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_valid_site_list()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, LocalMachineNames)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisSites, LocalWebSites)
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_valid()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, LocalMachineNames)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisSites, LocalWebSites)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisPages, LocalDefaultPages)
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        Assert.True(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub SplitItem_null_parameter()
        Dim _item As String = DeploymentSpike.Tasks.AddDefaultPage.SplitItem(Nothing, 0)
        Assert.Null(_item)
    End Sub

    <Fact()>
    Public Sub SplitItem_invalid_index_parameter()
        Dim _item As String = DeploymentSpike.Tasks.AddDefaultPage.SplitItem("fhfhf", 10)
        Assert.Null(_item)
    End Sub

    <Fact()>
    Public Sub SplitItem_valid_parameters()
        Dim _item As String = DeploymentSpike.Tasks.AddDefaultPage.SplitItem("fhfhf,lololpool,lokiji", 2)
        Assert.Equal("lokiji", _item)
    End Sub

    <Fact()>
    Public Sub execute_success_setup_new_default()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, Environment.MachineName)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisSites, DefaultWebSite)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisPages, DefaultPage)
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(Of TaskStatus)(_status, TaskStatus.Completed)
        'check to see if the specified page is now the first default.  and then remove it.
        Using serverManager As ServerManager = serverManager.OpenRemote(Environment.MachineName)
            Dim config As Microsoft.Web.Administration.Configuration = serverManager.GetWebConfiguration(DefaultWebSite)
            Dim defaultDocumentSection As ConfigurationSection = config.GetSection(Tasks.Constants.SystemwebServerdefaultDocument)
            defaultDocumentSection(Tasks.Constants.Enabled) = True
            Dim filesCollection As ConfigurationElementCollection = defaultDocumentSection.GetCollection(Tasks.Constants.Files)
            Dim _value As String = filesCollection(0).GetAttributeValue(Tasks.Constants.Value).ToString
            Assert.Contains(DefaultPage, _value)
            filesCollection.RemoveAt(0)
            serverManager.CommitChanges()
        End Using
    End Sub

    <Fact()>
    Public Sub execute_success_setup_new_default_virtual_directory()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, Environment.MachineName)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisSites, DefaultWebSite)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisVirtualDirectories, DefaultVirtualDirectory)
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisPages, DefaultPage)
        Dim _task As New DeploymentSpike.Tasks.AddDefaultPage(_defaultEmptySettings)
        Dim _status As TaskStatus = _task.Execute
        Assert.Equal(Of TaskStatus)(_status, TaskStatus.Completed)
        'check to see if the specified page is now the first default.  and then remove it.
        Using serverManager As ServerManager = serverManager.OpenRemote(Environment.MachineName)
            Dim config As Microsoft.Web.Administration.Configuration = serverManager.GetWebConfiguration(DefaultWebSite, DefaultVirtualDirectory)
            Dim defaultDocumentSection As ConfigurationSection = config.GetSection(Tasks.Constants.SystemwebServerdefaultDocument)
            defaultDocumentSection(Tasks.Constants.Enabled) = True
            Dim filesCollection As ConfigurationElementCollection = defaultDocumentSection.GetCollection(Tasks.Constants.Files)
            Dim _value As String = filesCollection(0).GetAttributeValue(Tasks.Constants.Value).ToString
            Assert.Contains(DefaultPage, _value)
            filesCollection.RemoveAt(0)
            serverManager.CommitChanges()
        End Using
    End Sub

End Class
