using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinySM;

namespace Simple
{
	public class TestClass 
	{
		public int IntValue;
	}

	[TestClass]
	public class ClassStateMachineTests : SimpleTemplateTestBase<TestClass>
	{
		protected override SimpleState<TestClass> RandomSimpleState()
		{
			return new SimpleState<TestClass>(new TestClass { IntValue = Random.Next() });
		}
	}
}
