using SailClubLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailClubLibrary.Interfaces
{
	public interface IRepositoryAsync<K, V> where V : IRepositoryItem<K>
	{
		#region Properties
		public Task<int> Count();
		#endregion

		#region Methods
		Task<List<V>> GetAllAsync();
		Task AddAsync(V item);
		Task RemoveAsync(K key);
		Task UpdateAsync(V item);
		Task<V?> SearchAsync(K key);
		Task<V?> SearchAsync(int id);
		Task<List<V>> FilterAsync(string filterCriteria);
		Task<int> SearchLowestNotTakenIdAsync();
        #endregion
    }
}
