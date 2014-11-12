Imports DeploymentSpike.Interfaces
Imports DeploymentSpike.Controller.Interfaces
Imports System.IO
Imports DeploymentSpike.Controller.Configuration
Imports Castle.Core.Logging

Namespace Factory

    ''' <summary>
    ''' Factory to load settings from a specified file.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class TaskNotificationSettingsFactory

        Private Sub New()

        End Sub

        Shared Function Create(fileName As String, logger As ILogger, settings As IConfigurationSettingsManager) As ISettings
            If logger Is Nothing Then Throw New ArgumentNullException(Constants.LoggerParameterName)
            If settings Is Nothing Then Throw New ArgumentNullException(Constants.SettingsParameterName)
            If String.IsNullOrWhiteSpace(filename) Then
                logger.Debug(My.Resources.Messages.TaskNameWasNull)
                Return GetDefaultSettings(settings)
            End If
            'determine the task to get the settings
            Dim _loadfile As String = GetFullPathName(settings, filename)

            'check settings to see if settings files has been specified
            If Not IO.File.Exists(_loadfile) Then
                logger.Debug(My.Resources.Messages.FileNotFound & _loadfile & My.Resources.Messages.ForComponent & filename)
                Return GetDefaultSettings(settings)
            End If
            'attempt to load settings
            Return LoadSettings(_loadfile, settings, logger)
        End Function

        Public Shared Function GetSettingsFolderName(configurationSettings As IConfigurationSettingsManager) As String
            If configurationSettings Is Nothing OrElse String.IsNullOrWhiteSpace(configurationSettings.ConfigFilePath) Then Return Utility.Constants.Config
            'check the default location for the existinacne of the file
            Return configurationSettings.ConfigFilePath
        End Function

        Public Shared Function GetFullPathName(settings As IConfigurationSettingsManager, fileName As String) As String
            If IO.Path.IsPathRooted(fileName) Then Return fileName
            Return IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & TaskNotificationSettingsFactory.GetSettingsFolderName(settings) & IO.Path.DirectorySeparatorChar & fileName
        End Function

        Public Shared Function LoadSettings(settingsFile As String, appSettings As IConfigurationSettingsManager, logger As ILogger) As ISettings
            If logger Is Nothing Then Throw New ArgumentNullException(Constants.LoggerParameterName)
            If String.IsNullOrWhiteSpace(settingsFile) Then Return GetDefaultSettings(appSettings)
            Using _writer As New FileStream(settingsFile, FileMode.Open)
                Dim _loadedSetting As ISettings = ServiceStack.Text.JsonSerializer.DeserializeFromStream(Of ISettings)(_writer)
                If _loadedSetting Is Nothing Then
                    logger.Debug(My.Resources.Messages.GeneratingDefaultSettings)
                    Return GetDefaultSettings(appSettings)
                Else
                    logger.Debug(My.Resources.Messages.UsingResultsFrom & settingsFile)
                    _loadedSetting.SettingsLocation = settingsFile
                    Return (MergeTaskSettings(GetDefaultSettings(appSettings), _loadedSetting, appSettings))
                End If
            End Using
        End Function

        Public Shared Function GetDefaultSettings(appSettings As IConfigurationSettingsManager) As ISettings
            Dim _defaultSettings As New Domain.TaskSettings
            _defaultSettings.SettingsLocation = My.Resources.Messages.NoLocationUsed
            _defaultSettings.Settings.Add(Utility.Constants.ConfigExecutionSwitch, CStr(If(appSettings Is Nothing, ExecutionType.NotSet, appSettings.ExecutionSwitch)))

            Return _defaultSettings
        End Function

        Public Shared Function MergeTaskSettings(defaultSettings As ISettings, loadedSettings As ISettings, appSettings As IConfigurationSettingsManager) As ISettings
            If defaultSettings Is Nothing AndAlso loadedSettings Is Nothing Then Return GetDefaultSettings(appSettings)
            If defaultSettings Is Nothing Then Return loadedSettings
            If loadedSettings Is Nothing OrElse loadedSettings.Settings Is Nothing OrElse loadedSettings.Settings.Keys.Count = 0 Then Return defaultSettings
            'loop through all loaded settings and either update value of load key,value
            Dim result As Dictionary(Of String, String) = defaultSettings.Settings
            For Each x In loadedSettings.Settings
                If Not result.ContainsKey(x.Key) Then
                    result.Add(x.Key, x.Value)
                Else
                    result(x.Key) = x.Value
                End If
            Next
            defaultSettings.Settings = result
            defaultSettings.SettingsLocation = loadedSettings.SettingsLocation
            Return defaultSettings
        End Function


    End Class


End Namespace
