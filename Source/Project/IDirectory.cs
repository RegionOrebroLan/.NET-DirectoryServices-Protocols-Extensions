namespace RegionOrebroLan.DirectoryServices.Protocols
{
	public interface IDirectory : IDirectory<IEntry> { }

	public interface IDirectory<out TEntry>
	{
		#region Methods

		IEnumerable<TEntry> Find(Action<IFindOptions> configureOptions);

		#endregion
	}
}