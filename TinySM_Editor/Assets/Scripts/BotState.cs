using System.Collections.Generic;
using TinySM;

public class UserMessage
{
	public string InputString;
}

public class ResponseMessage
{
	public string Response;
	public List<string> Choices;
}

public class BotState : State<UserMessage, ResponseMessage>
{
	public string Name;
	public ResponseMessage Entry = new ResponseMessage();
}

public class BotSMDefinition : StateMachineDefinition<UserMessage, ResponseMessage>
{
}
