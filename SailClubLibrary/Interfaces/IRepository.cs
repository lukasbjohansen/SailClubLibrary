using SailClubLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailClubLibrary.Interfaces
{
	public interface IRepository<K, V> where V : IRepositoryItem<K>
	{
		#region Properties
		public int Count { get; }
		#endregion

		#region Methods
		List<V> GetAll();
		void Add(V item);
		void Remove(K key);
		void Update(V item);
		V? Search(K key);
		List<V> Filter(string filterCriteria);
		#endregion
	}
}
