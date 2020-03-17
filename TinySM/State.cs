using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinySM.Conditions;

namespace TinySM
{
	public interface IState
	{
		Guid GUID { get; }
	}

	public delegate void InputEvent<TIn>(TIn input);

	/// <summary>
	/// A State receives some input of type TIn and emits some output of type TOut
	/// </summary>
	/// <typeparam name="TIn"></typeparam>
	/// <typeparam name="TOut"></typeparam>
	public class State<TIn, TOut> : TrackedObject, IState
	{
		public List<Transition<TIn, TOut>> Transitions { get; set; }
		[JsonIgnore]
		public StateMachineDefinition<TIn, TOut> Definition { get => m_definition; set => m_definition = value; }
		[JsonIgnore]
		public InputEvent<TIn> OnEntryEvent;
		[JsonIgnore]
		public InputEvent<TIn> OnReentryEvent;

		[JsonProperty]
		private Reference<StateMachineDefinition<TIn, TOut>> m_definition;

		public State()
		{
			Transitions = new List<Transition<TIn, TOut>>();
		}

		/// <summary>
		/// Add a transition from this state to another with a given condition.
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="condition"></param>
		/// <returns></returns>
		public State<TIn, TOut> AddTransition(State<TIn, TOut> destination, ICondition<TIn, TOut> condition)
		{
			if(Definition == null)
			{
				throw new Exception($"State {GUID} does not have an attached definition. Make sure you have called StateMachineDefintion<TIn, TOut>.AddState");
			}
			Definition.AddState(destination);	// We make sure the definition knows about the state
			Transitions.Add(new Transition<TIn, TOut>(this, destination, condition));
			return this;
		}

		/// <summary>
		/// Attempts to find the last added state adds a transition to it
		/// </summary>
		/// <param name="condition"></param>
		/// <returns></returns>
		public State<TIn, TOut> AddTransitionToPrevious(ICondition<TIn, TOut> condition)
		{
			var destination = Definition.States.Last(s => s.GUID != GUID);
			Transitions.Add(new Transition<TIn, TOut>(this, destination, condition));
			return this;
		}

		/// <summary>
		/// Fired when the state is entered for the first time
		/// </summary>
		/// <param name="input">The input to the state</param>
		/// <param name="output">The output of the state</param>
		/// <returns>Itself</returns>
		public virtual StepResult<TIn, TOut> OnEntry(TIn input)
		{
			OnEntryEvent?.Invoke(input);
			return default;
		}

		/// <summary>
		/// Fired when the state is reentered
		/// </summary>
		/// <param name="input">The input to the state</param>
		/// <param name="output">The output of the state</param>
		/// <returns>Itself</returns>
		public virtual StepResult<TIn, TOut> OnReentry(TIn input)
		{
			OnReentryEvent?.Invoke(input);
			return default;
		}

		/// <summary>
		/// Fired when the state is entered for the first time
		/// </summary>
		/// <param name="input">The input to the state</param>
		/// <param name="output">The output of the state</param>
		/// <returns>Itself</returns>
		public virtual async Task<StepResult<TIn, TOut>> OnEntryAsync(TIn input)
		{
			return OnEntry(input);
		}

		/// <summary>
		/// Fired when the state is reentered
		/// </summary>
		/// <param name="input">The input to the state</param>
		/// <param name="output">The output of the state</param>
		/// <returns>Itself</returns>
		public virtual async Task<StepResult<TIn, TOut>> OnReentryAsync(TIn input)
		{
			return OnReentry(input);
		}

		/// <summary>
		/// Fired when the state is left to another state
		/// </summary>
		/// <param name="input">The input to the next state</param>
		public virtual void OnExit(TIn input, State<TIn, TOut> next) { }
	}
}
