using System.DirectoryServices.Protocols;
using RegionOrebroLan.DirectoryServices.Protocols.Configuration;

namespace RegionOrebroLan.DirectoryServices.Protocols
{
	public interface ILdapConnectionFactory
	{
		#region Methods

		LdapConnection Create(LdapConnectionOptions options);

		#endregion
	}
}