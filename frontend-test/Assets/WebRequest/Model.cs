using System;
using System.Collections;

/// <summary>
/// A base model class. Extend this class to represent the differnt models in your game that are objects on the database.
/// This contains some useful parameters for parsing a json string, and building a version of the model in the client as a C# class.
/// To use this, just create a new instance of the model with the results you get from your WebRequest. Then, use the Get functions here
/// to get parameters as defined in the json file.
/// When extending this class, you can write function to get specific parameters that are defined in the json string. For example, the 
/// GetModel function gets the parameter 'model' from the included sample json.
/// </summary>
public class Model  {
	
	// raw data recieved from the server that builds this model
	protected Hashtable data {get; private set;}
	
	// This is a sample function which specifically refers to the included sample json, which represnts a json string we would recieve from our server call
	public string GetModelName ()
	{
		return GetStringFromData (data, "modelname", "");	
	}
	
	
	// These are helpful functions that can be used in any extended model. They get basic values that would be stored in a json string.
	public Model (Hashtable constructorData)
	{
		data = constructorData;
	}
	
	public bool GetBool (string key)
	{
		return GetBoolFromData(data, key, false);	
	}
	
	public int GetInt (string key)
	{
		return GetIntFromData(data, key, -1);	
	}
	
	public string GetString (string key)
	{
		return GetStringFromData(data, key, "");	
	}
	
	public Hashtable GetHashtable (string key)
	{
		return GetHashtableFromData(data, key);	
	}
	
	public ArrayList GetArrayList(string key)
	{
		return GetArrayListFromData (data, key);	
	}
	
	public static bool GetBoolFromData(Hashtable responseData, string key, bool defaultResult)
	{
		if(responseData != null && responseData.ContainsKey(key))
			return Convert.ToBoolean(responseData[key]);
		return defaultResult;
	}
	
	public static int GetIntFromData(Hashtable responseData, string key, int defaultResult)
	{
		if(responseData != null && responseData.ContainsKey(key))
			return Convert.ToInt32(responseData[key]);
		return defaultResult;
	}
	
	public static string GetStringFromData(Hashtable responseData, string key, string defaultResult)
	{
		if(responseData != null && responseData.ContainsKey(key))
			return responseData[key].ToString();
		return defaultResult;
	}
	
	public static ArrayList GetArrayListFromData(Hashtable responseData, string key)
	{
		if(responseData != null && responseData.ContainsKey(key) && responseData[key].GetType() == typeof(ArrayList))
			return (ArrayList)responseData[key];
		return null;
	}
	
	public static Hashtable GetHashtableFromData(Hashtable responseData, string key)
	{
		if(responseData != null && responseData.ContainsKey(key) && responseData[key].GetType() == typeof(Hashtable))
			return (Hashtable)responseData[key];
		return null;
	}
}
