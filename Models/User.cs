using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;



namespace Project.Models
{
    public class User : IdentityUser
    {
        
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int32 Id { get; set; }

        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }


        [DataType(DataType.ImageUrl)]
        public string UserImage { get; set; } = String.Empty;

        public ICollection<Book>? Books { get; set; }
        

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public string BirthDateAsString
        {
            get
            {
                return BirthDate.ToString("yyyy-MM-dd");
            }
        }

        public string? Role { get; set; } = "USER";
    }
}