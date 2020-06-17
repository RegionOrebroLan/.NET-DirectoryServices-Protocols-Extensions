using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;

namespace RegionOrebroLan.DirectoryServices.Protocols
{
	public class FindOptions : IFindOptions
	{
		#region Constructors

		public FindOptions(LdapConnection connection)
		{
			this.Connection = connection ?? throw new ArgumentNullException(nameof(connection));
		}

		#endregion

		#region Properties

		public virtual ISet<string> Attributes { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		public virtual LdapConnection Connection { get; }
		public virtual IFilterBuilder FilterBuilder { get; } = new FilterBuilder();
		public virtual IPaging Paging { get; } = new Paging();
		public virtual string RootDistinguishedName { get; set; }
		public virtual SearchScope SearchScope { get; set; } = SearchScope.Subtree;

		#endregion
	}
}