﻿using Newtonsoft.Json;
using System;
using TinySM.Conditions;

namespace TinySM
{
	public interface ITransition : ITrackedObject
	{
		IState OriginInterface { get; set; }
		IState DestinationInterface { get; set; }
		ICondition ConditionInterface { get; set; }
	}


	/// <summary>
	/// A transition represents a decision about whether to go from the OriginState to the DestinationState
	/// determined by the Condition
	/// </summary>
	/// <typeparam name="TIn"></typeparam>
	/// <typeparam name="TOut"></typeparam>
	public class Transition<TIn, TOut> : TrackedObject, ITransition
	{
		public Reference<State<TIn, TOut>> OriginState { get; set; }

		public Reference<State<TIn, TOut>> DestinationState { get; set; }

		public ICondition<TIn, TOut> Condition { get; set; }
		
		[JsonIgnore]
		public ICondition ConditionInterface { get => Condition; set => Condition = value as Condition<TIn, TOut>; }
		[JsonIgnore]
		public IState OriginInterface { get => OriginState.Value; set => OriginState = (State<TIn, TOut>)value; }
		[JsonIgnore]
		public IState DestinationInterface { get => DestinationState.Value; set => DestinationState = (State<TIn, TOut>)value; }

		public Transition() { }

		public Transition(State<TIn, TOut> origin, State<TIn, TOut> destination, ICondition<TIn, TOut> condition)
		{
			OriginState = origin;
			DestinationState = destination;
			Condition = condition;
		}

		public override string ToString()
		{
			if(Condition == null)
			{
				return "Null";
			}
			return Condition.ToString();
		}
	}
}
