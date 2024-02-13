namespace RegionOrebroLan.DirectoryServices.Protocols.Configuration
{
	public class DirectoryIdentifierOptions
	{
		#region Properties

		public virtual bool? Connectionless { get; set; }
		public virtual bool? FullyQualifiedDnsHostName { get; set; }
		public virtual int? Port { get; set; }
		public virtual IList<string> Servers { get; } = new List<string>();

		#endregion
	}
}