'Option Strict Off
Imports Castle.Core.Logging
Imports Microsoft.Web.Administration

''' <summary>
''' Task to recycle IIS application pools.
''' </summary>
''' <remarks></remarks>
Public Class RecycleApplicationPools
    Implements ITask

    Private _logger As ILogger

    Sub New(settings As Interfaces.ISettings)
        If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
        TaskSettings = settings
    End Sub

    <Security.SecurityCritical()>
    Public Function Execute() As Interfaces.TaskStatus Implements Interfaces.ITask.Execute
        Dim _list As List(Of String) = TaskSettings.Settings(Constants.ConfigIisServers).Split(Constants.Comma).Where(Function(x)
                                                                                                                          Return Not String.IsNullOrWhiteSpace(x)
                                                                                                                      End Function).ToList
        For Each item In _list
            RecyclePool(item)
        Next
        Return TaskStatus.Completed
    End Function

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

    Public Property MetaInformation As Interfaces.ITaskInformation Implements Interfaces.ITask.MetaInformation
    Public Property MetaInformationList As IEnumerable(Of Interfaces.ITaskInformation) Implements Interfaces.ITask.MetaInformationList
    Public Property TaskSettings As Interfaces.ISettings Implements Interfaces.ITask.TaskSettings

    Public Function ValidateSettings() As Boolean Implements Interfaces.ITask.ValidateSettings
        ' AppPoolFullPath must be in the form of: "IIS://" + machine +  "/W3SVC/AppPools/" + appPoolName
        If Not TaskSettings.Settings.ContainsKey(Constants.ConfigIisServers) OrElse String.IsNullOrWhiteSpace(TaskSettings.Settings(Constants.ConfigIisServers)) Then
            Logger.Fatal(My.Resources.Messages.MissingIisServerName)
            Return False
        End If
        Return True
    End Function

    'If you are getting this exception when trying to recycle the pool check to see if the ADSI provider is installed for IIS
    'System.Runtime.InteropServices.COMException : Unknown error (0x80005000)
    'http://blogs.msdn.com/b/jpsanders/archive/2009/05/13/iis-7-adsi-error-system-runtime-interopservices-comexception-0x80005000-unknown-error-0x80005000.aspx 
    'http://technet.microsoft.com/en-us/library/bb397374%28v=exchg.80%29.aspx
    'if you get this error then you need to give the user rights Logon as a Service"/ "Logon as a Batch Job -  System.Runtime.InteropServices.COMException : Access is denied.
    'at System.DirectoryServices.DirectoryEntry.Bind(Boolean throwIfFail)
    'at System.DirectoryServices.DirectoryEntry.Bind()
    'at System.DirectoryServices.DirectoryEntry.get_NativeObject()
    'at System.DirectoryServices.DirectoryEntry.Invoke(String methodName, Object[] args)

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")>
    Private Sub RecyclePool(serverAddress As String)
        Logger.DebugFormat(System.Globalization.CultureInfo.InvariantCulture, My.Resources.Messages.ApplicationPoolRecycled, serverAddress)
        'Private Const _Recycle As String = "Recycle"
        'Using apppool As New DirectoryEntry(serverAddress)
        '    apppool.Invoke(_Recycle)
        'End Using
        ' Connect to the WMI WebAdministration namespace.
        'Dim oWebAdmin = GetObject("winmgmts:root\WebAdministration")

        '' Specify the application pool.
        'Dim oAppPools = oWebAdmin.Get() '"ApplicationPool.Name='DefaultAppPool'"

        '' Recycle the application pool.
        'For Each item In oAppPools
        '    Debug.WriteLine(item)
        'Next
        'oAppPool.Recycle()
        Using serverManager As ServerManager = serverManager.OpenRemote(serverAddress)
            For Each item In serverManager.ApplicationPools
                Logger.Debug(item.Name)
                item.Recycle()
            Next
        End Using
    End Sub

End Class
