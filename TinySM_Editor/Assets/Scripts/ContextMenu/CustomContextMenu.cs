using System;
using System.Collections.Generic;
using UnityEngine;

public interface IContextMenuProvider
{
    CustomContextMenu GetMenu(object context);
}

public class CustomContextMenu
{
    public class Item
    {
        public string Label;
    }

    public class SubMenu : Item { }

    public class InvokeItem : Item
    {
        public Action<Vector2> Action;
    }

    public IEnumerable<Item> Items { get; }

    public CustomContextMenu(IEnumerable<Item> items)
    {
        Items = items;
    }
}
