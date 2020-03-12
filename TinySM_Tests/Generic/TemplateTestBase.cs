using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TinySM;
using TinySM.Conditions;

namespace Test
{
	public abstract class TemplateTestBase<TIn, TOut>
	{
		protected static Random Random = new Random();
		protected static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[Random.Next(s.Length)]).ToArray());
		}

		protected abstract State<TIn, TOut> RandomState();

		[TestMethod]
		public void CanInstantiate()
		{
			new StateMachineDefinition<TIn, TOut>().CreateStateMachine();
		}

		[TestMethod]
		public void CanInstantiateWithRootNode()
		{
			new StateMachineDefinition<TIn, TOut>()
				.AddState(RandomState())
				.Definition.CreateStateMachine();
		}

		/// <summary>
		/// Setup SM with two states and move from the first to the second
		/// </summary>
		[TestMethod]
		public void CanStepOnceWithAlwaysCondition()
		{
			var sm = new StateMachineDefinition<TIn, TOut>()
				.AddState(RandomState())
					.AddTransition(RandomState(), new AlwaysCondition<TIn, TOut>())
				.Definition.CreateStateMachine();
			var root = sm.CurrentState.Value.GUID;
			sm.Step(default);
			Assert.AreNotEqual(root, sm.CurrentState.GUID);
		}

		/// <summary>
		/// Setup SM with two cycling states and check we go back and forth between them
		/// </summary>
		[TestMethod]
		public void CanStepCycleWithAlwaysCondition()
		{
			var rootState = RandomState();
			var def = new StateMachineDefinition<TIn, TOut>(rootState);
			var secondState = def.AddState(RandomState());
			
			rootState.AddTransition(secondState, new AlwaysCondition<TIn, TOut>());
			secondState.AddTransition(rootState, new AlwaysCondition<TIn, TOut>());

			var sm = def.CreateStateMachine();
			var root = sm.CurrentState.Value;
			var input = GetInput();
			CheckOutput(secondState, input, sm.Step(input));
			Assert.AreEqual(secondState.GUID, sm.CurrentState.GUID);
			
			CheckOutput(root, input, sm.Step(input));
			Assert.AreEqual(root.GUID, sm.CurrentState.GUID);
		}

		protected abstract TIn GetInput();

		protected abstract void CheckOutput(State<TIn, TOut> state, TIn input, TOut result);
	}

}
