using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinySM;
using UnityEngine;
using UnityEngine.UI;

public class StateElement : MonoBehaviour
{
    public EntryNode EntryNode;
    public Guid GUID;

    public void Delete()
    {
        PromptWindow.LevelInstance.Prompt("Delete this State?", new[] {
            ("Delete", () => Destroy(gameObject)),
            ("Cancel", default(Action))
        });
    }
}

