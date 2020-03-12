namespace TinySM
{
	public class SimpleState<T> : State<T, T>
	{
		public T OutputValue { get; set; }

		public SimpleState(T output)
		{
			OutputValue = output;
		}

		public override State<T, T> OnEntry(T input, out T output)
		{
			output = OutputValue;
			return this;
		}

		public override State<T, T> OnReentry(T input, out T output)
		{
			output = OutputValue;
			return this;
		}
	}
}
