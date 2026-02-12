using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

public class CompareByMemberName : IComparer<Member>
{
    private readonly bool _descending;
    public CompareByMemberName(bool descending = false)
    {
        _descending = descending;
    }

    public int Compare(Member? x, Member? y)
    {
        int result;
        if (x == null && y == null) result = 0;
        if (y == null) result = 1;
        if (x == null) result = -1;
        result = (x.FirstName+x.SurName).CompareTo((y.FirstName + y.SurName));
        return _descending ? -result : result;
    }
}