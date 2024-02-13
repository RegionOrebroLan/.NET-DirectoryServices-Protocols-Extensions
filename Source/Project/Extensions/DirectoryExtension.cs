namespace RegionOrebroLan.DirectoryServices.Protocols.Extensions
{
	public static class DirectoryExtension
	{
		#region Methods

		public static IEnumerable<IEntry> Find(this IDirectory directory)
		{
			return directory.Find<IEntry>();
		}

		public static IEnumerable<TEntry> Find<TEntry>(this IDirectory<TEntry> directory)
		{
			if(directory == null)
				throw new ArgumentNullException(nameof(directory));

			return directory.Find(_ => { });
		}

		#endregion
	}
}