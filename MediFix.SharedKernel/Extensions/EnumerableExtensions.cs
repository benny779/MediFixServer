namespace MediFix.SharedKernel.Extensions;

public static class EnumerableExtensions
{
    public static bool HasSingle<T>(this IEnumerable<T> sequence)
    {
        if (sequence is ICollection<T> list)
        {
            return list.Count == 1;
        }

        using var iterator = sequence.GetEnumerator();
        return iterator.MoveNext() && !iterator.MoveNext();
    }

    public static bool HasSingle<T>(this IEnumerable<T> sequence, out T value)
    {
        if (sequence is IList<T> { Count: 1 } list)
        {
            value = list[0];
            return true;
        }

        using var iterator = sequence.GetEnumerator();
        if (iterator.MoveNext())
        {
            value = iterator.Current;
            if (!iterator.MoveNext())
            {
                return true;
            }
        }

        value = default(T);
        return false;
    }

    public static bool IsEmpty<T>(this IEnumerable<T> sequence)
    {
        if (sequence is ICollection<T> list)
        {
            return list.Count == 0;
        }

        using var iterator = sequence.GetEnumerator();
        return !iterator.MoveNext();
    }
}
