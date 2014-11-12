Namespace Configuration
    ''' <summary>
    ''' Class that will manage the list of valid assemblies from the current folder.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AssemblyManger

        Private _configuredAssemblies As String
        Private _logger As DeploymentSpike.Controller.Interfaces.IMemoryLogging
        Private _excludedAssemblies As IEnumerable(Of String)

        Sub New(configuredAssemblies As String, excludedAssemblies As IEnumerable(Of String), logger As DeploymentSpike.Controller.Interfaces.IMemoryLogging)
            _configuredAssemblies = configuredAssemblies
            If logger Is Nothing Then Throw New ArgumentNullException(DeploymentSpike.Interfaces.Constants.LoggerParameterName)
            _logger = logger
            If excludedAssemblies Is Nothing Then
                _excludedAssemblies = New List(Of String)
            Else
                _excludedAssemblies = excludedAssemblies
            End If
        End Sub

        Public ReadOnly Property GetValidAssemblies() As IEnumerable(Of String)
            Get
                'are there any items passed in the constructor?  are they valid?  if there is at least one valid existing assembly then use that and don't scan the folder.
                If String.IsNullOrEmpty(_configuredAssemblies) OrElse GetValidAssemblyCount() = 0 Then
                    _logger.Trace(My.Resources.Messages.InvalidConfigurationAssembly & My.Resources.Messages.Space & IO.Directory.GetCurrentDirectory)
                    _logger.Trace(My.Resources.Messages.ExcludedAssemblies & String.Join(My.Resources.Messages.Comma, _excludedAssemblies.ToArray()))
                    Return GetCurrentFolderAssemblies()
                End If
                Return GetValidConfigurationAssemblies()
            End Get
        End Property

        Private Function GetCurrentFolderAssemblies() As IEnumerable(Of String)
            Return IO.Directory.GetFiles(IO.Directory.GetCurrentDirectory).Where(Function(item)
                                                                                     Return IsItemValid(IO.Path.GetFileName(item))
                                                                                 End Function).ToList
        End Function

        Private Function GetValidConfigurationAssemblies() As IEnumerable(Of String)
            Return _configuredAssemblies.Split(Utility.Constants.Comma).Where(Function(item)
                                                                                  Return IsItemValid(item)
                                                                              End Function).ToList
        End Function


        Private Function GetValidAssemblyCount() As Integer
            Dim _count As Integer
            For Each item As String In _configuredAssemblies.Split(Utility.Constants.Comma)
                If IsItemValid(item) Then
                    _count += 1
                Else
                    _logger.Warning(My.Resources.Messages.InvalidAssembly & item)
                End If
            Next
            Return _count
        End Function

        Private Function IsItemValid(item As String) As Boolean
            If IO.File.Exists(IO.Directory.GetCurrentDirectory & IO.Path.DirectorySeparatorChar & item) _
                    AndAlso IO.Path.GetExtension(item) = Utility.Constants.ValidExtension _
                    AndAlso Not _excludedAssemblies.Contains(item) Then
                Return True
            Else
                Return False
            End If
        End Function

    End Class

End Namespace