Imports Castle.Core.Logging
Imports System.Net
Imports System.Net.Mail

''' <summary>
''' A notification implementation that will write information to an email and send it to the configured recipients.
''' </summary>
''' <remarks></remarks>
Public Class EmailNotifier
    Implements INotification

    Private _logger As ILogger

    ''' <summary>
    ''' Default constructor to set the settings.
    ''' </summary>
    ''' <param name="settings"></param>
    ''' <remarks></remarks>
    Public Sub New(settings As Interfaces.ISettings)
        If settings Is Nothing Then Throw New ArgumentNullException(Interfaces.Constants.SettingsParameterName)
        NotificationSettings = settings
        SetupDefaults()
    End Sub

    ''' <summary>
    ''' Method to set the defaults for the notifications.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetupDefaults()
        'there does not need to be a flag to determine which of the notificiations should be fired.
        'default them all to true.
        If Not NotificationSettings.Settings.ContainsKey(Constants.ConfigNotifyStart) Then
            NotificationSettings.Settings.Add(Constants.ConfigNotifyStart, Boolean.TrueString)
        End If
        If Not NotificationSettings.Settings.ContainsKey(Constants.ConfigNotifySuccess) Then
            NotificationSettings.Settings.Add(Constants.ConfigNotifySuccess, Boolean.TrueString)
        End If
        If Not NotificationSettings.Settings.ContainsKey(Constants.ConfigNotifyFail) Then
            NotificationSettings.Settings.Add(Constants.ConfigNotifyFail, Boolean.TrueString)
        End If
    End Sub

    Public Function NotifyCompletedFailure(task As Interfaces.ITask) As Boolean Implements Interfaces.INotification.NotifyCompletedFailure
        If task.MetaInformation Is Nothing Then
            Logger.Debug(My.Resources.Messages.Task & My.Resources.Messages.CompletedFailure)
        Else
            Logger.Debug(task.MetaInformation.DeploymentTask & My.Resources.Messages.Space & My.Resources.Messages.CompletedFailure)
        End If
        SendEmail(task, NotificationType.CompletedFailure)
        Return True
    End Function

    Public Function NotifyCompletedSuccess(task As Interfaces.ITask) As Boolean Implements Interfaces.INotification.NotifyCompletedSuccess
        If task.MetaInformation Is Nothing Then
            Logger.Debug(My.Resources.Messages.Task & My.Resources.Messages.CompletedSuccess)
        Else
            Logger.Debug(task.MetaInformation.DeploymentTask & My.Resources.Messages.Space & My.Resources.Messages.CompletedSuccess)
        End If
        SendEmail(task, NotificationType.CompletedSuccess)
        Return True
    End Function

    Public Function NotifyStarted(task As Interfaces.ITask) As Boolean Implements Interfaces.INotification.NotifyStarted
        If task.MetaInformation Is Nothing Then
            Logger.Debug(My.Resources.Messages.Task & My.Resources.Messages.Started)
        Else
            Logger.Debug(task.MetaInformation.DeploymentTask & My.Resources.Messages.Space & My.Resources.Messages.Started)
        End If
        SendEmail(task, NotificationType.Started)
        Return True
    End Function

    ''' <summary>
    ''' Logger instance for the notification.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Logger As Castle.Core.Logging.ILogger Implements Interfaces.INotification.Logger
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

    ''' <summary>
    ''' Settings for the notification.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NotificationSettings As Interfaces.ISettings Implements Interfaces.INotification.Settings

    Public ReadOnly Property Name As String Implements Interfaces.INotification.Name
        Get
            Return "EmailNotifier"
        End Get
    End Property

    Public Sub SendEmail(task As Interfaces.ITask, notifytype As NotificationType)
        Dim _toAddresses As List(Of String) = LoadAddresses(NotificationSettings.Settings(Constants.ConfigEmailFile).ToString)
        If _toAddresses.Count = 0 Then Exit Sub
        Dim fromAddress As New MailAddress(NotificationSettings.Settings(Constants.ConfigEmailFromAddress).ToString, NotificationSettings.Settings(Constants.ConfigEmailFromName).ToString)
        Dim toAddress As New MailAddress(_toAddresses(0))

        Dim smtp As New SmtpClient With
            {
               .Host = NotificationSettings.Settings(Constants.ConfigEmailHost).ToString,
               .Port = NotificationSettings.Settings(Constants.ConfigEmailPort).ToString,
               .EnableSsl = False,
               .DeliveryMethod = SmtpDeliveryMethod.Network,
               .UseDefaultCredentials = True
            }
        Dim message As New MailMessage(fromAddress, toAddress) With
            {
               .Subject = task.ToEmailSubjectLine(notifytype),
                .Body = task.ToEmailBody(notifytype),
                .IsBodyHtml = True
            }

        For i As Integer = 1 To _toAddresses.Count - 1
            message.To.Add(_toAddresses(i))
        Next
        smtp.Send(message)
    End Sub

    Public Function LoadAddresses(file As String) As List(Of String)
        'load a comma delimited list of adresses and then return it
        If Not IO.File.Exists(file) Then
            Logger.DebugFormat(My.Resources.Messages.AddressFileNotFound, file)
            Return New List(Of String)
        End If
        Dim text As String = IO.File.ReadAllText(file)
        Dim _list As List(Of String) = text.Split(Constants.Comma).Where(Function(i)
                                                                             Return Not String.IsNullOrWhiteSpace(i)
                                                                         End Function).ToList
        If _list.Count = 0 Then
            Logger.Debug(My.Resources.Messages.EmptyAddressList)
        End If
        Return _list
    End Function

    Public Function ValidateSettings() As Boolean Implements INotification.ValidateSettings
        'need email file, from address, from name, host address, host port
        If Not NotificationSettings.Settings.ContainsKey(Constants.ConfigEmailFile) OrElse String.IsNullOrWhiteSpace(NotificationSettings.Settings(Constants.ConfigEmailFile)) Then
            Logger.Fatal(My.Resources.Messages.ConfigAddressFileNotSpecified)
            Return False
        End If
        'validate that the file exists
        If IO.File.Exists(NotificationSettings.Settings(Constants.ConfigEmailFile).ToString) Then
            Logger.Debug(My.Resources.Messages.SpecifiedFileFound & NotificationSettings.Settings.ContainsKey(Constants.ConfigEmailFile))
        Else
            Logger.Fatal(My.Resources.Messages.SpecifiedFileNotFound & NotificationSettings.Settings.ContainsKey(Constants.ConfigEmailFile))
            Return False
        End If
        If Not NotificationSettings.Settings.ContainsKey(Constants.ConfigEmailFromAddress) OrElse String.IsNullOrWhiteSpace(NotificationSettings.Settings(Constants.ConfigEmailFromAddress)) Then
            Logger.Fatal(My.Resources.Messages.FromEmailAddressNotFound)
            Return False
        End If
        If Not NotificationSettings.Settings.ContainsKey(Constants.ConfigEmailFromName) OrElse String.IsNullOrWhiteSpace(NotificationSettings.Settings(Constants.ConfigEmailFromName)) Then
            Logger.Fatal(My.Resources.Messages.FromEmailNameNotFound)
            Return False
        End If
        If Not NotificationSettings.Settings.ContainsKey(Constants.ConfigEmailHost) OrElse String.IsNullOrWhiteSpace(NotificationSettings.Settings(Constants.ConfigEmailHost)) Then
            Logger.Fatal(My.Resources.Messages.EmailHostNotFound)
            Return False
        End If
        If Not NotificationSettings.Settings.ContainsKey(Constants.ConfigEmailPort) OrElse String.IsNullOrWhiteSpace(NotificationSettings.Settings(Constants.ConfigEmailPort)) Then
            Logger.Fatal(My.Resources.Messages.EmailPortNotFound)
            Return False
        End If

        Return True
    End Function

End Class
