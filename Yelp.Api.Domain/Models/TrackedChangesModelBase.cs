namespace Yelp.Api.Domain.Models;

public abstract class TrackedChangesModelBase : ModelBase
{
    private List<string> _changedProperties = new List<string>();

    protected internal override void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.NotifyPropertyChanged(propertyName);
        if (propertyName != null && _changedProperties.Contains(propertyName) == false)
            _changedProperties.Add(propertyName);
    }

    internal void ClearPropertiesChangedList()
    {
        _changedProperties.Clear();
    }

    public Dictionary<string, object> GetChangedProperties()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        foreach (var propertyName in _changedProperties)
        {
            PropertyInfo? pi = GetType().GetRuntimeProperty(propertyName);
            if (pi == null) continue;

            var jsonProp = pi.CustomAttributes.FirstOrDefault(f => f.AttributeType.GetTypeInfo() == typeof(JsonPropertyAttribute).GetTypeInfo());
            if (jsonProp != null && jsonProp.ConstructorArguments.Any())
            {
                var argument = jsonProp.ConstructorArguments.FirstOrDefault();
                var name = argument.Value?.ToString()?.Replace("\"", string.Empty);
                if (!string.IsNullOrEmpty(name))
                {
                    var value = pi.GetValue(this);
                    if (value != null)
                        dic.Add(name, value);
                }
            }
            else
            {
                var value = pi.GetValue(this);
                if (value != null)
                    dic.Add(propertyName, value);
            }
        }

        return dic;
    }
}