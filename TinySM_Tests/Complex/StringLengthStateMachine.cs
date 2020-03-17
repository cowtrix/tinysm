using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;
using TinySM;

namespace Complex
{
	public class StringLengthState : State<string, int>
	{
		public override StepResult<string, int> OnEntry(string input)
		{
			var result = new StepResult<string, int>()
			{
				State = this,
			};
			if(input == null)
			{
				result.Output = 0;
			}
			else
			{
				result.Output = input.Length;
			}
			return result;
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
