using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinySM.Conditions;

namespace TinySM
{
	public interface IState : ITrackedObject
	{
		IStateMachineDefinition DefinitionInterface { get; }
		IEnumerable<ITransition> TransitionInterfaces { get; }
	}

	public interface IState<TIn, TOut> : IState
	{
		IStateMachineDefinition<TIn, TOut> Definition { get; set; }
		List<Transition<TIn, TOut>> Transitions { get; }
		StepResult<TIn, TOut> OnEntry(TIn input);
		StepResult<TIn, TOut> OnReentry(TIn input);
		Task<StepResult<TIn, TOut>> OnEntryAsync(TIn input);
		Task<StepResult<TIn, TOut>> OnReentryAsync(TIn input);
		void OnExit(TIn input, IState<TIn, TOut> next);
	}

	public delegate void InputEvent<TIn>(TIn input);

	/// <summary>
	/// A State receives some input of type TIn and emits some output of type TOut
	/// </summary>
	/// <typeparam name="TIn"></typeparam>
	/// <typeparam name="TOut"></typeparam>
	public class State<TIn, TOut> : TrackedObject, IState<TIn, TOut>
	{
		public virtual List<Transition<TIn, TOut>> Transitions { get; private set; }

		[JsonIgnore]
		public IStateMachineDefinition<TIn, TOut> Definition 
		{ 
			get => m_definition.Value; 
			set => m_definition = new Reference<IStateMachineDefinition<TIn, TOut>>(value); 
		}
		[JsonIgnore]
		public IStateMachineDefinition DefinitionInterface => Definition;
		[JsonIgnore]
		public IEnumerable<ITransition> TransitionInterfaces => Transitions?.Cast<ITransition>();
		[JsonIgnore]
		public InputEvent<TIn> OnEntryEvent;
		[JsonIgnore]
		public InputEvent<TIn> OnReentryEvent;
		[JsonProperty]
		private Reference<IStateMachineDefinition<TIn, TOut>> m_definition;

		public State()
		{
			Transitions = new List<Transition<TIn, TOut>>();
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
		public virtual void OnExit(TIn input, IState<TIn, TOut> next) { }
	}
}
