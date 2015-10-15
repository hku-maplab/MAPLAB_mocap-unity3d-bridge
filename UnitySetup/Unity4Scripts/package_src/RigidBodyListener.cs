using UnityEngine;
using System.Collections;

public class RigidBodyListener : MonoBehaviour
{
	
	public string rigidBodyName;
	
	private GameObject natNetObj;
	private Parser natNetParser;
	
	private bool ready = false;
	private bool registered = false;
	
	// Use this for initialization
	void Start ()
	{
		tryToRegister ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!registered) {
			tryToRegister ();
		}
	}
	
	private bool tryToRegister ()
	{
		if (!ready) {
			// Check rigidBodyName
			if (rigidBodyName == null) {
				Debug.LogError ("rigidBodyName==null (" + gameObject.name + ")");
				return false;
			}
			
			// Check _NatNet obj
			natNetObj = GameObject.Find ("_NatNet");
			if (natNetObj == null) {
				Debug.LogError ("Could not find _NatNet obj?!");
				return false;
			}
			
			// Get Parser
			natNetParser = (Parser)natNetObj.GetComponent (typeof(Parser));
			if (natNetParser == null) {
				Debug.LogError ("Could not find _NatNet obj?!");
				return false;
			}
			
			// only once
			ready = true;
		}
		
		if (!registered) {
			if (natNetParser.requestRigidBodyUpdates (rigidBodyName, gameObject.name)) {
				registered = true;
				return true;
			}
		}
		
		return false;
	}
}