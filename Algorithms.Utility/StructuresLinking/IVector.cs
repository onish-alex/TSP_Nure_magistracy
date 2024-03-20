namespace Algorithms.Utility.StructuresLinking
{
    public interface IVector<T>
    {
        T this[string axis] { get; set; }
        T this[int index] { get; set; }

        int Count { get; }
    }
}
