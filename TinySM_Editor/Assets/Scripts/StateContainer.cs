using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateContainer : MonoBehaviour
{
	public string ExpandedText = "▼";
	public string CollapsedText = "◀";
	public RectTransform[] IgnoredChildren;
	public Toggle ExpandCollapseToggle;
	public Text ToggleText;

	private void Awake()
	{
		ExpandCollapseToggle.onValueChanged.AddListener(ExpandCollapse);
	}

	private void ExpandCollapse(bool toggle)
	{
		foreach (Transform child in transform)
		{
			if (IgnoredChildren.Contains(child))
			{
				continue;
			}
			child.gameObject.SetActive(toggle);
		}
	}

	private void Update()
	{
		ToggleText.text = ExpandCollapseToggle.isOn ? ExpandedText : CollapsedText;
	}
}
