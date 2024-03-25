namespace PrerequisiteTests
{
	public class EnvironmentTest
	{
		#region Methods

		[Fact]
		public async Task NewLine_Test()
		{
			await Task.CompletedTask;

			Assert.Equal("\r\n", Environment.NewLine);
		}

		#endregion
	}
}