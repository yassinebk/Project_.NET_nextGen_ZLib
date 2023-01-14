using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Project.Models
{
    [Authorize(Roles = "ADMIN")]
    public class Author
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int32 Id { get; set; }

        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        public string Bio { get; set; } = String.Empty;

        [DataType(DataType.ImageUrl)]
        public string AuthorImage { get; set; } = String.Empty;
        
        [JsonIgnore]
        public ICollection<Book>? Books { get; set; }

        public string WikiLink { get; set; } = String.Empty;

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

    }
}