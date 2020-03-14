using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EValidState
{
	None, Valid, Invalid
}

public abstract class FieldElement<T> : MonoBehaviour
{
	public InputField.ContentType InputType;
	public Text Label;
	public InputField Input;

	protected virtual void Update()
	{
		Input.contentType = InputType;
		switch (State)
		{
			case EValidState.None:
				Input.image.color = UiManager.LevelInstance.Skin.NeutralColor;
				break;
			case EValidState.Valid:
				Input.image.color = UiManager.LevelInstance.Skin.GoodColor;
				break;
			case EValidState.Invalid:
				Input.image.color = UiManager.LevelInstance.Skin.BadColor;
				break;
		}
	}

	public abstract T Value { get; set; }

	protected abstract EValidState State { get; }
}
