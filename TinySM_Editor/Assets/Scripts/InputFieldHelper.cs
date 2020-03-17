using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldHelper : MonoBehaviour
{
    private InputField m_input;

    private void Awake()
    {
        m_input = GetComponentInChildren<InputField>();
    }

    public void StartEdit()
    {
        TextEditorWindow.LevelInstance.Edit(m_input);
    }
}
