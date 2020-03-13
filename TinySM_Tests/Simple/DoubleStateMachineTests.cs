using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinySM;

namespace Simple
{
	[TestClass]
	public class DoubleStateMachineComparisonTests : ComparisonTests<double>
	{
		protected override SimpleState<double> RandomSimpleState()
		{
			return new SimpleState<double>(Random.NextDouble());
		}
	}

	[TestClass]
	public class DoubleStateMachineTests : SimpleTemplateTestBase<double>
	{
		protected override SimpleState<double> RandomSimpleState()
		{
			return new SimpleState<double>(Random.NextDouble());
		}
	}
}
