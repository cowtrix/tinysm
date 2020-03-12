using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinySM;

namespace Simple
{
	public struct TestStruct
	{
		public string MyValue;
	}

	[TestClass]
	public class StructStateMachineTests : SimpleTemplateTestBase<TestStruct>
	{
		protected override SimpleState<TestStruct> RandomSimpleState()
		{
			return new SimpleState<TestStruct>(new TestStruct { MyValue = RandomString(20) });
		}
	}
}
