using System.DirectoryServices.Protocols;
using RegionOrebroLan.DirectoryServices.Protocols.Configuration;

namespace UnitTests.Configuration
{
	public class LdapConnectionOptionsTest
	{
		#region Methods

		[Fact]
		public async Task ToString_Default_Test()
		{
			await Task.CompletedTask;

			var options = new LdapConnectionOptions();

			Assert.Equal(string.Empty, options.ToString());
		}

		[Fact]
		public async Task ToString_MoreComplete_Test()
		{
			await Task.CompletedTask;

			var options = new LdapConnectionOptions
			{
				AuthenticationType = AuthType.Basic,
				Credential =
				{
					Domain = "example",
					JoinDomainAndUserName = true,
					Password = "P@ssword12",
					UserName = "Alice",
				},
				Identifier =
				{
					Connectionless = true,
					FullyQualifiedDnsHostName = true,
					Port = 636,
					Servers = { "dc-01.example.org", "dc-02.example.org", "dc-03.example.org", "dc-04.example.org" }
				},
				Session =
				{
					AutoReconnect = true,
					DomainName = "example.org",
					HostName = "host.example.org",
					PingKeepAliveTimeout = TimeSpan.FromMinutes(10),
					PingLimit = 10,
					PingWaitTimeout = TimeSpan.FromMinutes(8),
					Locators = LocatorFlags.DirectoryServicesPreferred | LocatorFlags.GCRequired | LocatorFlags.KdcRequired | LocatorFlags.OnlyLdapNeeded | LocatorFlags.PdcRequired | LocatorFlags.ReturnDnsName,
					ProtocolVersion = 3,
					ReferralChasing = ReferralChasingOptions.Subordinate | ReferralChasingOptions.External,
					RootDseCache = false,
					Sealing = true,
					SecureSocketLayer = true,
					Signing = false,
					Sspi = 20,
					TcpKeepAlive = false
				},
				Timeout = TimeSpan.FromMinutes(5)
			};

			Assert.Equal("AuthenticationType=Basic;Credential.Domain=example;Credential.JoinDomainAndUserName=true;Credential.Password=**********;Credential.UserName=**********;Identifier.Connectionless=true;Identifier.FullyQualifiedDnsHostName=true;Identifier.Port=636;Identifier.Servers=dc-01.example.org,dc-02.example.org,dc-03.example.org,dc-04.example.org;Session.AutoReconnect=true;Session.DomainName=example.org;Session.HostName=host.example.org;Session.Locators=DirectoryServicesPreferred|GCRequired|PdcRequired|KdcRequired|OnlyLdapNeeded|ReturnDnsName;Session.PingKeepAliveTimeout=00:10:00;Session.PingLimit=10;Session.PingWaitTimeout=00:08:00;Session.ProtocolVersion=3;Session.ReferralChasing=All;Session.RootDseCache=false;Session.Sealing=true;Session.SecureSocketLayer=true;Session.Signing=false;Session.Sspi=20;Session.TcpKeepAlive=false;Timeout=00:05:00", options.ToString());
		}

		[Fact]
		public async Task ToString_QuiteSimple_Test()
		{
			await Task.CompletedTask;

			var options = new LdapConnectionOptions
			{
				AuthenticationType = AuthType.Negotiate,
				Credential =
				{
					Domain = "example",
					Password = "P@ssword12",
					UserName = "Alice",
				},
				Identifier =
				{
					Port = 389,
					Servers = { "dc-01.example.org", "dc-02.example.org", "dc-03.example.org", "dc-04.example.org" }
				},
				Session =
				{
					ProtocolVersion = 3,
					SecureSocketLayer = true,
				},
				Timeout = TimeSpan.FromDays(1)
			};

			Assert.Equal("AuthenticationType=Negotiate;Credential.Domain=example;Credential.Password=**********;Credential.UserName=**********;Identifier.Port=389;Identifier.Servers=dc-01.example.org,dc-02.example.org,dc-03.example.org,dc-04.example.org;Session.ProtocolVersion=3;Session.SecureSocketLayer=true;Timeout=1.00:00:00", options.ToString());

			options.AuthenticationType = AuthType.Basic;
			options.AutomaticBind = false;
			options.Identifier.ServerSeparator = '|';
			options.Session.Locators = LocatorFlags.DirectoryServicesRequired | LocatorFlags.IsDnsName;
			options.Session.PingLimit = 10;

			Assert.Equal("AuthenticationType=Basic;AutomaticBind=false;Credential.Domain=example;Credential.Password=**********;Credential.UserName=**********;Identifier.Port=389;Identifier.Servers=dc-01.example.org|dc-02.example.org|dc-03.example.org|dc-04.example.org;Identifier.ServerSeparator=|;Session.Locators=DirectoryServicesRequired|IsDnsName;Session.PingLimit=10;Session.ProtocolVersion=3;Session.SecureSocketLayer=true;Timeout=1.00:00:00", options.ToString());

			options.Session.FlagSeparator = ',';
			options.Timeout = TimeSpan.FromMinutes(5);

			Assert.Equal("AuthenticationType=Basic;AutomaticBind=false;Credential.Domain=example;Credential.Password=**********;Credential.UserName=**********;Identifier.Port=389;Identifier.Servers=dc-01.example.org|dc-02.example.org|dc-03.example.org|dc-04.example.org;Identifier.ServerSeparator=|;Session.FlagSeparator=,;Session.Locators=DirectoryServicesRequired,IsDnsName;Session.PingLimit=10;Session.ProtocolVersion=3;Session.SecureSocketLayer=true;Timeout=00:05:00", options.ToString());
		}

		#endregion
	}
}