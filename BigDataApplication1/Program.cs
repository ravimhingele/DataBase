using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace BigDataApplication1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Загрузить базу данных?");
            Console.WriteLine("1 - да");
            Console.WriteLine("2 - нет");
            string? answer = Console.ReadLine();
            string search;
            string numberS;
            if (answer == "1")
            {
                DataBaseLoad();
                DataBaseAnswer();
            }
            else if (answer == "2")
            {
                DataBaseAnswer();
            }
            else
            {
                Console.WriteLine("Неизвестный ответ...");
                Console.ReadKey();
            }

            void DataBaseAnswer()
            {
                while (true)
                {
                    Console.WriteLine("Что вы хотите найти?");
                    Console.WriteLine("1 - фильм");
                    Console.WriteLine("2 - актер/режиссер");
                    Console.WriteLine("3 - тэг");
                    numberS = Console.ReadLine();
                    if (numberS == "1")
                    {
                        using (ApplicationContext db = new ApplicationContext(false))
                        {
                            Console.WriteLine("Какой фильм вы хотите найти?");
                            search = Console.ReadLine();
                            string? movieID = db.MovieTitles //вытащили ID фильма по названию
                                .Where(m => m.Title == search)
                                .Select(m => m.MovieId)
                                .FirstOrDefault();

                            var movie = db.Movies
                                .Include(m => m.Actors)
                                .Include(m => m.Tags)
                                .FirstOrDefault(m => m.ID == movieID);
                            
                            if (movie != null)
                            {
                                var actors = movie?.Actors; //!!!!!!!!!!!!!!!!!!!!!!!!!!!
                                // Получаем список имен актеров фильма
                                //List<string> actorNames = actors.Select(a => a.Name).ToList();
                                StringBuilder aN = new StringBuilder();
                                foreach (var actor in actors)
                                {
                                    aN.Append($"{actor.Name}, ");
                                }
                                int l = aN.Length;
                                if (l > 0)
                                    aN.Remove(l - 2, 2);

                                var tags = movie?.Tags; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                                StringBuilder tgN = new StringBuilder();
                                foreach (var tag in tags)
                                {
                                    tgN.Append($"{tag.Name}, ");
                                }
                                l = tgN.Length;
                                if (l > 0)
                                    tgN.Remove(l - 2, 2);

                                string? directorName = db.Movies
                                .Where(m => m.ID == movieID)
                                .Select(m => m.Director.Name)
                                .FirstOrDefault();

                                string? movieRating = db.Movies
                                .Where(m => m.ID == movieID)
                                .Select(m => m.Rating)
                                .FirstOrDefault();

                                

                                Console.WriteLine($"Name: {search}");
                                Console.WriteLine($"Actors: {aN}");
                                Console.WriteLine($"Director: {directorName}");
                                Console.WriteLine($"Rating: {movieRating}");
                                Console.WriteLine($"Tags: {tgN}");

                                /*Console.WriteLine("Загрузка похожих фильмов...");

                                //List<Movie> similarMovies = FindSimilarMovies(movie);


                                var similarMovies = db.Movies
                                .Where(m => m.ID != movie.ID) // Исключение запрашиваемого фильма из списка
                                .AsEnumerable()
                                .Select(m => new
                                {
                                    Movie = m,
                                    Similarity = movie.GetSimilarity(m) // Рассчитываем коэффициент сходства
                                })
                                .OrderByDescending(m => m.Similarity)
                                .Take(10) // Получаем топ 10 по сходству
                                .Select(m => m.Movie)
                                .ToList();
                                StringBuilder simM = new StringBuilder();
                                foreach (var mie in similarMovies)
                                {
                                    simM.Append($"{db.MovieTitles
                                    .FirstOrDefault(mt => mt.MovieId == mie.ID)?.Title}, ");
                                }
                                Console.WriteLine($"Similar movies: {simM}");*/
                            }
                            else
                            {
                                Console.WriteLine("Фильм не найден!");
                            }
                        }
                    }
                    else if(numberS == "2")
                    {
                        Console.WriteLine("Введите имя: ");
                        search = Console.ReadLine();
                        using (ApplicationContext db = new ApplicationContext(false))
                        {
                            string? actorID = db.Actors //вытащили ID актера по имени
                                    .Where(a => a.Name == search)
                                    .Select(a => a.ID)
                                    .FirstOrDefault();

                            if (actorID != null)
                            {
                                var movies = db.Movies
                                .Where(m => m.Actors.Any(a => a.ID == actorID))
                                .ToList();

                                StringBuilder acM = new StringBuilder();
                                foreach (var m in movies)
                                {
                                    acM.Append($"{db.MovieTitles
                                    .FirstOrDefault(mt => mt.MovieId == m.ID)?.Title}, ");
                                }
                                int l = acM.Length;
                                if (l > 0)
                                    acM.Remove(l - 2, 2);

                                Console.WriteLine($"Name: {search}");
                                Console.WriteLine($"Movies: {acM}");
                            }
                            else
                            {
                                Console.WriteLine("Актер / режиссер не найден");
                            }
                        }
                    }
                    else if(numberS == "3")
                    {
                        Console.WriteLine("Какой тэг вы хотите найти?");
                        search = Console.ReadLine();
                        using (ApplicationContext db = new ApplicationContext(false))
                        {
                            string? tagID = db.Tags //вытащили ID актера по имени
                                    .Where(a => a.Name == search)
                                    .Select(a => a.ID)
                                    .FirstOrDefault();

                            if (tagID != null)
                            {
                                var movies = db.Movies
                                .Where(m => m.Tags.Any(a => a.ID == tagID))
                                .ToList();
                                StringBuilder tagM = new StringBuilder();
                                foreach(var m in movies)
                                {
                                    tagM.Append($"{db.MovieTitles
                                    .FirstOrDefault(mt => mt.MovieId == m.ID)?.Title}, ");
                                }

                                int l = tagM.Length;
                                if (l > 0)
                                    tagM.Remove(l - 2, 2);

                                Console.WriteLine($"Tag: {search}");
                                Console.WriteLine($"Movies: {tagM}");
                            }


                        }
                    }
                    else
                    {
                        Console.WriteLine("Неизвестный ответ...");
                        Console.ReadKey();
                    }
                }
            }

            void DataBaseLoad()
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                GC.Collect();

                Dictionary<string, Movie> movies = new Dictionary<string, Movie>(); // словарь фильмов по id

                var countries = new HashSet<string> { "RU", "US", "GB" };
                var languages = new HashSet<string> { "en", "ru" };


                //string fileP = "TESTMovieByCode.txt";
                string fileP = "MovieCodes_IMDB.tsv";
                using (StreamReader reader = new StreamReader(fileP))
                {
                    reader.ReadLine();
                    int t1, t2, t3, t4, t5;
                    int ordering = 0;
                    string country, language, index, name;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        t1 = line.IndexOf('\t');
                        t2 = line.IndexOf('\t', t1 + 1);
                        t3 = line.IndexOf('\t', t2 + 1);
                        t4 = line.IndexOf('\t', t3 + 1);
                        t5 = line.IndexOf('\t', t4 + 1);

                        country = line.Substring(t3 + 1, t4 - t3 - 1);
                        language = line.Substring(t4 + 1, t5 - t4 - 1);

                        if (countries.Contains(country) || languages.Contains(language))
                        {
                            index = line.Substring(0, t1);
                            name = line.Substring(t2 + 1, t3 - t2 - 1);
                            if (movies.TryGetValue(index, out Movie movie))
                            {
                                ordering += 1;
                                movie.AddTitle(new MovieTitle(name, movie.ID, movie, ordering));
                            }
                            else
                            {
                                ordering = 1;
                                //movies.Add(index, new Movie(index, new MovieTitle(name, movie.ID), country, language));
                                Movie movie1 = new Movie(index, country, language);
                                movie1.AddTitle(new MovieTitle(name, movie1.ID, movie1, ordering));
                                movies.Add(index, movie1);

                            }
                        }
                    }
                }

                //string fileP2 = "TESTActorsDirectorsNames_IMDB.txt";
                string fileP2 = "ActorsDirectorsNames_IMDB.txt";
                Dictionary<string, Actor> actorsCodes = new Dictionary<string, Actor>();

                using (StreamReader reader2 = new StreamReader(fileP2))
                {
                    reader2.ReadLine();
                    string acID, acName;
                    int t1, t2;
                    string line;
                    while ((line = reader2.ReadLine()) != null)
                    {
                        t1 = line.IndexOf('\t');
                        t2 = line.IndexOf('\t', t1 + 1);

                        acID = line.Substring(0, t1);

                        if (!actorsCodes.TryGetValue(acID, out Actor actor))
                        {
                            acName = line.Substring(t1 + 1, t2 - t1 - 1);
                            actorsCodes.Add(acID, new Actor(acID, acName));
                        }
                    }
                }

                /*Dictionary<string, Actor> actorsNames = new Dictionary<string, Actor>();
                foreach (var actor in actorsCodes)
                {
                    if (!actorsNames.TryGetValue(actor.Value.Name, out Actor actor1))
                    {
                        actorsNames.Add(actor.Value.Name, actor.Value);
                    }
                }*/

                //string filePath = "TESTActorsDirectorsCodes_IMDB.txt";
                string filePath = "ActorsDirectorsCodes_IMDB.tsv";
                using (StreamReader reader3 = new StreamReader(filePath)) 
                {
                    reader3.ReadLine();
                    string line;
                    int t1, t2, t3, t4, t5;
                    string movieID, acID, acP;
                    while ((line = reader3.ReadLine()) != null)
                    {
                        t1 = line.IndexOf('\t');
                        t2 = line.IndexOf('\t', t1 + 1);
                        t3 = line.IndexOf('\t', t2 + 1);
                        t4 = line.IndexOf('\t', t3 + 1);
                        t5 = line.IndexOf('\t', t4 + 1);

                        acP = line.Substring(t3 + 1, t4 - t3 - 1);

                        if (acP == "actor" || acP == "director")
                        {
                            movieID = line.Substring(0, t1);
                            acID = line.Substring(t2 + 1, t3 - t2 - 1);
                            if (actorsCodes.TryGetValue(acID, out Actor actor))
                            {
                                if (movies.TryGetValue(movieID, out Movie movie))
                                {
                                    //actor.AddMovie(movie);!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                                    if (acP == "actor")
                                    {
                                        movie.AddActors(actor);
                                        actor.AddMovie(movie);
                                    }
                                    else if (acP == "director")
                                    {
                                        movie.Director = actor;
                                        actor.AddMovie(movie);
                                    }
                                }
                            }
                        }
                    }
                    actorsCodes.Clear(); //Очищаем словарь после использования
                }

                //string filePath1 = "TESTRatings.txt";
                string filePath1 = "Ratings_IMDB.tsv";
                using (StreamReader reader4 = new StreamReader(filePath1))
                {
                    reader4.ReadLine();
                    string line;
                    while ((line = reader4.ReadLine()) != null)
                    {
                        string[] parts = line.Split('\t');
                        if (movies.TryGetValue(parts[0], out Movie movie))
                        {
                            movie.Rating = parts[1];
                        }
                    }
                }
                //string[] reader7 = File.ReadAllLines("TESTCode2.txt");
                string[] reader7 = File.ReadAllLines("links_IMDB_MovieLens.csv");
                Dictionary<string, string> links;

                links = (from t in reader7
                         let spl = t.Split(',')
                         select spl)
                          .AsParallel()
                          .ToDictionary(spl => spl[0], spl => "tt" + spl[1]);


                //string[] reader6 = File.ReadAllLines("TESTTagsCodes.txt");
                string[] reader6 = File.ReadAllLines("TagCodes_MovieLens.csv");
                Dictionary<string, Tag> tagCodesML; //словать тэгов

                tagCodesML = (from t in reader6
                              let spl = t.Split(',')
                              select new Tag(spl[0], spl[1]))
                          .AsParallel()
                          .ToDictionary(t => t.ID, t => t);

                reader6 = null;

                int f;
                string key;

                //string filePath3 = "TESTTagAndMovie.txt";
                string filePath3 = "TagScores_MovieLens.csv";
                using (StreamReader reader5 = new StreamReader(filePath3)) 
                {
                    string[] parts;
                    string line;
                    while ((line = reader5.ReadLine()) != null)
                    {
                        parts = line.Split(',');
                        key = links[parts[0]];
                        if (movies.TryGetValue(key, out Movie movie))
                        {
                            f = Convert.ToInt32(parts[2].Substring(2, 1));
                            if (f >= 5) //
                            {
                                tagCodesML.TryGetValue(parts[1], out Tag tag);
                                movie.Tags.Add(tag);
                                tag.AddMovie(movie);// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                            }
                        }
                    }
                }

                /*Dictionary<string, Tag> tagsNames = new Dictionary<string, Tag>();
                foreach (var i in tagCodesML)
                {
                    tagsNames.Add(i.Value.Name, i.Value);
                }*/


                Dictionary<string, Movie> moviesNames = new Dictionary<string, Movie>(); //словарь фильмов по названиям
                foreach (var movie in movies) 
                {
                    foreach (MovieTitle mov in movie.Value.Titles)
                    {
                        if (!moviesNames.TryGetValue(mov.Title, out Movie m))
                        {
                            moviesNames.Add(mov.Title, movie.Value);
                        }
                    }
                }

                Parallel.ForEach(movies.Values, movie =>
                {
                    movie.FindSimilarMovies();
                });


                //movies.Clear(); !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                using (ApplicationContext db = new ApplicationContext(true))
                {
                    
                    int t = 0;
                    foreach (var movie in moviesNames)
                    {
                        Console.WriteLine("EEEEEEEEEEEEEEEEEEEEEEEEE");
                        t++;

                        db.Add(movie.Value);

                        if (t % 1 == 0)
                        {
                            Console.WriteLine("есть" + t);
                        }

                    }
                    db.SaveChanges();
                    /*var films = db.Movies.ToList();

                    // Вычислить и присвоить топ 10 похожих фильмов для каждого фильма
                    foreach (var movie in films)
                    {
                        Console.WriteLine("AAAAAAAAAAAAAAAAAAAAA");
                        var similarMovies = films
                            .Where(m => m.ID != movie.ID)
                            .OrderByDescending(m => movie.GetSimilarity(m))
                            .Take(10)
                            .ToList();

                        movie.SimilarMovies = similarMovies;
                    }

                    // Сохранить изменения в базе данных
                    db.SaveChanges();*/
                }
                stopwatch.Stop();
                TimeSpan timeSpan = stopwatch.Elapsed;
                Console.WriteLine("Время: " + timeSpan);
            }
            Console.WriteLine();

            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            /*while (true)
            {
                Console.WriteLine("Что вы хотите найти?");
                Console.WriteLine("1 - фильм");
                Console.WriteLine("2 - актер/режиссер");
                Console.WriteLine("3 - тэг");
                string numberS = Console.ReadLine();
                string search;
                if (numberS == "1")
                {
                    Console.WriteLine("Какой фильм вы хотите найти?");
                    search = Console.ReadLine();
                    PrintMovieInfo(search);
                }
                else if (numberS == "2")
                {
                    Console.WriteLine("Введите имя: ");
                    search = Console.ReadLine();
                    //PrintActorInfo(search);
                }
                else if (numberS == "3")
                {
                    Console.WriteLine("Какой тэг вы хотите найти?");
                    search = Console.ReadLine();
                    //PrintTagInfo(search);
                }
                else
                {
                    Console.WriteLine("Нет такой команды. Повторите ввод:");
                }
            }*/

            /*void PrintMovieInfo(string mo)
            {
                if (moviesNames.TryGetValue(mo, out Movie movie))
                {
                    Console.WriteLine("Название фильма: " + mo);
                    StringBuilder joinedValues1 = new StringBuilder();
                    foreach (Actor actor in movie.Actors)
                    {
                        joinedValues1.Append(actor.Name + ", ");
                    }
                    int l = joinedValues1.Length;
                    if (l > 0)
                        joinedValues1.Remove(l - 2, 2);
                    Console.WriteLine("Актеры: " + joinedValues1);
                    Console.WriteLine("Режиссер: " + movie.Director);
                    StringBuilder joinedValues2 = new StringBuilder();
                    foreach (Tag tag in movie.Tags)
                    {
                        joinedValues2.Append(tag.Name + ", ");
                    }
                    l = joinedValues2.Length;
                    if (l > 0)
                        joinedValues2.Remove(l - 2, 2);
                    Console.WriteLine("Тэги: " + joinedValues2);
                    Console.WriteLine("Рейтинг: " + movie.Rating);
                    Console.WriteLine();
                    Console.WriteLine("Нажмите, чтобы продолжить..");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Фильм не найден((");
                    Console.WriteLine("Нажмите, чтобы продолжить..");
                    Console.ReadKey();
                }
            }*/


            /*
            void PrintActorInfo(string acName)
            {
                if (actorsNames.TryGetValue(acName, out Actor actor))
                {
                    Console.WriteLine("Имя: " + actor.Name);
                    StringBuilder joinedValues1 = new StringBuilder();
                    foreach (var movie in actor.Movies)
                    {
                        joinedValues1.Append(movie.Titles.First() + ", ");
                    }
                    int l = joinedValues1.Length;
                    if (l > 0)
                        joinedValues1.Remove(l - 2, 2);
                    Console.WriteLine("Фильмы: " + joinedValues1);
                    Console.WriteLine();
                    Console.WriteLine("Нажмите, чтобы продолжить..");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Актер/Режиссер не найден");
                    Console.WriteLine("Нажмите, чтобы продолжить..");
                    Console.ReadKey();
                }
            }*/

            /*void PrintTagInfo(string tag)
            {
                if (tagsNames.TryGetValue(tag, out Tag t))
                {
                    Console.WriteLine("Название тэга: " + t.Name);
                    StringBuilder joinedValues1 = new StringBuilder();
                    foreach (var movie in t.Movies)
                    {
                        joinedValues1.Append(movie + ", ");
                    }
                    int l = joinedValues1.Length;
                    if (l > 0)
                        joinedValues1.Remove(l - 2, 2);
                    Console.WriteLine("Фильмы: " + joinedValues1);
                    Console.WriteLine();
                    Console.WriteLine("Нажмите, чтобы продолжить..");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Тэг не найден");
                    Console.WriteLine("Нажмите, чтобы продолжить..");
                    Console.ReadKey();
                }
            }*/
        }

    }
}
