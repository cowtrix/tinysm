using Newtonsoft.Json;
using System;

namespace TinySM
{
	public interface IReference
	{
		Guid GUID { get; set; }
	}

	public struct Reference<T> : IReference where T : TrackedObject
	{
		public Guid GUID { get; set; }
		[JsonIgnore]
		public T Value => TrackedObject.Get<T>(GUID);

		public static implicit operator T(Reference<T> d) => d.Value;
		public static implicit operator Reference<T>(T b) => new Reference<T>(b);

		public Reference(T reference)
		{
			if(reference != null)
			{
				GUID = reference.GUID;
			}
		}

		public override string ToString()
		{
			return $"Ref: {Value?.ToString()}";
		}
	}
}
