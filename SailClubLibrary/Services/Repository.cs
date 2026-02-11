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
	public class Repository<K, V> : IRepository<K,V> where V : IRepositoryItem<K>
	{
		#region Instance Field
		protected Dictionary<K, V> _dictionary;

		public Repository(Dictionary<K, V> dictionary)
		{
			_dictionary = dictionary;
		}
		#endregion

		#region Properties
		public int Count { get { return _dictionary.Count; } }
		#endregion

		#region Methods
		public void Add(V item)
		{
			if (!_dictionary.ContainsKey(item.Key))
			{
				_dictionary[item.Key] = item;
				Console.WriteLine($"The item with key: {item.Key} was added to the list");
				return;
			}
			throw new BoatSailnumberExistsException($"Item with key: {item.Key} already exists");
		}
		public List<V> GetAll()
		{
			return _dictionary.Values.ToList();
		}
		public void Remove(K key)
		{
			_dictionary.Remove(key);
			Console.WriteLine($"The item with key: {key} was removed.");
		}
		public void Update(V item)
		{
			if (_dictionary.ContainsKey(item.Key))
			{
				_dictionary[item.Key] = item;
			}
		}
		public V? Search(K key)
		{
			if (_dictionary.ContainsKey(key))
			{
				return _dictionary[key];
			}
			return default;
		}
		public void PrintAll()
		{
			foreach (V item in _dictionary.Values)
			{
				Console.WriteLine(item.ToString());
			}
			Console.WriteLine();
		}
		public List<V> Filter(string filterCriteria)
		{
			List<V> items = new List<V>();
			foreach (V item in _dictionary.Values)
			{
				if (item.ToString().ToLower().Contains(filterCriteria.ToLower()))
					items.Add(item);
			}
			return items;
		}
		#endregion
	}
}
