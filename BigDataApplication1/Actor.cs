using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigDataApplication1
{
    public class Actor
    {
        [Key]
        public string ID { get; set; }
        public string Name { get; set; }
        //public string MovieIDs { get; set; }
        [NotMapped]
        public HashSet<Movie> Movies = new HashSet<Movie>();

        public Actor() { }

        public Actor(string id, string name)
        {
            ID = id;
            Name = name;
        }
        
        public void AddMovie(Movie movie)
        {
            Movies.Add(movie);
        }
    }
}

