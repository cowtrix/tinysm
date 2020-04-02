using Newtonsoft.Json;
using System;

namespace TinySM.Conditions
{
	public class EqualsCondition<TIn, TOut> : Condition<TIn, TOut>
	{
		public TIn Value;

		public EqualsCondition(TIn value, bool invert = false) : base(invert)
		{
			Value = value;
			Invert = invert;
		}


		protected override bool ShouldTransitionInternal(IState<TIn, TOut> origin, IState<TIn, TOut> destination, TIn input)
		{
			if (Value == null)
			{
				return input == null;
			}
			return Value.Equals(input);
		}
	}
}
