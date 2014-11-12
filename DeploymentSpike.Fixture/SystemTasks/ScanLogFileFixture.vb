

Namespace SystemTasks

    Public Class ScanLogFileFixture
        Private Const RegExpression_ErrorWarningException As String = "(Error |Warning |Exception)"

        <Fact()>
        Public Sub null_setting_parameter()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _task As New DeploymentSpike.Tasks.ScanLogFile(Nothing)
                                                    End Sub)
        End Sub

        <Fact()>
        Public Sub logger_property()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            Dim _task As New DeploymentSpike.Tasks.ScanLogFile(_defaultEmptySettings)
            Assert.False(_task.ValidateSettings)
            _task.Logger = Nothing
            _task.TaskSettings = Nothing
            Dim _settings As ISettings = _task.TaskSettings
        End Sub

        <Fact()> _
        Public Sub valid_settings_parameter_null_setting_dictionary()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            Dim _task As New DeploymentSpike.Tasks.ScanLogFile(_defaultEmptySettings)
            Assert.False(_task.ValidateSettings)
        End Sub

        <Fact()> _
        Public Sub valid_settings_invalid_config_file()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigFile, PurgeExistingArtifactsFixture.GetInvalidTempTextFile)
            Dim _task As New DeploymentSpike.Tasks.ScanLogFile(_defaultEmptySettings)
            Assert.False(_task.ValidateSettings)
        End Sub

        <Fact()> _
        Public Sub valid_settings_valid_config_file()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigFile, PurgeExistingArtifactsFixture.GetValidTempTextFile)
            Dim _task As New DeploymentSpike.Tasks.ScanLogFile(_defaultEmptySettings)
            Assert.False(_task.ValidateSettings)
        End Sub

        <Fact()> _
        Public Sub valid_settings_valid_regular_expression()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigFile, PurgeExistingArtifactsFixture.GetValidTempTextFile)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigRegularExpression, RegExpression_ErrorWarningException)
            Dim _task As New DeploymentSpike.Tasks.ScanLogFile(_defaultEmptySettings)
            Assert.True(_task.ValidateSettings)
        End Sub

        <Fact()> _
        Public Sub valid_settings_no_matches_found()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            Dim _tempFileName As String = IO.Path.GetTempFileName
            IO.File.AppendAllText(_tempFileName, PurgeExistingArtifactsFixture.Temp)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigFile, PurgeExistingArtifactsFixture.GetValidTempTextFile)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigRegularExpression, RegExpression_ErrorWarningException)
            Dim _task As New DeploymentSpike.Tasks.ScanLogFile(_defaultEmptySettings)
            Dim _status As TaskStatus = _task.Execute
            Assert.Equal(TaskStatus.Completed, _status)
            IO.File.Delete(_tempFileName)
        End Sub

        <Fact()> _
        Public Sub valid_settings_match_found()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            Dim _tempFileName As String = IO.Path.GetTempFileName
            IO.File.AppendAllText(_tempFileName, "This is a string that has an Exception in it.")
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigFile, _tempFileName)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigRegularExpression, RegExpression_ErrorWarningException)
            Dim _task As New DeploymentSpike.Tasks.ScanLogFile(_defaultEmptySettings)
            Dim _status As TaskStatus = _task.Execute
            Assert.Equal(TaskStatus.Failed, _status)
            IO.File.Delete(_tempFileName)
        End Sub

    End Class

End Namespace