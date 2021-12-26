using System;
using System.DirectoryServices.Protocols;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.DirectoryServices.Protocols.Configuration;

namespace UnitTests.Configuration
{
	[TestClass]
	public class LdapConnectionStringParserTest
	{
		#region Methods

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Parse_IfValueContainBothServerAndServer_ShouldThrowAnInvalidOperationException()
		{
			const string value = "Server=local.net;Servers=local.net,domain.net";

			try
			{
				new LdapConnectionStringParser().Parse(value);
			}
			catch(InvalidOperationException invalidOperationException)
			{
				if(invalidOperationException.Message.Equals($"Could not parse the value \"{value}\" to ldap-connection-options.", StringComparison.OrdinalIgnoreCase))
					throw;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Parse_IfValueContainsAnInvalidKey_ShouldThrowAnInvalidOperationException()
		{
			const string value = "Invalid-key=value";

			try
			{
				new LdapConnectionStringParser().Parse(value);
			}
			catch(InvalidOperationException invalidOperationException)
			{
				if(invalidOperationException.Message.Equals($"Could not parse the value \"{value}\" to ldap-connection-options.", StringComparison.OrdinalIgnoreCase))
					throw;
			}
		}

		[TestMethod]
		public void Parse_IfValueHasOnlyEmpty_ShouldReturnALdapConnectionOptionsInstanceWithoutPropertiesSet()
		{
			var ldapConnectionOptions = new LdapConnectionStringParser().Parse(" ;;  ;");
			Assert.IsNotNull(ldapConnectionOptions);
			Assert.IsNull(ldapConnectionOptions.AuthenticationType);
			Assert.IsNull(ldapConnectionOptions.Credential);
			Assert.IsNull(ldapConnectionOptions.DirectoryIdentifier);
			Assert.IsNull(ldapConnectionOptions.ProtocolVersion);
			Assert.IsNull(ldapConnectionOptions.Timeout);
		}

		[TestMethod]
		public void Parse_IfValueIsAnEmptyString_ShouldReturnALdapConnectionOptionsInstanceWithoutPropertiesSet()
		{
			var ldapConnectionOptions = new LdapConnectionStringParser().Parse(string.Empty);
			Assert.IsNotNull(ldapConnectionOptions);
			Assert.IsNull(ldapConnectionOptions.AuthenticationType);
			Assert.IsNull(ldapConnectionOptions.Credential);
			Assert.IsNull(ldapConnectionOptions.DirectoryIdentifier);
			Assert.IsNull(ldapConnectionOptions.ProtocolVersion);
			Assert.IsNull(ldapConnectionOptions.Timeout);
		}

		[TestMethod]
		public void Parse_IfValueIsNull_ShouldReturnNull()
		{
			Assert.IsNull(new LdapConnectionStringParser().Parse(null));
		}

		[TestMethod]
		public void Parse_IfValueIsWhitespace_ShouldReturnALdapConnectionOptionsInstanceWithoutPropertiesSet()
		{
			var ldapConnectionOptions = new LdapConnectionStringParser().Parse("   ");
			Assert.IsNotNull(ldapConnectionOptions);
			Assert.IsNull(ldapConnectionOptions.AuthenticationType);
			Assert.IsNull(ldapConnectionOptions.Credential);
			Assert.IsNull(ldapConnectionOptions.DirectoryIdentifier);
			Assert.IsNull(ldapConnectionOptions.ProtocolVersion);
			Assert.IsNull(ldapConnectionOptions.Timeout);
		}

		[TestMethod]
		public void Parse_Test()
		{
			var ldapConnectionOptions = new LdapConnectionStringParser().Parse("AuthenticationType=Basic;Connectionless=true;FullyQualifiedDnsHostName=true;Password=P@ssword12;Port=1234;Server=local.net;ProtocolVersion=2;Timeout=10;UserName=User-name");
			Assert.IsNotNull(ldapConnectionOptions);
			Assert.AreEqual(AuthType.Basic, ldapConnectionOptions.AuthenticationType);
			Assert.AreEqual(null, ldapConnectionOptions.Credential.Domain);
			Assert.AreEqual("P@ssword12", ldapConnectionOptions.Credential.Password);
			Assert.AreEqual("User-name", ldapConnectionOptions.Credential.UserName);
			Assert.AreEqual(true, ldapConnectionOptions.DirectoryIdentifier.Connectionless);
			Assert.AreEqual(true, ldapConnectionOptions.DirectoryIdentifier.FullyQualifiedDnsHostName);
			Assert.AreEqual(1234, ldapConnectionOptions.DirectoryIdentifier.Port);
			Assert.AreEqual(1, ldapConnectionOptions.DirectoryIdentifier.Servers.Count);
			Assert.AreEqual("local.net", ldapConnectionOptions.DirectoryIdentifier.Servers.First());
			Assert.AreEqual(2, ldapConnectionOptions.ProtocolVersion);
			Assert.AreEqual(TimeSpan.FromDays(10), ldapConnectionOptions.Timeout);

			ldapConnectionOptions = new LdapConnectionStringParser().Parse("authenticationtype=ntlm;CONNECTIONLESS=false;FullyQuAlIfIeDDnsHostName=true;Password=P@ssword12;Servers=local-1.net,local-2.net,local-3.net;ProtocolVersion=2;Timeout=10;UserName=User-name");
			Assert.IsNotNull(ldapConnectionOptions);
			Assert.AreEqual(AuthType.Ntlm, ldapConnectionOptions.AuthenticationType);
			Assert.AreEqual(null, ldapConnectionOptions.Credential.Domain);
			Assert.AreEqual("P@ssword12", ldapConnectionOptions.Credential.Password);
			Assert.AreEqual("User-name", ldapConnectionOptions.Credential.UserName);
			Assert.AreEqual(false, ldapConnectionOptions.DirectoryIdentifier.Connectionless);
			Assert.AreEqual(true, ldapConnectionOptions.DirectoryIdentifier.FullyQualifiedDnsHostName);
			Assert.IsNull(ldapConnectionOptions.DirectoryIdentifier.Port);
			Assert.AreEqual(3, ldapConnectionOptions.DirectoryIdentifier.Servers.Count);
			Assert.AreEqual("local-1.net", ldapConnectionOptions.DirectoryIdentifier.Servers.ElementAt(0));
			Assert.AreEqual("local-2.net", ldapConnectionOptions.DirectoryIdentifier.Servers.ElementAt(1));
			Assert.AreEqual("local-3.net", ldapConnectionOptions.DirectoryIdentifier.Servers.ElementAt(2));
			Assert.AreEqual(2, ldapConnectionOptions.ProtocolVersion);
			Assert.AreEqual(TimeSpan.FromDays(10), ldapConnectionOptions.Timeout);
		}

		#endregion
	}
}