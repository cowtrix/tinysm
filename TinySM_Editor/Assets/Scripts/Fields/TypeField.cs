using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class TypeField : FieldElement<Type>
{
	public InputField Input;

	protected override EValidState State
	{
		get
		{
			if(Value == null)
			{
				if (string.IsNullOrEmpty(Input.text))
				{
					return EValidState.None;
				}
				return EValidState.Invalid;
			}
			return EValidState.Valid;
		}
	}

	protected override void Update()
	{
		var types = TypeExtensions.GetTypes(Input.text);
		Value = types.Count() == 1 ? types.Single() : null;
		base.Update();
	}
}
