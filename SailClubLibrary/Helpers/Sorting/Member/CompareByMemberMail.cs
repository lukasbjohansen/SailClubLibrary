using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

public class CompareByMemberMail : IComparer<Member>
{
    private readonly bool _descending;
    public CompareByMemberMail(bool descending = false)
    {
        _descending = descending;
    }

    public int Compare(Member? x, Member? y)
    {
        int result;
        if (x == null && y == null) result = 0;
        if (y == null) result = 1;
        if (x == null) result = -1;
        result = x.Mail.CompareTo(y.Mail);
        return _descending ? -result : result;
    }
}