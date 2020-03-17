using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEditorWindow : LevelSingleton<TextEditorWindow>
{
    public InputField Field;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Export()
    {
        Field.onEndEdit.RemoveAllListeners();
        Field.text = JsonConvert.SerializeObject(UiManager.LevelInstance.Handler.DefinitionInterface,
            new JsonSerializerSettings 
            { 
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto
            });
        Field.readOnly = true;
        gameObject.SetActive(true);
    }

    public void Edit(InputField field)
    {
        Field.readOnly = false;
        Field.onEndEdit.RemoveAllListeners();
        Field.text = field.text;
        Field.onEndEdit.AddListener(x => field.text = x);
        gameObject.SetActive(true);
    }
}
