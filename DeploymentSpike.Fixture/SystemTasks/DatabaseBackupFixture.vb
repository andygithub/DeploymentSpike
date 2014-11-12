Imports Microsoft.SqlServer.Management.Smo
Imports Microsoft.SqlServer.Management.Common

Namespace SystemTasks

    Public Class DatabaseBackupFixture
        Implements IDisposable

        Private Const _CTempbackup As String = "C:\Temp\backup\"
        Private Const _BackupDatabase As String = "TestBackup"
        Private Const _localconnectionstring As String = "Server=(local);Database=tempdb;Trusted_Connection=True;"

        Sub New()
            CreateDatabase()
        End Sub

        <Fact()> _
        Public Sub null_logging_parameter()
            Assert.Throws(Of ArgumentNullException)(Sub()
                                                        Dim _task As New DeploymentSpike.Tasks.DatabaseBackup(Nothing)
                                                    End Sub)
        End Sub

        <Fact()> _
        Public Sub valid_settings_parameter_null_setting_dictionary()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            Dim _task As New DeploymentSpike.Tasks.DatabaseBackup(_defaultEmptySettings)
            Assert.False(_task.ValidateSettings)
        End Sub

        <Fact()> _
        Public Sub valid_settings_null_database_null_filename()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigConnectionString, _localconnectionstring)
            Dim _task As New DeploymentSpike.Tasks.DatabaseBackup(_defaultEmptySettings)
            Assert.False(_task.ValidateSettings)
        End Sub

        <Fact()> _
        Public Sub valid_settings_null_filename()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigConnectionString, _localconnectionstring)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDatabase, _BackupDatabase)
            Dim _task As New DeploymentSpike.Tasks.DatabaseBackup(_defaultEmptySettings)
            Assert.False(_task.ValidateSettings)
        End Sub

        <Fact()> _
        Public Sub valid_settings()
            Dim _tempfile As String = _CTempbackup & IO.Path.GetRandomFileName 'IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & '"C:\Temp\backup\temp02.bak"
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigConnectionString, _localconnectionstring)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDatabase, _BackupDatabase)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigBackupFileName, _tempfile)
            Dim _task As New DeploymentSpike.Tasks.DatabaseBackup(_defaultEmptySettings)
            Assert.True(_task.ValidateSettings)
        End Sub

        <Fact()> _
        Public Sub valid_execution()
            Dim _tempfile As String = _CTempbackup & IO.Path.GetRandomFileName 'IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & '"C:\Temp\backup\temp02.bak"
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigConnectionString, _localconnectionstring)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDatabase, _BackupDatabase)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigBackupFileName, _tempfile)
            Dim _task As New DeploymentSpike.Tasks.DatabaseBackup(_defaultEmptySettings)
            Dim _status As TaskStatus = _task.Execute
            Assert.Equal(TaskStatus.Completed, _status)
            Assert.True(IO.File.Exists(_tempfile))
            IO.File.Delete(_tempfile)
        End Sub

        <Fact()> _
        Public Sub valid_settings_null_backupsetdesc()
            Dim _tempfile As String = _CTempbackup & IO.Path.GetRandomFileName
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigConnectionString, _localconnectionstring)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDatabase, _BackupDatabase)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigBackupFileName, _tempfile)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigBackupSetDescription, String.Empty)
            Dim _task As New DeploymentSpike.Tasks.DatabaseBackup(_defaultEmptySettings)
            Dim _status As TaskStatus = _task.Execute
            Assert.Equal(TaskStatus.Completed, _status)
            Assert.True(IO.File.Exists(_tempfile))
            IO.File.Delete(_tempfile)
        End Sub

        <Fact()> _
        Public Sub valid_settings_valid_backupsetdesc()
            Dim _tempfile As String = _CTempbackup & IO.Path.GetRandomFileName
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigConnectionString, _localconnectionstring)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDatabase, _BackupDatabase)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigBackupFileName, _tempfile)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigBackupSetDescription, _BackupDatabase)
            Dim _task As New DeploymentSpike.Tasks.DatabaseBackup(_defaultEmptySettings)
            Dim _status As TaskStatus = _task.Execute
            Assert.Equal(TaskStatus.Completed, _status)
            Assert.True(IO.File.Exists(_tempfile))
            IO.File.Delete(_tempfile)
        End Sub

        <Fact()> _
        Public Sub valid_settings_null_backupsetname()
            Dim _tempfile As String = _CTempbackup & IO.Path.GetRandomFileName
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigConnectionString, _localconnectionstring)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDatabase, _BackupDatabase)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigBackupFileName, _tempfile)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigBackupSetName, String.Empty)
            Dim _task As New DeploymentSpike.Tasks.DatabaseBackup(_defaultEmptySettings)
            Dim _status As TaskStatus = _task.Execute
            Assert.Equal(TaskStatus.Completed, _status)
            Assert.True(IO.File.Exists(_tempfile))
            IO.File.Delete(_tempfile)
        End Sub

        <Fact()> _
        Public Sub valid_settings_null_incremental()
            Dim _tempfile As String = _CTempbackup & IO.Path.GetRandomFileName
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigConnectionString, _localconnectionstring)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDatabase, _BackupDatabase)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigBackupFileName, _tempfile)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigIncremental, String.Empty)
            Dim _task As New DeploymentSpike.Tasks.DatabaseBackup(_defaultEmptySettings)
            Dim _status As TaskStatus = _task.Execute
            Assert.Equal(TaskStatus.Completed, _status)
            Assert.True(IO.File.Exists(_tempfile))
            IO.File.Delete(_tempfile)
        End Sub

        <Fact()> _
        Public Sub valid_settings_null_expirationdate()
            Dim _tempfile As String = _CTempbackup & IO.Path.GetRandomFileName
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigConnectionString, _localconnectionstring)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDatabase, _BackupDatabase)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigBackupFileName, _tempfile)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigExpirationDate, String.Empty)
            Dim _task As New DeploymentSpike.Tasks.DatabaseBackup(_defaultEmptySettings)
            Dim _status As TaskStatus = _task.Execute
            Assert.Equal(TaskStatus.Completed, _status)
            Assert.True(IO.File.Exists(_tempfile))
            IO.File.Delete(_tempfile)
        End Sub

        <Fact()> _
        Public Sub valid_settings_null_logtruncation()
            Dim _tempfile As String = _CTempbackup & IO.Path.GetRandomFileName
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigConnectionString, _localconnectionstring)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigDatabase, _BackupDatabase)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigBackupFileName, _tempfile)
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigLogTruncation, String.Empty)
            Dim _task As New DeploymentSpike.Tasks.DatabaseBackup(_defaultEmptySettings)
            Dim _status As TaskStatus = _task.Execute
            Assert.Equal(TaskStatus.Completed, _status)
            Assert.True(IO.File.Exists(_tempfile))
            IO.File.Delete(_tempfile)
        End Sub

        <Fact()> _
        Public Sub valid_settings_get_settings()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            Dim _task As New DeploymentSpike.Tasks.DatabaseBackup(_defaultEmptySettings)
            Dim _setttings As ISettings = _task.TaskSettings
            Assert.Equal(_setttings, _defaultEmptySettings)
        End Sub

        <Fact()>
        Public Sub logger_property()
            Dim _defaultEmptySettings As New Controller.Domain.TaskSettings
            _defaultEmptySettings.Settings.Add(Tasks.Constants.ConfigBackupFileName, String.Empty)
            Dim _task As New DeploymentSpike.Tasks.DatabaseBackup(_defaultEmptySettings)
            Assert.IsType(Of Castle.Core.Logging.NullLogger)(_task.Logger)
            _task.Logger = Nothing
            _task.TaskSettings = Nothing
        End Sub

#Region "test database creation"

        Private Sub CreateDatabase()
            Using _sqlConnection As New SqlClient.SqlConnection(_localconnectionstring)
                Dim _server As New Server(New ServerConnection(_sqlConnection))
                Dim db As Database
                db = New Database(_server, _BackupDatabase)
                'Create the database on the instance of SQL Server.
                db.Create()
            End Using
        End Sub

#End Region

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then

                End If
                'drop created database
                Using _sqlConnection As New SqlClient.SqlConnection(_localconnectionstring)
                    Dim _server As New Server(New ServerConnection(_sqlConnection))
                    _server.Databases(_BackupDatabase).Drop()
                End Using
            End If
            Me.disposedValue = True
        End Sub

        Protected Overrides Sub Finalize()
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(False)
            MyBase.Finalize()
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

#End Region

    End Class

End Namespace