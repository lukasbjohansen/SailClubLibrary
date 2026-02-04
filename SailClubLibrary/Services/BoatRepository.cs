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
    /// <summary>
    /// Class for Constructing and calling Boat Repository Objects using the interface
    /// </summary>
    public class BoatRepository : IBoatRepository
    {
        #region Instance Field
        private Dictionary<string, Boat> _boats;
        #endregion

        #region Properties
        public int Count { get { return _boats.Count; } }
        #endregion  

        #region Constructor
        public BoatRepository()
        {
            //_boats = [];
            _boats = MockData.BoatData;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a Boat Object to the Dictionary. 
        /// </summary>
        public void AddBoat(Boat boat)
        {
            if (!_boats.ContainsKey(boat.SailNumber))
            {
                _boats[boat.SailNumber] = boat;
                Console.WriteLine($"Båden med sejlnummeret {boat.SailNumber} er blevet tilføjet til listen");
                return;
            }
            throw new BoatSailnumberExistsException($"Båden med sejlnummeret {boat.SailNumber} findes allerede.");
        }

        /// <summary>
        /// Collects all the Boats Objects in the Dictionary and files them into a list
        /// </summary>
        public List<Boat> GetAllBoats()
        {
            return _boats.Values.ToList();
        }

        /// <summary>
        /// Removes a Boat Object from the Dictionary
        /// </summary>
        public void RemoveBoat(string sailNumber)
        {
            _boats.Remove(sailNumber);
            Console.WriteLine($"Båden med sejlnummer {sailNumber} er blevet fjernet.");
        }

        /// <summary>
        /// Updates the info of a Boat Object found by parameter with input info
        /// </summary>
        public void UpdateBoat(Boat updatedBoat)
        {
            if (_boats.ContainsKey(updatedBoat.SailNumber))
            {
                Boat existingBoat = _boats[updatedBoat.SailNumber];

                existingBoat.TheBoatType = updatedBoat.TheBoatType;
                existingBoat.Model = updatedBoat.Model;
                existingBoat.EngineInfo = updatedBoat.EngineInfo;
                existingBoat.Draft = updatedBoat.Draft;
                existingBoat.Width = updatedBoat.Width;
                existingBoat.Length = updatedBoat.Length;
                existingBoat.YearOfConstruction = updatedBoat.YearOfConstruction;
            }
        }

        /// <summary>
        /// Searches through the boat dictionary and returns the boat with the given sailnumber. 
        /// </summary>
        public Boat? SearchBoat(string sailNumber)
        {
            if (_boats.ContainsKey(sailNumber))
            {
                return _boats[sailNumber];
            }
            return null;
        }

        /// <summary>
        /// Runs through the list and calls the toString() method of every index
        /// </summary>
        public void PrintAllBoats()
        {
            foreach (var boat in _boats)
            {
                Console.WriteLine(boat.ToString());
            }
            Console.WriteLine();
        }
		public int SearchLowestNotTakenIdBoolArray() // O(N)
		{
			int count = _boats.Count;
			if (count == 0)
				throw new Exception();

			bool[] present = new bool[count + 1];

			foreach (var boat in _boats.Values)
			{
				if (boat.Id >= 0 && boat.Id < count)
				{
					present[boat.Id] = true;
				}
			}

			for (int i = 0; i <= count; i++)
			{
				if (!present[i])
					return i;
			}

			return count;
		}
		public int SearchLowestNotTakenId() // O(N)
		{
            int count = _boats.Count;
			if (count == 0)
				throw new Exception();
            HashSet<int> ids = _boats.Values.Select(b => b.Id).ToHashSet();
            for (int i = 0; i < count; i++)
            {
                if (!ids.Contains(i))
                    return i;
            }
            return Count;
		}
		public int SearchLowestNotTakenIdNPlusSort() // O(Nlog(N))
		{
            if (_boats.Count == 0)
                throw new Exception();
			List<Boat> boats = _boats.Values.ToList();
			List<int> ids = boats.Select(b => b.Id).ToList();
            ids.Sort();

            int lowestAvailable = 0;
            foreach(int i in ids)
            {
                if (lowestAvailable == i)
                {
                    lowestAvailable++;
                }
                else
                {
                    return lowestAvailable;
                } 
            }
            return lowestAvailable;
		}
		public int SearchLowestNotTakenIdN2() // O(N^2)
		{
			List<Boat> boats = _boats.Values.ToList();

			int lowestId = 0;
			bool found = false;
			while (!found)
			{
				found = true;
				foreach (var boat in boats)
				{
					if (boat.Id == lowestId)
					{
						lowestId++;
						found = false;
						boats.Remove(boat);
						break;
					}
				}
			}
			return lowestId;
		}

		public List<Boat> FilterBoats(string filterCriteria)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
