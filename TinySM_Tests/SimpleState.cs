namespace TinySM
{
	public class SimpleState<T> : State<T, T>
	{
		public T OutputValue { get; set; }

		public SimpleState(T output)
		{
			OutputValue = output;
		}

		public override StepResult<T, T> OnEntry(T input)
		{
			return new StepResult<T, T>()
			{
				Output = OutputValue,
				State = this,
			};
		}

		public override StepResult<T, T> OnReentry(T input)
		{
			return new StepResult<T, T>()
			{
				Output = OutputValue,
				State = this,
			};
		}
	}
}
