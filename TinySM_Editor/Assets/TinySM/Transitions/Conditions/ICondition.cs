using System;

namespace TinySM.Conditions
{
	public interface ICondition<TIn, TOut>
	{
		bool ShouldTransition(State<TIn, TOut> origin, State<TIn, TOut> destination, TIn input);
		Guid GUID { get; }
	}
}
