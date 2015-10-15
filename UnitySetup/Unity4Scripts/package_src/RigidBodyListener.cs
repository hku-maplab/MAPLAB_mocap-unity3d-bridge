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
		// Check rigidBodyName
		if (rigidBodyName == null || rigidBodyName == "") {
			Debug.LogError ("RigidBodyListener: (" + gameObject.name + ") rigidBodyName==null");
			return;
		}
		
		// Check _NatNet obj
		natNetObj = GameObject.Find ("_NatNet");
		if (natNetObj == null) {
			Debug.LogError ("Could not find _NatNet obj?!");
			return;
		}
		
		// Get Parser
		natNetParser = (Parser)natNetObj.GetComponent (typeof(Parser));
		if (natNetParser == null) {
			Debug.LogError ("Could not find _NatNet obj?!");
			return;
		}
		
		// only set ready when all checks (above) are okay
		ready = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Only when ready && not registered: try to register
		if (ready && !registered) {
			registered = tryToRegister ();
		}
	}
	
	private bool tryToRegister ()
	{	
		if (!registered) {
			if (natNetParser.requestRigidBodyUpdates (rigidBodyName, gameObject.name)) {
				return true;
			}
		}
		
		return false;
	}
}