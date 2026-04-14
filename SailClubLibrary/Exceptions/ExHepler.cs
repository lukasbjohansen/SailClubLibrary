using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailClubLibrary.Exceptions;
public static class ExHepler
{
	public static void Print(this Exception ex)
	{
		Console.WriteLine(ex.GetType() + ": " + ex.Message);
	}
}
