using System;
using System.Collections.Generic;
using TinySM;
using TinySM.Conditions;

public interface IUIHandler
{
	Type InputType { get; }
	Type OutputType { get; }
	IStateMachineDefinition DefinitionInterface { get; }
	IEnumerable<Type> StateTypes { get; }
	ITransition DefaultTransition { get; }
}

public class UIHandler<TIn, TOut> : IUIHandler
{
	public StateMachineDefinition<TIn, TOut> StateMachineDefinition;

	public Type InputType => typeof(TIn);

	public Type OutputType => typeof(TOut);

	public IStateMachineDefinition DefinitionInterface => StateMachineDefinition;

	public IEnumerable<Type> StateTypes { get; private set; }

	public ITransition DefaultTransition => new Transition<TIn, TOut>();

	public UIHandler(StateMachineDefinition<TIn, TOut> definition)
	{
		StateMachineDefinition = definition;
		StateTypes = new[]
		{
			//typeof(State<,>).MakeGenericType(InputType, OutputType),
			typeof(BotState),
			typeof(ProxyState<TIn,TOut>)
		};
	}
}
