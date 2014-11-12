Imports DeploymentSpike.Interfaces
Imports DeploymentSpike.Controller.Domain

Namespace Domain

    Public Class TaskSettingsFixture

        <Fact()> _
        Public Sub initial_executiontype_value()
            Dim _item As New TaskSettings
            Assert.Equal(_item.GetExecutionFlag, ExecutionType.NotSet)
        End Sub

        <Fact()> _
        Public Sub invalid_executiontype_value()
            Dim _item As New TaskSettings
            _item.Settings.Add(Controller.Utility.Constants.ConfigExecutionSwitch, String.Empty)
            Assert.Equal(_item.GetExecutionFlag, ExecutionType.NotSet)
        End Sub
    End Class

End Namespace
