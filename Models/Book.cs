using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

/*
* Each book represents a physical copy of the titled book. That's why I am embedding the filename and the format as well as the language and 
* the number of pages. The price is also embedded in the book. The book cover is also embedded in the book.
* We will have some data duplicated but for the sake of simplicity we can keep it this way as it will not affect any features.
*
*/
namespace Project.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ISBN { set; get; } = String.Empty;

        public string Title { set; get; } = String.Empty;
        public string Summary { set; get; } = String.Empty;
        public string Format { set; get; } = String.Empty;

        public string Language { set; get; } = String.Empty;

        public string FileName { set; get; } = String.Empty;


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