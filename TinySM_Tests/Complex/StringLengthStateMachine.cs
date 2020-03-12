using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;
using TinySM;

namespace Complex
{
	public class StringLengthState : State<string, int>
	{
		public override State<string, int> OnEntry(string input, out int output)
		{
			if(input == null)
			{
				output = 0;
			}
			else
			{
				output = input.Length;
			}
			return this;
		}
	}

	[TestClass]
	public class StringLengthStateMachine : TemplateTestBase<string, int>
	{
		protected override void CheckOutput(State<string, int> state, string input, int result)
		{
			Assert.AreEqual(input.Length, result);
		}

		protected override string GetInput() => RandomString(32);

		protected override State<string, int> RandomState()
		{
			return new StringLengthState();
		}
	}
}
