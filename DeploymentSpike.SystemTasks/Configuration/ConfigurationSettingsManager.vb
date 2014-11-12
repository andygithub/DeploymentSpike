Imports DeploymentSpike.Controller.Interfaces
Imports DeploymentSpike.Controller.Utility

Namespace Configuration

    ''' <summary>
    ''' This file is intended to handle loading configuration values and merging in other configured settings.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ConfigurationSettingsManager
        Implements IConfigurationSettingsManager


        ''' <summary>
        ''' The default constructor for the class that handles loading settings.
        ''' </summary>
        ''' <param name="initialSettings">A list of configuration setting values that is expected to be populated from the configuration file.</param>
        ''' <param name="overwrittenSettings">A list of configuration setting values that is expected to be populated from the command line arguments.</param>
        ''' <remarks></remarks>
        Sub New(initialSettings As IDictionary(Of String, String), overwrittenSettings As IDictionary(Of String, String))
            ApplicationSettings = New Dictionary(Of String, String)
            LoadConfigurationSettings(initialSettings)
            LoadSetting(overWrittenSettings)
        End Sub

        Private Sub LoadConfigurationSettings(list As IDictionary(Of String, String))
            If list Is Nothing Then Exit Sub
            ApplicationSettings = list
        End Sub

        ''' <summary>
        ''' Method to update or create setting entry.
        ''' </summary>
        ''' <param name="key"></param>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        Public Sub LoadSetting(key As String, value As String) Implements IConfigurationSettingsManager.LoadSetting
            If key Is Nothing Then Exit Sub
            If ApplicationSettings.ContainsKey(key) Then
                ApplicationSettings(key) = value
            Else
                ApplicationSettings.Add(key, value)
            End If
        End Sub

        ''' <summary>
        ''' Method to update or create setting entry.
        ''' </summary>
        ''' <param name="list"></param>
        ''' <remarks></remarks>
        Public Sub LoadSetting(list As IDictionary(Of String, String)) Implements IConfigurationSettingsManager.LoadSetting
            If list Is Nothing Then Exit Sub
            For Each item As KeyValuePair(Of String, String) In list
                LoadSetting(item.Key, item.Value)
            Next
        End Sub

        ''' <summary>
        ''' The dictionary list of settings that have been loaded and consolidated by the manager.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ApplicationSettings() As IDictionary(Of String, String) Implements IConfigurationSettingsManager.ApplicationSettings

        ''' <summary>
        ''' The list of assemblies located in the application directory to be scanned.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property AssemblyList As String Implements IConfigurationSettingsManager.AssemblyList
            Get
                If ApplicationSettings.ContainsKey(Constants.ConfigAssemblyNamesKey) Then
                    Return ApplicationSettings.Item(Constants.ConfigAssemblyNamesKey)
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        ''' <summary>
        ''' The class to use for the loggging implementation.  This is used if there are multiple classes in the assemblies that implement the INotification interface and only a subset should be used.  However notification will loop through all implementations if nothing is specified.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NotificationClass As String Implements IConfigurationSettingsManager.NotificationClass
            Get
                If ApplicationSettings.ContainsKey(Constants.ConfigNotificationClass) Then
                    Return ApplicationSettings.Item(Constants.ConfigNotificationClass)
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        ''' <summary>
        ''' The list of tasks to be executed and the order to execute them in.  This task name should be a fully qualified name.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property TaskList As String Implements IConfigurationSettingsManager.TaskList
            Get
                If ApplicationSettings.ContainsKey(Constants.ConfigTaskList) Then
                    Return ApplicationSettings.Item(Constants.ConfigTaskList)
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        ''' <summary>
        ''' The folder path where the configuration files for tasks are located.  The default value is \config.  By default the configuration file will be the same name as the task.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ConfigFilePath As String Implements IConfigurationSettingsManager.ConfigFilePath
            Get
                If ApplicationSettings.ContainsKey(Constants.ConfigConfigFilePath) Then
                    Return ApplicationSettings.Item(Constants.ConfigConfigFilePath)
                Else
                    Return Constants.Config
                End If
            End Get
        End Property

        ''' <summary>
        ''' The xml log4net file to use for configuration of the deployment tool.  If no file is specified then default settings will be used.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property LogConfigurationFile As String Implements IConfigurationSettingsManager.LogConfigurationFile
            Get
                If ApplicationSettings.ContainsKey(Constants.ConfigLogConfigurationFile) Then
                    Return ApplicationSettings.Item(Constants.ConfigLogConfigurationFile)
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        Public ReadOnly Property ExecutionSwitch As DeploymentSpike.Interfaces.ExecutionType Implements IConfigurationSettingsManager.ExecutionSwitch
            Get
                If Not ApplicationSettings.ContainsKey(Constants.ConfigExecutionSwitch) Then Return ExecutionType.NotSet
                Dim _value As ExecutionType
                If [Enum].TryParse(ApplicationSettings(Constants.ConfigExecutionSwitch), _value) Then
                    Return _value
                Else
                    Return ExecutionType.NotSet
                End If
            End Get
        End Property
    End Class

End Namespace