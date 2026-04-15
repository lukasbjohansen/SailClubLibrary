using SailClubLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailClubLibrary.Models
{
    public class Member : IRepositoryItem<string>
    {
        #region Instance Fields
        #endregion

        #region Properties
        [Required(ErrorMessage = "Firstname required")]
        [StringLength(30, ErrorMessage = "Firstname must be at least 2 characters", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Surname required")]
        [StringLength(20, ErrorMessage = "Surname must be at least 2 characters", MinimumLength = 2)]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Phonenumber required")]
        [RegularExpression(@"^(\+\d{2})?\s?[2-9]\d\s?\d{2}\s?\d{2}\s?\d{2}$", ErrorMessage = "Phonenumber must be a valid format")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address required")]
        [StringLength(50, ErrorMessage = "Address must be at least 5 characters", MinimumLength = 5)]
        public string Address { get; set; }

        [Required(ErrorMessage = "City required")]
        [StringLength(30, ErrorMessage = "City must be at least 3 characters", MinimumLength = 3)]
        public string City { get; set; }

        [EmailAddress(ErrorMessage = "Invalid mail")]
        [StringLength(100, ErrorMessage = "Mail must be at least 3 characters", MinimumLength = 3)]
        public string Mail { get; set; }

        public MemberType TheMemberType { get; set; }
        public MemberRole TheMemberRole { get; set; }
        public int Id { get; set; }
        public string? MemberImage { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + SurName;
            }
        }
        public string Key { get => PhoneNumber; set => PhoneNumber = value; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor used for creating new member objects
        /// </summary>
        /// 

        public Member()
        {

        }
        public Member(int id, string name, string surName, string phoneNumber, string address, string city, string mail, MemberType theMemberType, MemberRole theMemberRole, string image)
        {
            FirstName = name;
            SurName = surName;
            PhoneNumber = phoneNumber;
            Address = address;
            City = city;
            Mail = mail;
            TheMemberType = theMemberType;
            TheMemberRole = theMemberRole;
            Id = id;
            MemberImage = image;
        }

        #endregion
        #region Methods
        /// <summary>
        /// ToString method used for printing out member information
        /// </summary>
        public override string ToString()
        {
            return $"Medlemsnummer: {Id}\nFornavn: {FirstName}\nEfternavn: {SurName}\nTelefonnummer: {PhoneNumber}\n" +
                $"Adresse: {Address}\nBy: {City}\nEmail: {Mail}\nType: {TheMemberType}\n" +
                $"Rolle: {TheMemberRole}";
        }
        #endregion 
    }
}
