using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

public class CompareByBookingEndDate : IComparer<Booking>
{
	private readonly bool _descending;
	public CompareByBookingEndDate(bool descending = false)
	{
		_descending = descending;
	}

	public int Compare(Booking? x, Booking? y)
	{
		int result;
		if (x == null && y == null)
			result = 0;
		if (y == null)
			result = 1;
		if (x == null)
			result = -1;
		result = x.EndDate.CompareTo(y.EndDate);
		return _descending ? -result : result;
	}
}