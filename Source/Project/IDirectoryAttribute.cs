using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace RegionOrebroLan.DirectoryServices.Protocols
{
	[SuppressMessage("Design", "CA1010:Generic interface should also be implemented")]
	[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
	public interface IDirectoryAttribute : IList
	{
		#region Properties

		int Capacity { get; set; }
		string Name { get; set; }

		#endregion

		#region Methods

		int Add(IEnumerable<byte> value);
		int Add(string value);
		int Add(Uri value);
		void AddRange(IEnumerable<object> values);
		void CopyTo(object[] array, int index);
		IEnumerable<object> GetValues(Type valuesType);
		void Insert(int index, IEnumerable<byte> value);
		void Insert(int index, string value);
		void Insert(int index, Uri value);

		#endregion
	}
}