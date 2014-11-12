Imports Castle.Core.Logging
Imports System.IO
''' <summary>
''' Task that will recursively copy files and folders from one location to another.
''' </summary>
''' <remarks></remarks>
Public Class CopyNewArtifacts
    Implements ITask

    Private _settings As Interfaces.ISettings
    Private _logger As ILogger

    Sub New(settings As Interfaces.ISettings)
        If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
        _settings = settings
    End Sub

    Public Function Execute() As Interfaces.TaskStatus Implements Interfaces.ITask.Execute
        PerformTask()
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

    Public Function ValidateSettings() As Boolean Implements Interfaces.ITask.ValidateSettings
        'require valid accessible folder.
        If Not _settings.Settings.ContainsKey(Constants.ConfigSourceFolder) OrElse String.IsNullOrWhiteSpace(_settings.Settings(Constants.ConfigSourceFolder)) Then
            Logger.Fatal(My.Resources.Messages.MissingSourceFolder)
            Return False
        End If
        If Not _settings.Settings.ContainsKey(Constants.ConfigDestinationFolder) OrElse String.IsNullOrWhiteSpace(_settings.Settings(Constants.ConfigDestinationFolder)) Then
            Logger.Fatal(My.Resources.Messages.MissingDestinationFolder)
            Return False
        End If
        'validate that the source and destination counts are the same.
        If _settings.Settings(Constants.ConfigSourceFolder).Split(Constants.Comma).Count <> _settings.Settings(Constants.ConfigDestinationFolder).Split(Constants.Comma).Count Then
            Logger.Fatal(My.Resources.Messages.DestinationSourceCountsDifferent)
            Return False
        End If
        'validate that all of the folders exist
        For Each item In _settings.Settings(Constants.ConfigSourceFolder).Split(Constants.Comma)
            If IO.Directory.Exists(item) Then
                Logger.Debug(My.Resources.Messages.SpecifiedFolderFound & item)
            Else
                Logger.Fatal(My.Resources.Messages.SpecifiedFolderNotFound & item)
                Return False
            End If
        Next
        For Each item In _settings.Settings(Constants.ConfigDestinationFolder).Split(Constants.Comma)
            If IO.Directory.Exists(item) Then
                Logger.Debug(My.Resources.Messages.SpecifiedFolderFound & item)
            Else
                Logger.Fatal(My.Resources.Messages.SpecifiedFolderNotFound & item)
                Return False
            End If
        Next

        Return True
    End Function

    Public Sub PerformTask()
        'copy all items in the specified folder but don't copy the folder itself
        Dim _source As String() = _settings.Settings(Constants.ConfigSourceFolder).Split(Constants.Comma)
        Dim _destination As String() = _settings.Settings(Constants.ConfigDestinationFolder).Split(Constants.Comma)
        For i As Integer = 0 To _source.Count - 1
            CopyFolder(_source(i), _destination(i))
        Next
    End Sub

    Private Sub CopyFolder(sourceFolder As String, destinationFolder As String)
        If Not Directory.Exists(destinationFolder) Then
            Directory.CreateDirectory(destinationFolder)
            Logger.Debug(My.Resources.Messages.FolderCreated & destinationFolder)
        End If
        Dim files As String() = Directory.GetFiles(sourceFolder)
        For Each fileitem As String In files
            Dim name As String = Path.GetFileName(fileitem)
            Dim dest As String = Path.Combine(destinationFolder, name)
            File.Copy(fileitem, dest)
            Logger.Debug(My.Resources.Messages.FileCopiedFrom & fileitem & My.Resources.Messages.ToSpace & dest)
        Next
        Dim folders As String() = Directory.GetDirectories(sourceFolder)
        For Each folder As String In folders
            Dim name As String = Path.GetFileName(folder)
            Dim dest As String = Path.Combine(destinationFolder, name)
            CopyFolder(folder, dest)
        Next
    End Sub

End Class
