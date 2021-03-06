﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
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

		protected abstract IState<TIn, TOut> RandomState();

		/// <summary>
		/// Do a round-trip serialization and check that nothing changes
		/// </summary>
		[TestMethod]
		public void CanSerializeDefinition()
		{
			var rootState = RandomState();
			var def = new StateMachineDefinition<TIn, TOut>(rootState);
			var secondState = def.AddState(RandomState());
			rootState.AddTransition(secondState, new AlwaysCondition<TIn, TOut>());
			secondState.AddTransition(rootState, new AlwaysCondition<TIn, TOut>());

			var jsonSettings = new JsonSerializerSettings()
			{
				TypeNameHandling = TypeNameHandling.Auto,
				Formatting = Formatting.Indented
			};

			var json = JsonConvert.SerializeObject(def, jsonSettings);
			var des = JsonConvert.DeserializeObject<StateMachineDefinition<TIn, TOut>>(json, jsonSettings);
			Assert.AreEqual(json, JsonConvert.SerializeObject(des, jsonSettings));
		}

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
		/// Setup SM with two states and move from the first to the second
		/// </summary>
		[TestMethod]
		public async Task Async_CanStepOnceWithAlwaysCondition()
		{
			var sm = new StateMachineDefinition<TIn, TOut>()
				.AddState(RandomState())
					.AddTransition(RandomState(), new AlwaysCondition<TIn, TOut>())
				.Definition.CreateStateMachine();
			var root = sm.CurrentState.Value.GUID;
			await sm.StepAsync(default);
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

		/// <summary>
		/// Setup SM with two cycling states and check we go back and forth between them
		/// </summary>
		[TestMethod]
		public async Task Async_CanStepCycleWithAlwaysCondition()
		{
			var rootState = RandomState();
			var def = new StateMachineDefinition<TIn, TOut>(rootState);
			var secondState = def.AddState(RandomState());
			rootState.AddTransition(secondState, new AlwaysCondition<TIn, TOut>());
			secondState.AddTransition(rootState, new AlwaysCondition<TIn, TOut>());

			var sm = def.CreateStateMachine();
			var root = sm.CurrentState.Value;
			var input = GetInput();
			CheckOutput(secondState, input, await sm.StepAsync(input));
			Assert.AreEqual(secondState.GUID, sm.CurrentState.GUID);

			CheckOutput(root, input, await sm.StepAsync(input));
			Assert.AreEqual(root.GUID, sm.CurrentState.GUID);
		}

		protected abstract TIn GetInput();

		protected abstract void CheckOutput(IState<TIn, TOut> state, TIn input, TOut result);
	}

}
