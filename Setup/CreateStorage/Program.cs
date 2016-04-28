using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CreateStorage
{
    class Program
    {
        private static ConsoleColor _defaultColor;

        static void Main(string[] args)
        {
            _defaultColor = Console.ForegroundColor;

            var connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
            var containerName = ConfigurationManager.AppSettings["BlobContainerName"];
            if (!ConfirmContinue(connectionString, containerName))
            {
                return;
            }


            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            var containerCreated = container.CreateIfNotExists();

            WriteLine();
            if (containerCreated)
            {
                WriteLine($"Created container {containerName}", ConsoleColor.Green);
            }
            else
            {
                WriteLine($"Container {containerName} already exists", ConsoleColor.DarkGray);
            }

            UploadFile(container, "Home/Index.cshtml");
            UploadFile(container, "Shared/_Layout.cshtml");
            UploadFile(container, "Shared/Error.cshtml");
            UploadFile(container, "_ViewImports.cshtml");
            UploadFile(container, "_ViewStart.cshtml");

            WriteLine();
            WriteLine("Setup complete. Press any key to exit...");
            Console.ReadKey();
        }

        static bool ConfirmContinue(string connectionString, string containerName)
        {
            WriteLine("Will attempt to use the following details to create the view storage required for this sample:");
            WriteLine();

            WriteLine($"   Connection string: {connectionString}", ConsoleColor.Yellow);
            WriteLine($"   Container name: {containerName}", ConsoleColor.Yellow);

            WriteLine();
            WriteLine("If this is not correct you need to update the app.config of CreateStorage to the correct details.");

            while (true)
            {
                Console.Write("Would you like to continue (Y/N)? ");
                var input = Console.ReadLine().ToUpper();
                if (input == "Y")
                {
                    return true;
                }
                else if (input == "N")
                {
                    return false;
                }
                else
                {
                    WriteLine("Invalid selection. Enter either Y or N", ConsoleColor.Red);
                }
            }
        }

        static void UploadFile(CloudBlobContainer container, string relativePath)
        {
            var source = new FileInfo($"../../../Views/{relativePath}");
            if (!source.Exists)
            {
                WriteLine($"Failed to upload {relativePath} - Could not find file {source.FullName}", ConsoleColor.Red);
            }

            try
            {
                var blob = container.GetBlockBlobReference(relativePath);
                if (blob.Exists())
                {
                    WriteLine($"{relativePath} already exists, skipping upload", ConsoleColor.DarkGray);
                    return;
                }

                blob.UploadFromFile(source.FullName, FileMode.Open);
                WriteLine($"{relativePath} uploaded successfully", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to upload {relativePath} - {ex.Message}", ConsoleColor.Red);
            }
        }



        static void WriteLine()
        {
            WriteLine(string.Empty, _defaultColor);
        }
        static void WriteLine(string line)
        {
            WriteLine(line, _defaultColor);
        }
        static void WriteLine(string line, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(line);
            Console.ForegroundColor = _defaultColor;
        }
    }
}
