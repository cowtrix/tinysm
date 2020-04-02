namespace TinySM.Conditions
{
	public class AlwaysCondition<TIn, TOut> : Condition<TIn, TOut>
	{
		public AlwaysCondition(bool invert = false) : base(invert)
		{
		}

		protected override bool ShouldTransitionInternal(IState<TIn, TOut> origin, IState<TIn, TOut> destination, TIn input)
		{
			return true;
		}

		public override string ToString()
		{
			return "Always";
		}
	}
}
