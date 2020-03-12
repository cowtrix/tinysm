using Newtonsoft.Json;
using System;
using TinySM.Conditions;

namespace TinySM
{
	/// <summary>
	/// A transition represents a decision about whether to go from the OriginState to the DestinationState
	/// determined by the Condition
	/// </summary>
	/// <typeparam name="TIn"></typeparam>
	/// <typeparam name="TOut"></typeparam>
	public class Transition<TIn, TOut> : TrackedObject
	{
		public Reference<State<TIn, TOut>> OriginState { get; set; }

		public Reference<State<TIn, TOut>> DestinationState { get; set; }

		public ICondition<TIn, TOut> Condition { get; set; }

		public Transition() { }

		public Transition(State<TIn, TOut> origin, State<TIn, TOut> destination, ICondition<TIn, TOut> condition)
		{
			OriginState = origin;
			DestinationState = destination;
			Condition = condition;
		}
	}
}
