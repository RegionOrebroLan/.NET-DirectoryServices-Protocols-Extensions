using RegionOrebroLan.DirectoryServices.Protocols.Configuration;

namespace UnitTests.Configuration
{
	public class LdapDirectoryIdentifierOptionsTest
	{
		#region Methods

		[Fact]
		public async Task Clone_ShouldCloneAllProperties()
		{
			await Task.CompletedTask;

			var options = new LdapDirectoryIdentifierOptions
			{
				Connectionless = true,
				FullyQualifiedDnsHostName = true,
				Port = 10,
				ServerSeparator = '|'
			};

			options.Servers.Add("A");
			options.Servers.Add("B");
			options.Servers.Add("C");

			var clone = options.Clone();

			options.Connectionless = false;
			options.FullyQualifiedDnsHostName = false;
			options.Port = 20;
			options.ServerSeparator = ',';
			options.Servers.Remove("B");
			options.Servers.Remove("C");
			options.Servers.Add("1");

			Assert.False(options.Connectionless);
			Assert.Equal(',', options.DefaultServerSeparator);
			Assert.False(options.FullyQualifiedDnsHostName);
			Assert.Equal(20, options.Port);
			Assert.Equal(',', options.ServerSeparator);
			Assert.Equal(2, options.Servers.Count);
			Assert.Equal("A", options.Servers.ElementAt(0));
			Assert.Equal("1", options.Servers.ElementAt(1));

			Assert.True(clone.Connectionless);
			Assert.Equal(',', clone.DefaultServerSeparator);
			Assert.True(clone.FullyQualifiedDnsHostName);
			Assert.Equal(10, clone.Port);
			Assert.Equal('|', clone.ServerSeparator);
			Assert.Equal(3, clone.Servers.Count);
			Assert.Equal("A", clone.Servers.ElementAt(0));
			Assert.Equal("B", clone.Servers.ElementAt(1));
			Assert.Equal("C", clone.Servers.ElementAt(2));
		}

		#endregion
	}
}