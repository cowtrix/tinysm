using Newtonsoft.Json;
using System;

namespace TinySM.Conditions
{
	public abstract class Condition<TIn, TOut> : ICondition<TIn, TOut>
	{
		public Guid GUID { get; set; }
		public bool Invert { get; set; }
		public string Name { get; set; }

		public Condition(bool invert)
		{
			GUID = Guid.NewGuid();
			Invert = false;
		}

		public bool ShouldTransition(IState<TIn, TOut> origin, IState<TIn, TOut> destination, TIn input)
		{
			var result = ShouldTransitionInternal(origin, destination, input);
			if(Invert)
			{
				return !result;
			}
			return result;
		}

		protected abstract bool ShouldTransitionInternal(IState<TIn, TOut> origin, IState<TIn, TOut> destination, TIn input);
	}
}
