using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

public class CompareByBookingBoat : IComparer<Booking>
{
	private readonly bool _descending;
	public CompareByBookingBoat(bool descending = false)
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
		result = x.TheBoat.SailNumber.CompareTo(y.TheBoat.SailNumber);
		return _descending ? -result : result;
	}
}