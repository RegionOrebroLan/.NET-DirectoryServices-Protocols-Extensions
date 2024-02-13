namespace RegionOrebroLan.DirectoryServices.Protocols
{
	public interface IEntry
	{
		#region Properties

		IDictionary<string, IDirectoryAttribute> Attributes { get; }
		string DistinguishedName { get; }

		#endregion
	}
}