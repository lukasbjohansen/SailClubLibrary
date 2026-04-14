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
	public class MemberRepositoryAsync : RepositoryAsync<string, Member>, IMemberRepositoryAsync
	{
		public async Task<int> Count()
		{
			string _countSql = "SELECT COUNT(*) FROM Member";
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
		public async Task RemoveAsync(string phoneNo)
		{
			string sql = $"DELETE FROM SailClubMember b WHERE b.PhoneNumber = @PhoneNo";
			await using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(sql, connection);
					await connection.OpenAsync();
					command.Parameters.AddWithValue("@PhoneNo", phoneNo);
					await command.ExecuteNonQueryAsync();
				}
				catch (Exception ex)
				{
					ex.Print();
				}
			}
		}
        public async Task<Member?> SearchAsync(string phoneNo)
        {
            string sql = $"SELECT * FROM SailClubMember b WHERE b.PhoneNumber = @PhoneNo";
            await using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@PhoneNo", phoneNo);
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
        public async Task<Member?> SearchAsync(int memberId)
        {
            string sql = $"SELECT * FROM SailClubMember b WHERE b.MemberId = @MemberId";
            await using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@MemberId", memberId);
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
					command.Parameters.AddWithValue("@MemberImage", member.MemberImage);
                    command.Parameters.AddWithValue("@PhoneNumber", member.PhoneNumber);
                    await command.ExecuteNonQueryAsync();
				}
				catch (Exception ex)
				{
					ex.Print();
				}
			}
		}
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
