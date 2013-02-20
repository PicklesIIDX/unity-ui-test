using UnityEngine;
using System.Collections;

public class LoadJSON : MonoBehaviour {
	
	public Hashtable hash;
	
	public static LoadJSON Instance {get; private set;}
	
	void Awake()
	{
		if(LoadJSON.Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}
	
	public IEnumerator SendRequest() {
		string url = "file://" + Application.dataPath + "/test.json";
		WWW request = new WWW(url);
		yield return StartCoroutine(WaitForRequest(request));
	}
	
	private IEnumerator WaitForRequest(WWW request)
	{
		while(!request.isDone){
			yield return 0;	
		}
		ProcessRequest(request);
	}
	
	private Hashtable ProcessRequest(WWW request)
	{
		object json = MiniJSON.jsonDecode(request.text);
		hash = new Hashtable(json as Hashtable);
		foreach (string key in hash.Keys)
		{
			Debug.LogError(key);
		}
		return hash;
	}
	
	
}
