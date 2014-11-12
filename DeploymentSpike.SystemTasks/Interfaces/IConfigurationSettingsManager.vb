Namespace Interfaces
    ''' <summary>
    ''' Interface for the application settings.
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IConfigurationSettingsManager

        Sub LoadSetting(ByVal key As String, ByVal value As String)
        Sub LoadSetting(ByVal list As IDictionary(Of String, String))
        Property ApplicationSettings() As IDictionary(Of String, String)
        ReadOnly Property AssemblyList() As String
        ReadOnly Property NotificationClass() As String
        ReadOnly Property TaskList() As String
        ReadOnly Property LogConfigurationFile As String
        ReadOnly Property ConfigFilePath() As String
        ReadOnly Property ExecutionSwitch As ExecutionType

    End Interface

End Namespace