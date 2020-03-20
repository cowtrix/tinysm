using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PromptWindow : LevelSingleton<PromptWindow>
{
	private Coroutine m_currentPrompt;

	public Button OptionPrefab;
	public RectTransform OptionContainer;
	public Text Text;

	private List<Button> m_options = new List<Button>();

	public void Start()
	{
		gameObject.SetActive(false);
	}

	public void Prompt(string text, IEnumerable<ValueTuple<string, Action>> options = null)
	{
		if (m_currentPrompt != null)
		{
			Debug.LogError("Prompt was already in progress");
			return;
		}
		gameObject.SetActive(true);
		StartCoroutine(PromptInternal(text, options));
	}

	IEnumerator PromptInternal(string text, IEnumerable<ValueTuple<string, Action>> options)
	{
		if(options == null || !options.Any())
		{
			options = new ValueTuple<string, Action>[]
			{
				("OK", null)
			};
		}
		Debug.Log("Prompting");
		Text.text = text;
		for (int i = 0; i < m_options.Count; i++)
		{
			Destroy(m_options[i].gameObject);
		}
		m_options.Clear();
		var sem = new SemaphoreSlim(0);
		Action action = null;
		foreach (var option in options)
		{
			var button = Instantiate(OptionPrefab);
			button.transform.SetParent(OptionContainer);
			var optionText = button.GetComponentInChildren<Text>();
			optionText.text = option.Item1;
			button.onClick.AddListener(() =>
			{
				Debug.Log("Choice was " + option.Item1);
				sem.Release();
				action = option.Item2;
			});
			m_options.Add(button);
		}
		while(sem.CurrentCount == 0)
		{
			yield return null;
		}

		Debug.Log("Sem released");

		gameObject.SetActive(false);
		action?.Invoke();
	}
}
