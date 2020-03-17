using System;
using System.Collections;
using System.Collections.Generic;
using TinySM;
using UnityEngine;
using UnityEngine.UI;

public class TrackedObjectPicker : LevelSingleton<TrackedObjectPicker>
{
    public GameObject RowPrefab;
    public RectTransform Content;
    private List<Button> m_buttons = new List<Button>();

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Pick<T>(Action<TrackedObject> onSelect) where T: TrackedObject
    {
        Pick(typeof(T), onSelect);
    }

    public void Pick(Type type, Action<TrackedObject> onSelect)
    {
        if(!typeof(TrackedObject).IsAssignableFrom(type))
        {
            throw new Exception("Invalid type " + type);
        }
        Debug.Log("Picking " + type);
        gameObject.SetActive(true);
        m_buttons.ForEach(b => Destroy(b?.gameObject));
        m_buttons.Clear();
        var allObjects = TrackedObject.GetAll(type ?? typeof(TrackedObject));
        foreach(var obj in allObjects)
        {
            var newButton = Instantiate(RowPrefab).GetComponent<Button>();
            newButton.transform.SetParent(Content);
            newButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                onSelect(obj);
            });
            newButton.GetComponentInChildren<Text>().text = obj.Name;
            m_buttons.Add(newButton);
        }
    }
}
