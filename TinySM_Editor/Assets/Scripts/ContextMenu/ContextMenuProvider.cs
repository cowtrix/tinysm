using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ContextMenuProvider : MonoBehaviour, IContextMenuProvider, IPointerClickHandler
{
	public abstract CustomContextMenu GetMenu(object context);

	public void OnPointerClick(PointerEventData eventData)
	{
		if(eventData.button != PointerEventData.InputButton.Right)
		{
			return;
		}
		ContextMenuManager.LevelInstance.SetData(eventData.position, GetMenu(eventData.pointerCurrentRaycast.gameObject));
	}
}