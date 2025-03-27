GameDataParser

Overview:

GameDataParser is a robust and user-friendly C# console application designed to read and process video game data stored in JSON files. It validates the file name and content before deserializing the JSON data into a list of video game objects and then displays the details (title, release year, and rating) on the console.

Features:

1. File Validation: Checks that the user-provided file name is not empty, has a .json extension, and exists on disk.
2. JSON Validation: Validates that the file content is well-formed JSON using System.Text.Json before processing.
3. Data Deserialization: Deserializes the JSON content into strongly typed VideoGameInfo objects with case-insensitive property matching.
4. Error Handling & Logging: Provides clear console feedback for invalid inputs and errors, while logging detailed exception information (including timestamps) to a errorlog.txt file.
5. User-Friendly Interface: Continuously prompts the user for input, with a simple exit option by typing "exit".

Getting Started:

Prerequisites
.NET 9.0 SDK

Installation:

Clone the repository:

git clone https://github.com/yourusername/GameDataParser.git
Open the solution in your preferred IDE (Visual Studio, VS Code, etc.) or use the .NET CLI.

Build the project:
dotnet build

Usage
Run the application:
dotnet run

When prompted, enter the JSON file name (or the full path to the file) containing your video game data.

The application will validate the file name and content, then display the list of video games if the JSON is valid.

Type "exit" to close the application.

Error Logging:
All errors encountered during file validation, JSON parsing, or processing are logged to a errorlog.txt file. Each log entry includes:

1. A timestamp (formatted with the local time zone).
2. A custom error message.
3. Detailed exception information.

Security Considerations & future enhancements:
1. Input Validation:
The application validates the file name to prevent incorrect file types and ensure the file exists.
2. Directory Traversal:
Although user input is used for file names, consider restricting file access to a designated folder to mitigate directory traversal risks.
3. Sensitive Logging:
Ensure the errorlog.txt file is secured with proper file permissions to avoid exposing sensitive internal error details.
4. Implement file size checks to avoid performance issues with large files.
5. Add additional input sanitization to further protect against malicious file path input.
6. Expand logging features to support different logging levels (e.g., info, warning, error).

Acknowledgement: This project was completed as part of the Udemy course ‘Complete C# Masterclass’ by Krystyna Ślusarczyk (link is provided below). Having significant prior experience with C#, my goal was to refresh my skills and familiarize myself with the latest updates, features, and best practices introduced in recent versions.

https://www.udemy.com/course/ultimate-csharp-masterclass/
