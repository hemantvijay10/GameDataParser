using System.Text.Json;

namespace GameDataParser
{
    internal class Program
    {
        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            while(true)
            {
                Console.WriteLine("Enter the JSON file name (or type 'exit' to quit):");
                string? inputFileName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(inputFileName))
                {
                    Console.WriteLine("\nInvalid input.");
                    continue;
                }

                if(inputFileName.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("\nExiting...");
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                }
                //TODO: Accepting any file name input can be risky. Consider normalizing the path using Path.GetFullPath and comparing it against an allowed directory to prevent directory traversal attacks.
                if (ValidateJsonFileName(inputFileName))
                {
                    //TODO: Consider checking if the file size is within a reasonable limit before reading, especially if there's any chance of very large files.
                    //TODO: Wrap the logging file write (File.AppendAllText) in its own try-catch block to handle any potential I/O issues (e.g., file locked by another process) without crashing the application.
                    string jsonContent = File.ReadAllText(inputFileName);

                    if (IsValidJsonContent(jsonContent))
                    {
                        ProcessJsonContent(jsonContent);
                    }
                }
            } //while ends
        }

        static bool ValidateJsonFileName(string inputFileName)
        {
            if (string.IsNullOrWhiteSpace(inputFileName))
            {
                Console.WriteLine("File name cannot be empty.");
                return false;
            }

            if (!inputFileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Please input file with '.json' extension.");
                return false;
            }

            if (!File.Exists(inputFileName))
            {
                Console.WriteLine("File does not exist.");
                return false;
            }
            Console.WriteLine("The file name is valid. Proceeding to validate it's content...");
            return true;
        }

        static bool IsValidJsonContent(string jsonContent)
        {   
            try
            {
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    Console.WriteLine("File cannot be empty.");
                    return false;
                }

                // Try to parse the JSON
                using (JsonDocument.Parse(jsonContent))
                {
                    // Parsing succeeded, so it's valid JSON
                }
                Console.WriteLine("File content is valid.");
                return true;
            }
            catch (JsonException ex)
            {
                LogExceptionInConsoleAndTextFile($"Parsing failed.", ex.ToString());
            }
            catch (IOException ex)
            {
                LogExceptionInConsoleAndTextFile($"I/O error.", ex.ToString());
            }
            return false;
        }

        static void ProcessJsonContent(string jsonContent)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    Console.WriteLine("File cannot be empty.");
                    return;
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Attempt to deserialize the JSON content into a list of VideoGame objects.
                List<VideoGameInfo>? games = JsonSerializer.Deserialize<List<VideoGameInfo>>(jsonContent, options);

                if ((games == null) || (games.Count == 0))
                {
                    Console.WriteLine("No games are present in the input file.");
                    return;
                }

                Console.WriteLine("Loaded games are:");
                foreach (var game in games)
                {
                    Console.WriteLine($"{game.Title}, released in {game.ReleaseYear}, rating: {game.Rating}\n");
                }
            }
            catch (JsonException ex)
            {
                LogExceptionInConsoleAndTextFile("Invalid JSON string encountered.", ex.ToString());
            }
            catch (Exception ex)
            {
                LogExceptionInConsoleAndTextFile("Error encountered while parsing JSON", ex.ToString());
            }
        }

        // Represents a video game with a title, release year, and rating.
        public class VideoGameInfo
        {
            public string Title { get; set; } = "";
            public int ReleaseYear { get; set; } = 0;
            public double Rating { get; set; } = 0;
        }

        // Logs exception details to a log file.
        static void LogExceptionInConsoleAndTextFile(string customeMsg, string exMsg)
        {
            try
            {
                Console.WriteLine(customeMsg);
                //Timestamp is in the format - [Day name], [Day]-[Month name]-[Year], HH:MM:SS:MMM [AM/PM] [Time zone]
                File.AppendAllText("errorlog.txt", $"[{FormatDateTimeWithTimeZone(DateTime.Now)}]: {customeMsg} Message: {exMsg}\n\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to write to log file: " + ex.Message);
            }
        }

        static string FormatDateTimeWithTimeZone(DateTime dt)
        {
            // Get local time zone info.
            TimeZoneInfo tz = TimeZoneInfo.Local;

            // Determine whether dt is in daylight saving time.
            string timeZoneName = tz.IsDaylightSavingTime(dt) ? tz.DaylightName : tz.StandardName;

            // Create an abbreviation by taking the first letter of each word.
            string abbreviation = "";
            foreach (string word in timeZoneName.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                abbreviation += word[0];
            }
            abbreviation = abbreviation.ToUpper();

            // Format the DateTime.
            // "ddd" gives abbreviated day name (e.g., Thu).
            // "dd-MMM-yyyy" gives day-month-year (e.g., 27-Mar-2025).
            // "h:mm:ss:fff tt" gives hour (non-padded), minute, second, millisecond, and AM/PM.
            string formatted = dt.ToString("ddd, dd-MMM-yyyy, h:mm:ss:fff tt");

            // Append the time zone abbreviation in parentheses.
            formatted += $" ({abbreviation})";

            return formatted;
        }
    }
}