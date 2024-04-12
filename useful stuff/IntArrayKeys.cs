public class IntArrayKeys : IEqualityComparer<int[]>
{
    public bool Equals(int[] x, int[] y)
    {
        if (x.Length != y.Length)
        {
            return false;
        }
        for (int i = 0; i < x.Length; i++)
        {
            if (x[i] != y[i])
            {
                return false;
            }
        }
        return true;
    }
    public int GetHashCode(int[] values)
    {
        int result = 0;
        int shift = 0;
        for (int i = 0; i < values.Length; i++)
        {
            shift = (shift + 11) % 21;
            result ^= (values[i]+1024) << shift;
        }
        return result;
    }
}
