namespace RegionOrebroLan.DirectoryServices.Protocols.Configuration
{
	public abstract class LdapConnectionStringOptions
	{
		#region Properties

		protected internal virtual string? Prefix => null;

		#endregion

		#region Methods

		protected internal abstract void AddItemsToDictionary(IDictionary<string, string> dictionary);

		protected internal virtual void AddItemToDictionary(IDictionary<string, string> dictionary, string key, string value)
		{
			dictionary.Add($"{this.Prefix}{key}", value);
		}

		protected internal virtual IDictionary<string, string> CreateDictionary()
		{
			return new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		}

		protected internal virtual IDictionary<string, string> ToDictionary()
		{
			var dictionary = this.CreateDictionary();

			this.AddItemsToDictionary(dictionary);

			return dictionary;
		}

		public override string ToString()
		{
			var dictionary = this.ToDictionary();

			return string.Join(";", dictionary.Select(item => $"{item.Key}={item.Value}"));
		}

		#endregion
	}
}