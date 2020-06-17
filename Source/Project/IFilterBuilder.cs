using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RegionOrebroLan.DirectoryServices.Protocols
{
	public interface IFilterBuilder
	{
		#region Properties

		IList<string> Filters { get; }

		[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
		FilterOperator Operator { get; set; }

		#endregion

		#region Methods

		string Build();

		#endregion
	}
}