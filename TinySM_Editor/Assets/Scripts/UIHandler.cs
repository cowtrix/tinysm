using System;
using System.Collections.Generic;
using TinySM;

public interface IUIHandler
{
	Type InputType { get; }
	Type OutputType { get; }
	IStateMachineDefinition DefinitionInterface { get; }
	IEnumerable<Type> StateTypes { get; }
}

public class UIHandler<TIn, TOut> : IUIHandler
{
	public StateMachineDefinition<TIn, TOut> StateMachineDefinition;

	public Type InputType => typeof(TIn);

	public Type OutputType => typeof(TOut);

	public IStateMachineDefinition DefinitionInterface => StateMachineDefinition;

	public IEnumerable<Type> StateTypes { get; private set; }

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
