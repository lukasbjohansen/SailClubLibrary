using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

public class CompareByMemberAddress : IComparer<Member>
{
    private readonly bool _descending;
    public CompareByMemberAddress(bool descending = false)
    {
        _descending = descending;
    }

    public int Compare(Member? x, Member? y)
    {
        int result;
        if (x == null && y == null) result = 0;
        if (y == null) result = 1;
        if (x == null) result = -1;
        result = x.Address.CompareTo(y.Address);
        return _descending ? -result : result;
    }
}