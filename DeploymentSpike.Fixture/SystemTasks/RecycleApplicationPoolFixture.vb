Public Class RecycleApplicationPoolFixture

    Private Function _localmachineName() As String
        Return Environment.MachineName & Controller.Utility.Constants.Comma & Environment.MachineName
    End Function

    <Fact()>
    Public Sub null_logging_parameter()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Dim _task As New DeploymentSpike.Tasks.RecycleApplicationPools(Nothing)
                                                End Sub)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_null_setting_dictionary()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Tasks.RecycleApplicationPools(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
        _task.Logger = Nothing
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, _localmachineName)
        Dim _task As New DeploymentSpike.Tasks.RecycleApplicationPools(_defaultEmptySettings)
        Assert.True(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub execute_exception_com_access_denied()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIisServers, _localmachineName)
        Dim _task As New DeploymentSpike.Tasks.RecycleApplicationPools(_defaultEmptySettings)
        Assert.Throws(Of System.UnauthorizedAccessException)(Sub()
                                                                 Dim _status As TaskStatus = _task.Execute
                                                             End Sub)
    End Sub

End Class
