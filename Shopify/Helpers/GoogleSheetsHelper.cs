namespace Shopify.Helpers;

using System.Reflection;

public static class GoogleSheetsHelper
{
    /// <summary>
    /// Converts any object to a row (IList<object>) for Google Sheets
    /// using its public properties in declaration order.
    /// </summary>
    public static IList<object> ToRow<T>(T obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        var type = typeof(T);

        // Get public instance properties, ordered by declaration
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .OrderBy(p => p.MetadataToken); // preserves order in source code

        var row = new List<object>();

        foreach (var prop in props)
        {
            var value = prop.GetValue(obj) ?? ""; // null -> empty string
            row.Add(value);
        }

        return row;
    }
}
