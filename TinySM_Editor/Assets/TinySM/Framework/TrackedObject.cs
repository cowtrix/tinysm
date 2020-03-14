using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TinySM
{
	public abstract class TrackedObject
	{
		private static Dictionary<Guid, TrackedObject> m_map = new Dictionary<Guid, TrackedObject>();

		public static T Get<T>(Guid guid) where T:TrackedObject
		{
			m_map.TryGetValue(guid, out var obj);
			return obj as T;
		}

		[JsonProperty]
		public Guid GUID { get; private set; }

		public TrackedObject()
		{
			GUID = Guid.NewGuid();
			m_map[GUID] = this;
		}

		[OnDeserialized]
		internal void OnDeserializingMethod(StreamingContext context)
		{
			m_map[GUID] = this;
		}

		public override string ToString()
		{
			return GUID.ToString();
		}
	}
}
