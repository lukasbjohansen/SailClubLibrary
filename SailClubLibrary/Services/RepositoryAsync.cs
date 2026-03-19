using Microsoft.Data.SqlClient;
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
	public class RepositoryAsync<K, V> : Connection, IRepositoryAsync<K, V> where V : IRepositoryItem<K>
	{
		protected string _tableName;
		protected List<string> _values;
		public Task<int> Count => throw new NotImplementedException();

		private string ValuesToString()
		{
			string result = "Values(";
			foreach(var item in _values)
			{
				result += item;
				result += ", ";
			}
			result = result.Substring(0, result.Length - 2);
			result += ")";
			return result;
		}
		public async Task Add(V item)
		{
			string sql = $"INSERT INTO {_tableName} {ValuesToString()}";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					await connection.OpenAsync();
					SqlCommand command = new SqlCommand(sql, connection);
					command.Parameters.AddWithValue(_values[0], item.)
				}
			}
		}

		public Task<List<V>> Filter(string filterCriteria)
		{
			throw new NotImplementedException();
		}

		public Task<List<V>> GetAll()
		{
			throw new NotImplementedException();
		}

		public Task Remove(K key)
		{
			throw new NotImplementedException();
		}

		public Task<V?> Search(K key)
		{
			throw new NotImplementedException();
		}

		public Task<int> SearchLowestNotTakenId()
		{
			throw new NotImplementedException();
		}

		public Task Update(V item)
		{
			throw new NotImplementedException();
		}
	}
}
