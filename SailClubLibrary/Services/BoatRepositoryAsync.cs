using Microsoft.Data.SqlClient;
using SailClubLibrary.Exceptions;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SailClubLibrary.Services
{
	public class BoatRepositoryAsync : RepositoryAsync<string, Boat>, IBoatRepositoryAsync
	{
		public async Task<int> Count()
		{ 
			string _countSql = "SELECT COUNT(*) FROM Boat";
			await using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(_countSql, connection);
					await connection.OpenAsync();
					return (int) (await command.ExecuteScalarAsync() ?? 0);
				}
				catch (Exception ex)
				{
					ex.Print();
					return 0;
				}
			}
		}

		public async Task AddAsync(Boat boat)
		{
			string _addBoatSql = "INSERT INTO Boat (Model, SailNumber, EngineInfo, Draft, Width, BoatLength, YearOfConstruction, BoatType, BoatImage) " +
                                           "VALUES (@Model, @SailNumber, @EngineInfo, @Draft, @Width, @BoatLength, @YearOfConstruction, @BoatType, @BoatImage)";
			await using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					await connection.OpenAsync();
					SqlCommand command = new SqlCommand(_addBoatSql, connection);
					command.Parameters.AddWithValue("@Model", boat.Model);
					command.Parameters.AddWithValue("@SailNumber", boat.SailNumber);
					command.Parameters.AddWithValue("@EngineInfo", boat.EngineInfo);
					command.Parameters.AddWithValue("@Draft", boat.Draft);
					command.Parameters.AddWithValue("@Width", boat.Width);
					command.Parameters.AddWithValue("@BoatLength", boat.Length);
					command.Parameters.AddWithValue("@YearOfConstruction", boat.YearOfConstruction);
					command.Parameters.AddWithValue("@BoatType", boat.TheBoatType);
                    command.Parameters.AddWithValue("@BoatImage", (object)boat.BoatImage ?? DBNull.Value);
                    await command.ExecuteNonQueryAsync();
				}
				catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
				{
					throw new BoatSailnumberExistsException("Database rejected duplicate SailNumber.");
				}
				catch (Exception ex)
				{
					ex.Print();
				}
			}
		}
		public override async Task<List<Boat>> GetAllAsync()
		{
			List<Boat> boats = new List<Boat>();
			string _getAllBoatsSql = "SELECT * From Boat";
			await using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(_getAllBoatsSql, connection);
					await command.Connection.OpenAsync();
					await using SqlDataReader reader = await command.ExecuteReaderAsync();
					while (await reader.ReadAsync())
					{
						boats.Add(MapBoatFromReader(reader));
					}
				}
				catch (Exception ex)
				{
					ex.Print();
				}
				return boats;
			}
		}
		public async Task<List<Boat>> FilterAsync(string filterCriteria)
		{
			List<Boat> boats = new List<Boat>();
			string _filterSql = @"SELECT * FROM Boat 
               WHERE CAST(Model AS NVARCHAR(MAX)) LIKE @Criteria 
               OR CAST(BoatId AS NVARCHAR(MAX)) LIKE @Criteria 
               OR CAST(SailNumber AS NVARCHAR(MAX)) LIKE @Criteria 
               OR CAST(EngineInfo AS NVARCHAR(MAX)) LIKE @Criteria
               OR CAST(Draft AS NVARCHAR(MAX)) LIKE @Criteria
               OR CAST(Width AS NVARCHAR(MAX)) LIKE @Criteria
               OR CAST(BoatLength AS NVARCHAR(MAX)) LIKE @Criteria
               OR CAST(YearOfConstruction AS NVARCHAR(MAX)) LIKE @Criteria
               OR CAST(BoatType AS NVARCHAR(MAX)) LIKE @Criteria";
			await using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(_filterSql, connection);
					await connection.OpenAsync();
					command.Parameters.AddWithValue("@Criteria", $"%{filterCriteria}%");
					await using SqlDataReader reader = await command.ExecuteReaderAsync();
					while (await reader.ReadAsync())
					{
						boats.Add(MapBoatFromReader(reader));
					}
				}
				catch (Exception ex)
				{
					ex.Print();
				}
			}
			return boats;
		}
		public async Task RemoveAsync(string sailNumber)
		{
			string sql = $"DELETE FROM Boat WHERE SailNumber = @SailNumber";
			await using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(sql, connection);
					await connection.OpenAsync();
					command.Parameters.AddWithValue("@SailNumber", sailNumber);
					await command.ExecuteNonQueryAsync();
				}
				catch (Exception ex)
				{
					ex.Print();
				}
			}
		}
		public async Task<Boat?> SearchAsync(string sailNumber)
		{
			string sql = $"SELECT * FROM Boat WHERE SailNumber = @SailNumber";
			await using(SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(sql,connection);
					await connection.OpenAsync();
					command.Parameters.AddWithValue("@SailNumber", sailNumber);
					await using SqlDataReader reader = await command.ExecuteReaderAsync();
					if (await reader.ReadAsync())
					{
						return MapBoatFromReader(reader);
					}
				}
				catch(Exception ex)
				{
					ex.Print();
				}
				return null;
			}
		}
		public async Task<Boat?> SearchAsync(int id)
		{
			string sql = $"SELECT * FROM Boat WHERE BoatId = @Id";
			await using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(sql,connection);
					await connection.OpenAsync();
					command.Parameters.AddWithValue("@Id", id);
					await using SqlDataReader reader = await command.ExecuteReaderAsync();
					if (await reader.ReadAsync())
					{
						return MapBoatFromReader(reader);
					}
				}
				catch (Exception ex)
				{
					ex.Print();
				}
				return null;
			}
		}
		public async Task UpdateAsync(Boat boat)
		{
			string sql = @"UPDATE Boat 
               SET BoatType = @TheBoatType, 
                   Model = @Model, 
                   EngineInfo = @EngineInfo, 
                   Draft = @Draft, 
                   Width = @Width, 
                   BoatLength = @Length, 
                   YearOfConstruction = @YearOfConstruction,
                   BoatImage = @BoatImage
               WHERE SailNumber = @SailNumber";
			await using(SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					await connection.OpenAsync();
					SqlCommand command = new SqlCommand(sql, connection);
					command.Parameters.AddWithValue("@TheBoatType", boat.TheBoatType);
					command.Parameters.AddWithValue("@Model", boat.Model);
					command.Parameters.AddWithValue("@EngineInfo", boat.EngineInfo);
					command.Parameters.AddWithValue("@Draft", boat.Draft);
					command.Parameters.AddWithValue("@Width", boat.Width);
					command.Parameters.AddWithValue("@Length", boat.Length);
					command.Parameters.AddWithValue("@YearOfConstruction", boat.YearOfConstruction);
                    command.Parameters.AddWithValue("@BoatImage", boat.BoatImage);
                    command.Parameters.AddWithValue("@SailNumber", boat.SailNumber);
					await command.ExecuteNonQueryAsync();
				}
				catch (Exception ex)
				{
					ex.Print();
				}
			}
		}
		private Boat MapBoatFromReader(SqlDataReader reader)
		{
			return new Boat(
				reader.GetInt32("BoatId"),
				(BoatType) reader.GetInt32("BoatType"),
				reader.GetString("Model"),
				reader.GetString("SailNumber"),
				reader.GetString("EngineInfo"),
				reader.GetDouble("Draft"),
				reader.GetDouble("Width"),
				reader.GetDouble("BoatLength"),
				reader.GetString("YearOfConstruction"),
                reader.IsDBNull(reader.GetOrdinal("BoatImage")) ? null : reader.GetString("BoatImage")
            );
		}
	}
}
