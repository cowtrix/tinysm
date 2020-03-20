using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum EValidState
{
	None, Valid, Invalid
}

public interface IFieldElement<T> : IFieldElement
{
	T Value { get; set; }
}

public interface IFieldElement
{
	Type Type { get; }
	object Object { get; set; }
	void Bind(MemberInfo member, object context);
	GameObject GameObject { get; }
}

public abstract class FieldElement<T> : MonoBehaviour, IFieldElement<T>
{
	public UnityEvent OnValueChanged;
	public Text Label;
	public Image StateImage;
	public Color BaseColor = Color.white;

	private MemberInfo m_memberInfo;
	private object m_context;

	protected virtual void Update()
	{
		if(!StateImage)
		{
			return;
		}
		switch (State)
		{
			case EValidState.None:
				StateImage.color = UiReferenceTracker.LevelInstance.Skin.NeutralColor;
				break;
			case EValidState.Valid:
				StateImage.color = UiReferenceTracker.LevelInstance.Skin.GoodColor;
				break;
			case EValidState.Invalid:
				StateImage.color = UiReferenceTracker.LevelInstance.Skin.BadColor;
				break;
		}
		StateImage.color *= BaseColor;
	}

	public virtual void Bind(MemberInfo member, object context)
	{
		if (Label)
		{
			Label.text = member.Name;
		}
		m_memberInfo = member;
		m_context = context;
		OnValueChanged.RemoveAllListeners();
		Value = (T)member.GetValue(context);
		OnValueChanged.AddListener(() =>
		{
			m_memberInfo.SetValue(m_context, Value);
		});
	}

	public T Value
	{
		get
		{
			return m_value;
		}
		set
		{
			if(m_value != null && m_value.Equals(value))
			{
				return;
			}
			m_value = value;
			OnValueChanged.Invoke();
		}
	}
	private T m_value;

	protected abstract EValidState State { get; }
	public object Object
	{
		get { return Value; }
		set { Value = (T)value; }
	}
	public Type Type => m_memberInfo == null ? typeof(T) : m_memberInfo.GetMemberType();
	public GameObject GameObject => gameObject;
}
