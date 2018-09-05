using System.Collections.Generic;

public static class CustomExtensions
{
    public static IReadOnlyList<T> ToIReadonlyList<T>(this Queue<T> queue)
    {
        var list = new List<T>();
        foreach(var element in queue)
        {
            list.Add(element);
        }
        return list;
    }
}
