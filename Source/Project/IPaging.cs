namespace RegionOrebroLan.DirectoryServices.Protocols
{
	public interface IPaging
	{
		#region Properties

		bool Enabled { get; set; }
		int PageSize { get; set; }

		#endregion
	}
}