using SailClubLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailClubLibrary.Helpers.Sorting;
public class CompareById : IComparer<IIdAble>
{
	public int Compare(IIdAble? x, IIdAble? y)
	{
		if (x == null && y == null) return 0;
		if (x == null) return -1;
		if (y == null) return 1;
		return x.Id.CompareTo(y.Id);
	}
}
