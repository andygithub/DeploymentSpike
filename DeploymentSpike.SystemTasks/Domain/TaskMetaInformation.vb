
Namespace Domain
    ''' <summary>
    ''' Default implementation of ITaskInformation
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TaskMetaInformation
        Implements DeploymentSpike.Interfaces.ITaskInformation

        Public Property ActualDate As Nullable(Of Date) Implements DeploymentSpike.Interfaces.ITaskInformation.ActualDate

        Public Property ActualDuration As Nullable(Of System.TimeSpan) Implements DeploymentSpike.Interfaces.ITaskInformation.ActualDuration

        Public Property ActualEndTime As Nullable(Of System.DateTime) Implements DeploymentSpike.Interfaces.ITaskInformation.ActualEndTime

        Public Property ActualStartTime As Nullable(Of System.DateTime) Implements DeploymentSpike.Interfaces.ITaskInformation.ActualStartTime

        Public Property Comments As String Implements DeploymentSpike.Interfaces.ITaskInformation.Comments

        Public Property CompletedStep As Boolean Implements DeploymentSpike.Interfaces.ITaskInformation.CompletedStep

        Public Property Dependencies As String Implements DeploymentSpike.Interfaces.ITaskInformation.Dependencies

        Public Property DeploymentTask As String Implements DeploymentSpike.Interfaces.ITaskInformation.DeploymentTask

        Public Property MigrationThread As String Implements DeploymentSpike.Interfaces.ITaskInformation.MigrationThread

        Public Property Owners As String Implements DeploymentSpike.Interfaces.ITaskInformation.Owners

        Public Property ProjectedDate As Nullable(Of Date) Implements DeploymentSpike.Interfaces.ITaskInformation.ProjectedDate

        Public Property ProjectedDuration As Nullable(Of System.TimeSpan) Implements DeploymentSpike.Interfaces.ITaskInformation.ProjectedDuration

        Public Property ProjectedEndTime As Nullable(Of System.DateTime) Implements DeploymentSpike.Interfaces.ITaskInformation.ProjectedEndTime

        Public Property ProjectedStartTime As Nullable(Of System.DateTime) Implements DeploymentSpike.Interfaces.ITaskInformation.ProjectedStartTime

        Public Property ServersImpacted As String Implements DeploymentSpike.Interfaces.ITaskInformation.ServersImpacted

        Public Property StepNumber As Integer Implements DeploymentSpike.Interfaces.ITaskInformation.StepNumber

        Public Property Support As String Implements DeploymentSpike.Interfaces.ITaskInformation.Support

        Public Property TaskComponentConfiguration As String Implements DeploymentSpike.Interfaces.ITaskInformation.TaskComponentConfiguration

        Public Property TaskComponentName As String Implements DeploymentSpike.Interfaces.ITaskInformation.TaskComponentName

        Public Property NotificationComponentConfiguration As String Implements DeploymentSpike.Interfaces.ITaskInformation.NotificationComponentConfiguration

    End Class

End Namespace