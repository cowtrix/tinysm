using TinySM;

public interface IUIHandler
{

}

public class UIHandler<TIn, TOut> : IUIHandler
{
	public StateMachineDefinition<TIn, TOut> StateMachine;

	
}
