using System;
using System.Linq;
using TinySM.Conditions;

namespace TinySM
{
	public static class StateMachineBuilderExtensions
	{
		/// <summary>
		/// Add a transition from this state to another with a given condition.
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="condition"></param>
		/// <returns></returns>
		public static IState<TIn, TOut> AddTransition<TIn, TOut>(this IState<TIn, TOut> origin, IState<TIn, TOut> destination, ICondition<TIn, TOut> condition)
		{
			if (origin.Definition == null)
			{
				throw new Exception($"State {origin.GUID} does not have an attached definition. Make sure you have called StateMachineDefintion<TIn, TOut>.AddState");
			}
			origin.Definition.AddState(destination);   // We make sure the definition knows about the state
			origin.Transitions.Add(new Transition<TIn, TOut>(origin, destination, condition));
			return origin;
		}

		public static IState<TIn, TOut> AddTransitionToPrevious<TIn, TOut>(this IState<TIn, TOut> origin, ICondition<TIn, TOut> condition)
		{
			var destination = origin.Definition.States.Last(s => s.GUID != origin.GUID);
			return origin.AddTransition(destination, condition);
		}


		public static StateMachine<TIn, TOut> CreateStateMachine<TIn, TOut>(this IStateMachineDefinition<TIn, TOut> definition)
		{
			return new StateMachine<TIn, TOut>(definition);
		}

	}
}
