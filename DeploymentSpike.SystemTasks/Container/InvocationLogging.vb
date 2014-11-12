Imports Castle.DynamicProxy

Namespace Container
    ''' <summary>
    ''' Class for translating the invoation into a string for logging.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class InvocationLogging

        Private Sub New()

        End Sub

        Public Shared Function CreateInvocationLogString(invocation As IInvocation) As String
            If invocation Is Nothing Then Return String.Empty
            Dim sb As New Text.StringBuilder(100)
            sb.AppendFormat(My.Resources.Messages.CalledMethod, invocation.TargetType.Name, invocation.Method.Name)
            For Each argument As Object In invocation.Arguments
                Dim argumentDescription As String = If(argument Is Nothing, "null", DumpObject(argument))
                sb.Append(argumentDescription).Append(",")
            Next
            If invocation.Arguments.Count() > 0 Then
                sb.Length -= 1
            End If
            sb.Append(")")
            Return sb.ToString()
        End Function

        Public Shared Function DumpObject(argument As Object) As String
            If argument Is Nothing Then Return String.Empty
            'Dim objtype As Type = argument.GetType()
            'If objtype = GetType(String) OrElse objtype.IsPrimitive OrElse Not objtype.IsClass Then
            '    Return objtype.ToString()
            'End If

            Return argument.GetType.ToString 'DataContractSerialize(argument, objtype)
        End Function

    End Class

End Namespace