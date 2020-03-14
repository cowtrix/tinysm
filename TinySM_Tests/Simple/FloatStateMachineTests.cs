using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinySM;

namespace Simple
{
	[TestClass]
	public class FloatStateMachineComparisonTests : ComparisonTests<float>
	{
		protected override SimpleState<float> RandomSimpleState()
		{
			return new SimpleState<float>((float)Random.NextDouble());
		}
	}

	[TestClass]
	public class FloatStateMachineTests : SimpleTemplateTestBase<float>
	{
		protected override SimpleState<float> RandomSimpleState()
		{
			return new SimpleState<float>((float)Random.NextDouble());
		}
	}
}
