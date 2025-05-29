using System.Text.Json;

namespace Crypto.Infrastructure.Common;
public static class JsonUtils
{
    public static JsonElement TryGetNestedProperty(this JsonElement element, string[] path)
    {
        JsonElement current = element;

        foreach (var key in path)
        {
            if (!current.TryGetProperty(key, out JsonElement next))
            {
                throw new KeyNotFoundException($"Missing property '{key}' in JSON path: {string.Join(".", path)}");
            }
            current = next;
        }

        return current;
    }
}
