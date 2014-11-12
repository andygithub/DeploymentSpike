Imports DeploymentSpike.Controller.Utility

Namespace Factory
    ''' <summary>
    ''' Generation of an html file from task data.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class ReportGenerationFactory

        Private Sub New()

        End Sub

        Public Shared Sub TaskStepsHtml(tasks As IEnumerable(Of ITaskInformation), file As String)
            Dim _temp As String = IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.OutFolder & IO.Path.DirectorySeparatorChar & file
            IO.Directory.CreateDirectory(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.OutFolder)
            IO.File.Delete(_temp)
            IO.File.AppendAllText(_temp, tasks.ToHtmlReport)
        End Sub

    End Class

End Namespace