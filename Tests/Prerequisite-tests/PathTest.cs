using System.Runtime.InteropServices;

namespace PrerequisiteTests
{
	public class PathTest
	{
		#region Methods

		[Fact]
		public async Task GetFullPath_IfThePathIsADirectoryNameWithoutLeadingOrTrailingSlashes_ShouldReturnTheFullPath()
		{
			await Task.CompletedTask;

			const string directoryName = "Some-directory";

			Assert.Equal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, directoryName), Path.GetFullPath(directoryName));
		}

#if NETCOREAPP3_1_OR_GREATER
		[Fact]
		public async Task IsPathFullyQualified_Test()
		{
			await Task.CompletedTask;

			Assert.False(Path.IsPathFullyQualified("test"));
			Assert.False(Path.IsPathFullyQualified("test/"));
			Assert.False(Path.IsPathFullyQualified("test.txt"));

			if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				Assert.False(Path.IsPathFullyQualified("/test"));
				Assert.False(Path.IsPathFullyQualified("/test/"));
				Assert.False(Path.IsPathFullyQualified("/test.txt"));
			}
			else
			{
				Assert.True(Path.IsPathFullyQualified("/test"));
				Assert.True(Path.IsPathFullyQualified("/test/"));
				Assert.True(Path.IsPathFullyQualified("/test.txt"));
			}
		}
#endif

		[Fact]
		public async Task IsPathRooted_Test()
		{
			await Task.CompletedTask;

			string? path = null;
			Assert.False(Path.IsPathRooted(path));

			path = string.Empty;
			Assert.False(Path.IsPathRooted(path));

			path = "/";
			Assert.True(Path.IsPathRooted(path));

			path = @"\";
			if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				Assert.True(Path.IsPathRooted(path));
			else
				Assert.False(Path.IsPathRooted(path));
		}

		#endregion
	}
}