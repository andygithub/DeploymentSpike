Namespace Provider

    ''' <summary>
    ''' TimeProvider sealed class.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class TimeProvider

        Private Shared _current As TimeProvider

        Shared Sub New()
            TimeProvider.Current = New DefaultTimeProvider()
        End Sub

        Public Shared Property Current() As TimeProvider
            Get
                Return _current
            End Get
            Set(value As TimeProvider)
                If value Is Nothing Then Throw New ArgumentNullException("value")
                _current = value
            End Set
        End Property

        Public MustOverride ReadOnly Property UtcNow() As DateTime

        Public Shared Sub ResetToDefault()
            TimeProvider.Current = New DefaultTimeProvider()
        End Sub

    End Class

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DefaultTimeProvider
        Inherits TimeProvider

        Public Overrides ReadOnly Property UtcNow() As DateTime
            Get
                Return DateTime.UtcNow
            End Get
        End Property

    End Class

End Namespace