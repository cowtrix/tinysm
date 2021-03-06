﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace TinySM
{
	public interface ITrackedObject
	{
		Guid GUID { get; }
		string Name { get; }
	}

	public abstract class TrackedObject : ITrackedObject
	{
		private static Dictionary<Guid, ITrackedObject> m_map = new Dictionary<Guid, ITrackedObject>();

		public virtual string Name { get; set; }

		public static T Get<T>(Guid guid) where T : class, ITrackedObject
		{
			m_map.TryGetValue(guid, out var obj);
			return obj as T;
		}

		public static IEnumerable<T> GetAll<T>() where T : ITrackedObject
		{
			return m_map.Values.OfType<T>();
		}

		public static ITrackedObject Get(Guid guid)
		{
			m_map.TryGetValue(guid, out var obj);
			return obj;
		}

		public static IEnumerable<ITrackedObject> GetAll(Type type)
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
