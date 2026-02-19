using SailClubLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailClubLibrary.Interfaces
{
    /// <summary>
    /// Interface for the BookingRepository class
    /// </summary>
    public interface IBookingRepository
    {
        int Count { get; }
		List<Booking> GetAll();
		void Add(Booking item);
        void Remove(int key);
        void Update(Booking item);
		Booking? Search(int key);
		List<Booking> Filter(string filterCriteria);
		int SearchLowestNotTakenId();
	}
}
