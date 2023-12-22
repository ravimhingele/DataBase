using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BigDataApplication1
{
    public class MovieTitle
    {
        public string MovieId { get; set; }
        public string Title { get; set; }
        public int Ordering { get; set; }
        public Movie Movie { get; set; }
        public MovieTitle() { }
        public MovieTitle(string title, string movieId, Movie movie, int ordering)
        {
            //Id = id;
            Title = title;
            MovieId = movieId;
            Movie = movie;
            Ordering = ordering;
        }
    }
}
