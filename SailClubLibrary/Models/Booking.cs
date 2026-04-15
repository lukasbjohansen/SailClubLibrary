using SailClubLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailClubLibrary.Models
{
    public class Booking : IRepositoryItem<int>
    {
        #region Properties

        [Required(ErrorMessage = "StartDate required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate required")]
        public DateTime EndDate { get; set; } 

        [Required(ErrorMessage = "Destination required")]
        [StringLength(30, ErrorMessage = "Destination must be at least 3 characters", MinimumLength = 3)]
        public string Destination { get; set; }

        public int Id { get; set; }
        public Member TheMember { get; set; }
        public Boat TheBoat { get; set; }
        public bool SailCompleted { get; set; }

        public int Key { get => Id; set => Id = value; }
        public bool IsActive
        {
            get
            {
                return StartDate <= DateTime.Now && DateTime.Now <= EndDate;
            }
        }
        #endregion

        #region Constructor
        public Booking(int id, DateTime startDate, DateTime endDate, string destination, Member member, Boat boat)
        {
            StartDate = startDate;
            EndDate = endDate;
            Destination = destination;
            Id = id;
            TheMember = member;
            TheBoat = boat;
        }
        public Booking()
        {

        }
        #endregion
        #region Methods
        public override string ToString()
        {
            return $"Id: {Id} " +
                $"\nStart Dato: {StartDate} " +
                $"\nSlut Dato: {EndDate} " +
                $"\nDestination: {Destination} " +
                $"\nBåden med sejlnummeret: {TheBoat.SailNumber}" +
                $"\nBooket af: {TheMember.FirstName}" +
                $"\nBåden er kommet i havn: {SailCompleted}";
        }
        #endregion
    }
}
