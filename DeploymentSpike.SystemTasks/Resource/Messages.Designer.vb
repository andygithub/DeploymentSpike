﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.269
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Public Class Messages
        
        Private Shared resourceMan As Global.System.Resources.ResourceManager
        
        Private Shared resourceCulture As Global.System.Globalization.CultureInfo
        
        <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend Sub New()
            MyBase.New
        End Sub
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Public Shared ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("DeploymentSpike.Controller.Messages", GetType(Messages).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Public Shared Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Attempting to Resolve Task:.
        '''</summary>
        Public Shared ReadOnly Property AttemptingToResolveTask() As String
            Get
                Return ResourceManager.GetString("AttemptingToResolveTask", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Called: {0}.{1} (.
        '''</summary>
        Public Shared ReadOnly Property CalledMethod() As String
            Get
                Return ResourceManager.GetString("CalledMethod", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to ,.
        '''</summary>
        Public Shared ReadOnly Property Comma() As String
            Get
                Return ResourceManager.GetString("Comma", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Completed loading the Task Assemblies..
        '''</summary>
        Public Shared ReadOnly Property CompleetedLoadingTaskAssemblies() As String
            Get
                Return ResourceManager.GetString("CompleetedLoadingTaskAssemblies", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Container Disposed.
        '''</summary>
        Public Shared ReadOnly Property ContainerDisposed() As String
            Get
                Return ResourceManager.GetString("ContainerDisposed", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to A logging class was not registered so the default SystemTask logging will be used..
        '''</summary>
        Public Shared ReadOnly Property DefaultLoggingTaskUsed() As String
            Get
                Return ResourceManager.GetString("DefaultLoggingTaskUsed", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to A notification class was not registered so the default SystemTask notification will be used..
        '''</summary>
        Public Shared ReadOnly Property DefaultNotificationTaskUsed() As String
            Get
                Return ResourceManager.GetString("DefaultNotificationTaskUsed", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Checking to see the value of the diagnostic flag..
        '''</summary>
        Public Shared ReadOnly Property DiagnosticFlagCheck() As String
            Get
                Return ResourceManager.GetString("DiagnosticFlagCheck", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Diagnostic flag is set and task will be exited before execution..
        '''</summary>
        Public Shared ReadOnly Property DiagnosticFlagSet() As String
            Get
                Return ResourceManager.GetString("DiagnosticFlagSet", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Ending Assembly Scan.
        '''</summary>
        Public Shared ReadOnly Property EndingAssemblyScan() As String
            Get
                Return ResourceManager.GetString("EndingAssemblyScan", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Exception was thrown: .
        '''</summary>
        Public Shared ReadOnly Property ExceptionThrown() As String
            Get
                Return ResourceManager.GetString("ExceptionThrown", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to The following assemblies are excluded:.
        '''</summary>
        Public Shared ReadOnly Property ExcludedAssemblies() As String
            Get
                Return ResourceManager.GetString("ExcludedAssemblies", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Execution Completed.
        '''</summary>
        Public Shared ReadOnly Property ExecutionCompleted() As String
            Get
                Return ResourceManager.GetString("ExecutionCompleted", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Task failed notification not fired due to configuration..
        '''</summary>
        Public Shared ReadOnly Property FailedNotFired() As String
            Get
                Return ResourceManager.GetString("FailedNotFired", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Specified file was not found:.
        '''</summary>
        Public Shared ReadOnly Property FileNotFound() As String
            Get
                Return ResourceManager.GetString("FileNotFound", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to  for: .
        '''</summary>
        Public Shared ReadOnly Property ForComponent() As String
            Get
                Return ResourceManager.GetString("ForComponent", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Generating default settings..
        '''</summary>
        Public Shared ReadOnly Property GeneratingDefaultSettings() As String
            Get
                Return ResourceManager.GetString("GeneratingDefaultSettings", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Configured assembly was determined invalid:.
        '''</summary>
        Public Shared ReadOnly Property InvalidAssembly() As String
            Get
                Return ResourceManager.GetString("InvalidAssembly", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Due to lack of configured assemblies the current directory will be scanned..
        '''</summary>
        Public Shared ReadOnly Property InvalidConfigurationAssembly() As String
            Get
                Return ResourceManager.GetString("InvalidConfigurationAssembly", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Kernel does not contain component:.
        '''</summary>
        Public Shared ReadOnly Property KernelMissingComponent() As String
            Get
                Return ResourceManager.GetString("KernelMissingComponent", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to List of Assemblies Loaded.
        '''</summary>
        Public Shared ReadOnly Property ListAssembliesLoaded() As String
            Get
                Return ResourceManager.GetString("ListAssembliesLoaded", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Load of Filed failed to retrieve settings:.
        '''</summary>
        Public Shared ReadOnly Property LoadFailedToRetrieveSettings() As String
            Get
                Return ResourceManager.GetString("LoadFailedToRetrieveSettings", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Loading the Task Assemblies..
        '''</summary>
        Public Shared ReadOnly Property LoadingTaskAssemblies() As String
            Get
                Return ResourceManager.GetString("LoadingTaskAssemblies", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to ERROR: .
        '''</summary>
        Public Shared ReadOnly Property LogError() As String
            Get
                Return ResourceManager.GetString("LogError", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &quot;Exception: {0}.{1}&quot;.
        '''</summary>
        Public Shared ReadOnly Property LoggedException() As String
            Get
                Return ResourceManager.GetString("LoggedException", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Logging:.
        '''</summary>
        Public Shared ReadOnly Property Logging() As String
            Get
                Return ResourceManager.GetString("Logging", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to TRACE: .
        '''</summary>
        Public Shared ReadOnly Property LogTrace() As String
            Get
                Return ResourceManager.GetString("LogTrace", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to WARNING: .
        '''</summary>
        Public Shared ReadOnly Property LogWarning() As String
            Get
                Return ResourceManager.GetString("LogWarning", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to There are multiple logging registrations in the container.  The first registration will be resolved unless there was a configured component logging..
        '''</summary>
        Public Shared ReadOnly Property MultipleLoggingRegistrations() As String
            Get
                Return ResourceManager.GetString("MultipleLoggingRegistrations", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to No Location used..
        '''</summary>
        Public Shared ReadOnly Property NoLocationUsed() As String
            Get
                Return ResourceManager.GetString("NoLocationUsed", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to There is not a configured component logging..
        '''</summary>
        Public Shared ReadOnly Property NoLoggingConfigurations() As String
            Get
                Return ResourceManager.GetString("NoLoggingConfigurations", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to No tasks were found to be executed.  .
        '''</summary>
        Public Shared ReadOnly Property NoTasks() As String
            Get
                Return ResourceManager.GetString("NoTasks", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to No tasks found..
        '''</summary>
        Public Shared ReadOnly Property NoTasksFound() As String
            Get
                Return ResourceManager.GetString("NoTasksFound", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Notification:.
        '''</summary>
        Public Shared ReadOnly Property Notification() As String
            Get
                Return ResourceManager.GetString("Notification", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Notification threw an exception..
        '''</summary>
        Public Shared ReadOnly Property NotificationException() As String
            Get
                Return ResourceManager.GetString("NotificationException", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to The notification did not complete successfully..
        '''</summary>
        Public Shared ReadOnly Property NotificationFailure() As String
            Get
                Return ResourceManager.GetString("NotificationFailure", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to The notification completed successfully..
        '''</summary>
        Public Shared ReadOnly Property NotificationSuccess() As String
            Get
                Return ResourceManager.GetString("NotificationSuccess", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Notification type: {0}.
        '''</summary>
        Public Shared ReadOnly Property NotificationType() As String
            Get
                Return ResourceManager.GetString("NotificationType", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to The following task failed to complete:.
        '''</summary>
        Public Shared ReadOnly Property NotifyTaskCompletedFailure() As String
            Get
                Return ResourceManager.GetString("NotifyTaskCompletedFailure", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to The following task completed successfully: .
        '''</summary>
        Public Shared ReadOnly Property NotifyTaskCompletedSuccess() As String
            Get
                Return ResourceManager.GetString("NotifyTaskCompletedSuccess", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to The following task has been started: .
        '''</summary>
        Public Shared ReadOnly Property NotifyTaskStarted() As String
            Get
                Return ResourceManager.GetString("NotifyTaskStarted", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Due to the exception the process will be halted at this point..
        '''</summary>
        Public Shared ReadOnly Property ProcessHaltedException() As String
            Get
                Return ResourceManager.GetString("ProcessHaltedException", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to The process was stopped due to a failed task status..
        '''</summary>
        Public Shared ReadOnly Property ProcessHaltedFailedTask() As String
            Get
                Return ResourceManager.GetString("ProcessHaltedFailedTask", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Attempting to resolve the tasks from the following configuration:.
        '''</summary>
        Public Shared ReadOnly Property ProcessTaskList() As String
            Get
                Return ResourceManager.GetString("ProcessTaskList", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Registered: {0} - {1}.
        '''</summary>
        Public Shared ReadOnly Property RegisteredComponent() As String
            Get
                Return ResourceManager.GetString("RegisteredComponent", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Attempting to resolve settings for item:.
        '''</summary>
        Public Shared ReadOnly Property ResolveSettingsAttempt() As String
            Get
                Return ResourceManager.GetString("ResolveSettingsAttempt", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Scanning Assembly:.
        '''</summary>
        Public Shared ReadOnly Property ScanningAssembly() As String
            Get
                Return ResourceManager.GetString("ScanningAssembly", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Settings not found.
        '''</summary>
        Public Shared ReadOnly Property SettingsNotFound() As String
            Get
                Return ResourceManager.GetString("SettingsNotFound", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to  .
        '''</summary>
        Public Shared ReadOnly Property Space() As String
            Get
                Return ResourceManager.GetString("Space", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Task started notification not fired due to configuration..
        '''</summary>
        Public Shared ReadOnly Property StartedNotFired() As String
            Get
                Return ResourceManager.GetString("StartedNotFired", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Starting Assembly Scan.
        '''</summary>
        Public Shared ReadOnly Property StartingAssemblyScan() As String
            Get
                Return ResourceManager.GetString("StartingAssemblyScan", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Starting Error Handling.
        '''</summary>
        Public Shared ReadOnly Property StartingErrorHandling() As String
            Get
                Return ResourceManager.GetString("StartingErrorHandling", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Starting Error Handling for notification.
        '''</summary>
        Public Shared ReadOnly Property StartingErrorHandlingNotification() As String
            Get
                Return ResourceManager.GetString("StartingErrorHandlingNotification", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Starting Notification Interception..
        '''</summary>
        Public Shared ReadOnly Property StartingNotificationInterception() As String
            Get
                Return ResourceManager.GetString("StartingNotificationInterception", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Starting TaskSettingsInjectionInterceptor.
        '''</summary>
        Public Shared ReadOnly Property StartingTaskSettingsInjectionInterceptor() As String
            Get
                Return ResourceManager.GetString("StartingTaskSettingsInjectionInterceptor", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Task success notification not fired due to configuration..
        '''</summary>
        Public Shared ReadOnly Property SuccessNotFired() As String
            Get
                Return ResourceManager.GetString("SuccessNotFired", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to The task finished executing with a status of:.
        '''</summary>
        Public Shared ReadOnly Property TaskCompletion() As String
            Get
                Return ResourceManager.GetString("TaskCompletion", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to The following exception was thrown when executing the task:.
        '''</summary>
        Public Shared ReadOnly Property TaskException() As String
            Get
                Return ResourceManager.GetString("TaskException", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to A task exited with a failure status.  .
        '''</summary>
        Public Shared ReadOnly Property TaskFailure() As String
            Get
                Return ResourceManager.GetString("TaskFailure", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Updating the run time information in the meta settings..
        '''</summary>
        Public Shared ReadOnly Property TaskMetaInterceptor() As String
            Get
                Return ResourceManager.GetString("TaskMetaInterceptor", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Task parameter name was null and returning empty settings..
        '''</summary>
        Public Shared ReadOnly Property TaskNameWasNull() As String
            Get
                Return ResourceManager.GetString("TaskNameWasNull", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to The task is configured to have a pause. Press enter to continue..
        '''</summary>
        Public Shared ReadOnly Property TaskPause() As String
            Get
                Return ResourceManager.GetString("TaskPause", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Tasks:.
        '''</summary>
        Public Shared ReadOnly Property Tasks() As String
            Get
                Return ResourceManager.GetString("Tasks", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to An unhandled exception has taken place.  The process was halted when the exception took place..
        '''</summary>
        Public Shared ReadOnly Property UnhandledException() As String
            Get
                Return ResourceManager.GetString("UnhandledException", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Using results from:.
        '''</summary>
        Public Shared ReadOnly Property UsingResultsFrom() As String
            Get
                Return ResourceManager.GetString("UsingResultsFrom", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Validate the notification settings..
        '''</summary>
        Public Shared ReadOnly Property ValidateNotificationSettings() As String
            Get
                Return ResourceManager.GetString("ValidateNotificationSettings", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Validation of notification settings failed..
        '''</summary>
        Public Shared ReadOnly Property ValidateNotificationSettingsFailed() As String
            Get
                Return ResourceManager.GetString("ValidateNotificationSettingsFailed", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Validation of notification settings succeeded..
        '''</summary>
        Public Shared ReadOnly Property ValidateNotificationSettingsSucceeded() As String
            Get
                Return ResourceManager.GetString("ValidateNotificationSettingsSucceeded", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Validate the task settings..
        '''</summary>
        Public Shared ReadOnly Property ValidateTaskSettings() As String
            Get
                Return ResourceManager.GetString("ValidateTaskSettings", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Validation of task settings failed..
        '''</summary>
        Public Shared ReadOnly Property ValidateTaskSettingsFailed() As String
            Get
                Return ResourceManager.GetString("ValidateTaskSettingsFailed", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Validation of task settings succeeded..
        '''</summary>
        Public Shared ReadOnly Property ValidateTaskSettingsSucceeded() As String
            Get
                Return ResourceManager.GetString("ValidateTaskSettingsSucceeded", resourceCulture)
            End Get
        End Property
    End Class
End Namespace
