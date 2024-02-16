using RegionOrebroLan.DirectoryServices.Protocols.Configuration;

namespace UnitTests.Configuration
{
	public class NetworkCredentialOptionsTest
	{
		#region Methods

		[Fact]
		public async Task Clone_ShouldCloneAllProperties()
		{
			await Task.CompletedTask;

			var options = new NetworkCredentialOptions
			{
				Domain = "example",
				JoinDomainAndUserName = true,
				Password = "Password",
				UserName = "UserName"
			};

			var clone = options.Clone();

			options.Domain = "example-2";
			options.JoinDomainAndUserName = false;
			options.Password = "Password-2";
			options.UserName = "UserName-2";

			Assert.Equal("example-2", options.Domain);
			Assert.False(options.JoinDomainAndUserName);
			Assert.Equal("Password-2", options.Password);
			Assert.Equal("UserName-2", options.UserName);

			Assert.Equal("example", clone.Domain);
			Assert.True(clone.JoinDomainAndUserName);
			Assert.Equal("Password", clone.Password);
			Assert.Equal("UserName", clone.UserName);
		}

		#endregion
	}
}