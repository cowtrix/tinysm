using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[DisallowMultipleComponent]
public class SkinElementApplier : MonoBehaviour
{
    public ESkinType Type;
    protected Skin Skin => UiReferenceTracker.LevelInstance.Skin;
    protected Text Text;
    protected Graphic Graphic;

    private void Awake()
    {
        Graphic = GetComponent<Graphic>();
        Text = GetComponent<Text>();
    }

    void Update()
    {
        if(Type == ESkinType.None)
        {
            return;
        }
        var ele = Skin.Elements.FirstOrDefault(e => e.Name == Type);
        if(ele == null)
        {
            return;
        }
        if(Graphic != null)
        {
            Graphic.color = ele.Color;
        }
        if (Text != null)
        {
            Text.font = ele.Font;
        }
    }
}
