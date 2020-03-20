using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiReferenceTracker : LevelSingleton<UiReferenceTracker> 
{
	public StateElement StatePrefab;
	public RectTransform GraphContainer;
	public Skin Skin;
	public GameObject StateDivider;
	public List<GameObject> Fields;
	public Button DefaultButton;
	public InputField DefaultInputField;
}
