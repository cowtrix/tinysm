using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TinySM
{
	public class StateMachine<TIn, TOut> : TrackedObject
	{
		public Reference<IStateMachineDefinition<TIn, TOut>> Definition { get; set; }
		public Reference<IState<TIn, TOut>> CurrentState { get; set; }
		public Dictionary<string, object> DataStore = new Dictionary<string, object>();

		public StateMachine(IStateMachineDefinition<TIn, TOut> definition) : base()
		{
			Definition = new Reference<IStateMachineDefinition<TIn, TOut>>(definition);
			CurrentState = definition.RootState;
		}

		public TOut Step(TIn input)
		{
			var result = Definition.Value.Step(CurrentState.Value, input);
			CurrentState = result.State;
			return result.Output;
		}

		public async Task<TOut> StepAsync(TIn input)
		{
			var result = await Definition.Value.StepAsync(CurrentState.Value, input);
			CurrentState = result.State;
			return result.Output;
		}

		public void Reset()
		{
			CurrentState = Definition.Value.RootState;
		}
	}
}
