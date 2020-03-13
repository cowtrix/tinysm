using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TinySM;
using TinySM.Conditions;

namespace TinySM_Tests.Complex
{
	[TestClass]
	public class AsyncTests
	{
		public class AsyncIntMultiplierState : State<int, int>
		{
			public override State<int, int> OnEntry(int input, out int output)
			{
				throw new NotImplementedException();
			}

			public async override Task<StepResult<int, int>> OnEntryAsync(int input)
			{
				await Task.Delay(1000);
				return new StepResult<int, int>
				{
					State = this,
					Output = input / 2,
				};
			}

			public override State<int, int> OnReentry(int input, out int output)
			{
				throw new NotImplementedException();
			}

			public async override Task<StepResult<int, int>> OnReentryAsync(int input)
			{
				await Task.Delay(1000);
				return new StepResult<int, int>
				{
					State = this,
					Output = input * 2,
				};
			}
		}

		[TestMethod]
		public async Task AsyncWork()
		{
			var def = new StateMachineDefinition<int, int>();
			var root = new State<int, int>();
			var workState = new AsyncIntMultiplierState();
			def.AddState(root);
			def.AddState(workState);

			root.AddTransition(workState, new AlwaysCondition<int, int>());
			var sm = def.CreateStateMachine();

			// Entry for first time should divide input by 2
			Assert.AreEqual(2, await sm.StepAsync(4));

			// Reentry should times by 2
			Assert.AreEqual(8, await sm.StepAsync(4));
		}

	}
}
