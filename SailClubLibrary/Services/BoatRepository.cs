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
    public class BoatRepository : Repository<string, Boat>, IBoatRepository
	{
		public BoatRepository() : base(new MockData().BoatData)
		{
		}
	}
}
