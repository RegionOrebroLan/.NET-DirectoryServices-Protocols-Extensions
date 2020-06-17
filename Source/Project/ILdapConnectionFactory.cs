using System.DirectoryServices.Protocols;

namespace RegionOrebroLan.DirectoryServices.Protocols
{
	public interface ILdapConnectionFactory
	{
		#region Methods

		LdapConnection Create();

		#endregion
	}
}