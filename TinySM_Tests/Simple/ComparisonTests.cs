using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinySM;
using TinySM.Conditions;

namespace Simple
{
	[TestClass]
	public class ComparisonTests<T> : SimpleTemplateTestBase<T>
	{
		T C(int x)
		{
			return (T)Convert.ChangeType(x, typeof(T));
		}

		[TestMethod]
		public void GreaterThan()
		{
			var rootState = RandomState();
			var def = new StateMachineDefinition<T, T>(rootState);
			var secondState = def.AddState(RandomState());
			rootState.AddTransition(secondState, new CompareCondition<T, T>(EComparisonType.GreaterThan, C(5)));

			var sm = def.CreateStateMachine();
			sm.Step(C(1));
			Assert.AreEqual(rootState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(C(5));
			Assert.AreEqual(rootState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(C(10));
			Assert.AreEqual(secondState.GUID, sm.CurrentState.GUID);
		}

		[TestMethod]
		public void GreaterThanOrEqual()
		{
			var rootState = RandomState();
			var def = new StateMachineDefinition<T, T>(rootState);
			var secondState = def.AddState(RandomState());
			rootState.AddTransition(secondState, new CompareCondition<T, T>(EComparisonType.GreaterThanOrEqual, C(5)));

			var sm = def.CreateStateMachine();
			sm.Step(C(1));
			Assert.AreEqual(rootState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(C(5));
			Assert.AreEqual(secondState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(C(10));
			Assert.AreEqual(secondState.GUID, sm.CurrentState.GUID);
		}

		[TestMethod]
		public void LessThan()
		{
			var rootState = RandomState();
			var def = new StateMachineDefinition<T, T>(rootState);
			var secondState = def.AddState(RandomState());
			rootState.AddTransition(secondState, new CompareCondition<T, T>(EComparisonType.LessThan, C(5)));

			var sm = def.CreateStateMachine();
			sm.Step(C(10));
			Assert.AreEqual(rootState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(C(5));
			Assert.AreEqual(rootState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(C(1));
			Assert.AreEqual(secondState.GUID, sm.CurrentState.GUID);
		}

		[TestMethod]
		public void LessThanOrEqual()
		{
			var rootState = RandomState();
			var def = new StateMachineDefinition<T, T>(rootState);
			var secondState = def.AddState(RandomState());
			rootState.AddTransition(secondState, new CompareCondition<T, T>(EComparisonType.LessThanOrEqual, C(5)));

			var sm = def.CreateStateMachine();
			sm.Step(C(10));
			Assert.AreEqual(rootState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(C(5));
			Assert.AreEqual(secondState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(C(1));
			Assert.AreEqual(secondState.GUID, sm.CurrentState.GUID);
		}

		[TestMethod]
		public void Equal()
		{
			var rootState = RandomState();
			var def = new StateMachineDefinition<T, T>(rootState);
			var secondState = def.AddState(RandomState());
			rootState.AddTransition(secondState, new CompareCondition<T, T>(EComparisonType.Equal, C(5)));

			var sm = def.CreateStateMachine();
			sm.Step(C(10));
			Assert.AreEqual(rootState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(C(1));
			Assert.AreEqual(rootState.GUID, sm.CurrentState.GUID);

			sm.Reset();
			sm.Step(C(5));
			Assert.AreEqual(secondState.GUID, sm.CurrentState.GUID);

		}

		protected override SimpleState<T> RandomSimpleState()
		{
			return new SimpleState<T>(C(Random.Next()));
		}
	}
}
