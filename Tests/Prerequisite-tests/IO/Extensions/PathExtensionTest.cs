using System.Runtime.InteropServices;
using Project.IO.Extensions;
using Xunit;

namespace PrerequisiteTests.IO.Extensions
{
	public class PathExtensionTest
	{
		#region Methods

		[Fact]
		public async Task EnsureTrailingDirectorySeparator_IfThePathDoesNotEndWithADirectorySeparator_ShouldReturnAPathWithATrailingDirectorySeparator()
		{
			await Task.CompletedTask;

			var path = "Some-directory";
			var expected = $"{path}{(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? @"\" : "/")}";
			Assert.Equal(expected, PathExtension.EnsureTrailingDirectorySeparator(path));

			path = "Some-directory/";
			expected = path;
			Assert.Equal(expected, PathExtension.EnsureTrailingDirectorySeparator(path));

			path = "Some-directory\\";
			expected = path;
			Assert.Equal(expected, PathExtension.EnsureTrailingDirectorySeparator(path));
		}

		[Fact]
		public async Task IsPathFullyQualified_Test()
		{
			await Task.CompletedTask;

			Assert.False(PathExtension.IsPathFullyQualified("test"));
			Assert.False(PathExtension.IsPathFullyQualified("test/"));
			Assert.False(PathExtension.IsPathFullyQualified("test.txt"));

			if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				Assert.False(PathExtension.IsPathFullyQualified("/test"));
				Assert.False(PathExtension.IsPathFullyQualified("/test/"));
				Assert.False(PathExtension.IsPathFullyQualified("/test.txt"));
			}
			else
			{
				Assert.True(PathExtension.IsPathFullyQualified("/test"));
				Assert.True(PathExtension.IsPathFullyQualified("/test/"));
				Assert.True(PathExtension.IsPathFullyQualified("/test.txt"));
			}
		}

		#endregion
	}
}