using System;
using System.Linq;
using TinySM;
using UnityEngine.UI;

public class ReferenceField : FieldElement<IReference>
{
	private Text m_buttonText;

	private void Awake()
	{
		m_buttonText = GetComponentInChildren<Button>().GetComponentInChildren<Text>();
	}

	protected override EValidState State
	{
		get
		{
			if(Value.GUID == default)
			{
				m_buttonText.text = "NULL";
				return EValidState.None;
			}
			var obj = TrackedObject.Get(Value.GUID);
			if (obj == null)
			{
				m_buttonText.text = "Invalid Ref";
				return EValidState.Invalid;
			}
			m_buttonText.text = obj.Name;
			return EValidState.Valid;
		}
	}

	public void OpenPicker()
	{
		TrackedObjectPicker.LevelInstance.Pick(Type.GetGenericArguments().Single(), obj => Value = (IReference)Activator.CreateInstance(Type, obj));
	}

}