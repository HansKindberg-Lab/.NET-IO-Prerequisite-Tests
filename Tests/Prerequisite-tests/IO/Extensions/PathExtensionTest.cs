using System.Runtime.InteropServices;
using Project.IO.Extensions;
using Xunit;

namespace PrerequisiteTests.IO.Extensions
{
	public class PathExtensionTest
	{
		#region Methods

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