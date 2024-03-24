using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project;

namespace PrerequisiteTests
{
	[TestClass]
	public class PackageTransformerTest
	{
		#region Properties

		protected internal virtual PackageTransformer PackageTransformer => new();

		#endregion

		#region Methods

		[TestMethod]
		public async Task ValidateFilePath_IfAllParametersAreNull_ShouldNotThrowAnException()
		{
			await Task.CompletedTask;

			this.PackageTransformer.ValidateFilePath(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ValidateFilePath_IfTheDirectoryIsNotAnAncestorOfTheFile_ShouldThrowAnInvalidOperationException()
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
						this.PackageTransformer.ValidateFilePath(action, directoryPath, filePath);
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
		[ExpectedException(typeof(ArgumentNullException))]
		public void ValidateFilePath_IfTheDirectoryPathIsNull_And_IfTheFilePathIsAbsolute_ShouldThrowAnArgumentNullException()
		{
			try
			{
				this.PackageTransformer.ValidateFilePath("Test", null, RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? @"C:\Test.txt" : "/Test.txt");
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(argumentNullException.ParamName!.Equals("directoryPath", StringComparison.Ordinal))
					throw;
			}
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
					this.PackageTransformer.ValidateFilePath("Test", directoryPath, filePath);
				}
				catch(ArgumentException argumentException)
				{
					if(string.Equals(argumentException.ParamName, nameof(directoryPath), StringComparison.Ordinal) && argumentException.Message.StartsWith($"The directory-path can not be relative ({directoryPath}).", StringComparison.Ordinal))
						exceptions.Add(argumentException);
				}
			}

			if(exceptions.Count == expectedNumberOfExceptions)
				throw exceptions.First();
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

			this.PackageTransformer.ValidateFilePath(null, null, filePath);
			this.PackageTransformer.ValidateFilePath(null, string.Empty, filePath);
			this.PackageTransformer.ValidateFilePath(string.Empty, null, filePath);
			this.PackageTransformer.ValidateFilePath(string.Empty, string.Empty, filePath);
			this.PackageTransformer.ValidateFilePath(null, "Test", filePath);
			this.PackageTransformer.ValidateFilePath("Test", null, filePath);
			this.PackageTransformer.ValidateFilePath("Test", "Test", filePath);

			if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				this.PackageTransformer.ValidateFilePath("Test", @"C:\Some-directory", filePath);
			}
		}

		#endregion
	}
}