using RegionOrebroLan.DirectoryServices.Protocols;

namespace IntegrationTests.Mocks
{
	public class EntryMock : Entry, IEntryMock
	{
		#region Properties

		public virtual ISet<string> ObjectClasses => new HashSet<string>(this.Attributes["objectClass"].GetValues(typeof(string)).Cast<string>(), StringComparer.OrdinalIgnoreCase);

		#endregion
	}
}