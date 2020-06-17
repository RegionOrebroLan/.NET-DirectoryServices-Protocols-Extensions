namespace RegionOrebroLan.DirectoryServices.Protocols.Configuration
{
	public class DirectoryOptions
	{
		#region Properties

		public virtual string BaseFilter { get; set; }
		public virtual PagingOptions Paging { get; set; } = new PagingOptions();
		public virtual string RootDistinguishedName { get; set; }

		#endregion
	}
}