using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Book
    {
        [Key]
        public string ISBN { set; get; } = String.Empty;
        public string Title { set; get; } = String.Empty;
        public string Summary { set; get; } = String.Empty;
        public string Format { set; get; } = String.Empty;

        [Display(Name = "Author")]
        public Author? BookAuthor { set; get; }
        public Int32 AuthorId { set; get; }
        public string Genre { set; get; } = String.Empty;

        [DataType(DataType.Currency)]
        public float Price { set; get; } = 0.0f;

        [Display(Name = "Number of Pages")]
        public Int32 NumberOfPages { get; set; }

        [Display(Name = "Publisher")]
        public Publisher? BookPublisher { get; set; }

        public Int32 PublisherId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Published")]
        public DateTime DatePublished { get; set; }


        [DataType(DataType.ImageUrl)]
        [Display(Name = "Book Cover")]
        public string BookCover { get; set; } = String.Empty;

    }
}