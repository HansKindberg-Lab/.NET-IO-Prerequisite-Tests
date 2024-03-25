using System.Runtime.InteropServices;

namespace PrerequisiteTests
{
	public class EnvironmentTest
	{
		#region Methods

		[Fact]
		public async Task NewLine_Test()
		{
			await Task.CompletedTask;

			Assert.Equal(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "\r\n" : "\n", Environment.NewLine);
		}

		#endregion
	}
}