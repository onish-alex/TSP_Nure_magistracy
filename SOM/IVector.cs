using System.Collections.Generic;

namespace SOM
{
    public interface IVector<T>: IEnumerable<T>
    {
        T this[string axis] { get; }
        T this[int index] { get; set; }

        int Count { get; }
    }
}
