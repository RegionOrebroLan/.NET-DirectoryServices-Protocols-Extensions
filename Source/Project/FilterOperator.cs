using System.ComponentModel;

namespace RegionOrebroLan.DirectoryServices.Protocols
{
	public enum FilterOperator
	{
		[Description("&")] And,
		[Description("!")] Not,
		[Description("|")] Or
	}
}