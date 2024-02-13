using RegionOrebroLan.DirectoryServices.Protocols;

namespace IntegrationTests.Mocks
{
	public interface IEntryMock : IEntry
	{
		#region Properties

		ISet<string> ObjectClasses { get; }

		#endregion
	}
}