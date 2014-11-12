Namespace Factory

    ''' <summary>
    ''' Simple logging factory.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class MemoryLoggingFactory

        Private Sub New()

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function Create() As Interfaces.IMemoryLogging
            Return New DefaultInstance.ConsoleLogging
        End Function

    End Class

End Namespace