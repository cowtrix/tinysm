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

	public interface IStateMachineDefinition : ITrackedObject
	{
		IEnumerable<IState> StateInterfaces { get; }
		IState AddState(IState obj);
		void RemoveState(IState state);
	}

	public interface IStateMachineDefinition<TIn, TOut> : IStateMachineDefinition
	{
		Reference<IState<TIn, TOut>> RootState { get; }
		List<IState<TIn, TOut>> States { get; }
		StepResult<TIn, TOut> Step(IState<TIn, TOut> value, TIn input);
		Task<StepResult<TIn, TOut>> StepAsync(IState<TIn, TOut> startingState, TIn input);
	}

	public class StateMachineDefinition<TIn, TOut> : TrackedObject, IStateMachineDefinition<TIn, TOut>
	{
		[JsonProperty]
		public List<IState<TIn, TOut>> States { get; private set; }
		[JsonProperty]
		public Reference<IState<TIn, TOut>> RootState { get; private set; }
		[JsonIgnore]
		public IEnumerable<IState> StateInterfaces => States.Cast<IState>();

		/// <summary>
		/// Constructor should only be called by Json deserializer
		/// </summary>
		public StateMachineDefinition() : base() 
		{
			States = new List<IState<TIn, TOut>>();
		}

		public StateMachineDefinition(IState<TIn, TOut> root = null) : this()
		{
			RootState = new Reference<IState<TIn, TOut>>(root);
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

		public IState<TIn, TOut> AddState(IState<TIn, TOut> state)
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
				RootState = new Reference<IState<TIn, TOut>>(state);
			}
			state.Definition = this;
			States.Add(state);
			return state;
		}

		public StepResult<TIn, TOut> Step(IState<TIn, TOut> startingState, TIn input)
		{
			var transition = startingState.Transitions
				.FirstOrDefault(t => t.Condition.ShouldTransition(startingState, t.DestinationState.Value, input));
			if(transition == null)
			{
				return startingState.OnReentry(input);
			}
			startingState.OnExit(input, transition.DestinationState.Value);
			return transition.DestinationState.Value.OnEntry(input);
		}

		public async Task<StepResult<TIn, TOut>> StepAsync(IState<TIn, TOut> startingState, TIn input)
		{
			var transition = startingState.Transitions
				.FirstOrDefault(t => t.Condition.ShouldTransition(startingState, t.DestinationState.Value, input));
			if (transition == null)
			{
				return await startingState.OnReentryAsync(input);
			}
			var destination = transition.DestinationState.Value;
			startingState.OnExit(input, destination);
			return await destination.OnEntryAsync(input);
		}

		public void RemoveState(IState state)
		{
			States = States.Where(s => s.GUID != state.GUID).ToList();
		}
	}
}
