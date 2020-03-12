namespace TinySM.Conditions
{
	public class CompareCondition<TIn, TOut> : Condition<TIn, TOut>
	{
		public enum EComparisonType
		{
			LessThan,
			LessThanOrEqual,
			Equal,
			GreaterThanOrEqual,
			GreaterThan,
		}

		public EComparisonType Type;
		public TIn Value;

		public CompareCondition(EComparisonType type, TIn value, bool invert = false) : base(invert)
		{
			Value = value;
			Invert = invert;
		}


		protected override bool ShouldTransitionInternal(State<TIn, TOut> origin, State<TIn, TOut> destination, TIn input)
		{
			if (Value == null)
			{
				return input == null;
			}
			return Value.Equals(input);
		}
	}
}
