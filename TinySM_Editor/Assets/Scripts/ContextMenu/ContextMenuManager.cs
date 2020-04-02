using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenuManager : LevelSingleton<ContextMenuManager>
{
    public GameObject Button;
    private List<GameObject> m_buttons = new List<GameObject>();

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void SetData(Vector2 position, CustomContextMenu menu)
    {
        if(menu == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        transform.position = position;
        m_buttons.ForEach(b => Destroy(b.gameObject));
        m_buttons.Clear();

        foreach(var item in menu.Items)
        {
            var b = Instantiate(Button).GetComponent<Button>();
            m_buttons.Add(b.gameObject);
            b.gameObject.SetActive(true);
            b.transform.SetParent(transform);
            b.GetComponentInChildren<Text>().text = item.Label;
            if(item is CustomContextMenu.InvokeItem invokeItem)
            {
                b.onClick.AddListener(() =>
                {
                    gameObject.SetActive(false);
                    invokeItem.Action(position);
                });
            }
        }
    }
}
