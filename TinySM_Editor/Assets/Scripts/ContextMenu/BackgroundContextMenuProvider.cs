using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BackgroundContextMenuProvider : ContextMenuProvider
{
	public override CustomContextMenu GetMenu(object context)
	{
		return new CustomContextMenu(new[]
			{
				new CustomContextMenu.InvokeItem
				{
					Label = "New State",
					Action = pos => UiManager.LevelInstance.NewState(pos)
				}
			});
	}
}
