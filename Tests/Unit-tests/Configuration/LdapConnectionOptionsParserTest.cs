using System.DirectoryServices.Protocols;
using RegionOrebroLan.DirectoryServices.Protocols.Configuration;

namespace UnitTests.Configuration
{
	public class LdapConnectionOptionsParserTest
	{
		#region Methods

		[Fact]
		public async Task Parse_IfValueContainsAnInvalidKey_ShouldThrowAnInvalidOperationException()
		{
			await Task.CompletedTask;

			const string value = "Invalid-key=value";

			var invalidOperationException = Assert.Throws<InvalidOperationException>(() => new LdapConnectionOptionsParser().Parse(value));
			Assert.Equal("Could not parse to ldap-connection-options.", invalidOperationException.Message);
			Assert.Equal("The following keys/properties are not allowed: Invalid-key", invalidOperationException.InnerException!.Message);
		}

		[Fact]
		public async Task Parse_IfValueContainsInvalidKeys_ShouldThrowAnInvalidOperationException()
		{
			await Task.CompletedTask;

			const string value = "Invalid-key-1=value-1;Invalid-key-2=value-2;Invalid-key-3=value-3";

			var invalidOperationException = Assert.Throws<InvalidOperationException>(() => new LdapConnectionOptionsParser().Parse(value));
			Assert.Equal("Could not parse to ldap-connection-options.", invalidOperationException.Message);
			Assert.Equal("The following keys/properties are not allowed: Invalid-key-1, Invalid-key-2, Invalid-key-3", invalidOperationException.InnerException!.Message);
		}

		[Fact]
		public async Task Parse_IfValueHasOnlyEmptyParts_ShouldReturnALdapConnectionOptionsInstance()
		{
			await Task.CompletedTask;

			var ldapConnectionOptions = new LdapConnectionOptionsParser().Parse(" ;;  ;");
			Assert.NotNull(ldapConnectionOptions);
			Assert.Null(ldapConnectionOptions.AuthenticationType);
			Assert.Null(ldapConnectionOptions.AutomaticBind);
			Assert.NotNull(ldapConnectionOptions.Credential);
			Assert.NotNull(ldapConnectionOptions.Identifier);
			Assert.Null(ldapConnectionOptions.Prefix);
			Assert.NotNull(ldapConnectionOptions.Session);
			Assert.Null(ldapConnectionOptions.Timeout);
		}

		[Fact]
		public async Task Parse_IfValueIsAnEmptyString_ShouldReturnALdapConnectionOptionsInstance()
		{
			await Task.CompletedTask;

			var ldapConnectionOptions = new LdapConnectionOptionsParser().Parse(" ;;  ;");
			Assert.NotNull(ldapConnectionOptions);
			Assert.Null(ldapConnectionOptions.AuthenticationType);
			Assert.Null(ldapConnectionOptions.AutomaticBind);
			Assert.NotNull(ldapConnectionOptions.Credential);
			Assert.NotNull(ldapConnectionOptions.Identifier);
			Assert.Null(ldapConnectionOptions.Prefix);
			Assert.NotNull(ldapConnectionOptions.Session);
			Assert.Null(ldapConnectionOptions.Timeout);
		}

		[Fact]
		public async Task Parse_IfValueIsNull_ShouldReturnNull()
		{
			await Task.CompletedTask;

			Assert.Null(new LdapConnectionOptionsParser().Parse(null));
		}

		[Fact]
		public async Task Parse_IfValueIsWhitespace_ShouldReturnALdapConnectionOptionsInstance()
		{
			await Task.CompletedTask;

			var ldapConnectionOptions = new LdapConnectionOptionsParser().Parse("   ");
			Assert.NotNull(ldapConnectionOptions);
			Assert.Null(ldapConnectionOptions.AuthenticationType);
			Assert.Null(ldapConnectionOptions.AutomaticBind);
			Assert.NotNull(ldapConnectionOptions.Credential);
			Assert.NotNull(ldapConnectionOptions.Identifier);
			Assert.Null(ldapConnectionOptions.Prefix);
			Assert.NotNull(ldapConnectionOptions.Session);
			Assert.Null(ldapConnectionOptions.Timeout);
		}

		[Fact]
		public async Task Parse_ServerSeparator_Test()
		{
			await Task.CompletedTask;

			var ldapConnectionOptions = new LdapConnectionOptionsParser().Parse("Identifier.ServerSeparator=|;Identifier.Servers=dc-01.example.org|dc-02.example.org|dc-03.example.org");
			Assert.NotNull(ldapConnectionOptions);
			Assert.Equal(3, ldapConnectionOptions.Identifier.Servers.Count);
			Assert.Equal("dc-01.example.org", ldapConnectionOptions.Identifier.Servers.First());
			Assert.Equal("dc-02.example.org", ldapConnectionOptions.Identifier.Servers.ElementAt(1));
			Assert.Equal("dc-03.example.org", ldapConnectionOptions.Identifier.Servers.Last());
		}

		[Fact]
		public async Task Parse_Test()
		{
			await Task.CompletedTask;

			var ldapConnectionOptions = new LdapConnectionOptionsParser().Parse("AuthenticationType=Basic;Credential.Password=P@ssword12;Credential.UserName=User-name;Identifier.Connectionless=true;Identifier.FullyQualifiedDnsHostName=true;Identifier.Port=1234;Identifier.Servers=local.net;Session.ProtocolVersion=2;Timeout=10");
			Assert.NotNull(ldapConnectionOptions);
			Assert.Equal(AuthType.Basic, ldapConnectionOptions.AuthenticationType);
			Assert.Null(ldapConnectionOptions.Credential.Domain);
			Assert.Equal("P@ssword12", ldapConnectionOptions.Credential.Password);
			Assert.Equal("User-name", ldapConnectionOptions.Credential.UserName);
			Assert.Equal(true, ldapConnectionOptions.Identifier.Connectionless);
			Assert.Equal(true, ldapConnectionOptions.Identifier.FullyQualifiedDnsHostName);
			Assert.Equal(1234, ldapConnectionOptions.Identifier.Port);
			Assert.Single(ldapConnectionOptions.Identifier.Servers);
			Assert.Equal("local.net", ldapConnectionOptions.Identifier.Servers.First());
			Assert.Equal(2, ldapConnectionOptions.Session.ProtocolVersion);
			Assert.Equal(TimeSpan.FromDays(10), ldapConnectionOptions.Timeout);

			ldapConnectionOptions = new LdapConnectionOptionsParser().Parse("authenticationtype=ntlm;credential.Password=P@ssword12;Credential.UserName=User-name;identiFIER.CONNECTIONLESS=false;IDENTIFIER.FullyQuAlIfIeDDnsHostName=true;Identifier.Servers=local-1.net,local-2.net,local-3.net;Session.ProtocolVersion=2;Timeout=10");
			Assert.NotNull(ldapConnectionOptions);
			Assert.Equal(AuthType.Ntlm, ldapConnectionOptions.AuthenticationType);
			Assert.Null(ldapConnectionOptions.Credential.Domain);
			Assert.Equal("P@ssword12", ldapConnectionOptions.Credential.Password);
			Assert.Equal("User-name", ldapConnectionOptions.Credential.UserName);
			Assert.Equal(false, ldapConnectionOptions.Identifier.Connectionless);
			Assert.Equal(true, ldapConnectionOptions.Identifier.FullyQualifiedDnsHostName);
			Assert.Null(ldapConnectionOptions.Identifier.Port);
			Assert.Equal(3, ldapConnectionOptions.Identifier.Servers.Count);
			Assert.Equal("local-1.net", ldapConnectionOptions.Identifier.Servers.ElementAt(0));
			Assert.Equal("local-2.net", ldapConnectionOptions.Identifier.Servers.ElementAt(1));
			Assert.Equal("local-3.net", ldapConnectionOptions.Identifier.Servers.ElementAt(2));
			Assert.Equal(2, ldapConnectionOptions.Session.ProtocolVersion);
			Assert.Equal(TimeSpan.FromDays(10), ldapConnectionOptions.Timeout);
		}

		#endregion
	}
}