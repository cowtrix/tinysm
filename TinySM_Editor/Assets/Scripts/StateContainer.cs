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
	public RectTransform Toolbar;
	public Toggle ExpandCollapseToggle;
	private Text m_toggleText;

	private void Awake()
	{
		m_toggleText = ExpandCollapseToggle.GetComponentInChildren<Text>();
		ExpandCollapseToggle.onValueChanged.AddListener(ExpandCollapse);
	}

	private void ExpandCollapse(bool toggle)
	{
		foreach (Transform child in transform)
		{
			if (child == Toolbar)
			{
				continue;
			}
			child.gameObject.SetActive(toggle);
		}
	}

	private void Update()
	{
		m_toggleText.text = ExpandCollapseToggle.isOn ? ExpandedText : CollapsedText;
	}
}
