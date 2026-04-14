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
	public abstract class RepositoryAsync<K, V> : Connection
	{
		public abstract Task<List<V>> GetAllAsync();
		public virtual async Task<int> SearchLowestNotTakenIdAsync()
		{
			List<V> values = await GetAllAsync();
			int count = values.Count();
			if (count == 0)
				return 0;
			HashSet<int> ids = values.Select(b => b.Id).ToHashSet();
			for (int i = 0; i < count; i++)
			{
				if (!ids.Contains(i))
					return i;
			}
			return count;
		}
	}
}
