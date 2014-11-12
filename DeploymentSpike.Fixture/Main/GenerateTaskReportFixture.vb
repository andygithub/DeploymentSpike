Public Class GenerateTaskReportFixture

    Private Const _Unithtml As String = "unit.html"

    <Fact()>
    Public Sub generate_task_report()

        DeploymentSpike.Controller.Factory.ReportGenerationFactory.TaskStepsHtml(GenerateTaskMetaSteps, _Unithtml)
        Assert.True(IO.File.Exists(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & Controller.Utility.Constants.OutFolder & IO.Path.DirectorySeparatorChar & _Unithtml))

    End Sub

    Public Shared Function GenerateTaskMetaSteps() As IEnumerable(Of Controller.Domain.TaskMetaInformation)
        Dim _stepsInit As New List(Of Controller.Domain.TaskMetaInformation)
        _stepsInit.Add(New Controller.Domain.TaskMetaInformation() With {.StepNumber = 2, .CompletedStep = False, .MigrationThread = "HCSIS / PWIM/EIM", .Dependencies = "2,4", .Comments = "Placeholder: Production dump used for rollback in TFP", .DeploymentTask = "Place and confirm HCSIS, PWIM & EIM Applications in ", .ServersImpacted = "PWISTFPWEB22", .Owners = "Database team", .TaskComponentName = FixtureConstants.MockSecondTaskFullName, .TaskComponentConfiguration = "none.json", .ActualDate = Controller.Provider.DefaultTimeProvider.Current.UtcNow, .Support = "Randy", .ActualStartTime = Controller.Provider.DefaultTimeProvider.Current.UtcNow.AddHours(-2), .ActualEndTime = Controller.Provider.DefaultTimeProvider.Current.UtcNow.AddMinutes(-20), .ActualDuration = .ActualEndTime - .ActualStartTime})
        _stepsInit.Add(New Controller.Domain.TaskMetaInformation() With {.StepNumber = 1, .CompletedStep = True, .MigrationThread = "EIM", .Dependencies = "", .Comments = "", .DeploymentTask = "Send an email confirming", .Owners = "Web team", .ServersImpacted = "", .TaskComponentName = FixtureConstants.MockTaskFullName, .TaskComponentConfiguration = "none.json", .ActualDate = Controller.Provider.DefaultTimeProvider.Current.UtcNow, .Support = "Data Team", .ActualStartTime = Controller.Provider.DefaultTimeProvider.Current.UtcNow.AddHours(-1), .ActualEndTime = Nothing, .ActualDuration = .ActualEndTime - .ActualStartTime})
        _stepsInit.Add(New Controller.Domain.TaskMetaInformation() With {.StepNumber = 3, .CompletedStep = False, .MigrationThread = "Duplicate Thread", .Dependencies = "", .Comments = "", .DeploymentTask = "Send an email confirming", .Owners = "Web team", .ServersImpacted = "", .TaskComponentName = FixtureConstants.MockTaskFullName, .TaskComponentConfiguration = "none.json", .ActualDate = Nothing, .Support = "Data Team", .ActualStartTime = Nothing, .ActualEndTime = Nothing, .ActualDuration = .ActualEndTime - .ActualStartTime})
        Return _stepsInit
    End Function

End Class
