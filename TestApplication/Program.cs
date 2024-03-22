using TestApplication.Services;

namespace TestApplication
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var userService = new UserService();
            var exportService = new ExportService();
            
            var users = await userService.GetAllUsersAsync();

            var destinationFolder = exportService.ChooseDestinationFolder();
            var desiredFormat = exportService.ChooseFileFormat();
            exportService.ExportUsersToFile(users, destinationFolder, desiredFormat);

            Console.WriteLine("Total amount of users: " + users.Count);
        }
    }
}
