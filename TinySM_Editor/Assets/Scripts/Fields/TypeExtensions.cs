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
		RegisterType(typeof(string), new[] { "string" });
		RegisterType(typeof(string), new[] { "string" });
	}

	static void RegisterType(Type t, IEnumerable<string> aliases = null)
	{
		m_typeCache[t.FullName] = t;
		if(aliases == null)
		{
			return;
		}
		foreach(var alias in aliases)
		{
			m_typeCache[alias] = t;
		}
	}

	public static IEnumerable<Type> InheritanceHierarchy(this Type root)
	{
		yield return root;
		if (root.BaseType == null)
		{
			yield break;
		}
		foreach (var childType in InheritanceHierarchy(root.BaseType))
		{
			yield return childType;
		}
	}

	public static IEnumerable<Type> GetTypes(string hint)
	{
		if(string.IsNullOrEmpty(hint))
		{
			return new Type[0];
		}
		var exact = m_typeCache.SingleOrDefault(kvp => kvp.Key == hint);
		if(exact.Value != null)
		{
			return new[] { exact.Value };
		}
		return m_typeCache.Where(kvp => kvp.Key.Contains(hint))
			.Select(kvp => kvp.Value);
	}
}
