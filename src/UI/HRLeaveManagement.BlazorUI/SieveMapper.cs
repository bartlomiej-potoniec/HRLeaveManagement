namespace HRLeaveManagement.BlazorUI;

public static class SieveMapper
{
    public static string MapToSieveFilter(string propertyName, object filter)
    {
        var filterType = filter.GetType();

        var operatorProperty = filterType.GetProperty("Operator");
        var operatorValue = operatorProperty.GetValue(filter).ToString();

        var filterValue = filterType.GetProperty("Value");
        var value = filterValue.GetValue(filter);

        var sieveValue = value is null or "" || operatorValue is "is empty" || operatorValue is "is not empty"
            ? ""
            : value.ToString();

        var sieveOperator = ConvertToSieveOperator(operatorValue);

        var result = $"{propertyName}{sieveOperator}{sieveValue}";

        return result;
    }

    public static string MapToSieveSort(string sortBy, bool isDescending)
    {
        if (sortBy is null) return ""; 

        var sign = isDescending ? "-" : "";
        return $"{ sign }{ sortBy }";
    }

    private static string ConvertToSieveOperator(string filterOperator) 
        => filterOperator switch
        {
            "contains" => "@=*",
            "not contains" => "!@=*",
            "equals" => "==*",
            "not equals" => "!=*",
            "starts with" => "_=*",
            "ends with" => "_-=*",
            "is empty" => "==\\null",
            "is not empty" => "!=\\null",
            "=" => "==",
            "!=" => "!=",
            ">" => ">",
            ">=" => ">=",
            "<" => "<",
            "<=" => "<=",
            "is" => "==",
            "is not" => "!=",
            "is after" => ">",
            "is on or after" => ">=",
            "is before" => "<",
            "is on or before" => "<=",
            _ => throw new NotSupportedException($"Unsupported operator: { filterOperator }")
        };
}
