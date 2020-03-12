using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinySM;

namespace Simple
{
	[TestClass]
	public class IntStateMachineTests : SimpleTemplateTestBase<int>
	{
		private static Random random = new Random();

		protected override SimpleState<int> RandomSimpleState()
		{
			return new SimpleState<int>(random.Next());
		}
	}
}
