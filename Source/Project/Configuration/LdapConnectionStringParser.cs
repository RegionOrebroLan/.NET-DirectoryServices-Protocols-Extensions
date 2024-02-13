using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Linq;
using RegionOrebroLan.DependencyInjection;

namespace RegionOrebroLan.DirectoryServices.Protocols.Configuration
{
	[ServiceConfiguration(ServiceType = typeof(IParser<LdapConnectionOptions>))]
	public class LdapConnectionStringParser : BasicParser<LdapConnectionOptions>
	{
		#region Methods

		protected internal virtual CredentialOptions CreateCredentialOptions(IDictionary<string, string> dictionary)
		{
			if(dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));

			CredentialOptions credentialOptions = null;

			var key = nameof(CredentialOptions.Password);

			if(dictionary.ContainsKey(key))
			{
				credentialOptions = new CredentialOptions
				{
					Password = dictionary[key]
				};

				dictionary.Remove(key);
			}

			key = nameof(CredentialOptions.UserName);

			// ReSharper disable All
			if(dictionary.ContainsKey(key))
			{
				credentialOptions ??= new CredentialOptions();

				credentialOptions.UserName = dictionary[key];

				dictionary.Remove(key);
			}
			// ReSharper restore All

			return credentialOptions;
		}

		protected internal virtual DirectoryIdentifierOptions CreateDirectoryIdentifierOptions(IDictionary<string, string> dictionary)
		{
			if(dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));

			DirectoryIdentifierOptions directoryIdentifierOptions = null;

			var key = nameof(DirectoryIdentifierOptions.Connectionless);

			if(dictionary.ContainsKey(key))
			{
				directoryIdentifierOptions = new DirectoryIdentifierOptions
				{
					Connectionless = bool.Parse(dictionary[key])
				};

				dictionary.Remove(key);
			}

			key = nameof(DirectoryIdentifierOptions.FullyQualifiedDnsHostName);

			// ReSharper disable All

			if(dictionary.ContainsKey(key))
			{
				directoryIdentifierOptions ??= new DirectoryIdentifierOptions();

				directoryIdentifierOptions.FullyQualifiedDnsHostName = bool.Parse(dictionary[key]);

				dictionary.Remove(key);
			}

			key = nameof(DirectoryIdentifierOptions.FullyQualifiedDnsHostName);

			if(dictionary.ContainsKey(key))
			{
				directoryIdentifierOptions ??= new DirectoryIdentifierOptions();

				directoryIdentifierOptions.FullyQualifiedDnsHostName = bool.Parse(dictionary[key]);

				dictionary.Remove(key);
			}

			key = nameof(DirectoryIdentifierOptions.Port);

			if(dictionary.ContainsKey(key))
			{
				directoryIdentifierOptions ??= new DirectoryIdentifierOptions();

				directoryIdentifierOptions.Port = int.Parse(dictionary[key], CultureInfo.InvariantCulture);

				dictionary.Remove(key);
			}

			key = "Server";

			if(dictionary.ContainsKey(key))
			{
				directoryIdentifierOptions ??= new DirectoryIdentifierOptions();

				directoryIdentifierOptions.Servers.Add(dictionary[key]);

				dictionary.Remove(key);
			}
			else
			{
				key = nameof(DirectoryIdentifierOptions.Servers);

				if(dictionary.ContainsKey(key))
				{
					directoryIdentifierOptions ??= new DirectoryIdentifierOptions();

					foreach(var server in dictionary[key].Split(',').Select(server => server.Trim()).Where(server => !string.IsNullOrWhiteSpace(server)))
					{
						directoryIdentifierOptions.Servers.Add(server);
					}

					dictionary.Remove(key);
				}
			}

			// ReSharper restore All

			return directoryIdentifierOptions;
		}

		protected internal virtual LdapConnectionOptions CreateLdapConnectionOptions(IDictionary<string, string> dictionary)
		{
			if(dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));

			var ldapConnectionOptions = new LdapConnectionOptions();

			var key = nameof(LdapConnectionOptions.AuthenticationType);

			if(dictionary.ContainsKey(key))
			{
				ldapConnectionOptions.AuthenticationType = (AuthType)Enum.Parse(typeof(AuthType), dictionary[key], true);
				dictionary.Remove(key);
			}

			key = nameof(LdapConnectionOptions.ProtocolVersion);

			if(dictionary.ContainsKey(key))
			{
				ldapConnectionOptions.ProtocolVersion = int.Parse(dictionary[key], CultureInfo.InvariantCulture);
				dictionary.Remove(key);
			}

			key = nameof(LdapConnectionOptions.Timeout);

			if(dictionary.ContainsKey(key))
			{
				ldapConnectionOptions.Timeout = TimeSpan.Parse(dictionary[key], CultureInfo.InvariantCulture);
				dictionary.Remove(key);
			}

			var credentialOptions = this.CreateCredentialOptions(dictionary);

			if(credentialOptions != null)
				ldapConnectionOptions.Credential = credentialOptions;

			var directoryIdentifierOptions = this.CreateDirectoryIdentifierOptions(dictionary);

			if(directoryIdentifierOptions != null)
				ldapConnectionOptions.DirectoryIdentifier = directoryIdentifierOptions;

			return ldapConnectionOptions;
		}

		public override LdapConnectionOptions Parse(string value)
		{
			if(value == null)
				return null;

			try
			{
				var dictionary = this.ParseToDictionary(value);

				var ldapConnectionOptions = this.CreateLdapConnectionOptions(dictionary);

				if(dictionary.Any())
					throw new InvalidOperationException($"The following keys/properties are not allowed: {string.Join(", ", dictionary.Keys)}");

				return ldapConnectionOptions;
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Could not parse the value \"{value}\" to ldap-connection-options.", exception);
			}
		}

		protected internal virtual IDictionary<string, string> ParseToDictionary(string value)
		{
			var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			// ReSharper disable InvertIf
			if(value != null)
			{
				foreach(var keyValuePair in value.Trim().Split(';').Select(keyValuePair => keyValuePair.Trim()).Where(keyValuePair => !string.IsNullOrWhiteSpace(keyValuePair)))
				{
					var parts = keyValuePair.Split(new[] { '=' }, 2).Select(part => part.Trim()).ToArray();
					dictionary.Add(parts[0], parts[1]);
				}
			}
			// ReSharper restore InvertIf

			return dictionary;
		}

		#endregion
	}
}