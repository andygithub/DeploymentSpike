Imports System.Runtime.CompilerServices

Public Module Conversion

    <Extension()>
    Public Function ToTaskStatus(value As Boolean) As TaskStatus
        If value Then Return TaskStatus.Completed
        Return TaskStatus.Failed
    End Function

End Module
