Imports Castle.Core.Logging
Imports System.Security.Permissions
''' <summary>
''' Task to reset IIS.
''' </summary>
''' <remarks></remarks>
Public Class ResetIis
    Implements ITask

    Private _logger As ILogger

    Sub New(settings As Interfaces.ISettings)
        If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
        TaskSettings = settings
    End Sub

    <Security.SecurityCritical()>
    Public Function Execute() As Interfaces.TaskStatus Implements Interfaces.ITask.Execute
        Return ExecuteTask()
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

    Public Function ValidateSettings() As Boolean Implements Interfaces.ITask.ValidateSettings
        Return True
    End Function

    Private Function ExecuteTask() As Interfaces.TaskStatus
        'if the list is null or empty try to reset the local copy of iis
        Dim _tempFlag As Boolean = True
        If Not TaskSettings.Settings.ContainsKey(Constants.ConfigIisServers) OrElse String.IsNullOrWhiteSpace(TaskSettings.Settings(Constants.ConfigIisServers)) Then
            _tempFlag = ResetIIS(String.Empty)
            Return _tempFlag.ToTaskStatus
        End If

        Dim _flag As Interfaces.TaskStatus = TaskStatus.Completed
        Dim _list As List(Of String) = TaskSettings.Settings(Constants.ConfigIisServers).Split(Constants.Comma).Where(Function(x)
                                                                                                                          Return Not String.IsNullOrWhiteSpace(x)
                                                                                                                      End Function).ToList
        For Each item In _list
            _tempFlag = ResetIIS(item)
            If Not _tempFlag Then _flag = TaskStatus.Failed
        Next
        Return _flag
    End Function

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")>
    Private Function ResetIIS(serverAddress As String) As Boolean
        Dim _startInfo As New Diagnostics.ProcessStartInfo(Constants.Cmdexe)
        _startInfo.UseShellExecute = False
        _startInfo.RedirectStandardOutput = True
        _startInfo.RedirectStandardError = True
        _startInfo.CreateNoWindow = True
        _startInfo.FileName = Constants.Iisresetexe
        _startInfo.Arguments = serverAddress
        Logger.Debug(My.Resources.Messages.ExecutingItem & _startInfo.FileName & My.Resources.Messages.Arguments & _startInfo.Arguments)
        Dim _output As String
        Dim _error As String
        Using _process As New Process With {.StartInfo = _startInfo}
            _process.Start()
            _output = _process.StandardOutput.ReadToEnd()
            _error = _process.StandardError.ReadToEnd()
            _process.WaitForExit()
            Logger.Debug(My.Resources.Messages.StandardOutput & _output)
            Logger.Debug(My.Resources.Messages.StandardError & _error)
        End Using
        Return WasCommandSuccessful(_output) AndAlso WasCommandSuccessful(_error)
    End Function

    Private Shared Function WasCommandSuccessful(output As String) As Boolean
        If output Is Nothing Then Return True
        If output.ToUpperInvariant.Contains(Constants.ErrorLabel) OrElse output.ToUpperInvariant.Contains(Constants.Failed) OrElse output.ToUpperInvariant.Contains(Constants.AccessDenied) Then Return False
        Return True
    End Function

End Class
