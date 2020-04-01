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

    struct ToolbarItem
    {
        public Button Button;
        public Func<bool> IsActive;
    }

    private Dictionary<string, ToolbarItem> m_items = new Dictionary<string, ToolbarItem>();

    public void Clear()
    {
        foreach(var kvp in m_items)
        {
            Destroy(kvp.Value.Button.gameObject);
        }
        m_items.Clear();
    }

    public void Add(string name, Action action, Func<bool> active = null, bool rightJustify = false)
    {
        if (m_items.TryGetValue(name, out var toolbarItem))
        {
            toolbarItem.Button.onClick.RemoveAllListeners();
        }
        else
        {
            toolbarItem = new ToolbarItem { 
                Button = Instantiate(UiReferenceTracker.LevelInstance.DefaultButton).GetComponent<Button>(),
                IsActive = active,
            };
            m_items[name] = toolbarItem;
            toolbarItem.Button.transform.SetParent(rightJustify ? RightJustification : LeftJustification);
        }
        toolbarItem.Button.onClick.AddListener(() => action());
        toolbarItem.Button.GetComponentInChildren<Text>().text = name;
    }

    private void Update()
    {
        foreach(var ti in m_items.Where(kvp => kvp.Value.IsActive != null))
        {
            ti.Value.Button.interactable = ti.Value.IsActive();
        }
    }
}
