using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TinySM;
using UnityEngine;

public class UiManager : LevelSingleton<UiManager>
{
	public StateElement StatePrefab;
	public RectTransform GraphContainer;
	public IUIHandler Handler;
	public Skin Skin;
	public GameObject StateDivider;
	public List<GameObject> Fields;
	private Dictionary<Type, IFieldElement> m_fields;

	public ExportWindow Export;

	private void Awake()
	{
		m_fields = new Dictionary<Type, IFieldElement>();
		foreach (var fieldGO in Fields)
		{
			var field = fieldGO.GetComponent(typeof(IFieldElement)) as IFieldElement;
			m_fields[field.Type] = field;
		}
		Handler = new UIHandler<UserMessage, ResponseMessage>(new BotSMDefinition());
	}

	public void NewState(Vector2 worldPos)
	{
		var obj = Instantiate(StatePrefab.gameObject);
		obj.transform.SetParent(GraphContainer);
		obj.transform.localScale = Vector3.one;
		obj.transform.position = worldPos;
		var state = obj.GetComponent<StateElement>();
		state.Bind((IState)Activator.CreateInstance(Handler.StateTypes.First()));
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
