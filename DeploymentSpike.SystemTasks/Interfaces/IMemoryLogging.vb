Imports System.Text

Namespace Interfaces
    ''' <summary>
    ''' Interface for a simple logger.
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IMemoryLogging

        Sub Trace(message As String)
        Sub Warning(message As String)
        Sub [Error](message As String)

        Function LogHistory() As IEnumerable(Of String)
        Sub ClearHistory()

    End Interface

End Namespace