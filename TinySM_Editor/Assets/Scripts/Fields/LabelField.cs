using UnityEngine.UI;

public class LabelField : FieldElement<string>
{
	public InputField Input;

	protected override EValidState State
	{
		get
		{
			return EValidState.None;
		}
	}

	protected override void Update()
	{
		Value = Input.text;
		base.Update();
	}
}
