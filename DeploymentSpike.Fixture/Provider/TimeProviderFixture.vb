Public Class TimeProviderFixture

    <Fact()> _
    Public Sub default_timeprovider_usage()
        Dim _date As DateTime = Controller.Provider.TimeProvider.Current.UtcNow
        Assert.NotNull(_date)
    End Sub

    <Fact()> _
    Public Sub null_current_exception()
        Assert.Throws(Of ArgumentNullException)(Sub()
                                                    Controller.Provider.TimeProvider.Current = Nothing
                                                End Sub)
    End Sub

    <Fact()> _
    Public Sub set_user_controlled_default_date()
        Dim startTime As New DateTime(2009, 8, 29)
        Dim timeProviderStub As Controller.Provider.TimeProvider = MockRepository.GenerateMock(Of Controller.Provider.TimeProvider)()
        timeProviderStub.Stub(Function(x)
                                  Return x.UtcNow
                              End Function).Return(startTime)
        Controller.Provider.TimeProvider.Current = timeProviderStub
        Assert.Equal(startTime, Controller.Provider.TimeProvider.Current.UtcNow)
    End Sub

    <Fact()> _
    Public Sub reset_to_default_controlled_default_date()
        Controller.Provider.TimeProvider.ResetToDefault()
        Dim startTime As New DateTime(2009, 8, 29)
        Dim timeProviderStub As Controller.Provider.TimeProvider = MockRepository.GenerateMock(Of Controller.Provider.TimeProvider)()
        timeProviderStub.Stub(Function(x)
                                  Return x.UtcNow
                              End Function).Return(startTime)
        Controller.Provider.TimeProvider.Current = timeProviderStub
        Controller.Provider.TimeProvider.ResetToDefault()
        Assert.NotEqual(startTime, Controller.Provider.TimeProvider.Current.UtcNow)
    End Sub

End Class
