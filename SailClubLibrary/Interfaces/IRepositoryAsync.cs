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
		public Task<int> Count { get; }
		#endregion

		#region Methods
		Task<List<V>> GetAll();
		Task Add(V item);
		Task Remove(K key);
		Task Update(V item);
		Task<V?> Search(K key);
		Task<List<V>> Filter(string filterCriteria);
		Task<int> SearchLowestNotTakenId();
        #endregion
    }
}
