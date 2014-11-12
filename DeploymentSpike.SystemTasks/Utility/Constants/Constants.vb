Namespace Utility

    Public NotInheritable Class Constants

        Private Sub New()

        End Sub

        Public Const MappingAssembliesParameterName As String = "mappingAssemblies"
        Public Const ContainerParameterName As String = "container"
        Public Const SettingsParameterName As String = "settings"

        Public Const ConfigAssemblyNamesKey As String = "AssemblyList"
        Public Const ConfigLogConfigurationFile As String = "LogConfigurationFile"
        Public Const ConfigNotificationClass As String = "NotificationClass"
        Public Const ConfigTaskList As String = "TaskList"
        Public Const ConfigConfigFilePath As String = "ConfigFilePath"
        Public Const ConfigExecutionSwitch As String = "ExecutionFlag"
        Public Const ValidExtension As String = ".dll"
        Public Const ValidSettingsExtension As String = ".json"
        Public Const Comma As Char = ","c

        Public Const Tasks As String = "tasks"
        Public Const Task As String = "task"
        Public Const FileName As String = "fileName"

        Public Const Config As String = "Config"
        Public Const Log As String = "Log"
        Public Const OutFolder As String = "out"

        Public Const InterceptionInstanceInternalPropertyName As String = "target"
        Public Const InvocationParameterName As String = "invocation"
        Public Const ModelParameterName As String = "model"

        Public Const TaskPauseSetting As String = "Pause"

        Public Const MethodExecute As String = "Execute"
        Public Const MethodNotifyStarted As String = "NotifyStarted"
        Public Const MethodNotifyCompletedSuccess As String = "NotifyCompletedSuccess"
        Public Const MethodNotifyCompletedFailure As String = "NotifyCompletedFailure"

        Public Const CompleteTaskStepsJson As String = "CompleteTaskSteps.json"
        Public Const CompleteTaskStepsReportHtml As String = "CompleteTaskSteps.html"

        Public Const ConfigNotifyStart As String = "NotifyStart"
        Public Const ConfigNotifyFail As String = "NotifyFail"
        Public Const ConfigNotifySuccess As String = "NotifySuccess"

    End Class

End Namespace