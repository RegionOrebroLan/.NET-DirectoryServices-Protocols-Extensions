using System.DirectoryServices.Protocols;

namespace RegionOrebroLan.DirectoryServices.Protocols
{
	public class Paging : IPaging
	{
		#region Properties

		public virtual bool Enabled { get; set; } = true;
		public virtual int PageSize { get; set; } = new PageResultRequestControl().PageSize;

		#endregion
	}
}