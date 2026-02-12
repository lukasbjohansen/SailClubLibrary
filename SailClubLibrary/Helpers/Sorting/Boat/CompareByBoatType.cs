using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

public class CompareByBoatType : IComparer<Boat>
{
    private readonly bool _descending;
    public CompareByBoatType(bool descending = false)
    {
        _descending = descending;
    }

    public int Compare(Boat? x, Boat? y)
    {
        int result;
        if (x == null && y == null) result = 0;
        if (y == null) result = 1;
        if (x == null) result = -1;
        result = x.TheBoatType.CompareTo(y.TheBoatType);
        return _descending ? -result : result;
    }
}