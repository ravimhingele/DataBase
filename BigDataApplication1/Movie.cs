using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigDataApplication1
{
    public class Movie
    {
        [Key]
        public string ID { get; set; } //IMDB
        public HashSet<MovieTitle> Titles = new HashSet<MovieTitle>();
        public string Country { get; set; }
        public string Langage { get; set; }
        public HashSet<Actor> Actors = new HashSet<Actor>();
        public Actor? Director { get; set; }
        public HashSet<Tag> Tags = new HashSet<Tag>();
        public string? Rating = "0.0";
        [NotMapped]
        public LinkedList<Movie> Top10 = new LinkedList<Movie>();
        [NotMapped]
        public LinkedList<int> Top10Ratings = new LinkedList<int>();
        public string Top10Codes { get; set; } = "";


        public Movie() { }

        public Movie(string id, MovieTitle title, string country, string lang)
        {
            ID = id;
            Titles.Add(title); // затем проверяем, есть ли такой фильм в словаре, если есть добавляем отдельно
            Country = country;
            Langage = lang;
        }
        public Movie(string id, string country, string lang)
        {
            ID = id;
            //Titles.Add(title); // затем проверяем, есть ли такой фильм в словаре, если есть добавляем отдельно
            Country = country;
            Langage = lang;
        }

        public void AddTitle(MovieTitle title)
        {
            Titles.Add(title);
        }
        public void AddActors(Actor actor)
        {
            Actors.Add(actor);
        }
        public void AddTag(Tag tag)
        {
            Tags.Add(tag);
        }

        /*public string GetName()
        {
            return this.Titles.First().Title;
        }*/

        public int GetSimilarity(Movie? movie)
        {
            int answer = Convert.ToInt32(movie.Rating.Replace(".", ""));
            if (this.Director != null && movie.Director != null && this.Director == movie.Director)
            {
                answer += 20;
            }
            int amOfActors = Actors.Intersect(movie.Actors).Count();
            answer += amOfActors * 10;

            int amOfTags = Tags.Intersect(movie.Tags).Count();
            answer += amOfTags;

            return answer;
        }


        public void FindSimilarMovies()
        {


            LinkedListNode<Movie> tMovie;
            LinkedListNode<int> tInt;
            HashSet<Movie> closeMovies = new HashSet<Movie>();


            if (Director != null)
            {
                if (Director.Movies != null)
                {
                    foreach (Movie movie in Director.Movies)
                    {
                        closeMovies.Add(movie);
                    }
                }

            }

            if (Actors != null)
            {
                foreach (Actor actor in Actors)
                {
                    if (actor.Movies != null)
                    {
                        foreach (Movie movie in actor.Movies)
                        {
                            closeMovies.Add(movie);
                        }
                    }
                }
            }

            if (Tags != null)
            {
                foreach (Tag tag in Tags)
                {
                    if (tag.Movies != null)
                    {
                        foreach (Movie movie in tag.Movies)
                        {
                            closeMovies.Add(movie);
                        }
                    }
                }
            }
            foreach (Movie movie in closeMovies)
            {
                if (movie != this)
                {
                    int r = this.GetSimilarity(movie);
                    if (Top10.Count == 0)
                    {
                        Top10.AddFirst(movie);
                        Top10Ratings.AddFirst(r);
                    }
                    else
                    {
                        tMovie = Top10.First;
                        tInt = Top10Ratings.First;
                        while (tMovie.Next != null && tInt.Next != null && tInt.Value > r)
                        {
                            tMovie = tMovie.Next;
                            tInt = tInt.Next;
                        }
                        if (tInt.Value < r)
                        {
                            Top10.AddBefore(tMovie, movie);
                            Top10Ratings.AddBefore(tInt, r);
                        }
                        else
                        {
                            Top10.AddLast(movie);
                            Top10Ratings.AddLast(r);
                        }
                        while (Top10.Count > 10)
                        {
                            Top10.RemoveLast();
                            Top10Ratings.RemoveLast();
                        }
                    }
                }
            }
            foreach (Movie movie in Top10)
            {
                if (Top10Codes == "")
                {
                    Top10Codes = movie.ID;
                }
                else
                {
                    Top10Codes += ", " + movie.ID;
                }
            }
        }
    }
}
