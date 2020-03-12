using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinySM;
using TinySM.Conditions;

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

		[TestMethod]
		public void GreaterThan()
		{
			var rootState = RandomState();
			var def = new StateMachineDefinition<int, int>(rootState);
			var secondState = def.AddState(RandomState());
			rootState.AddTransition(secondState, new CompareCondition<int, int>(EComparisonType.GreaterThan, 5));

			var sm = def.CreateStateMachine();
			sm.Step(1);
			Assert.AreEqual(rootState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(5);
			Assert.AreEqual(rootState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(10);
			Assert.AreEqual(secondState.GUID, sm.CurrentState.GUID);
		}

		[TestMethod]
		public void GreaterThanOrEqual()
		{
			var rootState = RandomState();
			var def = new StateMachineDefinition<int, int>(rootState);
			var secondState = def.AddState(RandomState());
			rootState.AddTransition(secondState, new CompareCondition<int, int>(EComparisonType.GreaterThanOrEqual, 5));

			var sm = def.CreateStateMachine();
			sm.Step(1);
			Assert.AreEqual(rootState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(5);
			Assert.AreEqual(secondState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(10);
			Assert.AreEqual(secondState.GUID, sm.CurrentState.GUID);
		}

		[TestMethod]
		public void LessThan()
		{
			var rootState = RandomState();
			var def = new StateMachineDefinition<int, int>(rootState);
			var secondState = def.AddState(RandomState());
			rootState.AddTransition(secondState, new CompareCondition<int, int>(EComparisonType.LessThan, 5));

			var sm = def.CreateStateMachine();
			sm.Step(10);
			Assert.AreEqual(rootState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(5);
			Assert.AreEqual(rootState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(1);
			Assert.AreEqual(secondState.GUID, sm.CurrentState.GUID);
		}

		[TestMethod]
		public void LessThanOrEqual()
		{
			var rootState = RandomState();
			var def = new StateMachineDefinition<int, int>(rootState);
			var secondState = def.AddState(RandomState());
			rootState.AddTransition(secondState, new CompareCondition<int, int>(EComparisonType.LessThanOrEqual, 5));

			var sm = def.CreateStateMachine();
			sm.Step(10);
			Assert.AreEqual(rootState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(5);
			Assert.AreEqual(secondState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(1);
			Assert.AreEqual(secondState.GUID, sm.CurrentState.GUID);
		}

		[TestMethod]
		public void Equal()
		{
			var rootState = RandomState();
			var def = new StateMachineDefinition<int, int>(rootState);
			var secondState = def.AddState(RandomState());
			rootState.AddTransition(secondState, new CompareCondition<int, int>(EComparisonType.Equal, 5));

			var sm = def.CreateStateMachine();
			sm.Step(10);
			Assert.AreEqual(rootState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(1);
			Assert.AreEqual(rootState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(5);
			Assert.AreEqual(secondState.GUID, sm.CurrentState.GUID);

		}
	}
}
