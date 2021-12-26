using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.Protocols;
using System.Linq;

namespace RegionOrebroLan.DirectoryServices.Protocols
{
	[SuppressMessage("Design", "CA1010:Generic interface should also be implemented")]
	public class DirectoryAttributeWrapper : IDirectoryAttribute
	{
		#region Constructors

		public DirectoryAttributeWrapper(DirectoryAttribute directoryAttribute)
		{
			this.DirectoryAttribute = directoryAttribute ?? throw new ArgumentNullException(nameof(directoryAttribute));
		}

		#endregion

		#region Properties

		public virtual int Capacity
		{
			get => this.DirectoryAttribute.Capacity;
			set => this.DirectoryAttribute.Capacity = value;
		}

		public virtual int Count => this.DirectoryAttribute.Count;
		protected internal virtual DirectoryAttribute DirectoryAttribute { get; }
		public virtual bool IsFixedSize => ((IList)this.DirectoryAttribute).IsFixedSize;
		public virtual bool IsReadOnly => ((IList)this.DirectoryAttribute).IsReadOnly;
		public virtual bool IsSynchronized => ((IList)this.DirectoryAttribute).IsSynchronized;

		public virtual object this[int index]
		{
			get => this.DirectoryAttribute[index];
			set => this.DirectoryAttribute[index] = value;
		}

		public virtual string Name
		{
			get => this.DirectoryAttribute.Name;
			set => this.DirectoryAttribute.Name = value;
		}

		public virtual object SyncRoot => ((IList)this.DirectoryAttribute).SyncRoot;

		#endregion

		#region Methods

		public virtual int Add(IEnumerable<byte> value)
		{
			return this.DirectoryAttribute.Add(value?.ToArray());
		}

		public virtual int Add(string value)
		{
			return this.DirectoryAttribute.Add(value);
		}

		public virtual int Add(Uri value)
		{
			return this.DirectoryAttribute.Add(value);
		}

		public virtual int Add(object value)
		{
			return ((IList)this.DirectoryAttribute).Add(value);
		}

		public virtual void AddRange(IEnumerable<object> values)
		{
			// ReSharper disable AssignNullToNotNullAttribute
			this.DirectoryAttribute.AddRange(values?.ToArray());
			// ReSharper restore AssignNullToNotNullAttribute
		}

		public virtual void Clear()
		{
			this.DirectoryAttribute.Clear();
		}

		public virtual bool Contains(object value)
		{
			// ReSharper disable AssignNullToNotNullAttribute
			return this.DirectoryAttribute.Contains(value);
			// ReSharper restore AssignNullToNotNullAttribute
		}

		public virtual void CopyTo(Array array, int index)
		{
			((IList)this.DirectoryAttribute).CopyTo(array, index);
		}

		public virtual void CopyTo(object[] array, int index)
		{
			this.DirectoryAttribute.CopyTo(array, index);
		}

		public override bool Equals(object obj)
		{
			return this.DirectoryAttribute.Equals(obj);
		}

		public virtual IEnumerator GetEnumerator()
		{
			return this.DirectoryAttribute.GetEnumerator();
		}

		public override int GetHashCode()
		{
			return this.DirectoryAttribute.GetHashCode();
		}

		public virtual IEnumerable<object> GetValues(Type valuesType)
		{
			return this.DirectoryAttribute.GetValues(valuesType);
		}

		public virtual int IndexOf(object value)
		{
			return this.DirectoryAttribute.IndexOf(value);
		}

		public virtual void Insert(int index, object value)
		{
			((IList)this.DirectoryAttribute).Insert(index, value);
		}

		public virtual void Insert(int index, IEnumerable<byte> value)
		{
			this.DirectoryAttribute.Insert(index, value?.ToArray());
		}

		public virtual void Insert(int index, string value)
		{
			this.DirectoryAttribute.Insert(index, value);
		}

		public virtual void Insert(int index, Uri value)
		{
			this.DirectoryAttribute.Insert(index, value);
		}

		#region Implicit operators

		public static implicit operator DirectoryAttributeWrapper(DirectoryAttribute directoryAttribute)
		{
			return directoryAttribute != null ? new DirectoryAttributeWrapper(directoryAttribute) : null;
		}

		#endregion

		public virtual void Remove(object value)
		{
			this.DirectoryAttribute.Remove(value);
		}

		public virtual void RemoveAt(int index)
		{
			this.DirectoryAttribute.RemoveAt(index);
		}

		public static DirectoryAttributeWrapper ToDirectoryAttributeWrapper(DirectoryAttribute directoryAttribute)
		{
			return directoryAttribute;
		}

		public override string ToString()
		{
			return this.DirectoryAttribute.ToString();
		}

		#endregion
	}
}