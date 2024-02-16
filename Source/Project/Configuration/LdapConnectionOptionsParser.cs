using System.DirectoryServices.Protocols;

namespace RegionOrebroLan.DirectoryServices.Protocols.Configuration
{
	public class LdapConnectionOptionsParser : ILdapConnectionOptionsParser
	{
		#region Methods

		protected internal virtual LdapConnectionOptions CreateLdapConnectionOptions(IDictionary<string, string?> dictionary)
		{
			if(dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));

			var connection = new LdapConnectionOptions();

			var key = this.GetLdapConnectionOptionsKey(connection, nameof(LdapConnectionOptions.AuthenticationType));
			if(dictionary.TryGetValue(key, out var value) && value != null)
			{
				connection.AuthenticationType = (AuthType)Enum.Parse(typeof(AuthType), value, true);
				dictionary.Remove(key);
			}

			key = this.GetLdapConnectionOptionsKey(connection, nameof(LdapConnectionOptions.AutomaticBind));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				connection.AutomaticBind = bool.Parse(value);
				dictionary.Remove(key);
			}

			connection.Credential = this.CreateNetworkCredentialOptions(dictionary);
			connection.Identifier = this.CreateLdapDirectoryIdentifierOptions(dictionary);
			connection.Session = this.CreateLdapSessionOptions(dictionary);

			key = this.GetLdapConnectionOptionsKey(connection, nameof(LdapConnectionOptions.Timeout));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				connection.Timeout = TimeSpan.Parse(value, null);
				dictionary.Remove(key);
			}

			return connection;
		}

		protected internal virtual LdapDirectoryIdentifierOptions CreateLdapDirectoryIdentifierOptions(IDictionary<string, string?> dictionary)
		{
			if(dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));

			var identifier = new LdapDirectoryIdentifierOptions();

			var key = this.GetLdapDirectoryIdentifierOptionsKey(identifier, nameof(LdapDirectoryIdentifierOptions.Connectionless));
			if(dictionary.TryGetValue(key, out var value) && value != null)
			{
				identifier.Connectionless = bool.Parse(value);
				dictionary.Remove(key);
			}

			key = this.GetLdapDirectoryIdentifierOptionsKey(identifier, nameof(LdapDirectoryIdentifierOptions.FullyQualifiedDnsHostName));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				identifier.FullyQualifiedDnsHostName = bool.Parse(value);
				dictionary.Remove(key);
			}

			key = this.GetLdapDirectoryIdentifierOptionsKey(identifier, nameof(LdapDirectoryIdentifierOptions.Port));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				identifier.Port = int.Parse(value, null);
				dictionary.Remove(key);
			}

			key = this.GetLdapDirectoryIdentifierOptionsKey(identifier, nameof(LdapDirectoryIdentifierOptions.ServerSeparator));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				identifier.ServerSeparator = char.Parse(value);
				dictionary.Remove(key);
			}

			key = this.GetLdapDirectoryIdentifierOptionsKey(identifier, nameof(LdapDirectoryIdentifierOptions.Servers));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				identifier.Servers.Clear();
				foreach(var server in value.Split(identifier.ServerSeparator))
				{
					identifier.Servers.Add(server.Trim());
				}

				dictionary.Remove(key);
			}

			return identifier;
		}

		protected internal virtual LdapSessionOptions CreateLdapSessionOptions(IDictionary<string, string?> dictionary)
		{
			if(dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));

			var session = new LdapSessionOptions();

			var key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.FlagSeparator));
			if(dictionary.TryGetValue(key, out var value) && value != null)
			{
				session.FlagSeparator = char.Parse(value);
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.AutoReconnect));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				session.AutoReconnect = bool.Parse(value);
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.DomainName));
			if(dictionary.TryGetValue(key, out value))
			{
				session.DomainName = value;
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.HostName));
			if(dictionary.TryGetValue(key, out value))
			{
				session.HostName = value;
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.Locators));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				session.Locators = this.ParseFlag<LocatorFlags>(session.FlagSeparator, value);
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.PingKeepAliveTimeout));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				session.PingKeepAliveTimeout = TimeSpan.Parse(value, null);
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.PingLimit));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				session.PingLimit = int.Parse(value, null);
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.PingWaitTimeout));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				session.PingWaitTimeout = TimeSpan.Parse(value, null);
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.ProtocolVersion));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				session.ProtocolVersion = int.Parse(value, null);
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.ReferralChasing));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				session.ReferralChasing = this.ParseFlag<ReferralChasingOptions>(session.FlagSeparator, value);
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.ReferralHopLimit));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				session.ReferralHopLimit = int.Parse(value, null);
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.RootDseCache));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				session.RootDseCache = bool.Parse(value);
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.SaslMethod));
			if(dictionary.TryGetValue(key, out value))
			{
				session.SaslMethod = value;
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.Sealing));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				session.Sealing = bool.Parse(value);
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.SecureSocketLayer));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				session.SecureSocketLayer = bool.Parse(value);
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.SendTimeout));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				session.SendTimeout = TimeSpan.Parse(value, null);
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.Signing));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				session.Signing = bool.Parse(value);
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.Sspi));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				session.Sspi = int.Parse(value, null);
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.TcpKeepAlive));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				session.TcpKeepAlive = bool.Parse(value);
				dictionary.Remove(key);
			}

			key = this.GetLdapSessionOptionsKey(session, nameof(LdapSessionOptions.TransportLayerSecurity));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				session.TransportLayerSecurity = bool.Parse(value);
				dictionary.Remove(key);
			}

			return session;
		}

		protected internal virtual NetworkCredentialOptions CreateNetworkCredentialOptions(IDictionary<string, string?> dictionary)
		{
			if(dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));

			var credential = new NetworkCredentialOptions();

			var key = this.GetNetworkCredentialOptionsKey(credential, nameof(NetworkCredentialOptions.Domain));
			if(dictionary.TryGetValue(key, out var value))
			{
				credential.Domain = value;
				dictionary.Remove(key);
			}

			key = this.GetNetworkCredentialOptionsKey(credential, nameof(NetworkCredentialOptions.JoinDomainAndUserName));
			if(dictionary.TryGetValue(key, out value) && value != null)
			{
				credential.JoinDomainAndUserName = bool.Parse(value);
				dictionary.Remove(key);
			}

			key = this.GetNetworkCredentialOptionsKey(credential, nameof(NetworkCredentialOptions.Password));
			if(dictionary.TryGetValue(key, out value))
			{
				credential.Password = value;
				dictionary.Remove(key);
			}

			key = this.GetNetworkCredentialOptionsKey(credential, nameof(NetworkCredentialOptions.UserName));
			if(dictionary.TryGetValue(key, out value))
			{
				credential.UserName = value;
				dictionary.Remove(key);
			}

			return credential;
		}

		protected internal virtual string GetKey(string name, string? prefix)
		{
			return $"{prefix}{name}";
		}

		protected internal virtual string GetLdapConnectionOptionsKey(LdapConnectionOptions connection, string name)
		{
			return this.GetKey(name, connection.Prefix);
		}

		protected internal virtual string GetLdapDirectoryIdentifierOptionsKey(LdapDirectoryIdentifierOptions identifier, string name)
		{
			return this.GetKey(name, identifier.Prefix);
		}

		protected internal virtual string GetLdapSessionOptionsKey(LdapSessionOptions session, string name)
		{
			return this.GetKey(name, session.Prefix);
		}

		protected internal virtual string GetNetworkCredentialOptionsKey(NetworkCredentialOptions credential, string name)
		{
			return this.GetKey(name, credential.Prefix);
		}

		public virtual LdapConnectionOptions? Parse(string? value)
		{
			if(value == null)
				return null;

			try
			{
				var dictionary = this.ParseToDictionary(value);

				var connection = this.CreateLdapConnectionOptions(dictionary);

				if(dictionary.Any())
					throw new InvalidOperationException($"The following keys/properties are not allowed: {string.Join(", ", dictionary.Keys)}");

				return connection;
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException("Could not parse to ldap-connection-options.", exception);
			}
		}

		protected internal virtual T ParseFlag<T>(char flagSeparator, string value) where T : Enum
		{
			return (T)Enum.Parse(typeof(T), value.Replace(" ", string.Empty).Replace(flagSeparator, ','), true);
		}

		protected internal virtual IDictionary<string, string?> ParseToDictionary(string? value)
		{
			var dictionary = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

			// ReSharper disable InvertIf
			if(value != null)
			{
				foreach(var keyValuePair in value.Trim().Split(';').Select(keyValuePair => keyValuePair.Trim()).Where(keyValuePair => !string.IsNullOrWhiteSpace(keyValuePair)))
				{
					var parts = keyValuePair.Split(['='], 2).Select(part => part.Trim()).ToArray();
					dictionary.Add(parts[0], parts.Length > 1 ? parts[1] : null);
				}
			}
			// ReSharper restore InvertIf

			return dictionary;
		}

		#endregion
	}
}