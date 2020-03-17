using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExportWindow : MonoBehaviour
{
    public InputField Field;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Export()
    {
        Field.text = JsonConvert.SerializeObject(UiManager.LevelInstance.Handler.DefinitionInterface,
            new JsonSerializerSettings 
            { 
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto
            });
        gameObject.SetActive(true);
    }
}
