using System;
using System.Collections.Generic;

namespace RegionOrebroLan.DirectoryServices.Protocols
{
	public class Entry : IEntry
	{
		#region Properties

		public virtual IDictionary<string, IDirectoryAttribute> Attributes { get; } = new Dictionary<string, IDirectoryAttribute>(StringComparer.OrdinalIgnoreCase);
		public virtual string DistinguishedName { get; set; }

		#endregion
	}
}