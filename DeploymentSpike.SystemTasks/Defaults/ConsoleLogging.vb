Imports System.Text

Namespace DefaultInstance

    ''' <summary>
    ''' Simple logging implementation.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ConsoleLogging
        Implements Interfaces.IMemoryLogging


        Dim _history As New List(Of String)

        Public Sub [Error](message As String) Implements Interfaces.IMemoryLogging.Error
            _history.Add(message)
            Debug.WriteLine(message)
            Console.WriteLine(My.Resources.Messages.LogError & message & My.Resources.Messages.Space & Controller.Provider.TimeProvider.Current.UtcNow.ToString(System.Globalization.CultureInfo.InvariantCulture))
        End Sub

        Public Sub Trace(message As String) Implements Interfaces.IMemoryLogging.Trace
            _history.Add(message)
            Debug.WriteLine(message)
            Console.WriteLine(My.Resources.Messages.LogTrace & message & My.Resources.Messages.Space & Controller.Provider.TimeProvider.Current.UtcNow.ToString(System.Globalization.CultureInfo.InvariantCulture))
        End Sub

        Public Sub Warning(message As String) Implements Interfaces.IMemoryLogging.Warning
            _history.Add(message)
            Debug.WriteLine(message)
            Console.WriteLine(My.Resources.Messages.LogWarning & message & My.Resources.Messages.Space & Controller.Provider.TimeProvider.Current.UtcNow.ToString(System.Globalization.CultureInfo.InvariantCulture))
        End Sub

        Public Function LogHistory() As IEnumerable(Of String) Implements Interfaces.IMemoryLogging.LogHistory
            Return _history
        End Function

        Public Sub ClearHistory() Implements Interfaces.IMemoryLogging.ClearHistory
            _history.Clear()
        End Sub
    End Class

End Namespace