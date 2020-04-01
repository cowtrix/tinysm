using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TinySM;
using UnityEngine.EventSystems;

public class BackgroundContextMenuProvider : ContextMenuProvider
{
	public override CustomContextMenu GetMenu(object context)
	{
		return new CustomContextMenu(UiManager.LevelInstance.GetTypes<IState>().Select(
			t => new CustomContextMenu.InvokeItem { Label = $"New {t.Name}", Action = (pos) => UiManager.LevelInstance.NewState(pos, t) }));
	}
}
