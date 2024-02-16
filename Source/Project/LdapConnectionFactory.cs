using System.DirectoryServices.Protocols;
using System.Net;
using RegionOrebroLan.DirectoryServices.Protocols.Configuration;
using LdapSessionOptions = RegionOrebroLan.DirectoryServices.Protocols.Configuration.LdapSessionOptions;

namespace RegionOrebroLan.DirectoryServices.Protocols
{
	public class LdapConnectionFactory : ILdapConnectionFactory
	{
		#region Methods

		public virtual LdapConnection Create(LdapConnectionOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			using(var defaultConnection = new LdapConnection("Server"))
			{
				var defaultIdentifier = defaultConnection.Directory as LdapDirectoryIdentifier;

				var identifier = this.CreateIdentifier(defaultIdentifier, options.Identifier);

				var connection = new LdapConnection(identifier);

				if(options.AuthenticationType != null)
					connection.AuthType = options.AuthenticationType.Value;

				if(options.AutomaticBind != null)
					connection.AutoBind = options.AutomaticBind.Value;

				if(connection.AuthType != AuthType.Anonymous)
				{
					var credential = this.CreateCredential(options.Credential);
					connection.Credential = credential;
				}

				this.SetSession(connection, options.Session);

				if(options.Timeout != null)
					connection.Timeout = options.Timeout.Value;

				return connection;
			}
		}

		protected internal virtual NetworkCredential CreateCredential(NetworkCredentialOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(options.JoinDomainAndUserName != null && options.JoinDomainAndUserName.Value)
			{
				var userName = options.UserName;

				if(!string.IsNullOrEmpty(options.Domain))
					userName = options.Domain + @"\" + userName;

				return new NetworkCredential(userName, options.Password);
			}

			return new NetworkCredential(options.UserName, options.Password, options.Domain);
		}

		protected internal virtual LdapDirectoryIdentifier CreateIdentifier(LdapDirectoryIdentifier? defaultIdentifier, LdapDirectoryIdentifierOptions options)
		{
			if(defaultIdentifier == null)
				throw new ArgumentNullException(nameof(defaultIdentifier));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			return new LdapDirectoryIdentifier(options.Servers.ToArray(), options.Port ?? defaultIdentifier.PortNumber, options.FullyQualifiedDnsHostName ?? defaultIdentifier.FullyQualifiedDnsHostName, options.Connectionless ?? defaultIdentifier.Connectionless);
		}

		protected internal virtual void SetSession(LdapConnection connection, LdapSessionOptions options)
		{
			if(connection == null)
				throw new ArgumentNullException(nameof(connection));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(options.AutoReconnect != null)
				connection.SessionOptions.AutoReconnect = options.AutoReconnect.Value;

			if(options.DomainName != null)
				connection.SessionOptions.DomainName = options.DomainName;

			if(options.HostName != null)
				connection.SessionOptions.HostName = options.HostName;

			if(options.Locators != null)
				connection.SessionOptions.LocatorFlag = options.Locators.Value;

			if(options.PingKeepAliveTimeout != null)
				connection.SessionOptions.PingKeepAliveTimeout = options.PingKeepAliveTimeout.Value;

			if(options.PingLimit != null)
				connection.SessionOptions.PingLimit = options.PingLimit.Value;

			if(options.PingWaitTimeout != null)
				connection.SessionOptions.PingWaitTimeout = options.PingWaitTimeout.Value;

			if(options.ProtocolVersion != null)
				connection.SessionOptions.ProtocolVersion = options.ProtocolVersion.Value;

			if(options.QueryClientCertificateFunction != null)
				connection.SessionOptions.QueryClientCertificate = (ldapConnection, clientCertificates) => options.QueryClientCertificateFunction(ldapConnection, clientCertificates);

			if(options.ReferralCallback != null)
				connection.SessionOptions.ReferralCallback = options.ReferralCallback;

			if(options.ReferralChasing != null)
				connection.SessionOptions.ReferralChasing = options.ReferralChasing.Value;

			if(options.ReferralHopLimit != null)
				connection.SessionOptions.ReferralHopLimit = options.ReferralHopLimit.Value;

			if(options.RootDseCache != null)
				connection.SessionOptions.RootDseCache = options.RootDseCache.Value;

			if(options.SaslMethod != null)
				connection.SessionOptions.SaslMethod = options.SaslMethod;

			if(options.Sealing != null)
				connection.SessionOptions.Sealing = options.Sealing.Value;

			if(options.SecureSocketLayer != null)
				connection.SessionOptions.SecureSocketLayer = options.SecureSocketLayer.Value;

			if(options.SendTimeout != null)
				connection.SessionOptions.SendTimeout = options.SendTimeout.Value;

			if(options.Signing != null)
				connection.SessionOptions.Signing = options.Signing.Value;

			if(options.Sspi != null)
				connection.SessionOptions.SspiFlag = options.Sspi.Value;

			if(options.TcpKeepAlive != null)
				connection.SessionOptions.TcpKeepAlive = options.TcpKeepAlive.Value;

			if(options.TransportLayerSecurity != null && options.TransportLayerSecurity.Value)
				connection.SessionOptions.StartTransportLayerSecurity(null);

			if(options.VerifyServerCertificateFunction != null)
				connection.SessionOptions.VerifyServerCertificate = (ldapConnection, certificate) => options.VerifyServerCertificateFunction(ldapConnection, certificate);
		}

		#endregion
	}
}