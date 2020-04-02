using UnityEngine;
using System;
using System.Collections.Generic;

public enum ESkinType
{
	None,
	DefaultPanel,
	DefaultText,
	PromptPanel,
	Toolbar,
	ToolbarText,
	Scrollbar,
	ScrollbarHandle,
	Background,
	Button,
	Field,
}

[Serializable]
public class SkinElement
{
	public ESkinType Name;
	public Color Color;
	public Font Font;
}

[CreateAssetMenu]
public class Skin : ScriptableObject
{
	public Font ParagraphFont;
	
	public Color BadColor;
	public Color NeutralColor;
	public Color GoodColor;

	public Color StateColor;
	public Color StateMachineDefinitionColor;
	public Color ConditionColor;
	
	public List<SkinElement> Elements;
}
