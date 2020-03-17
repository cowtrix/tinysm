using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class LabelListField : FieldElement<List<string>>
{
	protected override EValidState State
	{
		get
		{
			return EValidState.None;
		}
	}

	public GameObject RowItem;
	List<IFieldElement<string>> m_subFields = new List<IFieldElement<string>>();

	protected override void Update()
	{
		if(m_subFields.Any())
		{
			Value = m_subFields.Select(f => f.Value).ToList();
		}
		base.Update();
	}

	public override void Bind(MemberInfo member, object context)
	{
		base.Bind(member, context);
		if(Value == null)
		{
			Value = new List<string>();
		}
		foreach(var element in Value)
		{
			Add(element);
		}
	}

	public void Add(string content = "")
	{
		var newFieldGO = Instantiate(RowItem);
		newFieldGO.SetActive(true);
		var newField = newFieldGO.GetComponent<LabelField>();
		newField.Input.text = content;
		newFieldGO.transform.SetParent(transform);
		newFieldGO.transform.localScale = Vector3.one;
		newFieldGO.GetComponentInChildren<Button>().onClick.AddListener(() => Delete(newField));
		m_subFields.Add(newField);
	}

	public void Delete(IFieldElement<string> val)
	{
		var sf = m_subFields.Single(f => f == val);
		m_subFields.Remove(sf);
		Destroy(sf.GameObject);
	}
}
