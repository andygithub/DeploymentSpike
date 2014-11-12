Imports Castle.Core.Logging
Imports System.IO
''' <summary>
''' Task that will extract files from a zip and place them into a configured location.
''' </summary>
''' <remarks></remarks>
Public Class CopyArtifactsFromZip
    Implements ITask

    Private Const AllFiles As String = "*.*"

    Private _settings As Interfaces.ISettings
    Private _logger As ILogger

    Sub New(settings As Interfaces.ISettings)
        If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
        _settings = settings
        'there does not need to be a filter for each item or any folder filter set
        If Not _settings.Settings.ContainsKey(Constants.ConfigZipFilter) Then
            _settings.Settings.Add(Constants.ConfigZipFilter, String.Empty)
        End If
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
        'require valid accessible zip file.
        If Not _settings.Settings.ContainsKey(Constants.ConfigSourceZip) OrElse String.IsNullOrWhiteSpace(_settings.Settings(Constants.ConfigSourceZip)) Then
            Logger.Fatal(My.Resources.Messages.MissingSourceZipFile)
            Return False
        End If
        'require valid accessible folder.
        If Not _settings.Settings.ContainsKey(Constants.ConfigDestinationFolder) OrElse String.IsNullOrWhiteSpace(_settings.Settings(Constants.ConfigDestinationFolder)) Then
            Logger.Fatal(My.Resources.Messages.MissingDestinationFolder)
            Return False
        End If
        'validate that the zip files exist
        For Each item In _settings.Settings(Constants.ConfigSourceZip).Split(Constants.Comma)
            If IO.File.Exists(item) Then
                Logger.Debug(My.Resources.Messages.SpecifiedFileFound & item)
            Else
                Logger.Fatal(My.Resources.Messages.SpecifiedFileNotFound & item)
                Return False
            End If
        Next
        'validate that all of the folders exist
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
        Dim _source As String() = _settings.Settings(Constants.ConfigSourceZip).Split(Constants.Comma)
        Dim _destination As String() = _settings.Settings(Constants.ConfigDestinationFolder).Split(Constants.Comma)
        Dim _filter As String
        For i As Integer = 0 To _source.Count - 1
            _filter = CoalesceFilter(i)
            If String.IsNullOrWhiteSpace(_filter) Then
                UnzipSource(_source(i), _destination(i))
            Else
                UnzipSource(_source(i), _destination(i), CoalesceFilter(i))
            End If
        Next
    End Sub

    Private Sub UnzipSource(sourceZipFile As String, destinationFolder As String)
        Using zip1 As Ionic.Zip.ZipFile = Ionic.Zip.ZipFile.Read(sourceZipFile)
            AddHandler zip1.ExtractProgress, AddressOf ExtractionLog
            zip1.ExtractAll(destinationFolder, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently)
        End Using
    End Sub

    Private Sub UnzipSource(sourceZipFile As String, destinationFolder As String, directoryPathinArchive As String)
        Using zip1 As Ionic.Zip.ZipFile = Ionic.Zip.ZipFile.Read(sourceZipFile)
            AddHandler zip1.ExtractProgress, AddressOf ExtractionLog
            zip1.ExtractSelectedEntries(AllFiles, directoryPathinArchive, destinationFolder, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently)
        End Using
    End Sub

    Private Sub ExtractionLog(sender As Object, e As Ionic.Zip.ExtractProgressEventArgs)
        If e.EventType = Ionic.Zip.ZipProgressEventType.Extracting_BeforeExtractEntry Then
            Logger.Debug(My.Resources.Messages.ExtractFromZip & e.CurrentEntry.FileName)
            Exit Sub
        End If
        If e.EventType = Ionic.Zip.ZipProgressEventType.Extracting_BeforeExtractAll Then
            Logger.Debug(My.Resources.Messages.ExtractionStarted & e.ArchiveName)
            Exit Sub
        End If
        If e.EventType = Ionic.Zip.ZipProgressEventType.Extracting_AfterExtractAll Then
            Logger.Debug(My.Resources.Messages.ExtractionCompleted & e.ArchiveName)
            Exit Sub
        End If
    End Sub

    Private Function CoalesceFilter(index As Integer) As String
        Dim _filter As String() = _settings.Settings(Constants.ConfigZipFilter).Split(Constants.Comma)
        If index > _filter.Count Then Return Nothing
        Return _filter(index)
    End Function


End Class
