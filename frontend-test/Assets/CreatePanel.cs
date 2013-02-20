using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatePanel : MonoBehaviour {
	
	GameObject anchor;
	
	void Start()
	{
		anchor = GameObject.Find("Anchor");
		StartCoroutine(GetMenuHash());
	}
	
	IEnumerator GetMenuHash()
	{
		yield return StartCoroutine(LoadJSON.Instance.SendRequest());
		yield return StartCoroutine(BuildPanels());
	}
	
	IEnumerator BuildPanels()
	{
		Hashtable hash = LoadJSON.Instance.hash;
		while(hash == null)
			yield return 0;
		ArrayList panels = new ArrayList();
		panels = hash["panels"] as ArrayList;
		foreach(Hashtable panel in panels)
		{
			// Create the panel itself
			GameObject newPanel = new GameObject(panel["title"].ToString());
			newPanel.AddComponent<UIPanel>();
			newPanel.transform.parent = anchor.transform;
			newPanel.transform.localScale = new Vector3(1,1,1);
			newPanel.transform.localPosition = new Vector3(System.Convert.ToInt32(panel["x"]), System.Convert.ToInt32(panel["y"]), 0);
			// Create the buttons on the panel
			ArrayList buttons = new ArrayList();
			buttons = panel["buttons"] as ArrayList;
			foreach (Hashtable button in buttons)
			{
				GameObject newButton = new GameObject(button["title"].ToString());
				newButton.transform.parent = newPanel.transform;
				newButton.transform.localScale = new Vector3(1,1,1);
				newButton.AddComponent<UIButton>();
				newButton.transform.localPosition = new Vector3(System.Convert.ToInt32(button["x"]), System.Convert.ToInt32(button["y"]), 0);
			}
		}
	}
}
