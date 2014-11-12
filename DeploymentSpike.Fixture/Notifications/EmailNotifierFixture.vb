Imports DeploymentSpike.Notification
Imports DeploymentSpike.Tasks

Public Class EmailNotifierFixture
    Private Const _LocalHost As String = "localhost"
    Private Const _DefaultPort As String = "25"
    Public Const XunitTask As String = "xunit Task"
    Private Const _testEmailAddress As String = "amaurer@deloitte.com"
    Private Const _Emailtxt As String = "email.txt"

    <Fact()>
    Public Sub null_logging_parameter()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Dim _task As New DeploymentSpike.Notification.EmailNotifier(Nothing)
                                                End Sub)

    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_null_setting_dictionary()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
        _task.Logger = Nothing
        Dim _name As String = _task.Name
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_invalid_file()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFile, PurgeExistingArtifactsFixture.GetInvalidTempTextFile)
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_valid_file()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFile, PurgeExistingArtifactsFixture.GetValidTempTextFile)
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_valid_from_email()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFile, PurgeExistingArtifactsFixture.GetValidTempTextFile)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromAddress, FixtureConstants.CastleCore)
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_valid_from_name()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFile, PurgeExistingArtifactsFixture.GetValidTempTextFile)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromAddress, FixtureConstants.CastleCore)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromName, FixtureConstants.CastleCore)
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_valid_host()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFile, PurgeExistingArtifactsFixture.GetValidTempTextFile)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromAddress, FixtureConstants.CastleCore)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromName, FixtureConstants.CastleCore)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailHost, _LocalHost)
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Assert.False(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub valid_settings_parameter_valid_port()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFile, PurgeExistingArtifactsFixture.GetValidTempTextFile)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromAddress, FixtureConstants.CastleCore)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromName, FixtureConstants.CastleCore)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailHost, _LocalHost)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailPort, _DefaultPort)
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Assert.True(_task.ValidateSettings)
    End Sub

    <Fact()>
    Public Sub loadaddresses_invalid_file()
        Dim _file As String = PurgeExistingArtifactsFixture.GetInvalidTempTextFile()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Dim _items As List(Of String) = _task.LoadAddresses(_file)
        Assert.True(_items.Count = 0)
        IO.File.Delete(_file)
    End Sub

    <Fact()>
    Public Sub loadaddresses_invalid__empty_file()
        Dim _file As String = PurgeExistingArtifactsFixture.GetValidTempTextFile(String.Empty)
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Dim _items As List(Of String) = _task.LoadAddresses(_file)
        Assert.True(_items.Count = 0)
        IO.File.Delete(_file)
    End Sub

    <Fact()>
    Public Sub loadaddresses_valid_file()
        Dim _contents As String = "non@none.com,noone@none.com,one@none.com"
        Dim _file As String = PurgeExistingArtifactsFixture.GetValidTempTextFile(_contents)
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Dim _items As List(Of String) = _task.LoadAddresses(_file)
        Assert.True(_items.Count = _contents.Split(Controller.Utility.Constants.Comma).Count)
        IO.File.Delete(_file)
    End Sub

    <Fact()>
    Public Sub subjectline_extension_method_starting()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Dim _line As String = _task.ToEmailSubjectLine(NotificationType.Started)
        Assert.Contains(Notification.My.Resources.Messages.DefaultLabel, _line)
    End Sub

    <Fact()>
    Public Sub subjectline_extension_method_completedsuccess()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Dim _line As String = _task.ToEmailSubjectLine(NotificationType.CompletedSuccess)
        Assert.Contains(Notification.My.Resources.Messages.DefaultLabel, _line)
    End Sub

    <Fact()>
    Public Sub subjectline_extension_method_completedfailure()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Dim _line As String = _task.ToEmailSubjectLine(NotificationType.CompletedFailure)
        Assert.Contains(Notification.My.Resources.Messages.DefaultLabel, _line)
    End Sub

    <Fact()>
    Public Sub subjectline_extension_method_completedfailure_non_default_taskname()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        _task.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.DeploymentTask = XunitTask}
        Dim _line As String = _task.ToEmailSubjectLine(NotificationType.CompletedFailure)
        Assert.Contains(XunitTask, _line)
    End Sub

    <Fact()>
    Public Sub emailbody_extension_method_completedfailure_non_default_taskname()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        _task.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.DeploymentTask = XunitTask}
        Dim _line As String = _task.ToEmailBody(NotificationType.CompletedFailure)
        Assert.Contains(XunitTask, _line)
    End Sub

    <Fact()>
    Public Sub emailbody_extension_method_completedfailure_non_default_taskname_meta_properies()
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        Dim _task As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        _task.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.DeploymentTask = XunitTask}
        _task.MetaInformationList = GenerateTaskReportFixture.GenerateTaskMetaSteps
        Dim _line As String = _task.ToEmailBody(NotificationType.CompletedFailure)
        Assert.Contains(XunitTask, _line)
    End Sub

    <Fact()>
    Public Sub send_email_start()
        Dim _tempfile As String = IO.Path.GetTempPath & IO.Path.DirectorySeparatorChar & _Emailtxt
        IO.File.WriteAllText(_tempfile, _testEmailAddress)
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFile, _tempfile)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromAddress, _testEmailAddress)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromName, FixtureConstants.CastleCore)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailHost, _LocalHost)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailPort, _DefaultPort)
        Dim _purgetask As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        _purgetask.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.DeploymentTask = XunitTask}
        _purgetask.MetaInformationList = GenerateTaskReportFixture.GenerateTaskMetaSteps
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Assert.Throws(Of System.Net.Mail.SmtpException)(Sub()
                                                            _task.SendEmail(_purgetask, NotificationType.Started)
                                                        End Sub)
        'Assert.True(True)
    End Sub

    <Fact()>
    Public Sub send_email_start_multiple_email_list()
        Dim _tempfile As String = IO.Path.GetTempPath & IO.Path.DirectorySeparatorChar & _Emailtxt
        IO.File.WriteAllText(_tempfile, _testEmailAddress & Controller.Utility.Constants.Comma & _testEmailAddress)
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFile, _tempfile)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromAddress, _testEmailAddress)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromName, FixtureConstants.CastleCore)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailHost, _LocalHost)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailPort, _DefaultPort)
        Dim _purgetask As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        _purgetask.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.DeploymentTask = XunitTask}
        _purgetask.MetaInformationList = GenerateTaskReportFixture.GenerateTaskMetaSteps
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Assert.Throws(Of System.Net.Mail.SmtpException)(Sub()
                                                            _task.SendEmail(_purgetask, NotificationType.Started)
                                                        End Sub)
        'Assert.True(True)
    End Sub

    <Fact()>
    Public Sub send_email_start_null_email_list()
        Dim _tempfile As String = IO.Path.GetTempPath & IO.Path.DirectorySeparatorChar & _Emailtxt
        IO.File.WriteAllText(_tempfile, String.Empty)
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFile, _tempfile)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromAddress, _testEmailAddress)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromName, FixtureConstants.CastleCore)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailHost, _LocalHost)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailPort, _DefaultPort)
        Dim _purgetask As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        _purgetask.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.DeploymentTask = XunitTask}
        _purgetask.MetaInformationList = GenerateTaskReportFixture.GenerateTaskMetaSteps
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        _task.SendEmail(_purgetask, NotificationType.Started)
        Assert.True(True)
    End Sub

    <Fact()>
    Public Sub NotifyStarted_populated_metainformation()
        Dim _tempfile As String = IO.Path.GetTempPath & IO.Path.DirectorySeparatorChar & _Emailtxt
        IO.File.WriteAllText(_tempfile, _testEmailAddress)
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFile, _tempfile)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromAddress, _testEmailAddress)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromName, FixtureConstants.CastleCore)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailHost, _LocalHost)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailPort, _DefaultPort)
        Dim _purgetask As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        _purgetask.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.DeploymentTask = XunitTask}
        _purgetask.MetaInformationList = GenerateTaskReportFixture.GenerateTaskMetaSteps
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Assert.Throws(Of System.Net.Mail.SmtpException)(Sub()
                                                            _task.NotifyStarted(_purgetask)
                                                        End Sub)
        'Assert.True(True)
    End Sub

    <Fact()>
    Public Sub NotifyStarted_null_metainformation()
        Dim _tempfile As String = IO.Path.GetTempPath & IO.Path.DirectorySeparatorChar & _Emailtxt
        IO.File.WriteAllText(_tempfile, _testEmailAddress)
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFile, _tempfile)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromAddress, _testEmailAddress)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromName, FixtureConstants.CastleCore)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailHost, _LocalHost)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailPort, _DefaultPort)
        Dim _purgetask As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Assert.Throws(Of System.Net.Mail.SmtpException)(Sub()
                                                            _task.NotifyStarted(_purgetask)
                                                        End Sub)
        'Assert.True(True)
    End Sub

    <Fact()>
    Public Sub NotifyCompletedSuccess_populated_metainformation()
        Dim _tempfile As String = IO.Path.GetTempPath & IO.Path.DirectorySeparatorChar & _Emailtxt
        IO.File.WriteAllText(_tempfile, _testEmailAddress)
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFile, _tempfile)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromAddress, _testEmailAddress)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromName, FixtureConstants.CastleCore)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailHost, _LocalHost)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailPort, _DefaultPort)
        Dim _purgetask As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        _purgetask.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.DeploymentTask = XunitTask}
        _purgetask.MetaInformationList = GenerateTaskReportFixture.GenerateTaskMetaSteps
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Assert.Throws(Of System.Net.Mail.SmtpException)(Sub()
                                                            _task.NotifyCompletedSuccess(_purgetask)
                                                        End Sub)
        'Assert.True(True)
    End Sub

    <Fact()>
    Public Sub NotifyCompletedSuccess_null_metainformation()
        Dim _tempfile As String = IO.Path.GetTempPath & IO.Path.DirectorySeparatorChar & _Emailtxt
        IO.File.WriteAllText(_tempfile, _testEmailAddress)
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFile, _tempfile)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromAddress, _testEmailAddress)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromName, FixtureConstants.CastleCore)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailHost, _LocalHost)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailPort, _DefaultPort)
        Dim _purgetask As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Assert.Throws(Of System.Net.Mail.SmtpException)(Sub()
                                                            _task.NotifyCompletedSuccess(_purgetask)
                                                        End Sub)
        'Assert.True(True)
    End Sub

    <Fact()>
    Public Sub NotifyCompletedFailure_populated_metainformation()
        Dim _tempfile As String = IO.Path.GetTempPath & IO.Path.DirectorySeparatorChar & _Emailtxt
        IO.File.WriteAllText(_tempfile, _testEmailAddress)
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFile, _tempfile)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromAddress, _testEmailAddress)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromName, FixtureConstants.CastleCore)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailHost, _LocalHost)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailPort, _DefaultPort)
        Dim _purgetask As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        _purgetask.MetaInformation = New Controller.Domain.TaskMetaInformation() With {.DeploymentTask = XunitTask}
        _purgetask.MetaInformationList = GenerateTaskReportFixture.GenerateTaskMetaSteps
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Assert.Throws(Of System.Net.Mail.SmtpException)(Sub()
                                                            _task.NotifyCompletedFailure(_purgetask)
                                                        End Sub)
        'Assert.True(True)
    End Sub

    <Fact()>
    Public Sub NotifyCompletedFailure_null_metainformation()
        Dim _tempfile As String = IO.Path.GetTempPath & IO.Path.DirectorySeparatorChar & _Emailtxt
        IO.File.WriteAllText(_tempfile, _testEmailAddress)
        Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFile, _tempfile)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromAddress, _testEmailAddress)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailFromName, FixtureConstants.CastleCore)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailHost, _LocalHost)
        _defaultEmptySettings.Settings.Add(Notification.Constants.ConfigEmailPort, _DefaultPort)
        Dim _purgetask As New Tasks.PurgeExistingArtifacts(_defaultEmptySettings)
        Dim _task As New DeploymentSpike.Notification.EmailNotifier(_defaultEmptySettings)
        Assert.Throws(Of System.Net.Mail.SmtpException)(Sub()
                                                            _task.NotifyCompletedFailure(_purgetask)
                                                        End Sub)
        'Assert.True(True)
    End Sub

End Class
