Namespace Container
    ''' <summary>
    ''' Class that applies filters on the interceptors so that only certain methods on the class invoke interception.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class MethodInterceptionFilter

        Private Sub New()

        End Sub

        Public Shared Function IsMethodIntercepted(invocation As Castle.DynamicProxy.IInvocation) As Boolean
            If invocation Is Nothing Then Return False
            'this is for the ITask
            If invocation.Method.Name = Utility.Constants.MethodExecute Then Return True
            'this is the the INotification
            If invocation.Method.Name = Utility.Constants.MethodNotifyStarted OrElse invocation.Method.Name = Utility.Constants.MethodNotifyCompletedSuccess OrElse invocation.Method.Name = Utility.Constants.MethodNotifyCompletedFailure Then Return True
            Return False
        End Function

    End Class

End Namespace