using System.Runtime.InteropServices;

namespace PrerequisiteTests
{
	public class FileTest
	{
		#region Methods

		private static async Task<string> GetResourceFilePath(string fileName)
		{
			await Task.CompletedTask;

			return Path.Combine(Global.ProjectDirectory.FullName, "Resources", "PathTest", fileName);
		}

		[Fact]
		public async Task ReadAllText_ANewLineIsDifferentOnWindowsComparedToLinuxAndMacOs()
		{
			var path = await GetResourceFilePath("Text-file.txt");
			var content = File.ReadAllText(path);

			Assert.Equal(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 10 : 7, content.Length);
		}

		#endregion
	}
}