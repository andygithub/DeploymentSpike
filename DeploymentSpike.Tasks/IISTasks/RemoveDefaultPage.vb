Imports Castle.Core.Logging
Imports Microsoft.Web.Administration
''' <summary>
''' Task to remove a default page to an IIS site or virtual directory.
''' </summary>
''' <remarks></remarks>
Public Class RemoveDefaultPage
    Implements ITask

    Private _logger As ILogger

    Sub New(settings As Interfaces.ISettings)
        If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
        TaskSettings = settings
    End Sub

    Public Function Execute() As Interfaces.TaskStatus Implements Interfaces.ITask.Execute
        Dim _serverlist As List(Of String) = TaskSettings.Settings(Constants.ConfigIisServers).Split(Constants.Comma).Where(Function(x)
                                                                                                                                Return Not String.IsNullOrWhiteSpace(x)
                                                                                                                            End Function).ToList
        For i As Integer = 0 To _serverlist.Count - 1
            RemoveDefaultPage(_serverlist(i), AddDefaultPage.SplitItem(TaskSettings.Settings(Constants.ConfigIisSites), i), AddDefaultPage.SplitItem(TaskSettings.Settings(Constants.ConfigIisVirtualDirectories), i), AddDefaultPage.SplitItem(TaskSettings.Settings(Constants.ConfigIisPages), i))
        Next
        Return TaskStatus.Completed
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
        If Not TaskSettings.Settings.ContainsKey(Constants.ConfigIisServers) OrElse String.IsNullOrWhiteSpace(TaskSettings.Settings(Constants.ConfigIisServers)) Then
            Logger.Fatal(My.Resources.Messages.MissingIisServerName)
            Return False
        End If
        If Not TaskSettings.Settings.ContainsKey(Constants.ConfigIisSites) OrElse String.IsNullOrWhiteSpace(TaskSettings.Settings(Constants.ConfigIisSites)) Then
            Logger.Fatal(My.Resources.Messages.MissingIisSites)
            Return False
        End If
        'validate that the server and site counts are the same.
        If TaskSettings.Settings(Constants.ConfigIisServers).Split(Constants.Comma).Count <> TaskSettings.Settings(Constants.ConfigIisSites).Split(Constants.Comma).Count Then
            Logger.Fatal(My.Resources.Messages.ServerNameSitesCountsDifferent)
            Return False
        End If
        If Not TaskSettings.Settings.ContainsKey(Constants.ConfigIisPages) OrElse String.IsNullOrWhiteSpace(TaskSettings.Settings(Constants.ConfigIisPages)) Then
            Logger.Fatal(My.Resources.Messages.MissingIisPages)
            Return False
        End If
        'validate that the server and page counts are the same.
        If TaskSettings.Settings(Constants.ConfigIisServers).Split(Constants.Comma).Count <> TaskSettings.Settings(Constants.ConfigIisPages).Split(Constants.Comma).Count Then
            Logger.Fatal(My.Resources.Messages.ServerNamePagesCountsDifferent)
            Return False
        End If

        Return True
    End Function

    Private Shared Sub RemoveDefaultPage(server As String, site As String, virtualDirectory As String, pagename As String)
        Using serverManager As ServerManager = serverManager.OpenRemote(server)

            Dim config As Configuration
            If Not String.IsNullOrWhiteSpace(virtualDirectory) Then
                config = serverManager.GetWebConfiguration(site, virtualDirectory)
            Else
                config = serverManager.GetWebConfiguration(site)
            End If
            Dim defaultDocumentSection As ConfigurationSection = config.GetSection(Constants.SystemwebServerdefaultDocument)

            defaultDocumentSection(Constants.Enabled) = True

            Dim filesCollection As ConfigurationElementCollection = defaultDocumentSection.GetCollection(Constants.Files)
            Dim _matchIndexes As New List(Of Integer)
            For i As Integer = filesCollection.Count - 1 To 0 Step -1
                If filesCollection(i).GetAttributeValue(Constants.Value).ToString = pagename Then
                    _matchIndexes.Add(i)
                End If
            Next
            _matchIndexes.ForEach(Sub(i)
                                      filesCollection.RemoveAt(i)
                                  End Sub)
            serverManager.CommitChanges()
        End Using
    End Sub

End Class
