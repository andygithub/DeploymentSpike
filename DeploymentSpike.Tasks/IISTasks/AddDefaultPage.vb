Imports Castle.Core.Logging
Imports Microsoft.Web.Administration
''' <summary>
''' Task to add a new default page to an IIS site or virtual directory.
''' </summary>
''' <remarks></remarks>
Public Class AddDefaultPage
    Implements ITask


    Private _logger As ILogger

    Sub New(settings As Interfaces.ISettings)
        If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
        TaskSettings = settings
        'there does not need to be a filter for each item or any folder filter set
        If Not TaskSettings.Settings.ContainsKey(Constants.ConfigIisVirtualDirectories) Then
            TaskSettings.Settings.Add(Constants.ConfigIisVirtualDirectories, String.Empty)
        End If
    End Sub

    Public Function Execute() As Interfaces.TaskStatus Implements Interfaces.ITask.Execute
        Dim _serverlist As List(Of String) = TaskSettings.Settings(Constants.ConfigIisServers).Split(Constants.Comma).Where(Function(x)
                                                                                                                                Return Not String.IsNullOrWhiteSpace(x)
                                                                                                                            End Function).ToList
        For i As Integer = 0 To _serverlist.Count - 1
            AddDefaultPage(_serverlist(i), SplitItem(TaskSettings.Settings(Constants.ConfigIisSites), i), SplitItem(TaskSettings.Settings(Constants.ConfigIisVirtualDirectories), i), SplitItem(TaskSettings.Settings(Constants.ConfigIisPages), i))
        Next
        Return TaskStatus.Completed
    End Function

    Public Shared Function SplitItem(delimitedList As String, index As Integer) As String
        If String.IsNullOrWhiteSpace(delimitedList) Then Return Nothing
        Dim _list As List(Of String) = delimitedlist.Split(Constants.Comma).Where(Function(x)
                                                                                      Return Not String.IsNullOrWhiteSpace(x)
                                                                                  End Function).ToList
        If index <= _list.Count - 1 Then
            Return _list(index)
        Else
            Return Nothing
        End If
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

    Private Shared Sub AddDefaultPage(server As String, site As String, virtualDirectory As String, pagename As String)
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
            Dim addElement As ConfigurationElement = filesCollection.CreateElement(Constants.Add)
            addElement(Constants.Value) = pagename
            filesCollection.AddAt(0, addElement)
            serverManager.CommitChanges()
        End Using
    End Sub

End Class
