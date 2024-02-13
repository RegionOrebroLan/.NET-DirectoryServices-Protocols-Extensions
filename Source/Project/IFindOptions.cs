using System.DirectoryServices.Protocols;

namespace RegionOrebroLan.DirectoryServices.Protocols
{
	public interface IFindOptions
	{
		#region Properties

		ISet<string> Attributes { get; }
		LdapConnection Connection { get; }
		IFilterBuilder FilterBuilder { get; }
		IPaging Paging { get; }
		string RootDistinguishedName { get; set; }
		SearchScope SearchScope { get; set; }

		#endregion
	}
}