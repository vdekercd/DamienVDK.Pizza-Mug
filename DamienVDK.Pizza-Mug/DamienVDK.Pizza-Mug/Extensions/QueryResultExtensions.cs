namespace DamienVDK.Pizza_Mug.Extensions;

public static class QueryResultExtensions
{
    public static int GetIntFieldOrDefaultValue(this QueryResult @this, string fieldName,  int defaultValue = default)
    {
        var intFromRequest = @this.Parameters.Fields.FirstOrDefault(item => item.Key == fieldName);
        return int.TryParse(intFromRequest.Value.ToString(), out var intResponse) ? intResponse : defaultValue;
    }

    public static string GetStringFieldOrDefaultValue(this QueryResult @this, string fieldName, string defaultValue = "")
    {
        var stringFromRequest = @this.Parameters.Fields.FirstOrDefault(item => item.Key == fieldName);
        return string.IsNullOrWhiteSpace(stringFromRequest.Value.ToString()) ? defaultValue : stringFromRequest.Value.ToString();
    }
    
    public static List<double> GetIntFieldList(this QueryResult @this, string fieldName,  int defaultValue = default)
    {
        var intListFromRequest = @this.Parameters.Fields.FirstOrDefault(item => item.Key == fieldName).Value;
        return intListFromRequest.ListValue.Values.Select(item => item.NumberValue).ToList();
    }
    public static List<string> GetStringFieldList(this QueryResult @this, string fieldName,  string defaultValue = "")
    {
        var stringListFromRequest = @this.Parameters.Fields.FirstOrDefault(item => item.Key == fieldName).Value;
        var list = stringListFromRequest.ListValue.Values.Select(item => item.StringValue).ToList();
        return list.Select(item => string.IsNullOrWhiteSpace(item.ToString()) ? defaultValue : item.ToString()).ToList();
    }
}