
Namespace Domain

    ''' <summary>
    ''' Implementation of ISettings.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TaskSettings
        Implements DeploymentSpike.Interfaces.ISettings

        Sub New()
            Settings = New Dictionary(Of String, String)
        End Sub

        Public Property SettingsLocation As String Implements DeploymentSpike.Interfaces.ISettings.SettingsLocation
        Public Property Settings As System.Collections.Generic.Dictionary(Of String, String) Implements DeploymentSpike.Interfaces.ISettings.Settings

        Public Function GetExecutionFlag() As DeploymentSpike.Interfaces.ExecutionType Implements DeploymentSpike.Interfaces.ISettings.GetExecutionFlag
            If Not Settings.ContainsKey(Utility.Constants.ConfigExecutionSwitch) Then Return ExecutionType.NotSet
            Dim _value As ExecutionType
            If [Enum].TryParse(Settings(Utility.Constants.ConfigExecutionSwitch), _value) Then
                Return _value
            Else
                Return ExecutionType.NotSet
            End If
        End Function

    End Class

End Namespace