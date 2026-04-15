using SailClubLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailClubLibrary.Data;
public static class MockData
{
    public static List<User> UserData
    { 
        get
        {
            return [new User("Poul", "123"), new User("Charlotte", "123")];
        } 
    }
}
