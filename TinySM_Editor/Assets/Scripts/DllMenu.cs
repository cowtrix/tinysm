using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DllMenu : MonoBehaviour
{
	public AssemblyDisplay DllRowPrefab;
	public RectTransform ListContainer;
	private List<AssemblyDisplay> m_displays = new List<AssemblyDisplay>();

	private void Awake()
	{
		gameObject.SetActive(false);
		Toolbar.LevelInstance.Add("Manage DLLs", Open, true);
	}

	public void Open()
	{
		gameObject.SetActive(true);
		RefreshList();
		UiManager.LevelInstance.OnAssemblyLoaded += a => RefreshList();
	}

	public void RefreshList()
	{
		HashSet<AssemblyDisplay> dirty = new HashSet<AssemblyDisplay>();
		foreach (var ass in UiManager.LevelInstance.LoadedAssemblies)
		{
			var disp = m_displays.SingleOrDefault(a => a.AssemblyData.Assembly == ass.Assembly);
			if(disp == null)
			{
				var newB = Instantiate(DllRowPrefab.gameObject);
				newB.SetActive(true);
				newB.transform.SetParent(ListContainer);
				disp = newB.GetComponent<AssemblyDisplay>();
				m_displays.Add(disp);
			}
			disp.SetData(ass);
			dirty.Add(disp);
		}
		for(var i = m_displays.Count - 1; i >= 0; --i)
		{
			if(dirty.Contains(m_displays[i]))
			{
				continue;
			}
			Destroy(m_displays[i].gameObject);
			m_displays.RemoveAt(i);
		}
	}
}
