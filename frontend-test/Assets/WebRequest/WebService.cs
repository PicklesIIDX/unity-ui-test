using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// A Web service class used to send and recieve JSON requests between this class and a remote or local server.
/// </summary>
public class WebService : MonoBehaviour {
	
	protected Queue<WebRequest> requestQueue {get; private set;}
	private WebRequest currentRequest;
	// Point towards local files inside the unity project
	public static string SERVERURL = "file://" + Application.dataPath + "/";
	// point towards a local server
	//public static string SERVERURL = "http://localhost:3000";
	
	void Awake()
	{
		// Initialize the Queue object so we don't receive a null error.
		requestQueue = new Queue<WebRequest>();	
	}
	
	void Update()
	{
		// Cycle through our current queue and deal with requests in order.
		ProcessRequests();	
	}
	
	void ProcessRequests()
	{
		// If we have any requests in queue, stare processing them, in the order we received them.
		if(currentRequest == null && requestQueue.Count > 0)
		{
			currentRequest = requestQueue.Dequeue();
			StartCoroutine(ProcessRequest(currentRequest));
		}
	}
	
	/// <summary>
	/// Creates a web request, which sends a standard HTTP request to a specified service under the game's known server url.
	/// </summary>
	/// <returns>
	/// The request to be added to this class' queue for processing.
	/// </returns>
	/// <param name='method'>
	/// The method of contacting the url (such as GET or POST).
	/// </param>
	/// <param name='serviceName'>
	/// Name of the service or file to be reached at the path of the game's server url. You just have to add the relative address here.
	/// So '/users/id', not 'http://localhost.com/users/id'
	/// </param>
	/// <param name='parameters'>
	/// An object containing parameters to send to the server. Typically, this will be a json string, but your server code will determine what you need to put in here.
	/// </param>
	/// <param name='listener'>
	/// The handler that listens to and uses information from the response sent back from the server
	/// </param>
	public WebRequest CreateRequest (string method, string serviceName, object parameters, WebRequestListener listener)
	{
		WebRequest request = new WebRequest (SERVERURL + serviceName, method, parameters, listener);	
		requestQueue.Enqueue (request);
		return request;
	}
	
	public WebRequest Get(string serviceName, object parameters, WebRequestListener listener)
	{
		return CreateRequest("GET", serviceName, parameters, listener);
	}
	
	public WebRequest Post(string serviceName, object parameters, WebRequestListener listener)
	{
		return CreateRequest("POST", serviceName, parameters, listener);	
	}
	
	/// <summary>
	/// A function that can be extended to interprets multiple results into a formatted ArrayList to be easily parsed in C#.
	/// Does not actually do anything, but is here to be overriden for specific implementation based on the service class.
	/// </summary>
	/// <returns>
	/// An object of formatted results
	/// </returns>
	/// <param name='result'>
	///	Raw results sent back from the server.
	/// </param>
	protected virtual object PostProcessResults(object result)
	{
		return result;	
	}
	
	/// <summary>
	/// Converts a request sent to the server into a json string, and then parses a json response to be handled by the assigned request listener.
	/// </summary>
	/// <param name='request'>
	/// The request to be made to the server.
	/// </param>
	private IEnumerator ProcessRequest(WebRequest request)
	{
		WWW www = new WWW(SERVERURL);
		// Get information from the url
		if(request.method == "GET")
		{
			www = new WWW(request.url);	
		}
		// Send a json string to the server
		else
		{
			if(request.parameters.GetType() == typeof(string))
			{
				string jsonString = MiniJSON.jsonEncode(request.parameters);
				var postHeader = new Hashtable();
				postHeader.Add ("Content-Type", "application/json");
				postHeader.Add ("Content-Length", jsonString.Length);
				UTF8Encoding encoding = new UTF8Encoding();
				www = new WWW(request.url, encoding.GetBytes(jsonString), postHeader);
				Debug.Log("[WebService.cs]: Sending request parameters: " + jsonString);
			}
		}
		// Wait for a response to come back back
		yield return www;
		
		// Handle any errors that we recieve from the server
		if(www.error != null)
		{
			Debug.LogError("[WebSerivce.cs]: Request Failed at " + www.url + " with the error: " + www.error);
			if(request.listener != null)
				request.listener.HandleError(request, www.error);
		}
		// Otherwise, parse the results
		else
		{
			if(request.listener != null)
			{
				object result = MiniJSON.jsonDecode(www.text.Trim());
				result = this.PostProcessResults(result);
				if(result != null) 
				{
					// Handle the json results based on the WebService
					request.listener.HandleResults(request, result);	
				}
				else
				{
					// Handle an error of not being able to parse the JSON string
					request.listener.HandleError(request, "Unable to parse JSON string: " + www.text);	
				}
			}
		}
	}
	
}
