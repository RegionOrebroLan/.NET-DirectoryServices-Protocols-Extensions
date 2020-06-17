using System.Collections.Generic;

namespace RegionOrebroLan.DirectoryServices.Protocols.IntegrationTests.Mocks
{
	public interface IEntryMock : IEntry
	{
		#region Properties

		ISet<string> ObjectClasses { get; }

		#endregion
	}
}