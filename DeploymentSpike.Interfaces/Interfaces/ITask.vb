Imports Castle.Core.Logging
''' <summary>
''' This is the interface on a task.
''' </summary>
''' <remarks></remarks>
Public Interface ITask

    Function Execute() As TaskStatus
    Property Logger As ILogger
    Property MetaInformation As ITaskInformation
    Property MetaInformationList As IEnumerable(Of ITaskInformation)
    Property TaskSettings() As ISettings
    Function ValidateSettings() As Boolean


End Interface
