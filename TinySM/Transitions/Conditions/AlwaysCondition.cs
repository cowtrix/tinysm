namespace TinySM.Conditions
{
	public class AlwaysCondition<TIn, TOut> : Condition<TIn, TOut>
	{
		public AlwaysCondition(bool invert = false) : base(invert)
		{
		}

		protected override bool ShouldTransitionInternal(State<TIn, TOut> origin, State<TIn, TOut> destination, TIn input)
		{
			return true;
		}
	}
}
