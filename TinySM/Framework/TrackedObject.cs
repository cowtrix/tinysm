using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace TinySM
{
	public abstract class TrackedObject
	{
		private static Dictionary<Guid, TrackedObject> m_map = new Dictionary<Guid, TrackedObject>();

		public virtual string Name { get; set; }

		public static T Get<T>(Guid guid) where T:TrackedObject
		{
			m_map.TryGetValue(guid, out var obj);
			return obj as T;
		}

		public static IEnumerable<T> GetAll<T>() where T : TrackedObject
		{
			return m_map.Values.OfType<T>();
		}

		public static TrackedObject Get(Guid guid)
		{
			m_map.TryGetValue(guid, out var obj);
			return obj;
		}

		public static IEnumerable<TrackedObject> GetAll(Type type)
		{
			return m_map.Values.Where(val => type.IsAssignableFrom(val.GetType()));
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
