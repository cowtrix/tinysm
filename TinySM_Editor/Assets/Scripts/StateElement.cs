using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TinySM;
using UnityEngine;
using UnityEngine.UI;

public class StateElement : MonoBehaviour
{
    public EntryNode EntryNode;
    public Guid GUID => State != null ? State.GUID : default;

    public IState State;

    public void Delete()
    {
        PromptWindow.LevelInstance.Prompt("Delete this State?", new[] {
            ("Delete", () => Destroy(gameObject)),
            ("Cancel", default(Action))
        });
    }

    public void Bind(IState obj)
    {
        State = obj;
        var type = obj.GetType();
        var members = type.GetMembers().Where(m => m is PropertyInfo || m is FieldInfo);
        foreach(var member in members)
        {
            IFieldElement field = UiManager.LevelInstance.GetFieldForType(member.GetMemberType());
        }
    }
}

