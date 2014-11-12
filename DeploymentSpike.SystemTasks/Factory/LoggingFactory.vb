Imports Castle.Core.Logging
Imports log4net
Imports Castle.Services.Logging.Log4netIntegration
Imports System.Xml.Linq

'Imports log4net.Config

Namespace Factory

    ''' <summary>
    ''' Implemetnation of Castle ILoggerFactory so that custom settings can be injected.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LoggingFactory
        Implements ILoggerFactory

        Public Sub New()
            'Configure the loggers this should only happen once.
            InitializeLog4Net()
        End Sub

        Public Function Create(name As String) As Castle.Core.Logging.ILogger Implements Castle.Core.Logging.ILoggerFactory.Create
            Dim _log As ILog = LogManager.GetLogger(name)
            Return (New Log4netLogger(_log.Logger, New Log4netFactory))
        End Function

        Public Function Create(name As String, level As Castle.Core.Logging.LoggerLevel) As Castle.Core.Logging.ILogger Implements Castle.Core.Logging.ILoggerFactory.Create
            'level is ignored
            Dim _log As ILog = LogManager.GetLogger(name)
            Return (New Log4netLogger(_log.Logger, New Log4netFactory))
        End Function

        Public Function Create(type As System.Type) As Castle.Core.Logging.ILogger Implements Castle.Core.Logging.ILoggerFactory.Create
            Dim _log As ILog = LogManager.GetLogger(type)
            Return (New Log4netLogger(_log.Logger, New Log4netFactory))
        End Function

        Public Function Create(type As System.Type, level As Castle.Core.Logging.LoggerLevel) As Castle.Core.Logging.ILogger Implements Castle.Core.Logging.ILoggerFactory.Create
            'level is ignored
            Dim _log As ILog = LogManager.GetLogger(type)
            Return (New Log4netLogger(_log.Logger, New Log4netFactory))
        End Function

        Private Shared Sub InitializeLog4Net()
            Dim _xdoc As New Xml.XmlDocument
            Dim _xmlElement As Xml.XmlElement = CType(_xdoc.ReadNode(GetConfiguration.CreateReader()), Xml.XmlElement)
            log4net.Config.XmlConfigurator.Configure(_xmlElement)
        End Sub

        Private Shared Function GetConfiguration() As XElement
            Return XElement.Parse(My.Resources.LogConfiguration.DefaultLogNet)
        End Function

    End Class

End Namespace