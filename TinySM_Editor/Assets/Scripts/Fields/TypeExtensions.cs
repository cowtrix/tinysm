using System;
using System.Collections.Generic;
using System.Linq;

public static class TypeExtensions
{
	private static Dictionary<string, Type> m_typeCache = new Dictionary<string, Type>();

	static TypeExtensions()
	{
		Refresh();
	}

	public static void Refresh()
	{
		var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(ass => ass.GetTypes());
		foreach(var t in allTypes)
		{
			m_typeCache[t.FullName] = t;
		}
	}

	public static IEnumerable<Type> GetTypes(string hint)
	{
		var exact = m_typeCache.SingleOrDefault(kvp => kvp.Key == hint);
		if(exact.Value != null)
		{
			return new[] { exact.Value };
		}
		return m_typeCache.Where(kvp => kvp.Key.ToLowerInvariant().Contains(hint.ToLowerInvariant()))
			.Select(kvp => kvp.Value);
	}
}
