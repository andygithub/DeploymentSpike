Imports Castle.Core.Logging
Imports Microsoft.SqlServer.Management.Smo
Imports Microsoft.SqlServer.Management.Common

''' <summary>
''' Task for backing up a SQL Server database.
''' </summary>
''' <remarks></remarks>
Public Class DatabaseBackup
    Implements ITask

    Private _settings As Interfaces.ISettings
    Private _logger As ILogger

    Sub New(settings As Interfaces.ISettings)
        If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
        _settings = settings
    End Sub

    Public Function Execute() As Interfaces.TaskStatus Implements Interfaces.ITask.Execute
        PopulateSettingsDefaults()
        PerformDatabaseBackup()
        Return TaskStatus.Completed
    End Function

    Public Property MetaInformation As Interfaces.ITaskInformation Implements Interfaces.ITask.MetaInformation

    Public Property MetaInformationList As IEnumerable(Of Interfaces.ITaskInformation) Implements Interfaces.ITask.MetaInformationList

    Public Property Logger As Castle.Core.Logging.ILogger Implements Interfaces.ITask.Logger
        Get
            If _logger Is Nothing Then
                _logger = New Castle.Core.Logging.NullLogger
            End If
            Return _logger
        End Get
        Set(value As Castle.Core.Logging.ILogger)
            _logger = value
        End Set
    End Property

    Public Property TaskSettings As ISettings Implements Interfaces.ITask.TaskSettings
        Get
            Return _settings
        End Get
        Set(value As ISettings)
            _settings = value
        End Set
    End Property

    Public Function ValidateSettings() As Boolean Implements ITask.ValidateSettings
        'require connection string, database name and backup file
        If Not _settings.Settings.ContainsKey(Constants.ConfigConnectionString) OrElse String.IsNullOrWhiteSpace(_settings.Settings(Constants.ConfigConnectionString)) Then
            Logger.Fatal(My.Resources.Messages.MissingConnectionStringSetting)
            Return False
        End If
        If Not _settings.Settings.ContainsKey(Constants.ConfigDatabase) OrElse String.IsNullOrWhiteSpace(_settings.Settings(Constants.ConfigDatabase)) Then
            Logger.Fatal(My.Resources.Messages.MissingDatabaseSetting)
            Return False
        End If
        If Not _settings.Settings.ContainsKey(Constants.ConfigBackupFileName) OrElse String.IsNullOrWhiteSpace(_settings.Settings(Constants.ConfigBackupFileName)) Then
            Logger.Fatal(My.Resources.Messages.MissingBackupFile)
            Return False
        End If
        Return True
    End Function

    Private Sub PerformDatabaseBackup()
        'assume that all settings have been validated and properly defaulted
        Dim _backupFileName As String = _settings.Settings(Constants.ConfigBackupFileName)

        'Connection
        Dim _connectionString As String = _settings.Settings(Constants.ConfigConnectionString)

        Using _sqlConnection As New SqlClient.SqlConnection(_connectionString)
            Dim _server As New Server(New ServerConnection(_sqlConnection))
            Logger.Debug(My.Resources.Messages.OpenServerConnection)
            'Define a Backup object variable. 
            Dim bk As New Backup
            'Specify the type of backup, the description, the name, and the database to be backed up.
            bk.Action = BackupActionType.Database
            bk.BackupSetDescription = _settings.Settings(Constants.ConfigBackupSetDescription)
            bk.BackupSetName = _settings.Settings(Constants.ConfigBackupSetName)
            bk.Database = _settings.Settings(Constants.ConfigDatabase)
            'Declare a BackupDeviceItem by supplying the backup device file name in the constructor, and the type of device is a file.
            Dim bdi As New BackupDeviceItem(_backupFileName, DeviceType.File)
            'Add the device to the Backup object.
            bk.Devices.Add(bdi)
            'Set the Incremental property to False to specify that this is a full database backup.
            bk.Incremental = CBool(_settings.Settings(Constants.ConfigIncremental))
            'Set the expiration date.
            bk.ExpirationDate = CDate(_settings.Settings(Constants.ConfigExpirationDate))
            'Specify that the log must be truncated after the backup is complete.
            bk.LogTruncation = CType(_settings.Settings(Constants.ConfigLogTruncation), BackupTruncateLogType)
            'Run SqlBackup to perform the full database backup on the instance of SQL Server.
            Logger.Debug(My.Resources.Messages.StartingBackup)
            bk.SqlBackup(_server)
            Logger.Debug(My.Resources.Messages.EndingBackup)
            'Remove the backup device from the Backup object.
            bk.Devices.Remove(bdi)
        End Using

    End Sub

    ''' <summary>
    ''' This method is intened to set all settings values that have not been specified to valid defaults.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateSettingsDefaults()
        Dim _newValues As New Dictionary(Of String, String)
        'add validate values
        _newValues.Add(Constants.ConfigConnectionString, _settings.Settings(Constants.ConfigConnectionString))
        _newValues.Add(Constants.ConfigDatabase, _settings.Settings(Constants.ConfigDatabase))
        _newValues.Add(Constants.ConfigBackupFileName, _settings.Settings(Constants.ConfigBackupFileName))

        'set defaults to rest of the settings if they weren't supplied
        _newValues.Add(Constants.ConfigBackupSetDescription, GetValidSettingValue(Constants.ConfigBackupSetDescription, My.Resources.Messages.BackupofDatabase))
        _newValues.Add(Constants.ConfigBackupSetName, GetValidSettingValue(Constants.ConfigBackupSetName, My.Resources.Messages.DatabaseBackup & Now.ToString(System.Globalization.CultureInfo.InvariantCulture)))
        _newValues.Add(Constants.ConfigIncremental, GetValidSettingValue(Constants.ConfigIncremental, Boolean.FalseString))
        _newValues.Add(Constants.ConfigExpirationDate, GetValidSettingValue(Constants.ConfigExpirationDate, Now.AddMonths(1).ToString(System.Globalization.CultureInfo.InvariantCulture)))
        _newValues.Add(Constants.ConfigLogTruncation, GetValidSettingValue(Constants.ConfigLogTruncation, CStr(BackupTruncateLogType.Truncate)))

        _settings.Settings = _newValues
    End Sub

    Private Function GetValidSettingValue(key As String, defaultValue As String) As String
        If Not _settings.Settings.ContainsKey(key) Then Return defaultValue
        If String.IsNullOrWhiteSpace(_settings.Settings(key)) Then Return defaultValue
        Return _settings.Settings(key)
    End Function

End Class
