using SailClubLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailClubLibrary.Models
{
    /// <summary>
    /// Generic Class for Constructing Boat Objects using the interface
    /// </summary>
    public class Boat : IRepositoryItem<string>
    {
        #region Instance Fields

        #endregion

        #region Properties
        [Required(ErrorMessage = "Model required")]
        [StringLength(30, ErrorMessage = "Model must be at least 2 characters", MinimumLength = 2)]
        public string Model { get; set; }

        [Required(ErrorMessage = "Sailnumber required")]
        [StringLength(10, ErrorMessage = "SailNumber must be at least 2 characters", MinimumLength = 2)]
        public string SailNumber { get; set; }

        [Required(ErrorMessage = "Engine info required")]
        [StringLength(20, ErrorMessage = "EngineInfo must be at least 2 characters", MinimumLength = 2)]
        public string EngineInfo { get; set; }

        [Required(ErrorMessage = "Draft required")]
        public double Draft { get; set; }

        [Required(ErrorMessage = "Width required")]
        public double Width { get; set; }

        [Required(ErrorMessage = "Length required")]
        public double Length { get; set; }

        [Required(ErrorMessage = "YearOfConstruction required")]
        [StringLength(4, ErrorMessage = "Year of construction must be 4 characters", MinimumLength = 4)]
        public string YearOfConstruction { get; set; }

        public BoatType TheBoatType { get; set; }
        public int Id { get; set; }
        public string? BoatImage { get; set; }

        public string Key { get => SailNumber; set => SailNumber = value; }
        #endregion
        public Boat()
        {

        }

        #region Constructor
        public Boat(int id, BoatType boatType, string model, string sailNumber, string engineInfo,
            double draft, double width, double length, string yearOfConstruction, string boatImage)
        {
            Id = id;
            TheBoatType = boatType;
            Model = model;
            SailNumber = sailNumber;
            EngineInfo = engineInfo;
            Draft = draft;
            Width = width;
            Length = length;
            YearOfConstruction = yearOfConstruction;
            BoatImage = boatImage;
         }

        #endregion

        #region Methods
        /// <summary>
        /// Returns a writeline featuring the contents of the object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ($"\nBåd Nr.{Id}: " +
                $"\nBådinfo..." +
                $"\n{YearOfConstruction} {Model} {TheBoatType} {SailNumber} " +
                $"\nMotorinfo: {EngineInfo} " +
                $"\nDimensioner... " +
                $"\nDybgang: {Draft}, Bredde: {Width}, Længde: {Length}");
        }
        #endregion

    }
}
