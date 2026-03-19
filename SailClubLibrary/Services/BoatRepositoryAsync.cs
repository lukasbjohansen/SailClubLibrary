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
    public class BoatRepositoryAsync : RepositoryAsync<string, Boat>, IBoatRepositoryAsync
	{
		public BoatRepositoryAsync()
		{
			_tableName = "Boat";
			_values = ["@ID", "@Model", "@SailNumber", "@EngineInfo", "@Draft", "@Width", "@BoatLength", "@YearOfConstruction", "@BoatType"];
		}
	}
}
