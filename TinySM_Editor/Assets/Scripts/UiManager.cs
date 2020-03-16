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

	public List<GameObject> Fields;
	private Dictionary<Type, IFieldElement> m_fields;

	private void Awake()
	{
		m_fields = new Dictionary<Type, IFieldElement>();
		foreach (var fieldGO in Fields)
		{
			var field = fieldGO.GetComponent(typeof(IFieldElement)) as IFieldElement;
			m_fields[field.Type] = field;
		}
		Handler = new UIHandler<string, string>();

	}

	public void NewState()
	{
		var obj = Instantiate(StatePrefab.gameObject);
		obj.transform.position = transform.position;
		obj.transform.SetParent(GraphContainer);
	}

	public IFieldElement GetFieldForType(Type type)
	{
		if(m_fields.TryGetValue(type, out var field))
		{
			return field;
		}
		return null;
	}
}
