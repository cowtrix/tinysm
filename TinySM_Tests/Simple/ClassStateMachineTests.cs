using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinySM;

namespace Simple
{
	public class TestClass { }

	[TestClass]
	public class ClassStateMachineTests : SimpleTemplateTestBase<TestClass>
	{
		protected override SimpleState<TestClass> RandomSimpleState()
		{
			return new SimpleState<TestClass>(new TestClass());
		}
	}
}
