using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A template for web services to be made from. This class shows an example of making a GET request from a url and from a local file in order to bring data into the client.
/// No sample POST is included as that requires logic on the server's end to do something with the request. You'll have to write that yourself c:
/// </summary>
public class ModelWebServices : WebService {

	public WebRequest GetLocalFile (string filename, WebRequestListener listener)
	{
		// Get data from a local file
		return Get (filename, null, listener);
	}
	
	public WebRequest GetServerData (string addressParameters, WebRequestListener listener)
	{
		// Get data from a url address
		// To actually use this funciton, you would have to have:
		// 1. a server set up
		// 2. a string address pointing to the server's root using the SERVERURL constant in the WebService.cs class
		// 3. some code at the given server address to give a result to this client.
		return Get ("data/"+addressParameters, null, listener);	
	}
	
	public WebRequest SendData(string uniqueID, WebRequestListener listener)
	{
		// Post data to a server
		// To actually use this funciton, you would have to have:
		// 1. a server set up
		// 2. a string address pointing to the server's root using the SERVERURL constant in the WebService.cs class
		// 3. some code at the given server address to give a result to this client.
		
		// Parameters are stored in a hashtable and sent with the request
		Hashtable parameters = new Hashtable();
		// This shows an example of adding a unique ID parameter so the server could know who is sending this request
		parameters.Add ("device_id", uniqueID);	
		// We only need to identify the specific address where the code to deal with this request is, as our WebService.cs already holds onto the SERVERURL
		return Post("sample/test", parameters, listener);
	}
	
	
	protected override object PostProcessResults(object results)
	{
		// This is where you can convert the data into a model to be sent to the listener to handle properly, as opposed to sending and parsing a pure json string
		ArrayList models = new ArrayList();
		// no results found
		if(results == null)
			return null;
		// Only a single result returned
		if(results.GetType() == typeof(Hashtable))
		{
			models.Add(new Model (results as Hashtable));	
		}
		// Multiple results returned
		else if (results.GetType() == typeof(ArrayList))
		{
			ArrayList modelResults = (ArrayList)results;
			foreach (Hashtable result in modelResults)
				models.Add (new Model(result));
		}
		return results;	
	}
}
