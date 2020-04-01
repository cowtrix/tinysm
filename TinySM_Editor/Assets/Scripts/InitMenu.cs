using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TinySM;
using UnityEngine;
using UnityEngine.UI;

public class InitMenu : MonoBehaviour
{
    public Button NewButton;
    List<Type> m_stateMachineTypes = new List<Type>();

    private void Update()
    {
        m_stateMachineTypes = UiManager.LevelInstance.GetTypes<IStateMachineDefinition>();
        NewButton.interactable = m_stateMachineTypes.Any();
    }
}
