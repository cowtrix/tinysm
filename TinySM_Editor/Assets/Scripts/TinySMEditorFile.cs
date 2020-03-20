using System;
using System.Collections.Generic;
using TinySM;
using TinySM.Conditions;
using UnityEngine;

public interface IUIHandler
{
	Type InputType { get; }
	Type OutputType { get; }
	IStateMachineDefinition DefinitionInterface { get; }
	IEnumerable<Type> StateTypes { get; }
	ITransition DefaultTransition { get; }
	void TryAddType(Type type);
}

public class TinySMEditorFile<TIn, TOut> : IUIHandler
{
	public StateMachineDefinition<TIn, TOut> StateMachineDefinition;

	public Type InputType => typeof(TIn);

	public Type OutputType => typeof(TOut);

	public IStateMachineDefinition DefinitionInterface => StateMachineDefinition;

	public IEnumerable<Type> StateTypes { get; private set; }
	private List<Type> __stateTypes = new List<Type>();

	public ITransition DefaultTransition => new Transition<TIn, TOut>();

	public TinySMEditorFile(StateMachineDefinition<TIn, TOut> definition)
	{
		StateMachineDefinition = definition;
		__stateTypes = new List<Type>
		{
			typeof(ProxyState<TIn,TOut>)
		};
	}

	public void TryAddType(Type type)
	{
		if(typeof(State<TIn, TOut>).IsAssignableFrom(type))
		{
			Debug.Log($"Loaded new State {type}");
			__stateTypes.Add(type);
		}
	}
}
