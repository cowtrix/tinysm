using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;
using TinySM;

namespace Simple
{
	public abstract class SimpleTemplateTestBase<T> : TemplateTestBase<T, T>
	{
		protected override void CheckOutput(IState<T, T> state, T input, T result)
		{
			var simpleState = state as SimpleState<T>;
			Assert.AreEqual(simpleState.OutputValue, result);
		}

		protected abstract SimpleState<T> RandomSimpleState();

		protected override IState<T, T> RandomState() => RandomSimpleState();

		protected override T GetInput() => default;
	}
}
