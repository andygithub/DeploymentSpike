Imports System.Security.Permissions
Imports System.Reflection

Namespace SystemTasks

    Public Class ResetIISFixture
        Private Const _WasCommandSuccessful As String = "WasCommandSuccessful"

        Private _localmachineName As String = Environment.MachineName & Controller.Utility.Constants.Comma & Environment.MachineName

        Sub New()

        End Sub

        <Fact()>
        Public Sub null_logging_parameter()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _task As New DeploymentSpike.Tasks.ResetIIS(Nothing)
                                                    End Sub)
        End Sub

        <Fact()>
        Public Sub valid_settings_parameter_null_setting_dictionary()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            Dim _task As New DeploymentSpike.Tasks.ResetIIS(_defaultEmptySettings)
            Assert.True(_task.ValidateSettings)
        End Sub

        <PermissionSetAttribute(SecurityAction.LinkDemand, Name:="FullTrust")>
        <Fact()>
        Public Sub execute_task_valid_settings_parameter_null_setting_dictionary()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            Dim _task As New DeploymentSpike.Tasks.ResetIIS(_defaultEmptySettings)
            Dim _result As Interfaces.TaskStatus = _task.Execute()
            Assert.Equal(Interfaces.TaskStatus.Failed, _result)
        End Sub

        <PermissionSetAttribute(SecurityAction.LinkDemand, Name:="FullTrust")>
        <Fact()>
        Public Sub execute_task_valid_settings_parameter_two_servers_setting_dictionary()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIISServers, _localmachineName)
            Dim _task As New DeploymentSpike.Tasks.ResetIIS(_defaultEmptySettings)
            Dim _result As Interfaces.TaskStatus = _task.Execute()
            Assert.Equal(Interfaces.TaskStatus.Failed, _result)
        End Sub

        <Fact()>
        Public Sub get_task_settings_parameter_two_servers_setting_dictionary()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIISServers, _localmachineName)
            Dim _task As New DeploymentSpike.Tasks.ResetIIS(_defaultEmptySettings)
            Dim _result As ISettings = _task.TaskSettings()
            Assert.Equal(_defaultEmptySettings.Settings.Count, _result.Settings.Count)
        End Sub

        <Fact()>
        Public Sub logger_property()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIISServers, _localmachineName)
            Dim _task As New DeploymentSpike.Tasks.ResetIIS(_defaultEmptySettings)
            Assert.IsType(Of Castle.Core.Logging.NullLogger)(_task.Logger)
            _task.Logger = Nothing
            _task.TaskSettings = Nothing
        End Sub

        <Fact()>
        Public Sub WasCommandSuccessful_null_parameters()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIISServers, _localmachineName)
            Dim _task As New DeploymentSpike.Tasks.ResetIIS(_defaultEmptySettings)
            Dim dynMethod As MethodInfo = _task.GetType().GetMethod(_WasCommandSuccessful, BindingFlags.NonPublic Or BindingFlags.Static)
            Dim result As Object = dynMethod.Invoke(_task, New Object() {Nothing})
            Assert.IsType(Of Boolean)(result)
            Assert.True(CBool(result))
        End Sub

        <Fact()>
        Public Sub WasCommandSuccessful_valid_parameters()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIISServers, _localmachineName)
            Dim _task As New DeploymentSpike.Tasks.ResetIIS(_defaultEmptySettings)
            Dim dynMethod As MethodInfo = _task.GetType().GetMethod(_WasCommandSuccessful, BindingFlags.NonPublic Or BindingFlags.Static)
            Dim result As Object = dynMethod.Invoke(_task, New Object() {"There was not a problem."})
            Assert.IsType(Of Boolean)(result)
            Assert.True(CBool(result))
        End Sub

        <Fact()>
        Public Sub WasCommandSuccessful_invalid_parameters()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIISServers, _localmachineName)
            Dim _task As New DeploymentSpike.Tasks.ResetIIS(_defaultEmptySettings)
            Dim dynMethod As MethodInfo = _task.GetType().GetMethod(_WasCommandSuccessful, BindingFlags.NonPublic Or BindingFlags.Static)
            Dim result As Object = dynMethod.Invoke(_task, New Object() {"There was not a problem ERROR."})
            Assert.IsType(Of Boolean)(result)
            Assert.False(CBool(result))
        End Sub

    End Class

End Namespace