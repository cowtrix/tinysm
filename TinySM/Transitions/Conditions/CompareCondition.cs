using System;
using System.Linq.Expressions;

namespace TinySM.Conditions
{
	public enum EComparisonType
	{
		LessThan,
		LessThanOrEqual,
		Equal,
		GreaterThanOrEqual,
		GreaterThan,
	}

	public class CompareCondition<TIn, TOut> : Condition<TIn, TOut>
	{
		public EComparisonType Type;
		public TIn Value;

		public CompareCondition(EComparisonType type, TIn value, bool invert = false) : base(invert)
		{
			Type = type;
			Value = value;
			Invert = invert;
		}

		protected override bool ShouldTransitionInternal(IState<TIn, TOut> origin, IState<TIn, TOut> destination, TIn input)
		{
			if(Value == null)
			{
				return false;
			}
			var inputExpression = Expression.Constant(input, typeof(TIn));
			var valueExpression = Expression.Constant(Value, typeof(TIn));
			Expression finalExpression = null;
			switch (Type)
			{
				case EComparisonType.Equal:
					return Value.Equals(input);
				case EComparisonType.GreaterThan:
					finalExpression = Expression.GreaterThan(inputExpression, valueExpression);
					break;
				case EComparisonType.GreaterThanOrEqual:
					finalExpression = Expression.GreaterThanOrEqual(inputExpression, valueExpression);
					break;
				case EComparisonType.LessThan:
					finalExpression = Expression.LessThan(inputExpression, valueExpression);
					break;
				case EComparisonType.LessThanOrEqual:
					finalExpression = Expression.LessThanOrEqual(inputExpression, valueExpression);
					break;
			}
			return Expression.Lambda<Func<bool>>(finalExpression).Compile()();
		}
	}
}
