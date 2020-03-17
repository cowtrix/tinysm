using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TinySM
{
	public class ProxyState<Tin, TOut> : State<Tin, TOut>
	{
		[JsonIgnore]
		public override List<Transition<Tin, TOut>> Transitions => null;

		public Reference<State<Tin, TOut>> State { get; set; }

		public override StepResult<Tin, TOut> OnEntry(Tin input)
		{
			return State.Value.OnEntry(input);
		}

		public override Task<StepResult<Tin, TOut>> OnEntryAsync(Tin input)
		{
			return State.Value.OnEntryAsync(input);
		}

		public override void OnExit(Tin input, State<Tin, TOut> next)
		{
			State.Value.OnExit(input, next);
		}

		public override StepResult<Tin, TOut> OnReentry(Tin input)
		{
			return State.Value.OnReentry(input);
		}

		public override Task<StepResult<Tin, TOut>> OnReentryAsync(Tin input)
		{
			return State.Value.OnReentryAsync(input);
		}
	}
}
