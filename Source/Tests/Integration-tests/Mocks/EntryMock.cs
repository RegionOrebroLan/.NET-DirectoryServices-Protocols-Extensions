using System;
using System.Collections.Generic;
using System.Linq;

namespace RegionOrebroLan.DirectoryServices.Protocols.IntegrationTests.Mocks
{
	public class EntryMock : Entry, IEntryMock
	{
		#region Properties

		public virtual ISet<string> ObjectClasses => new HashSet<string>(this.Attributes["objectClass"].GetValues(typeof(string)).Cast<string>(), StringComparer.OrdinalIgnoreCase);

		#endregion
	}
}