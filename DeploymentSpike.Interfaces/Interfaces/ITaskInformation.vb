''' <summary>
''' This is the interface for a task step.
''' </summary>
''' <remarks></remarks>
Public Interface ITaskInformation

    Property CompletedStep As Boolean
    Property StepNumber As Integer
    Property DeploymentTask As String
    Property TaskComponentName As String
    Property TaskComponentConfiguration As String
    Property NotificationComponentConfiguration As String
    Property MigrationThread As String
    Property ServersImpacted As String
    Property Comments As String
    Property Dependencies As String
    Property ProjectedDate As Nullable(Of DateTime)
    Property ProjectedStartTime As Nullable(Of DateTime)
    Property ProjectedEndTime As Nullable(Of DateTime)
    Property ProjectedDuration As Nullable(Of TimeSpan)
    Property ActualDate As Nullable(Of DateTime)
    Property ActualStartTime As Nullable(Of DateTime)
    Property ActualEndTime As Nullable(Of DateTime)
    Property ActualDuration As Nullable(Of TimeSpan)
    Property Owners As String
    Property Support As String

End Interface
