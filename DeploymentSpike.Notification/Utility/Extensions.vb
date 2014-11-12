Imports System.Runtime.CompilerServices

Public Module Extensions

    Private Const TimeZoneFormat As String = "HH:mm:ss tt zzz"
    Private Const emptytablecell As String = "<td style=""border-bottom: 1px solid #dddddd;padding: 8px;""></td>"
    Private Const placeholdertablecell As String = "<td style=""border-bottom: 1px solid #dddddd;padding: 8px;"">{0}</td>"
    Private Const _headerstyleDefault As String = "style=""border-bottom: 1px solid #dddddd;padding: 8px;background-color: #f5f5f5;"""
    Private Const _cellstyleDefault As String = "style=""border-bottom: 1px solid #dddddd;padding: 8px;"""

    <Extension()>
    Public Function ToEmailSubjectLine(value As Interfaces.ITask, notifytype As NotificationType) As String
        'get the meta step name of the current task
        Dim _taskDesc As String = My.Resources.Messages.DefaultLabel
        If Not value.MetaInformation Is Nothing AndAlso Not String.IsNullOrWhiteSpace(value.MetaInformation.DeploymentTask) Then
            _taskDesc = value.MetaInformation.DeploymentTask
        End If
        Select Case notifytype
            Case NotificationType.Started
                Return String.Format(My.Resources.Messages.TaskStarted, _taskDesc, Now)
            Case NotificationType.CompletedSuccess
                Return String.Format(My.Resources.Messages.TaskCompleteSucccess, _taskDesc, Now)
            Case Else
                Return String.Format(My.Resources.Messages.TaskCompleteFailure, _taskDesc, Now)
        End Select
    End Function

    <Extension()>
    Public Function ToEmailBody(value As Interfaces.ITask, notifytype As NotificationType) As String
        'template downloaded from - http://htmlemailboilerplate.com/
        Dim _body As New Text.StringBuilder
        _body.AppendLine(My.Resources.Messages.EmailBodyHeaderStyle & My.Resources.Messages.EmailBodyTableContainer)
        _body.AppendLine(value.ToEmailSubjectLine(notifytype))
        _body.AppendLine(My.Resources.Messages.CurrentStep)
        Dim _list As New List(Of ITaskInformation)
        _list.Add(value.MetaInformation)
        _body.AppendLine(_list.ToEmailHtmlTable)
        _body.AppendLine(My.Resources.Messages.StepList)
        ''put in the entire table and highlight the current step.
        _body.AppendLine(value.MetaInformationList.ToEmailHtmlTable)

        _body.AppendLine(My.Resources.Messages.EmailBodyContactInformation)

        _body.AppendLine("</td></tr></table>  <!-- End of wrapper table --></body></html>")

        Return _body.ToString
    End Function

    <Extension()>
    Public Function ToEmailHtmlTable(value As IEnumerable(Of ITaskInformation)) As String

        If value Is Nothing Then value = New List(Of ITaskInformation)
        Dim _table As New Text.StringBuilder
        'translate tasks to html table
        _table.AppendLine(My.Resources.Messages.EmailTableHeader)
        _table.AppendFormat(My.Resources.Messages.EmailBodyTaskHeader, _headerstyleDefault)
        value.OrderBy(Function(i)
                          If i Is Nothing Then Return 0
                          Return i.StepNumber
                      End Function).ToList.ForEach(Sub(item)
                                                       If item Is Nothing Then Exit Sub
                                                       _table.AppendLine("<tr>")
                                                       If item.CompletedStep Then
                                                           _table.AppendFormat("<td {0}>Yes</td>", _cellstyleDefault)
                                                       Else
                                                           _table.AppendFormat("<td {0}>No</td>", _cellstyleDefault)
                                                       End If
                                                       _table.AppendFormat(placeholdertablecell, item.StepNumber.ToString(System.Globalization.CultureInfo.InvariantCulture))
                                                       _table.AppendFormat(placeholdertablecell, item.DeploymentTask)
                                                       _table.AppendFormat(placeholdertablecell, item.MigrationThread)
                                                       _table.AppendFormat(placeholdertablecell, item.ServersImpacted)
                                                       _table.AppendFormat(placeholdertablecell, item.Comments)
                                                       _table.AppendFormat(placeholdertablecell, item.Dependencies)
                                                       If item.ActualDate.HasValue Then
                                                           _table.AppendFormat(placeholdertablecell, item.ActualDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture))
                                                       Else
                                                           _table.Append(emptytablecell)
                                                       End If
                                                       If item.ActualStartTime.HasValue Then
                                                           _table.AppendFormat(placeholdertablecell, item.ActualStartTime.Value.ToString(TimeZoneFormat, System.Globalization.CultureInfo.InvariantCulture))
                                                       Else
                                                           _table.Append(emptytablecell)
                                                       End If
                                                       If item.ActualEndTime.HasValue Then
                                                           _table.AppendFormat(placeholdertablecell, item.ActualEndTime.Value.ToString(TimeZoneFormat, System.Globalization.CultureInfo.InvariantCulture))
                                                       Else
                                                           _table.Append(emptytablecell)
                                                       End If
                                                       If item.ActualDuration.HasValue Then
                                                           _table.AppendFormat(placeholdertablecell, item.ActualDuration.Value.ToString("c", System.Globalization.CultureInfo.CurrentCulture))
                                                       Else
                                                           _table.Append(emptytablecell)
                                                       End If
                                                       _table.AppendFormat(placeholdertablecell, item.Owners)
                                                       _table.AppendFormat(placeholdertablecell, item.Support)
                                                       _table.AppendLine("</tr>")
                                                   End Sub)
        _table.AppendLine("</tbody></table>")
        Return _table.ToString
    End Function

    <Extension()>
    Public Function ToHtmlReport(value As IEnumerable(Of ITaskInformation)) As String
        Dim _html As New Text.StringBuilder

        _html.AppendLine("<!DOCTYPE html><html lang=""en""><head><meta charset=""utf-8""><title>Maintenance Page for Deployment</title><meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">")
        _html.AppendLine("<link href=""http://twitter.github.com/bootstrap/assets/css/bootstrap.css"" rel=""stylesheet"" />")
        _html.AppendLine("</head><body>    <div class=""page-header""><h1>Maintenance Page for Deployment</h1></div><div class=""container-fluid""><div class=""row-fluid""><div class=""span12""><p/>")

        'inject table into template
        _html.Append(value.ToHtmlTable)
        'write rest of content
        _html.AppendFormat("</div></div></div><hr/><footer><p>Generated on {0}</p></footer></body></html>", Now)

        Return _html.ToString
    End Function

    <Extension()>
    Public Function ToHtmlTable(value As IEnumerable(Of ITaskInformation)) As String
        If value Is Nothing Then value = New List(Of ITaskInformation)
        Const emptytablecell As String = "<td></td>"
        Const placeholdertablecell As String = "<td>{0}</td>"
        Dim _table As New Text.StringBuilder
        'translate tasks to html table
        _table.AppendLine("<table class=""table table-bordered table-striped"">")
        _table.AppendLine("<thead><tr><th>Check if Complete</th><th>Step</th><th>Deployment Task</th><th>Migration Thread</th><th>Servers Impacted</th><th>Comments</th><th>Dependencies</th><th>Start Date</th><th>Start Time</th><th>End Time</th><th>Duration</th><th>DPW Owner (s)</th><th>DC Support</th></tr></thead>")
        _table.AppendLine("<tbody>")
        value.OrderBy(Function(i)
                          'If i Is Nothing Then Return 0
                          Return i.StepNumber
                      End Function).ToList.ForEach(Sub(item)
                                                       'If item Is Nothing Then Exit Sub
                                                       _table.AppendLine("<tr>")
                                                       If item.CompletedStep Then
                                                           _table.Append("<td><i class=""icon-ok""></i></td>")
                                                           _table.AppendFormat("<td><span class=""badge badge-success"">{0}</span></td>", item.StepNumber.ToString(System.Globalization.CultureInfo.InvariantCulture))
                                                       Else
                                                           _table.Append("<td><i class=""icon-remove""></i></td>")
                                                           _table.AppendFormat("<td><span class=""badge badge-info"">{0}</span></td>", item.StepNumber.ToString(System.Globalization.CultureInfo.InvariantCulture))
                                                       End If
                                                       _table.AppendFormat(placeholdertablecell, item.DeploymentTask)
                                                       _table.AppendFormat(placeholdertablecell, item.MigrationThread)
                                                       _table.AppendFormat(placeholdertablecell, item.ServersImpacted)
                                                       _table.AppendFormat(placeholdertablecell, item.Comments)
                                                       _table.AppendFormat(placeholdertablecell, item.Dependencies)
                                                       If item.ActualDate.HasValue Then
                                                           _table.AppendFormat(placeholdertablecell, item.ActualDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture))
                                                       Else
                                                           _table.Append(emptytablecell)
                                                       End If
                                                       If item.ActualStartTime.HasValue Then
                                                           _table.AppendFormat(placeholdertablecell, item.ActualStartTime.Value.ToString(TimeZoneFormat, System.Globalization.CultureInfo.InvariantCulture))
                                                       Else
                                                           _table.Append(emptytablecell)
                                                       End If
                                                       If item.ActualEndTime.HasValue Then
                                                           _table.AppendFormat(placeholdertablecell, item.ActualEndTime.Value.ToString(TimeZoneFormat, System.Globalization.CultureInfo.InvariantCulture))
                                                       Else
                                                           _table.Append(emptytablecell)
                                                       End If
                                                       If item.ActualDuration.HasValue Then
                                                           _table.AppendFormat(placeholdertablecell, item.ActualDuration.Value.ToString("c", System.Globalization.CultureInfo.InvariantCulture))
                                                       Else
                                                           _table.Append(emptytablecell)
                                                       End If
                                                       _table.AppendFormat(placeholdertablecell, item.Owners)
                                                       _table.AppendFormat(placeholdertablecell, item.Support)
                                                       _table.AppendLine("</tr>")
                                                   End Sub)
        _table.AppendLine("</tbody>")
        _table.AppendLine("</table>")
        Return _table.ToString
    End Function

End Module
