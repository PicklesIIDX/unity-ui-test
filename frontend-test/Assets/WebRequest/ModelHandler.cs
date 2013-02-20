using UnityEngine;
using System.Collections;

/// <summary>
/// This is an example class which handles the results of making requests for a sample object called a 'model' (see Model.cs for the object's details).
/// This is a class which you attach to a game object in your scene. That game object then gets the ability to send and receive requests based on the logic you input here
/// For this example, we simply make a request at Start() to get a local file.
/// In this example, the local file is formatted into a json object, so we can use the classes here to parse it and read its data
/// This is a good sample for how you might query a database for some information, parse it, and then use it in your game.
/// </summary>
public class ModelHandler : MonoBehaviour, WebRequestListener {
	
	// We need to create an instance of the Services we'll use to make a request. This instance handles making the actual
	//requests from the server, and receiving the results to give back to us in a form we can interpret here.
	private ModelWebServices getModel;
	// We neet to create an instance of a web request. This request is what is passed around by our WebService.cs through our Services above.
	// The code is structured in a way so that we have a reference to this request, and when the request is returned to us by the server,
	// We have results from the server to parse inside this class. This way, we identify when we send out a request, we deal with the response
	// as soon as it is returned, and we can send out multiple requests in succession.
	private WebRequest modelRequest;
	
	void Awake()
	{
		// When we initialze this game object, we need to add the service as a component so we can make requests to our server
		getModel = gameObject.AddComponent<ModelWebServices>();
	}
	
	void Start()
	{
		// This is a sample request being made through our ModelWEbServices.
		// This calls that services' function to get a local file, and return to us the results, which we handle below.
		modelRequest = getModel.GetLocalFile("WebRequest/model.json", this);	
	}
	
	public void HandleResults(WebRequest request, object result)
	{
		// We do a check to make sure that this is the results for the request we have sent out
		if(request == modelRequest)
		{
			Debug.Log("[ModelHandler.cs]: I received a model result from the server as : " + result);
			
			// This is more example code which actually builds a Model out of the information returned from the server.
			// In this example, the server is replaced by a local file which is formatted into a json string.
			// Creating an instance of the Model is a clean way for us to get the specific information we want from the json string
			Model model = new Model(result as Hashtable);
			
			// This line shows us getting the name from the model as we would from any client side C# class
			Debug.Log (model.GetModelName());
			// But, the above function is specific to the Model class. Since Model is something that is to be extendable, 
			// if you want to access specific json objects in the string, you can specify those object's key values
			// See Model.cs for more information
			Debug.Log(model.GetString("modelname"));
			
		}
	}
	
	public void HandleError(WebRequest request, string error)
	{
		// We only want to parse errors for requests we have sent out
		if(request == modelRequest)
		{
			Debug.LogError("[ModelHandler.cs]: Oh no! I got an error when requesting a model! Error : " + error);	
		}
	}
}
