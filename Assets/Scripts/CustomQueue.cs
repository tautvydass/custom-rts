using System.Collections.Generic;

public class CustomQueue<T>
{
    private List<T> list;

    public int Count => list.Count;

    public IReadOnlyList<T> GetReadonlyList() => list;

    public CustomQueue()
    {
        list = new List<T>();
    }

    public void Enqueue(T objToEnqueue)
    {
        list.Add(objToEnqueue);
    }

    public T Dequeue()
    {
        var objToDequeue = list[0];
        list.Remove(objToDequeue);
        return objToDequeue;
    }

    public T Remove(int index)
    {
        if(index < 0 || index > list.Count - 1)
        {
            throw new System.Exception($"Index out of range: { index }");
        }

        var objToRemove = list[index];
        list.Remove(objToRemove);

        return objToRemove;
    }

    public T Peek()
    {
        return list[list.Count - 1];
    }
}
