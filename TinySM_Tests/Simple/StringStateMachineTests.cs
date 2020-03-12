using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TinySM;

namespace Simple
{
	[TestClass]
	public class StringStateMachineTests : SimpleTemplateTestBase<string>
	{
		

		protected override SimpleState<string> RandomSimpleState()
		{
			return new SimpleState<string>(RandomString(32));
		}
	}
}
