
Public Class ConsoleLoggingFixture

    <Fact()> _
    Public Sub console_logging_error()
        Dim _logger As New Controller.DefaultInstance.ConsoleLogging
        _logger.Error(Boolean.FalseString)
        Assert.Contains(Boolean.FalseString, _logger.LogHistory)
    End Sub

    <Fact()> _
    Public Sub console_logging_warning()
        Dim _logger As New Controller.DefaultInstance.ConsoleLogging
        _logger.Warning(Boolean.FalseString)
        Assert.Contains(Boolean.FalseString, _logger.LogHistory)
    End Sub

    <Fact()> _
    Public Sub console_logging_trace()
        Dim _logger As New Controller.DefaultInstance.ConsoleLogging
        _logger.Trace(Boolean.FalseString)
        Assert.Contains(Boolean.FalseString, _logger.LogHistory)
    End Sub

    <Fact()> _
    Public Sub console_logging_clear_history()
        Dim _logger As New Controller.DefaultInstance.ConsoleLogging
        _logger.Warning(Boolean.FalseString)
        _logger.ClearHistory()
        Assert.Equal(0, _logger.LogHistory.Count)
    End Sub

End Class
