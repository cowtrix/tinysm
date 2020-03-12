using System;

namespace TinySM
{
	public struct Reference<T> where T : TrackedObject
	{
		public Guid GUID;
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
