using NLog;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();
logger.Info("Program started");

string scrubbedFile = Directory.GetCurrentDirectory() + "\\movies.csv";
logger.Info(scrubbedFile);

string choice = ""; 

do
{
    // ask user a question
    Console.WriteLine("1) Add Movie");
    Console.WriteLine("2) Display All Movies");
    Console.WriteLine("Enter or any other key to quit.");

    // input response
    choice = Console.ReadLine();
    logger.Info($"User choice: {choice}");

    try
    {
        int choiceInt = Convert.ToInt32(choice);

        MovieFile movieFile = new MovieFile(scrubbedFile);

        switch(choiceInt)
        {
            case 1: // Add Movie
                Movie movie = new Movie();
                Console.Write("Enter moive title: ");
                movie.title = Console.ReadLine();

                if(movieFile.isUniqueTitle(movie.title))
                {
                    string inputGenre = "";
                    do
                    {
                        Console.WriteLine("Enter genre (or done to quit)");
                        inputGenre = Console.ReadLine();
                        if(inputGenre != "done")
                            movie.genres.Add(inputGenre);
                    }
                    while(inputGenre != "done");

                    if(movie.genres.Count == 0)
                    {
                        movie.genres.Add("(no genres listed)");
                    }

                    Console.Write("Enter movie director: ");
                    movie.director = Console.ReadLine();
                    Console.Write("Enter running time (h:m:s): ");
                    string runTime = Console.ReadLine();
                    movie.runningTime = TimeSpan.Parse(runTime);

                    movieFile.AddMovie(movie);
                }
            break;

            case 2: // Display All Movies
                foreach(Movie m in movieFile.Movies)
                {
                    Console.WriteLine(m.Display());
                }
            break;
        }
    }
    catch(Exception ex)
    {
        logger.Error(ex.Message);
    }

} while(choice == "1" || choice == "2");

logger.Info("Program ended.");