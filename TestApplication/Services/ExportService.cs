using Newtonsoft.Json;
using TestApplication.Enums;
using TestApplication.Models;

namespace TestApplication.Services
{
    public class ExportService
    {
        public void ExportUsersToFile(IEnumerable<User> users, string folderPath, FileFormat fileFormat)
        {
            switch (fileFormat)
            {
                case FileFormat.JSON:
                    ExportToJson(users, folderPath);
                    break;
                case FileFormat.CSV:
                    ExportToCsv(users, folderPath);
                    break;
                default:
                    throw new ArgumentException("Unsupported export format");
            }
        }

        private void ExportToJson(IEnumerable<User> users, string folderPath)
        {
            var filePath = Path.Combine(folderPath, $"users-json-{Guid.NewGuid()}.json");

            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private void ExportToCsv(IEnumerable<User> users, string folderPath)
        {
            var filePath = Path.Combine(folderPath, $"users-csv-{Guid.NewGuid()}.csv");

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("FirstName, LastName, Email, SourceId");

                foreach (var user in users)
                {
                    string firstName = EscapeCsvField(user.FirstName);
                    string lastName = EscapeCsvField(user.LastName);
                    string email = EscapeCsvField(user.Email);
                    string sourceId = EscapeCsvField(user.SourceId);

                    writer.WriteLine($"{firstName}, {lastName}, {email}, {sourceId}");
                }
            }
        }

        private static string EscapeCsvField(string field)
        {
            return "\"" + field.Replace("\"", "\"\"") + "\"";
        }

        public string ChooseDestinationFolder()
        {
            while (true)
            {
                Console.WriteLine("Please enter the destination folder path:");
                string folderPath = Console.ReadLine();
                
                if (Directory.Exists(folderPath))
                {
                    Console.WriteLine();
                    return folderPath;
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(folderPath);
                        Console.WriteLine();
                        return folderPath;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error occurred creating folder: {ex.Message}");
                    }
                }
            }
        }

        public FileFormat ChooseFileFormat()
        {
            while (true)
            {
                Console.WriteLine("Choose the desired file format:");
                Console.WriteLine("1. JSON");
                Console.WriteLine("2. CSV");

                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key == ConsoleKey.D1 || keyInfo.Key == ConsoleKey.NumPad1)
                {
                    Console.WriteLine();
                    return FileFormat.JSON;
                }
                else if (keyInfo.Key == ConsoleKey.D2 || keyInfo.Key == ConsoleKey.NumPad2)
                {
                    Console.WriteLine();
                    return FileFormat.CSV;
                }
                else
                {
                    Console.WriteLine("Invalid input: press 1 for JSON or 2 for CSV.");
                }
            }
        }
    }
}
