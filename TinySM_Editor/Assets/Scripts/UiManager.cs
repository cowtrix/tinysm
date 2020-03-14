using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : LevelSingleton<UiManager>
{
	public StateElement StatePrefab;
	public RectTransform GraphContainer;

	public IUIHandler Handler;

	public Skin Skin;

	public void NewState()
	{
		var obj = Instantiate(StatePrefab.gameObject);
		obj.transform.position = transform.position;
		obj.transform.SetParent(GraphContainer);
	}
}
