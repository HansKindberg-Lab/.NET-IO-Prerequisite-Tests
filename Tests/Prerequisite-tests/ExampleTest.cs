using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project;

namespace PrerequisiteTests
{
	[TestClass]
	public class ExampleTest
	{
		#region Properties

		protected internal virtual Example Example => new();

		#endregion

		#region Methods

		[TestMethod]
		public async Task ValidateFilePath_IfAllParametersAreNull_ShouldNotThrowAnException()
		{
			await Task.CompletedTask;

			this.Example.ValidateFilePath(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ValidateFilePath_IfTheDirectoryIsNotABaseOfTheFile_ShouldThrowAnInvalidOperationException()
		{
			const string action = "test";
			var directoryPaths = new List<string>();
			var exceptions = new List<InvalidOperationException>();
			var filePaths = new List<string>();

			if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				directoryPaths.AddRange([@"C:\Some-directory", @"C:\Some-directory\"]);
				filePaths.AddRange([@"C:\Some-other-directory\Some-file.txt", @"C:\Some-other-directory\", @"C:\Some-other-directory"]);
			}
			else
			{
				directoryPaths.AddRange(["/Some-directory", @"/Some-directory/"]);
				filePaths.AddRange([@"/Some-other-directory/Some-file.txt", "/Some-other-directory/", "/Some-other-directory"]);
			}

			foreach(var directoryPath in directoryPaths)
			{
				foreach(var filePath in filePaths)
				{
					try
					{
						this.Example.ValidateFilePath(action, directoryPath, filePath);
					}
					catch(InvalidOperationException invalidOperationException)
					{
						if(string.Equals(invalidOperationException.Message, $"It is not allowed to {action} the file \"{filePath}\". The file is outside the directory-path \"{directoryPath}\".", StringComparison.Ordinal))
							exceptions.Add(invalidOperationException);
					}
				}
			}

			if(exceptions.Count == 6)
				throw exceptions.First();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ValidateFilePath_IfTheDirectoryPathIsRelative_ShouldThrowAnArgumentException()
		{
			var exceptions = new List<ArgumentException>();
			var directoryPaths = new List<string>();
			var expectedNumberOfExceptions = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 5 : 2;
			string filePath;

			if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				directoryPaths.AddRange(["Directory", "/Directory", "/Directory/", @"\Directory", @"\Directory\"]);
				filePath = @"C:\Test.txt";
			}
			else
			{
				directoryPaths.AddRange(["Directory", "Directory/"]);
				filePath = "/Test.txt";
			}

			foreach(var directoryPath in directoryPaths)
			{
				try
				{
					this.Example.ValidateFilePath("Test", directoryPath, filePath);
				}
				catch(ArgumentException argumentException)
				{
					if(string.Equals(argumentException.ParamName, nameof(directoryPath), StringComparison.Ordinal) && argumentException.Message.StartsWith($"Could not create an absolute uri from directory-path \"{directoryPath}\".", StringComparison.Ordinal))
						exceptions.Add(argumentException);
				}
			}

			if(exceptions.Count == expectedNumberOfExceptions)
				throw exceptions.First();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ValidateFilePath_IfTheDirectoryPathParameterAndTheFilePathParameterAreEqual_ShouldThrowAnInvalidOperationException()
		{
			var directoryPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? @"C:\Some-directory" : "/Some-directory";

			try
			{
				this.Example.ValidateFilePath("Test", directoryPath, directoryPath);
			}
			catch(InvalidOperationException invalidOperationException)
			{
				if(string.Equals(invalidOperationException.Message, "The directory-path and file-path can not be equal.", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		public async Task ValidateFilePath_IfTheFilePathParameterIsAnEmptyString_ShouldNotThrowAnException()
		{
			await this.ValidateFilePathShouldNotThrowAnException(string.Empty);
		}

		[TestMethod]
		public async Task ValidateFilePath_IfTheFilePathParameterIsARelativePath_ShouldNotThrowAnException()
		{
			await this.ValidateFilePathShouldNotThrowAnException("Text.txt");
			await this.ValidateFilePathShouldNotThrowAnException("./Text.txt");
			await this.ValidateFilePathShouldNotThrowAnException("../Text.txt");
			await this.ValidateFilePathShouldNotThrowAnException("Directory/Text.txt");
			await this.ValidateFilePathShouldNotThrowAnException("./Directory/Text.txt");
			await this.ValidateFilePathShouldNotThrowAnException("../Directory/Text.txt");

			if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				await this.ValidateFilePathShouldNotThrowAnException("/Text.txt");
				await this.ValidateFilePathShouldNotThrowAnException(@"\Text.txt");
				await this.ValidateFilePathShouldNotThrowAnException(@"Directory\Text.txt");
				await this.ValidateFilePathShouldNotThrowAnException("/Directory/Text.txt");
				await this.ValidateFilePathShouldNotThrowAnException(@"\Directory\Text.txt");
			}
		}

		[TestMethod]
		public async Task ValidateFilePath_IfTheFilePathParameterIsNull_ShouldNotThrowAnException()
		{
			await this.ValidateFilePathShouldNotThrowAnException(null);
		}

		[TestMethod]
		public async Task ValidateFilePath_IfTheFilePathParameterIsOnlyWhitespaces_ShouldNotThrowAnException()
		{
			await this.ValidateFilePathShouldNotThrowAnException("    ");
		}

		protected internal virtual async Task ValidateFilePathShouldNotThrowAnException(string? filePath)
		{
			await Task.CompletedTask;

			this.Example.ValidateFilePath(null, null, filePath);
			this.Example.ValidateFilePath(null, string.Empty, filePath);
			this.Example.ValidateFilePath(string.Empty, null, filePath);
			this.Example.ValidateFilePath(string.Empty, string.Empty, filePath);
			this.Example.ValidateFilePath(null, "Test", filePath);
			this.Example.ValidateFilePath("Test", null, filePath);
			this.Example.ValidateFilePath("Test", "Test", filePath);

			if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				this.Example.ValidateFilePath("Test", @"C:\Some-directory", filePath);
			}
		}

		#endregion
	}
}