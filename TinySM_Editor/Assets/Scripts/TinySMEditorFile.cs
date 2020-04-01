﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TinySM;
using TinySM.Conditions;
using UnityEngine;

public class EditorFile
{
	public IStateMachineDefinition StateMachineDefinition { get; set; }
	public List<AssemblyData> LoadedAssemblies { get; set; }
	[JsonIgnore]
	public Action<AssemblyData> OnAssemblyLoaded { get; set; }

	public EditorFile()
	{
		LoadedAssemblies = new List<AssemblyData>();
	}

	public void LoadDLL(string assemblyPath)
	{
		var data = new AssemblyData(assemblyPath);
		LoadedAssemblies.Add(data);
		Debug.Log($"Loaded {data.Assembly.FullName}");
		OnAssemblyLoaded?.Invoke(data);
	}

	public IEnumerable<Type> GetTypes<T>()
	{
		return LoadedAssemblies.SelectMany(ass => ass.Types)
			.Where(t => typeof(T).IsAssignableFrom(t)).ToList();
	}

	internal ITransition DefaultTransition()
	{
		return Activator.CreateInstance(typeof(Transition<,>).MakeGenericType(StateMachineDefinition.GetType().GetGenericTypeDefinition())) as ITransition;
	}
}
