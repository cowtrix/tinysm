using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using TinySM;
using UnityEngine;

public class AssemblyData
{
	public string Path { get; set; }
	[JsonIgnore]
	public Assembly Assembly
	{
		get
		{
			if (m_assembly == null)
			{
				m_assembly = Assembly.LoadFrom(Path);
				var types = m_assembly.GetTypes();
				foreach (var type in types)
				{
					if (!typeof(ITrackedObject).IsAssignableFrom(type) || type.IsInterface || type.IsAbstract || type.ContainsGenericParameters)
					{
						continue;
					}
					if (type.IsValueType)
					{
						PromptWindow.LevelInstance.Prompt($"Could not load type {type} as it was a struct");
					}
					Debug.Log("Discovered tracked type " + type);
					Types.Add(type);
				}
			}
			return m_assembly;
		}
	}
	[JsonIgnore]
	private Assembly m_assembly;
	[JsonIgnore]
	public List<Type> Types { get; }
	public AssemblyData(string assemblyPath)
	{
		Path = assemblyPath;
		Types = new List<Type>();
	}
}
