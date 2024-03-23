using System.Runtime.InteropServices;
using Project;

namespace PrerequisiteTests
{
	public class ExampleTest
	{
		#region Properties

		protected internal virtual Example Example => new();

		#endregion

		#region Methods

		[Fact]
		public async Task ValidateFilePath_IfAllParametersAreNull_ShouldNotThrowAnException()
		{
			await Task.CompletedTask;

			this.Example.ValidateFilePath(null, null, null);
		}

		[Fact]
		public async Task ValidateFilePath_IfTheFilePathParameterIsAnEmptyString_ShouldNotThrowAnException()
		{
			await this.ValidateFilePathShouldNotThrowAnException(string.Empty);
		}

		//[TestMethod]
		//[ExpectedException(typeof(InvalidOperationException))]
		//public void ValidateFilePath_IfTheDirectoryIsNotABaseOfTheFile_ShouldThrowAnInvalidOperationException()
		//{
		//	const string action = "test";
		//	var exceptions = new List<InvalidOperationException>();

		//	foreach(var directoryPath in new[] { @"C:\Some-directory", @"C:\Some-directory\" })
		//	{
		//		foreach(var filePath in new[] { @"C:\Some-other-directory\Some-file.txt", @"C:\Some-other-directory\", @"C:\Some-other-directory" })
		//		{
		//			try
		//			{
		//				this.PackageTransformer.ValidateFilePath(action, directoryPath, filePath);
		//			}
		//			catch(InvalidOperationException invalidOperationException)
		//			{
		//				if(string.Equals(invalidOperationException.Message, $"It is not allowed to {action} the file \"{filePath}\". The file is outside the directory-path \"{directoryPath}\".", StringComparison.Ordinal))
		//					exceptions.Add(invalidOperationException);
		//			}
		//		}
		//	}

		//	if(exceptions.Count == 6)
		//		throw exceptions.First();
		//}

		//[TestMethod]
		//[ExpectedException(typeof(ArgumentException))]
		//public void ValidateFilePath_IfTheDirectoryPathIsRelative_ShouldThrowAnArgumentException()
		//{
		//	var exceptions = new List<ArgumentException>();

		//	foreach(var directoryPath in new[] { "Directory", "/Directory", @"\Directory" })
		//	{
		//		try
		//		{
		//			this.PackageTransformer.ValidateFilePath("Test", directoryPath, @"C:\Test.txt");
		//		}
		//		catch(ArgumentException argumentException)
		//		{
		//			if(string.Equals(argumentException.ParamName, nameof(directoryPath), StringComparison.Ordinal) && argumentException.Message.StartsWith($"Could not create an absolute uri from directory-path \"{directoryPath}\".", StringComparison.Ordinal))
		//				exceptions.Add(argumentException);
		//		}
		//	}

		//	if(exceptions.Count == 3)
		//		throw exceptions.First();
		//}

		//[TestMethod]
		//[ExpectedException(typeof(InvalidOperationException))]
		//public void ValidateFilePath_IfTheDirectoryPathParameterAndTheFilePathParameterAreEqual_ShouldThrowAnInvalidOperationException()
		//{
		//	const string directoryPath = @"C:\Some-directory";

		//	try
		//	{
		//		this.PackageTransformer.ValidateFilePath("Test", directoryPath, directoryPath);
		//	}
		//	catch(InvalidOperationException invalidOperationException)
		//	{
		//		if(string.Equals(invalidOperationException.Message, "The directory-path and file-path can not be equal.", StringComparison.Ordinal))
		//			throw;
		//	}
		//}

		[Fact]
		public async Task ValidateFilePath_IfTheFilePathParameterIsARelativePath_ShouldNotThrowAnException()
		{
			await this.ValidateFilePathShouldNotThrowAnException("Text.txt");
			await this.ValidateFilePathShouldNotThrowAnException("Directory/Text.txt");

			if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				await this.ValidateFilePathShouldNotThrowAnException("/Text.txt");
				await this.ValidateFilePathShouldNotThrowAnException(@"\Text.txt");
				await this.ValidateFilePathShouldNotThrowAnException(@"Directory\Text.txt");
				await this.ValidateFilePathShouldNotThrowAnException("/Directory/Text.txt");
				await this.ValidateFilePathShouldNotThrowAnException(@"\Directory\Text.txt");
			}
		}

		[Fact]
		public async Task ValidateFilePath_IfTheFilePathParameterIsNull_ShouldNotThrowAnException()
		{
			await this.ValidateFilePathShouldNotThrowAnException(null);
		}

		[Fact]
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