using System;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Options;
using RegionOrebroLan.DirectoryServices.Protocols.Configuration;

namespace RegionOrebroLan.DirectoryServices.Protocols
{
	public class LdapConnectionFactory : ILdapConnectionFactory
	{
		#region Constructors

		[CLSCompliant(false)]
		public LdapConnectionFactory(Func<LdapConnectionOptions> optionsFunction)
		{
			this.OptionsFunction = optionsFunction ?? throw new ArgumentNullException(nameof(optionsFunction));
		}

		[CLSCompliant(false)]
		public LdapConnectionFactory(IOptionsMonitor<LdapConnectionOptions> optionsMonitor)
		{
			if(optionsMonitor == null)
				throw new ArgumentNullException(nameof(optionsMonitor));

			this.OptionsFunction = () => optionsMonitor.CurrentValue;
		}

		#endregion

		#region Properties

		protected internal virtual LdapDirectoryIdentifier DefaultLdapDirectoryIdentifier { get; } = new LdapDirectoryIdentifier("Server");

		[CLSCompliant(false)]
		protected internal virtual Func<LdapConnectionOptions> OptionsFunction { get; }

		protected internal virtual int ProtocolVersion => 3;

		#endregion

		#region Methods

		public virtual LdapConnection Create()
		{
			var options = this.OptionsFunction();

			var ldapDirectoryIdentifier = this.CreateLdapDirectoryIdentifier(options.DirectoryIdentifier);
			var networkCredential = this.CreateNetworkCredential(options.Credential);

			using(var defaultLdapConnection = new LdapConnection("Server"))
			{
				var ldapConnection = new LdapConnection(ldapDirectoryIdentifier, networkCredential, options.AuthenticationType ?? defaultLdapConnection.AuthType);

				ldapConnection.SessionOptions.ProtocolVersion = options.ProtocolVersion ?? this.ProtocolVersion;

				if(options.Timeout != null)
					ldapConnection.Timeout = options.Timeout.Value;

				return ldapConnection;
			}
		}

		protected internal virtual LdapDirectoryIdentifier CreateLdapDirectoryIdentifier(DirectoryIdentifierOptions options)
		{
			return options != null
				? new LdapDirectoryIdentifier(
					options.Servers.ToArray(),
					options.Port ?? this.DefaultLdapDirectoryIdentifier.PortNumber,
					options.FullyQualifiedDnsHostName ?? this.DefaultLdapDirectoryIdentifier.FullyQualifiedDnsHostName,
					options.Connectionless ?? this.DefaultLdapDirectoryIdentifier.Connectionless)
				: null;
		}

		protected internal virtual NetworkCredential CreateNetworkCredential(CredentialOptions options)
		{
			return options != null
				? new NetworkCredential(
					options.UserName,
					new NetworkCredential("UserName", options.Password).SecurePassword,
					options.Domain)
				: null;
		}

		#endregion
	}
}