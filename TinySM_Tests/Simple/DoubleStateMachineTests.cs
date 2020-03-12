using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinySM;

namespace Simple
{
	[TestClass]
	public class DoubleStateMachineTests : SimpleTemplateTestBase<double>
	{
		private static Random random = new Random();

		protected override SimpleState<double> RandomSimpleState()
		{
			return new SimpleState<double>(random.NextDouble());
		}
	}
}
