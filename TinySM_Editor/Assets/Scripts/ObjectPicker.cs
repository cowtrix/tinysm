using System;
using System.Collections;
using System.Collections.Generic;
using TinySM;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPicker : LevelSingleton<ObjectPicker>
{
    public GameObject RowPrefab;
    public RectTransform Content;
    private List<Button> m_buttons = new List<Button>();

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Pick(Type type, Action<TrackedObject> onSelect)
    {
        if(!typeof(TrackedObject).IsAssignableFrom(type))
        {
            throw new Exception("Invalid type " + type);
        }
        Debug.Log("Picking " + type);
        var allObjects = TrackedObject.GetAll(type ?? typeof(TrackedObject));
        Pick(allObjects, onSelect);
    }

    public void Pick<T>(IEnumerable<T> options, Action<T> onSelect)
    {
        gameObject.SetActive(true);
        m_buttons.ForEach(b => Destroy(b?.gameObject));
        m_buttons.Clear();
        foreach (var obj in options)
        {
            var newButton = Instantiate(RowPrefab).GetComponent<Button>();
            newButton.transform.SetParent(Content);
            newButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                onSelect(obj);
            });
            newButton.GetComponentInChildren<Text>().text = obj.ToString();
            m_buttons.Add(newButton);
        }
    }
}
