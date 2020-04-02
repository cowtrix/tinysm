using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TinySM;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPicker : LevelSingleton<ObjectPicker>
{
    public Button CloseButton;
    public GameObject RowPrefab;
    public RectTransform Content;

    Func<IEnumerable> m_pickFunction;
    Action<object> m_selectFunction;
    Dictionary<object, Button> m_buttons = new Dictionary<object, Button>();
    bool m_canCancel;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Pick(Type type, Action<ITrackedObject> onSelect, bool canCancel = false)
    {
        if(!typeof(TrackedObject).IsAssignableFrom(type))
        {
            throw new Exception("Invalid type " + type);
        }
        Debug.Log("Picking " + type);
        var allObjects = TrackedObject.GetAll(type ?? typeof(TrackedObject));
        Pick(allObjects, onSelect);
    }

    public void Pick<T>(IEnumerable<T> options, Action<T> onSelect, bool canCancel = false)
    {
        Pick(() => options, onSelect);
    }

    public void Pick<T>(Func<IEnumerable<T>> options, Action<T> onSelect, bool canCancel = false)
    {
        gameObject.SetActive(true);
        m_pickFunction = options;
        m_selectFunction = o => onSelect((T)o);
        m_canCancel = canCancel;
    }

    private void OnDisable()
    {
        foreach (var val in m_buttons.Values)
        {
            Destroy(val.gameObject);
        }
        m_buttons.Clear();
    }

    private void Update()
    {
        CloseButton.gameObject.SetActive(m_canCancel);
        if(m_pickFunction == null)
        {
            return;
        }
        foreach (var obj in m_pickFunction())
        {
            if(m_buttons.ContainsKey(obj))
            {
                return;
            }
            var newButton = Instantiate(RowPrefab).GetComponent<Button>();
            newButton.transform.SetParent(Content);
            newButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                m_selectFunction(obj);
            });
            newButton.GetComponentInChildren<Text>().text = obj.ToString();
            m_buttons.Add(obj, newButton);
        }
    }
}
