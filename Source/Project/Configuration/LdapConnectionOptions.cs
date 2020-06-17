using System;
using System.DirectoryServices.Protocols;

namespace RegionOrebroLan.DirectoryServices.Protocols.Configuration
{
	public class LdapConnectionOptions
	{
		#region Properties

		public virtual AuthType? AuthenticationType { get; set; }
		public virtual CredentialOptions Credential { get; set; }
		public virtual DirectoryIdentifierOptions DirectoryIdentifier { get; set; }
		public virtual int? ProtocolVersion { get; set; }
		public virtual TimeSpan? Timeout { get; set; }

		#endregion
	}
}