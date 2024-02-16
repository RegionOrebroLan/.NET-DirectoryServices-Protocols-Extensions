using System.DirectoryServices.Protocols;
using RegionOrebroLan.DirectoryServices.Protocols;
using RegionOrebroLan.DirectoryServices.Protocols.Configuration;
#if !NETFRAMEWORK
using System.Net;
using System.Reflection;
#endif

namespace UnitTests
{
	public class LdapConnectionFactoryTest
	{
#if !NETFRAMEWORK
		#region Fields

		private static readonly FieldInfo _ldapConnectionCredentialField = typeof(LdapConnection).GetField("_directoryCredential", BindingFlags.Instance | BindingFlags.NonPublic)!;

		#endregion
#endif

		#region Methods

		[Fact]
		public async Task Create_IfTheOptionsParameterIsNewDefault_ShouldWork()
		{
			await Task.CompletedTask;

			using(var connection = new LdapConnectionFactory().Create(new LdapConnectionOptions()))
			{
				Assert.NotNull(connection);

				var identifier = connection.Directory as LdapDirectoryIdentifier;
				Assert.NotNull(identifier);
				Assert.Empty(identifier.Servers);
				Assert.Equal(389, identifier.PortNumber);
			}
		}

		[Fact]
		public async Task Create_IfTheOptionsParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			await Task.CompletedTask;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
			var argumentNullException = Assert.Throws<ArgumentNullException>(() => new LdapConnectionFactory().Create(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
			Assert.StartsWith("Value cannot be null.", argumentNullException.Message);
			Assert.Equal("options", argumentNullException.ParamName);
		}

		[Fact]
		public async Task Create_Test()
		{
			await Task.CompletedTask;

			var options = new LdapConnectionOptionsParser().Parse("AuthenticationType=Basic;Credential.Domain=example;Credential.JoinDomainAndUserName=true;Credential.Password=P@ssword12;Credential.UserName=Alice;Identifier.Connectionless=false;Identifier.FullyQualifiedDnsHostName=true;Identifier.Port=636;Identifier.Servers=dc-01.example.org,dc-02.example.org,dc-03.example.org,dc-04.example.org;Session.AutoReconnect=true;Session.DomainName=example.org;Session.HostName=host.example.org;Session.Locators=DirectoryServicesPreferred|GCRequired|PdcRequired|KdcRequired|OnlyLdapNeeded|ReturnDnsName;Session.PingKeepAliveTimeout=00:10:00;Session.PingLimit=10;Session.PingWaitTimeout=00:00:10;Session.ProtocolVersion=3;Session.ReferralChasing=All;Session.RootDseCache=false;Session.Sealing=true;Session.SecureSocketLayer=true;Session.Signing=false;Session.Sspi=20;Session.TcpKeepAlive=false;Timeout=00:05:00");

			using(var connection = new LdapConnectionFactory().Create(options!))
			{
				Assert.NotNull(connection);

				Assert.Equal(AuthType.Basic, connection.AuthType);
				Assert.True(connection.AutoBind);
				Assert.Equal(TimeSpan.FromMinutes(5), connection.Timeout);

#if !NETFRAMEWORK
				var credential = (NetworkCredential)_ldapConnectionCredentialField.GetValue(connection)!;
				Assert.Equal(string.Empty, credential.Domain);
				Assert.Equal("P@ssword12", credential.Password);
				Assert.Equal(@"example\Alice", credential.UserName);
#endif

				var identifier = (LdapDirectoryIdentifier)connection.Directory;
				Assert.False(identifier.Connectionless);
				Assert.True(identifier.FullyQualifiedDnsHostName);
				Assert.Equal(636, identifier.PortNumber);
				Assert.Equal(4, identifier.Servers.Length);
				Assert.Equal("dc-01.example.org", identifier.Servers[0]);
				Assert.Equal("dc-02.example.org", identifier.Servers[1]);
				Assert.Equal("dc-03.example.org", identifier.Servers[2]);
				Assert.Equal("dc-04.example.org", identifier.Servers[3]);

				Assert.True(connection.SessionOptions.AutoReconnect);
				Assert.Equal("example.org", connection.SessionOptions.DomainName);
				Assert.Equal("host.example.org", connection.SessionOptions.HostName);
				Assert.Equal(LocatorFlags.DirectoryServicesPreferred | LocatorFlags.GCRequired | LocatorFlags.PdcRequired | LocatorFlags.KdcRequired | LocatorFlags.OnlyLdapNeeded | LocatorFlags.ReturnDnsName, connection.SessionOptions.LocatorFlag);
				Assert.Equal(TimeSpan.FromMinutes(10), connection.SessionOptions.PingKeepAliveTimeout);
				Assert.Equal(10, connection.SessionOptions.PingLimit);
				Assert.Equal(TimeSpan.FromSeconds(10), connection.SessionOptions.PingWaitTimeout);
				Assert.Equal(3, connection.SessionOptions.ProtocolVersion);
				Assert.Equal(ReferralChasingOptions.All, connection.SessionOptions.ReferralChasing);
				Assert.False(connection.SessionOptions.RootDseCache);
				Assert.True(connection.SessionOptions.Sealing);
				// connection.SessionOptions.SecureSocketLayer will return false even if it is set to true. Something else is set by it, but I don't know what.
				Assert.False(connection.SessionOptions.SecureSocketLayer);
				Assert.False(connection.SessionOptions.Signing);
				Assert.Equal(20, connection.SessionOptions.SspiFlag);
				Assert.False(connection.SessionOptions.TcpKeepAlive);
			}

			options!.AutomaticBind = false;
			options.Credential.JoinDomainAndUserName = false;

			using(var connection = new LdapConnectionFactory().Create(options!))
			{
				Assert.False(connection.AutoBind);
#if !NETFRAMEWORK
				var credential = (NetworkCredential)_ldapConnectionCredentialField.GetValue(connection)!;
				Assert.Equal("example", credential.Domain);
				Assert.Equal("P@ssword12", credential.Password);
				Assert.Equal("Alice", credential.UserName);
#endif
			}
		}

		#endregion
	}
}