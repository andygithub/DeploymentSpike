Imports Castle.Core.Logging
''' <summary>
''' Task to execute a specific file.
''' </summary>
''' <remarks></remarks>
Public Class ExecuteFile
    Implements ITask

    Private _logger As ILogger

    Sub New(settings As Interfaces.ISettings)
        If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
        TaskSettings = settings
        SetupDefaults()
    End Sub

    Private Sub SetupDefaults()
        'there does not need to be arguments
        If Not TaskSettings.Settings.ContainsKey(Constants.ConfigExecutingArguments) Then
            TaskSettings.Settings.Add(Constants.ConfigExecutingArguments, String.Empty)
        End If
        'there does not need to be redirect
        If Not TaskSettings.Settings.ContainsKey(Constants.ConfigExecutingRedirect) Then
            TaskSettings.Settings.Add(Constants.ConfigExecutingRedirect, Boolean.TrueString)
        End If
    End Sub

    <Security.SecurityCritical()>
    Public Function Execute() As Interfaces.TaskStatus Implements Interfaces.ITask.Execute
        Dim _result As Boolean = ExecuteFile(TaskSettings.Settings(Constants.ConfigExecutingFileName), TaskSettings.Settings(Constants.ConfigExecutingArguments), TaskSettings.Settings(Constants.ConfigExecutingRedirect))
        Return _result.ToTaskStatus
    End Function

    Public Property MetaInformation As Interfaces.ITaskInformation Implements Interfaces.ITask.MetaInformation
    Public Property MetaInformationList As IEnumerable(Of Interfaces.ITaskInformation) Implements Interfaces.ITask.MetaInformationList
    Public Property TaskSettings As ISettings Implements Interfaces.ITask.TaskSettings

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

    Public Function ValidateSettings() As Boolean Implements Interfaces.ITask.ValidateSettings
        'need executing file name eg ping.exe, don't need redirect io can default to true, don't need arguments for the execution
        If Not TaskSettings.Settings.ContainsKey(Constants.ConfigExecutingFileName) OrElse String.IsNullOrWhiteSpace(TaskSettings.Settings(Constants.ConfigExecutingFileName)) Then
            Logger.Fatal(My.Resources.Messages.MissingExecutingFileName)
            Return False
        End If
        Return True
    End Function

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")>
    Private Function ExecuteFile(filename As String, arguments As String, redirect As String) As Boolean
        Dim _redirect As Boolean = True
        If redirect = Boolean.FalseString Then _redirect = False
        Dim _startInfo As New Diagnostics.ProcessStartInfo(Constants.CmdExe)
        _startInfo.UseShellExecute = Not _redirect
        _startInfo.RedirectStandardOutput = _redirect
        _startInfo.RedirectStandardError = _redirect
        _startInfo.CreateNoWindow = True
        _startInfo.FileName = filename
        _startInfo.Arguments = arguments
        Logger.Debug(My.Resources.Messages.ExecutingItem & _startInfo.FileName & My.Resources.Messages.Arguments & _startInfo.Arguments)
        Dim _output As String = Nothing
        Dim _error As String = Nothing
        Using _process As New Process With {.StartInfo = _startInfo}
            _process.Start()
            If _redirect Then
                _output = _process.StandardOutput.ReadToEnd()
                _error = _process.StandardError.ReadToEnd()
            End If
            _process.WaitForExit()
            Logger.Debug(My.Resources.Messages.StandardOutput & _output)
            Logger.Debug(My.Resources.Messages.StandardError & _error)
        End Using
        Return WasCommandSuccessful(_output) AndAlso String.IsNullOrWhiteSpace(_error)
    End Function

    Public Shared Function WasCommandSuccessful(output As String) As Boolean
        If output Is Nothing Then Return True
        If output.ToUpperInvariant.Contains(Constants.ErrorLabel) OrElse output.ToUpperInvariant.Contains(Constants.Failed) OrElse output.ToUpperInvariant.Contains(Constants.AccessDenied) Then Return False
        Return True
    End Function

End Class
