Imports DeploymentSpike.Controller
Imports DeploymentSpike.Controller.Configuration

Namespace Configuration

    Public Class CommandLineArgumentManagerFixture

        Sub New()

        End Sub

        <Fact()> _
        Public Sub Null_setting_parameter()
            Dim _argManager As New CommandLineArgumentManager(Nothing)
            Assert.True(_argManager.Parameters.Count = 0)
        End Sub

        <Fact()> _
        Public Sub One_setting_parameter()
            Dim _argManager As New CommandLineArgumentManager(New String() {"-h:details"})
            Assert.True(_argManager.Parameters.Count = 1)
            Assert.True(_argManager.Item("h") = "details")
        End Sub

        <Fact()> _
        Public Sub Two_setting_parameter()
            Dim _argManager As New CommandLineArgumentManager(New String() {"/list", "-h:details"})
            Assert.True(_argManager.Parameters.Count = 2)
            Assert.True(_argManager.Item("h") = "details")
            Assert.True(CBool(_argManager.Item("list")) = True)
        End Sub

        <Fact()> _
        Public Sub Two_setting_spacevalue_parameter()
            Dim _argManager As New CommandLineArgumentManager(New String() {"/list", "-h: d etails"})
            Assert.True(_argManager.Parameters.Count = 2)
            Assert.True(_argManager.Item("h") = " d etails")
            Assert.True(CBool(_argManager.Item("list")) = True)
        End Sub

        <Fact()> _
        Public Sub Two_setting_space_parameter()
            Dim _argManager As New CommandLineArgumentManager(New String() {"/list", "param1 value1"})
            Dim valu As String = _argManager.Parameters.Keys(0).ToString
            Assert.True(_argManager.Parameters.Count = 1)
            Assert.True(_argManager.Item("list") = "param1 value1")
        End Sub

        <Fact()> _
        Public Sub Two_setting_edge_values_parameter()
            Dim _argManager As New CommandLineArgumentManager(New String() {"--param2", "/param3:""Test-:-work"""})
            Assert.True(_argManager.Parameters.Count = 2)
            Assert.True(_argManager.Item("param3") = "Test-:-work")
        End Sub

        <Fact()> _
        Public Sub Two_setting_edge_values_two_parameter()
            Dim _argManager As New CommandLineArgumentManager(New String() {"/param4=happy", "-param5='--=nice=--'"})
            Dim _val As String = _argManager.Item("param5")
            Assert.True(_argManager.Parameters.Count = 2)
            Assert.True(_argManager.Item("param5") = "--=nice=--")
        End Sub

    End Class

End Namespace