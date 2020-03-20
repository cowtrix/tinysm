using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Toolbar : LevelSingleton<Toolbar>
{
    public RectTransform LeftJustification;
    public RectTransform RightJustification;

    private Dictionary<string, Button> m_buttons = new Dictionary<string, Button>();

    public void Clear()
    {
        foreach(var kvp in m_buttons)
        {
            Destroy(kvp.Value.gameObject);
        }
        m_buttons.Clear();
    }

    public void Add(string name, Action action, bool rightJustify = false)
    {
        if (m_buttons.TryGetValue(name, out var button))
        {
            button.onClick.RemoveAllListeners();
        }
        else
        {
            button = Instantiate(UiReferenceTracker.LevelInstance.DefaultButton).GetComponent<Button>();
            button.transform.SetParent(rightJustify ? RightJustification : LeftJustification);
        }
        button.onClick.AddListener(() => action());
        button.GetComponentInChildren<Text>().text = name;
    }
}
