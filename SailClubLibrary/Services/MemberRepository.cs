using SailClubLibrary.Data;
using SailClubLibrary.Exceptions;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailClubLibrary.Services
{
	public class MemberRepository : Repository<string, Member>, IMemberRepository
	{
		public MemberRepository() : base(new MockData().MemberData)
		{
		}
		public int SearchLowestNotTakenId()
		{
			int count = _dictionary.Count;
			if (count == 0)
				throw new Exception();
			HashSet<int> ids = _dictionary.Values.Select(b => b.Id).ToHashSet();
			for (int i = 0; i < count; i++)
			{
				if (!ids.Contains(i))
					return i;
			}
			return Count;
		}
	}
}
