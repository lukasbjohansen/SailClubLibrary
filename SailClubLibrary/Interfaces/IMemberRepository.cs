using SailClubLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailClubLibrary.Interfaces
{
	public interface IMemberRepository
	{
		#region Properties
		public int Count { get; }
		#endregion

		#region Methods
		List<Member> GetAll();
		void Add(Member item);
		void Remove(string key);
		void Update(Member item);
		Member? Search(string key);
		List<Member> Filter(string filterCriteria);
		int SearchLowestNotTakenId();
		#endregion
	}
}
