Imports Castle.Core.Logging
Imports System.IO
Imports System.Text.RegularExpressions

''' <summary>
''' Task to scan a file with a regular expression.
''' </summary>
''' <remarks></remarks>
Public Class ScanLogFile
    Implements ITask

    Private _settings As Interfaces.ISettings
    Private _logger As ILogger

    Sub New(settings As Interfaces.ISettings)
        If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
        _settings = settings
    End Sub

    Public Function Execute() As Interfaces.TaskStatus Implements Interfaces.ITask.Execute
        Dim _matchesFound As Boolean = PerformTask()
        Return (Not _matchesFound).ToTaskStatus
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

    Public Function ValidateSettings() As Boolean Implements Interfaces.ITask.ValidateSettings
        'require valid accessible file.
        If Not _settings.Settings.ContainsKey(Constants.ConfigFile) OrElse String.IsNullOrWhiteSpace(_settings.Settings(Constants.ConfigFile)) Then
            Logger.Fatal(My.Resources.Messages.MissingFile)
            Return False
        End If
        'validate that the file exists
        If IO.File.Exists(_settings.Settings(Constants.ConfigFile)) Then
            Logger.Debug(My.Resources.Messages.SpecifiedFileFound & _settings.Settings(Constants.ConfigFile))
        Else
            Logger.Fatal(My.Resources.Messages.SpecifiedFileNotFound & _settings.Settings(Constants.ConfigFile))
            Return False
        End If
        'require a regular expression
        If Not _settings.Settings.ContainsKey(Constants.ConfigRegularExpression) OrElse String.IsNullOrWhiteSpace(_settings.Settings(Constants.ConfigRegularExpression)) Then
            Logger.Fatal(My.Resources.Messages.MissingRegularExpression)
            Return False
        End If
        Return True
    End Function

    Public Function PerformTask() As Boolean
        Logger.DebugFormat(System.Globalization.CultureInfo.InvariantCulture, My.Resources.Messages.ScanningFile, _settings.Settings(Constants.ConfigFile), _settings.Settings(Constants.ConfigRegularExpression))
        Dim readText() As String = File.ReadAllLines(_settings.Settings(Constants.ConfigFile))
        Dim _line As String
        Dim _matchFound As Boolean
        Dim _matchCount As Integer
        For i As Integer = 0 To readText.Count - 1
            _line = readText(i)
            If Regex.IsMatch(_line, _settings.Settings(Constants.ConfigRegularExpression)) Then
                _matchCount += 1
                _matchFound = True
                Logger.DebugFormat(System.Globalization.CultureInfo.InvariantCulture, My.Resources.Messages.MatchFound, i, _line)
            End If
        Next
        If _matchFound Then
            Logger.DebugFormat(System.Globalization.CultureInfo.InvariantCulture, My.Resources.Messages.FileScanCompletedMatches, _matchCount)
        Else
            Logger.Debug(My.Resources.Messages.FileScanCompletedNoMatches)
        End If
        Return _matchFound
    End Function

End Class
