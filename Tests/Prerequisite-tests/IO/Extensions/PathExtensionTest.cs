using Project.IO.Extensions;

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
		}

		#endregion
	}
}