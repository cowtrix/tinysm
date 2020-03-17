using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TinySM
{
	public struct StepResult<TIn, TOut>
	{
		public State<TIn, TOut> State;
		public TOut Output;
	}

	public interface IStateMachineDefinition
	{
		Guid GUID { get; }
		IEnumerable<IState> StateInterfaces { get; }
		IState AddState(IState obj);
	}

	public class StateMachineDefinition<TIn, TOut> : TrackedObject, IStateMachineDefinition
	{
		[JsonProperty]
		public List<State<TIn, TOut>> States { get; private set; }
		[JsonProperty]
		public Reference<State<TIn, TOut>> RootState { get; private set; }
		[JsonIgnore]
		public IEnumerable<IState> StateInterfaces => States.Cast<IState>();

		/// <summary>
		/// Constructor should only be called by Json deserializer
		/// </summary>
		internal StateMachineDefinition() : base() 
		{
			States = new List<State<TIn, TOut>>();
		}

		public StateMachineDefinition(State<TIn, TOut> root = null) : this()
		{
			RootState = root;
			AddState(root);
		}

		public IState AddState(IState state)
		{
			if(!(state is State<TIn, TOut> typedState))
			{
				throw new InvalidCastException();
			}
			return AddState(typedState);
		}

		public State<TIn, TOut> AddState(State<TIn, TOut> state)
		{
			if(state == null)
			{
				return null;
			}
			if(States.Any(s => s.GUID == state.GUID))
			{
				// We already have this state registered
				return state;
			}
			if(RootState.Value == null)
			{
				// Set this to be the root state if none exists
				RootState = state;
			}
			state.Definition = this;
			States.Add(state);
			return state;
		}

		public StepResult<TIn, TOut> Step(State<TIn, TOut> startingState, TIn input)
		{
			var transition = startingState.Transitions
				.FirstOrDefault(t => t.Condition.ShouldTransition(startingState, t.DestinationState, input));
			if(transition == null)
			{
				return startingState.OnReentry(input);
			}
			startingState.OnExit(input, transition.DestinationState);
			return transition.DestinationState.Value.OnEntry(input);
		}

		public async Task<StepResult<TIn, TOut>> StepAsync(State<TIn, TOut> startingState, TIn input)
		{
			var transition = startingState.Transitions
				.FirstOrDefault(t => t.Condition.ShouldTransition(startingState, t.DestinationState, input));
			if (transition == null)
			{
				return await startingState.OnReentryAsync(input);
			}
			startingState.OnExit(input, transition.DestinationState);
			return await transition.DestinationState.Value.OnEntryAsync(input);
		}

		public StateMachine<TIn, TOut> CreateStateMachine()
		{
			return new StateMachine<TIn, TOut>(this);
		}
	}
}
