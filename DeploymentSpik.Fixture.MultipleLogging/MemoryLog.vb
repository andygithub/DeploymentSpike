Public Class MemoryLog
    Implements ILogging

    Public Sub [Error](message As String) Implements Interfaces.ILogging.Error

    End Sub

    Public Function LogHistory() As System.Text.StringBuilder Implements Interfaces.ILogging.LogHistory
        Return Nothing
    End Function

    Public Sub Trace(message As String) Implements Interfaces.ILogging.Trace

    End Sub

    Public Sub Warning(message As String) Implements Interfaces.ILogging.Warning

    End Sub
End Class
