Imports System.Reflection
Imports System.Runtime.CompilerServices

Namespace Utility

    ''' <summary>
    ''' Extension methods for the project.
    ''' </summary>
    ''' <remarks></remarks>
    Public Module ReflectionExtensions
        Private Const TimeZoneFormat As String = "HH:mm:ss tt zzz"

        ''' <summary>
        ''' Returns a private Property Value from a given Object. Uses Reflection.
        ''' Throws a ArgumentOutOfRangeException if the Property is not found.
        ''' </summary>
        ''' <typeparam name="T">Type of the Property</typeparam>
        ''' <param name="value">Object from where the Property Value is returned</param>
        ''' <param name="propName">Propertyname as string.</param>
        ''' <returns>PropertyValue</returns>
        <Extension()> _
        Public Function GetPrivateFieldValue(Of T)(value As Object, propName As String) As T
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If
            Dim _t As Type = value.[GetType]()
            Dim fi As FieldInfo = Nothing
            While fi Is Nothing AndAlso _t IsNot Nothing
                fi = _t.GetField(propName, BindingFlags.[Public] Or BindingFlags.NonPublic Or BindingFlags.Instance)
                _t = _t.BaseType
            End While
            If fi Is Nothing Then
                Throw New ArgumentOutOfRangeException("propName", String.Format(System.Globalization.CultureInfo.InvariantCulture, "Field {0} was not found in Type {1}", propName, value.[GetType]().FullName))
            End If
            Return DirectCast(fi.GetValue(value), T)
        End Function

        <Extension()>
        Public Function ToHtmlReport(value As IEnumerable(Of ITaskInformation)) As String
            Dim _html As New Text.StringBuilder

            _html.AppendLine("<!DOCTYPE html><html lang=""en""><head><meta charset=""utf-8""><title>Completed Task Report</title><meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">")
            _html.AppendLine("<link href=""http://twitter.github.com/bootstrap/assets/css/bootstrap.css"" rel=""stylesheet"" />")
            _html.AppendLine("</head><body>    <div class=""page-header""><h1>Completed Task Report</h1></div><div class=""container-fluid""><div class=""row-fluid""><div class=""span12""><p/>")

            'inject table into template
            _html.Append(value.ToHtmlTable)
            'write rest of content
            _html.AppendFormat("</div></div></div><hr/><footer><p>Generated on {0}</p></footer></body></html>", Controller.Provider.DefaultTimeProvider.Current.UtcNow)

            Return _html.ToString
        End Function

        <Extension()>
        Public Function ToHtmlTable(value As IEnumerable(Of ITaskInformation)) As String
            Const emptytablecell As String = "<td></td>"
            Const placeholdertablecell As String = "<td>{0}</td>"
            Dim _table As New Text.StringBuilder
            'translate tasks to html table
            _table.AppendLine("<table class=""table table-bordered table-striped"">")
            _table.AppendLine("<thead><tr><th>Check if Complete</th><th>Step</th><th>Deployment Task</th><th>Migration Thread</th><th>Servers Impacted</th><th>Comments</th><th>Dependencies</th><th>Start Date</th><th>Start Time</th><th>End Time</th><th>Duration</th><th>DPW Owner (s)</th><th>DC Support</th></tr></thead>")
            _table.AppendLine("<tbody>")
            value.OrderBy(Function(i)
                              Return i.StepNumber
                          End Function).ToList.ForEach(Sub(item)
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

End Namespace