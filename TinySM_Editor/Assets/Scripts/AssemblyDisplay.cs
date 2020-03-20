using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine;
using TinySM;
using TinySM.Conditions;

public class AssemblyDisplay : MonoBehaviour
{
    public Text AssemblyName;
    public GameObject TypeDisplay;
    public Toggle Toggle;
    public UiManager.AssemblyData AssemblyData;

    private List<GameObject> m_types = new List<GameObject>();

    public void SetData(UiManager.AssemblyData assembly)
    {
        AssemblyData = assembly;
        AssemblyName.text = $"{AssemblyData.Assembly.GetName().Name} [{AssemblyData.Types.Count}]";
        m_types.ForEach(go => Destroy(go));
        m_types.Clear();
        foreach (var type in AssemblyData.Types)
        {
            var typeDisplayGO = Instantiate(TypeDisplay.gameObject);
            m_types.Add(typeDisplayGO);
            typeDisplayGO.transform.SetParent(transform);
            var typeDisplay = typeDisplayGO.GetComponentInChildren<Text>();
            typeDisplay.text = type.Name;

            var img = typeDisplayGO.GetComponent<Image>();
            var skin = UiReferenceTracker.LevelInstance.Skin;
            if(typeof(IState).IsAssignableFrom(type))
            {
                img.color = skin.StateColor;
            }
            else if (typeof(IStateMachineDefinition).IsAssignableFrom(type))
            {
                img.color = skin.StateMachineDefinitionColor;
            }
            else if (typeof(ICondition).IsAssignableFrom(type))
            {
                img.color = skin.ConditionColor;
            }
            else
            {
                img.color = skin.NeutralColor;
            }
            typeDisplayGO.SetActive(Toggle.isOn);
        }
    }
}
