using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Web request listener which takes a WebRequest and the results of the request to parse and use in the client.
/// </summary>
public interface WebRequestListener
{
	void HandleResults(WebRequest request, object results);
	
	void HandleError(WebRequest request, string error);
}

/// <summary>
/// A class to store parameters for making a request through unity's WWW class. 
/// </summary>
public class WebRequest {
	
	public string url {get; private set;}
	public string method {get; private set;}
	public object parameters {get; private set;}
	public WebRequestListener listener {get; private set;}
	
	public WebRequest (string requestUrl, string requestMethod, object requestParameters, WebRequestListener requestListener)
	{
		url = requestUrl;
		method = requestMethod;
		parameters = requestParameters;
		listener = requestListener;
	}
	
}
