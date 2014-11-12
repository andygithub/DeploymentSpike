Imports Castle.Core.Logging
Imports System.IO
''' <summary>
''' Task that will empty a specified folder of all files and folders.
''' </summary>
''' <remarks></remarks>
Public Class PurgeExistingArtifacts
    Implements ITask

    Private _logger As ILogger

    Sub New(settings As Interfaces.ISettings)
        If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
        TaskSettings = settings
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

    Public Function ValidateSettings() As Boolean Implements Interfaces.ITask.ValidateSettings
        'require valid accessible folder.
        If Not TaskSettings.Settings.ContainsKey(Constants.ConfigPurgeFolder) OrElse String.IsNullOrWhiteSpace(TaskSettings.Settings(Constants.ConfigPurgeFolder)) Then
            Logger.Fatal(My.Resources.Messages.MissingPurgeFolder)
            Return False
        End If
        'validate that all of the folders exist
        For Each item In TaskSettings.Settings(Constants.ConfigPurgeFolder).Split(Constants.Comma)
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
        'delete all items in the specified folder but don't delete the folder itself
        For Each item In TaskSettings.Settings(Constants.ConfigPurgeFolder).Split(Constants.Comma)
            EmptyFolder(item)
        Next
    End Sub

    Private Sub EmptyFolder(directory As String)
        Dim _dirinfo As New DirectoryInfo(directory)
        'delete files
        For Each _file As FileInfo In _dirinfo.GetFiles        
            _file.Delete()
            Logger.Debug(My.Resources.Messages.FileDeleted & _file.FullName)
        Next
        'delete folders
        For Each _folder As DirectoryInfo In _dirinfo.GetDirectories
            _folder.Delete(True)
            Logger.Debug(My.Resources.Messages.FolderDeleted & _folder.FullName)
        Next

    End Sub


End Class
