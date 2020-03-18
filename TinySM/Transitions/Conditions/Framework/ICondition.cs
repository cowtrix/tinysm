using System;

namespace TinySM.Conditions
{
	public interface ICondition : ITrackedObject
	{
	}

	public interface ICondition<TIn, TOut> : ICondition
	{
		bool ShouldTransition(State<TIn, TOut> origin, State<TIn, TOut> destination, TIn input);
	}
}
