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
    /// <summary>
    /// Repository for CRUD of members via ADO.NET to MS SQL server. Every operation is performed asynchronous.
    /// </summary>
    public class MemberRepositoryAsync : RepositoryAsync<string, Member>, IMemberRepositoryAsync
	{
        /// <summary>
        /// Gets the count of members from the database asynchronous.
        /// </summary>
        /// <returns>The amount of <see cref="Member"/> in the database.</returns>
        public async Task<int> Count()
		{
			string _countSql = "SELECT COUNT(*) FROM SailClubMember";
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
        /// <summary>
        /// Adds a new <see cref="Member"/> to the database.
        /// </summary>
        /// <param name="member">The new <see cref="Member"/> to add to database.</param>
        /// <returns>A <see cref="Task"/> since the method is asynchronous.</returns>
        /// <exception cref="ArgumentException">Thrown if the new <see cref="Member"/> shares <see cref="Member.PhoneNumber"/> with an existing member.</exception>
        public async Task AddAsync(Member member)
		{
			string _addMemberSql = "INSERT INTO SailClubMember (FirstName, SurName, PhoneNumber, MemberAddress, City, Mail, MemberType, MemberRole, MemberImage) " +
                                           "VALUES (@FirstName, @SurName, @PhoneNumber, @MemberAddress, @City, @Mail, @MemberType, @MemberRole, @MemberImage)";
			await using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					await connection.OpenAsync();
					SqlCommand command = new SqlCommand(_addMemberSql, connection);
					command.Parameters.AddWithValue("@FirstName", member.FirstName);
					command.Parameters.AddWithValue("@SurName", member.SurName);
					command.Parameters.AddWithValue("@PhoneNumber", member.PhoneNumber);
					command.Parameters.AddWithValue("@MemberAddress", member.Address);
					command.Parameters.AddWithValue("@City", member.City);
					command.Parameters.AddWithValue("@Mail", member.Mail);
					command.Parameters.AddWithValue("@MemberType", member.TheMemberType);
					command.Parameters.AddWithValue("@MemberRole", member.TheMemberRole); 
					command.Parameters.AddWithValue("@MemberImage", (object)member.MemberImage ?? DBNull.Value);
                    await command.ExecuteNonQueryAsync();
				}
				catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
				{
					throw new ArgumentException("Database rejected duplicate PhoneNumber.");
				}
				catch (Exception ex)
				{
					ex.Print();
				}
			}
		}
		/// <summary>
		/// Fetches all <see cref="Member"/> from the database.
		/// </summary>
		/// <returns>A <see cref="List{T}"/> of <see cref="Member"/> containing all members from the database.</returns>
		public override async Task<List<Member>> GetAllAsync()
		{
			List<Member> members = new List<Member>();
			string _getAllMembersSql = "SELECT * From SailClubMember";
			await using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(_getAllMembersSql, connection);
					await command.Connection.OpenAsync();
					await using SqlDataReader reader = await command.ExecuteReaderAsync();
					while (await reader.ReadAsync())
					{
						members.Add(await MapMemberFromReader(reader));
					}
				}
				catch (Exception ex)
				{
					ex.Print();
				}
				return members;
			}
		}
		/// <summary>
		/// Fetches all members from the database that matches the <paramref name="filterCriteria"/> in any attribute.
		/// </summary>
		/// <param name="filterCriteria">A string used to filter <see cref="Member"/> only with at least one attribute containing it.</param>
		/// <returns>A <see cref="List{T}"/> of <see cref="Member"/> containing all <see cref="Member"/> that match the <paramref name="filterCriteria"/>.</returns>
		public async Task<List<Member>> FilterAsync(string filterCriteria)
		{
			List<Member> members = new List<Member>();
			string _filterSql = @"SELECT * FROM SailClubMember 
               WHERE CAST(FirstName AS NVARCHAR(MAX)) LIKE @Criteria 
               OR CAST(SurName AS NVARCHAR(MAX)) LIKE @Criteria 
               OR CAST(PhoneNumber AS NVARCHAR(MAX)) LIKE @Criteria
               OR CAST(MemberAddress AS NVARCHAR(MAX)) LIKE @Criteria
               OR CAST(City AS NVARCHAR(MAX)) LIKE @Criteria
               OR CAST(Mail AS NVARCHAR(MAX)) LIKE @Criteria
               OR CAST(MemberType AS NVARCHAR(MAX)) LIKE @Criteria
               OR CAST(MemberRole AS NVARCHAR(MAX)) LIKE @Criteria";
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
						members.Add(await MapMemberFromReader(reader));
					}
				}
				catch (Exception ex)
				{
					ex.Print();
				}
			}
			return members;
		}
        /// <summary>
        /// Removes a member from the database with the specific <paramref name="phoneNumber"/>.
        /// </summary>
        /// <param name="phoneNumber">The phonenumber of the <see cref="Member"/> that should be removed.</param>
        /// <returns>A <see cref="Task"/> since the method is asynchronous.</returns>
        public async Task RemoveAsync(string phoneNumber)
		{
			string sql = $"DELETE FROM SailClubMember WHERE PhoneNumber = @PhoneNo";
			await using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(sql, connection);
					await connection.OpenAsync();
					command.Parameters.AddWithValue("@PhoneNo", phoneNumber);
					await command.ExecuteNonQueryAsync();
				}
				catch (Exception ex)
				{
					ex.Print();
				}
			}
		}
		/// <summary>
		/// Fetches a specific member matched by the given <paramref name="phoneNumber"/>.
		/// </summary>
		/// <param name="phoneNumber">The phonenumber of the <see cref="Member"/> to search for.</param>
		/// <returns>A <see cref="Member"/> with <paramref name="phoneNumber"/> as their phonenumber or <see langword="null"/> if no <see cref="Member"/> exists with that phonenumber.</returns>
        public async Task<Member?> SearchAsync(string phoneNumber)
        {
            string sql = $"SELECT * FROM SailClubMember WHERE PhoneNumber = @PhoneNo";
            await using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@PhoneNo", phoneNumber);
                    await using SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        return await MapMemberFromReader(reader);
                    }
                }
                catch (Exception ex)
                {
                    ex.Print();
                }
                return null;
            }
        }
        /// <summary>
        /// Fetches a specific member matched by the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the <see cref="Member"/> to search for.</param>
        /// <returns>A <see cref="Member"/> with <paramref name="id"/> as their id or <see langword="null"/> if no <see cref="Member"/> exists with that id.</returns>
        public async Task<Member?> SearchAsync(int id)
        {
            string sql = $"SELECT * FROM SailClubMember WHERE MemberId = @MemberId";
            await using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@MemberId", id);
                    await using SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        return await MapMemberFromReader(reader);
                    }
                }
                catch (Exception ex)
                {
                    ex.Print();
                }
                return null;
            }
        }
        /// <summary>
        /// Updates a <see cref="Member"/> with same phonenumber as <paramref name="member"/> in the database with the values of <paramref name="member"/>. The old <see cref="Member"/> retains their original id.
        /// </summary>
        /// <param name="member">The new <see cref="Member"/> object.</param>
        /// <returns>A <see cref="Task"/> since the method is asynchronous.</returns>
        public async Task UpdateAsync(Member member)
		{
			string sql = @"UPDATE SailClubMember 
               SET FirstName = @FirstName, 
				   SurName = @SurName,
                   MemberAddress = @MemberAddress, 
                   City = @City, 
                   Mail = @Mail,
                   MemberType = @MemberType,
                   MemberRole = @MemberRole,
                   MemberImage = @MemberImage
               WHERE PhoneNumber = @PhoneNumber";
			await using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					await connection.OpenAsync();
					SqlCommand command = new SqlCommand(sql, connection);
					command.Parameters.AddWithValue("@FirstName", member.FirstName);
					command.Parameters.AddWithValue("@SurName", member.SurName);
					command.Parameters.AddWithValue("@MemberAddress", member.Address);
					command.Parameters.AddWithValue("@City", member.City);
					command.Parameters.AddWithValue("@Mail", member.Mail);
					command.Parameters.AddWithValue("@MemberType", member.TheMemberType);
					command.Parameters.AddWithValue("@MemberRole", member.TheMemberRole); 
					command.Parameters.AddWithValue("@MemberImage", (object)member.MemberImage ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PhoneNumber", member.PhoneNumber);
                    await command.ExecuteNonQueryAsync();
				}
				catch (Exception ex)
				{
					ex.Print();
				}
			}
		}
		/// <summary>
		/// A private helper method for fetching a <see cref="Member"/> object from a <paramref name="reader"/>. Used by various methods in this class to enforce the DRY principle.
		/// </summary>
		/// <param name="reader">The reader containing a <see cref="Member"/></param>
		/// <returns>A new <see cref="Member"/> object with info taken from the <paramref name="reader"/></returns>
		private async Task<Member> MapMemberFromReader(SqlDataReader reader)
		{
            return new Member(
				reader.GetInt32("MemberId"),
				reader.GetString("FirstName"),
				reader.GetString("SurName"),
				reader.GetString("PhoneNumber"),
				reader.GetString("MemberAddress"),
				reader.GetString("City"),
				reader.GetString("Mail"),
				(MemberType) reader.GetInt32("MemberType"),
				(MemberRole) reader.GetInt32("MemberRole"),
                reader.IsDBNull(reader.GetOrdinal("MemberImage")) ? null : reader.GetString("MemberImage")
			);
		}
	}
}
