using SailClubLibrary.Data;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailClubLibrary.Services;
public class UserService : IUserService
{
    public List<User> GetAllUsers()
    {
        return MockData.UserData;
    }
    public User VerifyUser(string username, string password)
    {
        foreach(var user in GetAllUsers())
        {
            if (username.Equals(user.Username) && password.Equals(user.Password))
            {
                return user;
            }
        }
        return null;
    }
}
