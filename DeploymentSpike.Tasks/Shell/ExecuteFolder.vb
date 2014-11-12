Imports Castle.Core.Logging
Imports System.IO
''' <summary>
''' Task to execute all files with a specific extension in a specific folder.
''' </summary>
''' <remarks></remarks>
Public Class ExecuteFolder
    Implements ITask

    Private _logger As ILogger

    Sub New(settings As Interfaces.ISettings)
        If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
        TaskSettings = settings
        SetupDefaults()
    End Sub

    Private Sub SetupDefaults()
        'there does not need to be redirect
        If Not TaskSettings.Settings.ContainsKey(Constants.ConfigExecutingRedirect) Then
            TaskSettings.Settings.Add(Constants.ConfigExecutingRedirect, Boolean.TrueString)
        End If
        If Not TaskSettings.Settings.ContainsKey(Constants.ConfigExecutingFileName) Then
            TaskSettings.Settings.Add(Constants.ConfigExecutingFileName, String.Empty)
        End If
    End Sub

    Public Function Execute() As Interfaces.TaskStatus Implements Interfaces.ITask.Execute
        Dim _value As Boolean = ExecuteFolder()
        Return _value.ToTaskStatus
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
        'need executing file name eg ping.exe, need folder to execute and file extension to execute and need the arguments string to inject the custom file name property from the folder.
        If Not TaskSettings.Settings.ContainsKey(Constants.ConfigExecutingFolder) OrElse String.IsNullOrWhiteSpace(TaskSettings.Settings(Constants.ConfigExecutingFolder)) Then
            Logger.Fatal(My.Resources.Messages.MissingExecutingFileName)
            Return False
        End If
        'validate that the folder exists
        If IO.Directory.Exists(TaskSettings.Settings(Constants.ConfigExecutingFolder).ToString) Then
            Logger.Debug(My.Resources.Messages.SpecifiedFolderFound & TaskSettings.Settings.ContainsKey(Constants.ConfigExecutingFolder))
        Else
            Logger.Fatal(My.Resources.Messages.SpecifiedFolderNotFound & TaskSettings.Settings.ContainsKey(Constants.ConfigExecutingFolder))
            Return False
        End If
        If Not TaskSettings.Settings.ContainsKey(Constants.ConfigExecutingFileExtension) OrElse String.IsNullOrWhiteSpace(TaskSettings.Settings(Constants.ConfigExecutingFileExtension)) Then
            Logger.Fatal(My.Resources.Messages.MissingExecutingFileName)
            Return False
        End If
        If Not TaskSettings.Settings.ContainsKey(Constants.ConfigExecutingArguments) OrElse String.IsNullOrWhiteSpace(TaskSettings.Settings(Constants.ConfigExecutingArguments)) Then
            Logger.Fatal(My.Resources.Messages.MissingExecutingArguments)
            Return False
        End If
        Return True
    End Function

    Private Function ExecuteFolder() As Boolean
        'loop through folder and get all files
        'merge file into the arguments setting
        'write tests for batch files and sqlcmd
        Dim folder As String = TaskSettings.Settings(Constants.ConfigExecutingFolder)
        Dim extension As String = TaskSettings.Settings(Constants.ConfigExecutingFileExtension)
        Dim _files As String() = IO.Directory.GetFiles(folder, extension, SearchOption.TopDirectoryOnly)
        If _files Is Nothing OrElse _files.Count = 0 Then
            Logger.DebugFormat(System.Globalization.CultureInfo.InvariantCulture, My.Resources.Messages.FilesNotFoundinSpecifiedFolder, folder, extension)
            Return True
        End If
        Dim _flags As New List(Of Boolean)
        For Each _file In _files
            If Not String.IsNullOrWhiteSpace(TaskSettings.Settings(Constants.ConfigExecutingFileName)) Then
                _flags.Add(ExecuteFile(TaskSettings.Settings(Constants.ConfigExecutingFileName), String.Format(System.Globalization.CultureInfo.InvariantCulture, TaskSettings.Settings(Constants.ConfigExecutingArguments), _file), TaskSettings.Settings(Constants.ConfigExecutingRedirect)))
            Else
                _flags.Add(ExecuteFile(_file, String.Empty, TaskSettings.Settings(Constants.ConfigExecutingRedirect)))
            End If
        Next

        Return Not _flags.Contains(False)
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
