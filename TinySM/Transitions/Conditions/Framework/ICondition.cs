using System;

namespace TinySM.Conditions
{
	public interface ICondition : ITrackedObject
	{
	}

	public interface ICondition<TIn, TOut> : ICondition
	{
		bool ShouldTransition(IState<TIn, TOut> origin, IState<TIn, TOut> destination, TIn input);
	}
}
