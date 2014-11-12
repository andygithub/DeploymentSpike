Imports Castle.Windsor
Imports Castle.MicroKernel
Imports Castle.MicroKernel.Handlers
Imports Castle.MicroKernel.Registration
Imports Castle.Windsor.Configuration.Interpreters

Imports DeploymentSpike.Interfaces
Imports DeploymentSpike.Controller
Imports System.Configuration
Imports DeploymentSpike.Controller.Configuration
Imports Castle.Core.Logging

''' <summary>
''' Entry point for the application.
''' </summary>
''' <remarks></remarks>
Module DefaultModule

    Dim _locallogger As DeploymentSpike.Controller.Interfaces.IMemoryLogging
    Dim _containerLogger As ILogger
    Dim _container As IWindsorContainer
    Dim _commandline As CommandLineArgumentManager
    Dim _appConfiguration As DeploymentSpike.Controller.Interfaces.IConfigurationSettingsManager

    Private ReadOnly _excludedAssemblies As New List(Of String)(New String() {"Castle.Core.dll", "Castle.Facilities.Logging.dll", "Castle.Windsor.dll", "log4net.dll", "Castle.Services.Logging.Log4netIntegration.dll", "DeploymentSpike.Interfaces.dll", "DeploymentSpike.Controller.dll"})

    ''' <summary>
    ''' Default starting method for the application.
    ''' </summary>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Sub Main(args As String())
        Try
            InitializeApplication(args)
            'setup container and check configuration for assemblies.
            Dim _assembly As New AssemblyManger(_appConfiguration.AssemblyList, _excludedAssemblies, _locallogger)

            Using _compreg As New Container.ComponentRegistration(_locallogger, _appConfiguration)
                _container = _compreg.BootstrapContainer(_assembly.GetValidAssemblies)
                FlushMemoryLoggerToContainerLogger() 'all calls downstream from this method should use the container logger and not the memory logger.
                If _container.Kernel.GetAssignableHandlers(GetType(ITask)).Count = 0 Then
                    _containerLogger.Error(Controller.My.Resources.Messages.NoTasksFound)
                    Console.WriteLine(Controller.My.Resources.Messages.NoTasksFound) 'write to the console as well in case the logger is not configured properly
                    Exit Sub
                End If
                'execute list of tasks
                ExecuteTasks()
            End Using
            _containerLogger.Debug(DeploymentSpike.Controller.My.Resources.Messages.ContainerDisposed)
            Console.Write(Boolean.TrueString)
        Catch ex As Exception
            Console.WriteLine(Controller.My.Resources.Messages.UnhandledException)
            Console.WriteLine(ex.Message)
            Console.WriteLine(ex.StackTrace)
            If ex.InnerException IsNot Nothing Then
                Console.WriteLine(ex.InnerException.Message)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Method to intialize settings for the application
    ''' </summary>
    ''' <param name="commandlineArgs"></param>
    ''' <remarks></remarks>
    Private Sub InitializeApplication(commandlineArgs As String())
        'check args for any parameters
        _commandline = New CommandLineArgumentManager(commandlineArgs)
        'setup default console logger so that container setup can be logged.
        _locallogger = Factory.MemoryLoggingFactory.Create
        'push any command line arguments into the configuration settings
        _appConfiguration = New ConfigurationSettingsManager(TranslateAppSettingToDictionary(ConfigurationManager.AppSettings), _commandline.Parameters)
    End Sub

    ''' <summary>
    ''' Method to flush all initial logging into the container logger.  This is done because the container logger doesn't exist until the container has been bootstrapped and all of the container acvtivity needs to be logged.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub FlushMemoryLoggerToContainerLogger()
        _containerLogger = _container.Resolve(Of ILogger)()
        _locallogger.LogHistory.ToList.ForEach(Sub(x)
                                                   _containerLogger.Debug(x)
                                               End Sub)
        _locallogger = Nothing
    End Sub

    Private Function TranslateAppSettingToDictionary(settings As System.Collections.Specialized.NameValueCollection) As Dictionary(Of String, String)
        Dim _dictionary As New Dictionary(Of String, String)
        For Each key In settings.AllKeys
            _dictionary.Add(key, settings(key))
        Next
        Return _dictionary
    End Function

    ''' <summary>
    ''' Method to pause the operation with a readline if the configuration flag is set.s
    ''' </summary>
    ''' <param name="task"></param>
    ''' <remarks></remarks>
    Private Sub UpdateUI(task As ITask)
        If task.TaskSettings.Settings.ContainsKey(Controller.Utility.Constants.TaskPauseSetting) AndAlso task.TaskSettings.Settings(Controller.Utility.Constants.TaskPauseSetting) = Boolean.TrueString Then
            _containerLogger.Debug(Controller.My.Resources.Messages.TaskPause)
            Console.ReadLine()
        End If
    End Sub

    ''' <summary>
    ''' Method to execute all loaded tasks.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ExecuteTasks()
        _containerLogger.Debug(DeploymentSpike.Controller.My.Resources.Messages.ProcessTaskList & _appConfiguration.TaskList)
        Dim tasks As IEnumerable(Of Interfaces.ITaskInformation) = Factory.TaskStepsFactory.Create(_appConfiguration.TaskList, _containerLogger, _appConfiguration)
        If tasks Is Nothing OrElse tasks.Count = 0 Then
            _containerLogger.Warn(DeploymentSpike.Controller.My.Resources.Messages.NoTasksFound)
            Exit Sub
        End If
        Dim _task As ITask = Nothing
        Dim _taskResult As TaskStatus
        For Each _taskItem In tasks
            _containerLogger.Debug(DeploymentSpike.Controller.My.Resources.Messages.AttemptingToResolveTask & _taskItem.TaskComponentName)
            If Not _container.Kernel.HasComponent(_taskItem.TaskComponentName) Then
                _containerLogger.Fatal(DeploymentSpike.Controller.My.Resources.Messages.KernelMissingComponent & _taskItem.TaskComponentName)
                _containerLogger.Fatal(Controller.My.Resources.Messages.ProcessHaltedException)
                Exit For
            End If
            Try
                _task = _container.Resolve(Of ITask)(_taskItem.TaskComponentName)
            Catch ex As Castle.MicroKernel.ComponentActivator.ComponentActivatorException
                _containerLogger.Fatal(ex.Message)
                If ex.InnerException IsNot Nothing Then
                    _containerLogger.Fatal(ex.InnerException.Message)
                End If
                Exit For
            End Try
            'assume that task was properly resolved
            _task.MetaInformation = _taskItem
            _task.MetaInformationList = tasks
            _taskResult = _task.Execute()
            UpdateUI(_task)
            If _taskResult = TaskStatus.Completed Then
                _task.MetaInformation.CompletedStep = True
            End If
            'update the meta information list with all of the diagnostic information from the run.
            _taskItem = _task.MetaInformation
            _container.Release(_task)
            If _taskResult = TaskStatus.Failed Then
                'stop process on a failed task.
                _containerLogger.Debug(DeploymentSpike.Controller.My.Resources.Messages.ProcessHaltedFailedTask)
                Exit For
            End If
        Next
        Controller.Factory.TaskStepsFactory.SaveTaskSteps(_appConfiguration, tasks, Controller.Utility.Constants.CompleteTaskStepsjson)
        Controller.Factory.ReportGenerationFactory.TaskStepsHtml(tasks, Controller.Utility.Constants.CompleteTaskStepsReportHtml)
    End Sub

End Module
