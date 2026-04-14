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
	public class BookingRepositoryAsync : RepositoryAsync<int, Booking>, IBookingRepositoryAsync
	{
		private IMemberRepositoryAsync _memberRepositoryAsync;
		private IBoatRepositoryAsync _boatRepositoryAsync;
		public BookingRepositoryAsync(IMemberRepositoryAsync memberRepositoryAsync, IBoatRepositoryAsync boatRepositoryAsync)
		{
			_memberRepositoryAsync = memberRepositoryAsync;
			_boatRepositoryAsync = boatRepositoryAsync;
		}
		public async Task<int> Count()
		{
			string _countSql = "SELECT COUNT(*) FROM Booking";
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

		public async Task AddAsync(Booking booking)
		{
			string _addBookingSql = "INSERT INTO Booking (StartDate, EndDate, SailCompleted, Destination, MemberId, BoatId) " +
										   "VALUES (@StartDate, @EndDate, @SailCompleted, @Destination, @MemberId, @BoatId)";
			await using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					await connection.OpenAsync();
					SqlCommand command = new SqlCommand(_addBookingSql, connection);
					command.Parameters.AddWithValue("@StartDate", booking.StartDate);
					command.Parameters.AddWithValue("@EndDate", booking.EndDate);
					command.Parameters.AddWithValue("@SailCompleted", booking.SailCompleted);
					command.Parameters.AddWithValue("@Destination", booking.Destination);
					command.Parameters.AddWithValue("@MemberId", booking.TheMember.Id);
					command.Parameters.AddWithValue("@BoatId", booking.TheBoat.Id);
					await command.ExecuteNonQueryAsync();
				}
				catch (Exception ex)
				{
					ex.Print();
				}
			}
		}
		public override async Task<List<Booking>> GetAllAsync()
		{
			List<Booking> bookings = new List<Booking>();
			string _getAllBookingsSql = "SELECT * From Booking";
			await using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(_getAllBookingsSql, connection);
					await command.Connection.OpenAsync();
					await using SqlDataReader reader = await command.ExecuteReaderAsync();
					while (await reader.ReadAsync())
					{
						bookings.Add(await MapBookingFromReader(reader));
					}
				}
				catch (Exception ex)
				{
					ex.Print();
				}
				return bookings;
			}
		}
		public async Task<List<Booking>> FilterAsync(string filterCriteria)
		{
			List<Booking> bookings = new List<Booking>();
			string _filterSql = @"SELECT * FROM Booking 
               WHERE CAST(StartDate AS NVARCHAR(MAX)) LIKE @Criteria 
               OR CAST(BookingId AS NVARCHAR(MAX)) LIKE @Criteria 
               OR CAST(EndDate AS NVARCHAR(MAX)) LIKE @Criteria 
               OR CAST(SailCompleted AS NVARCHAR(MAX)) LIKE @Criteria
               OR CAST(Destination AS NVARCHAR(MAX)) LIKE @Criteria
               OR CAST(MemberId AS NVARCHAR(MAX)) LIKE @Criteria
               OR CAST(BoatId AS NVARCHAR(MAX)) LIKE @Criteria";
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
						bookings.Add(await MapBookingFromReader(reader));
					}
				}
				catch (Exception ex)
				{
					ex.Print();
				}
			}
			return bookings;
		}
		public async Task RemoveAsync(int bookingId)
		{
			string sql = $"DELETE FROM Booking b WHERE b.BookingId = @BookingId";
			await using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(sql, connection);
					await connection.OpenAsync();
					command.Parameters.AddWithValue("@BookingId", bookingId);
					await command.ExecuteNonQueryAsync();
				}
				catch (Exception ex)
				{
					ex.Print();
				}
			}
		}
		public async Task<Booking?> SearchAsync(int bookingId)
		{
			string sql = $"SELECT * FROM Booking b WHERE b.BookingId = @BookingId";
			await using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(sql,connection);
					await connection.OpenAsync();
					command.Parameters.AddWithValue("@BookingId", bookingId);
					await using SqlDataReader reader = await command.ExecuteReaderAsync();
					if (await reader.ReadAsync())
					{
						return await MapBookingFromReader(reader);
					}
				}
				catch (Exception ex)
				{
					ex.Print();
				}
				return null;
			}
		}
		public async Task UpdateAsync(Booking booking)
		{
			string sql = @"UPDATE Booking 
               SET StartDate = @StartDate, 
				   EndDate = @EndDate,
                   SailCompleted = @SailCompleted, 
                   Destination = @Destination, 
                   MemberId = @MemberId, 
                   BoatId = @BoatId
               WHERE BookingId = @BookingId";
			await using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					await connection.OpenAsync();
					SqlCommand command = new SqlCommand(sql, connection);
					command.Parameters.AddWithValue("@StartDate", booking.StartDate);
					command.Parameters.AddWithValue("@EndDate", booking.EndDate);
					command.Parameters.AddWithValue("@SailCompleted", booking.SailCompleted);
					command.Parameters.AddWithValue("@Destination", booking.Destination);
					command.Parameters.AddWithValue("@MemberId", booking.TheMember.Id);
					command.Parameters.AddWithValue("@BoatId", booking.TheBoat.Id);
					command.Parameters.AddWithValue("@BookingId", booking.Id);
					await command.ExecuteNonQueryAsync();
				}
				catch (Exception ex)
				{
					ex.Print();
				}
			}
		}
		private async Task<Booking> MapBookingFromReader(SqlDataReader reader)
		{
			Member? member = await _memberRepositoryAsync.SearchAsync(reader.GetInt32("MemberId"));
			Boat? boat = await _boatRepositoryAsync.SearchAsync(reader.GetInt32("BoatId"));
			return new Booking(
				reader.GetInt32("BookingId"),
				reader.GetDateTime("StartDate"),
				reader.GetDateTime("EndDate"),
				reader.GetString("Destination"),
				member,
				boat
			);
		}
	}
}
