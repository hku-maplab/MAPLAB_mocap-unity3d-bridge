using UnityEngine;
using System.Collections;
using System.Xml;

//=============================================================================----
// Copyright © NaturalPoint, Inc. All Rights Reserved.
// 
// This software is provided by the copyright holders and contributors "as is" and
// any express or implied warranties, including, but not limited to, the implied
// warranties of merchantability and fitness for a particular purpose are disclaimed.
// In no event shall NaturalPoint, Inc. or contributors be liable for any direct,
// indirect, incidental, special, exemplary, or consequential damages
// (including, but not limited to, procurement of substitute goods or services;
// loss of use, data, or profits; or business interruption) however caused
// and on any theory of liability, whether in contract, strict liability,
// or tort (including negligence or otherwise) arising in any way out of
// the use of this software, even if advised of the possibility of such damage.
//=============================================================================----

// Attach Body.cs to an empty Game Object and it will parse and create visual
// game objects based on bone data.  Body.cs is meant to be a simple example 
// of how to parse and display skeletal data in Unity.

// In order to work properly, this class is expecting that you also have instantiated
// another game object and attached the Slip Stream script to it.  Alternatively
// they could be attached to the same object.

public class Parser : MonoBehaviour
{
	
	public GameObject SlipStreamObject;
	
	private XmlDocument xmlDoc;
	private XmlDocument xmlRoutes;
	
	// Use this for initialization
	void Start ()
	{
		Debug.Log ("_NatNet.Parser.Start()");
		xmlDoc = new XmlDocument ();
		xmlRoutes = new XmlDocument ();
		SlipStreamObject.GetComponent<SlipStream> ().PacketNotification += new PacketReceivedHandler (OnPacketReceived);
	}
	
	// packet received
	void OnPacketReceived (object sender, string Packet)
	{
		//Debug.Log ("Parser#OnPacketReceived: " + Packet);
		xmlDoc.LoadXml (Packet);
		
		//== skeletons ==--
		
		XmlNodeList boneList = xmlDoc.GetElementsByTagName ("Bone");
		
		for (int index=0; index<boneList.Count; index++) {
			
			//int id = System.Convert.ToInt32 (boneList [index].Attributes ["ID"].InnerText);
			
			float x = (float)System.Convert.ToDouble (boneList [index].Attributes ["x"].InnerText);
			float y = (float)System.Convert.ToDouble (boneList [index].Attributes ["y"].InnerText);
			float z = (float)System.Convert.ToDouble (boneList [index].Attributes ["z"].InnerText);
			
			float qx = (float)System.Convert.ToDouble (boneList [index].Attributes ["qx"].InnerText);
			float qy = (float)System.Convert.ToDouble (boneList [index].Attributes ["qy"].InnerText);
			float qz = (float)System.Convert.ToDouble (boneList [index].Attributes ["qz"].InnerText);
			float qw = (float)System.Convert.ToDouble (boneList [index].Attributes ["qw"].InnerText);
			
			//== coordinate system conversion (right to left handed) ==--
			
			z = -z;
			qz = -qz;
			qw = -qw;
			
			//== bone pose ==--
			
			Vector3 position = new Vector3 (x, y, z);
			Quaternion orientation = new Quaternion (qx, qy, qz, qw);
			
			//== locate or create bone object ==--
			
			// Check xml for elems..
			XmlNodeList routeList = xmlRoutes.GetElementsByTagName (boneList [index].Attributes ["Name"].InnerText);
			//Debug.Log ("Number of matching bones: "+routeList.Count);
			for (int j = 0; j < routeList.Count; j++) {
				GameObject go = GameObject.Find (routeList [j].Attributes ["target"].InnerText);
				if (go == null) {
					continue;
				}
				//Debug.Log("MoCap update for: "+go.name);
				go.transform.position = position;
				go.transform.rotation = orientation;
			}
			
			
		}
		
		//== rigid bodies ==--
		
		XmlNodeList rbList = xmlDoc.GetElementsByTagName ("RigidBody");
		
		for (int index = 0; index < rbList.Count; index++) {
			
			//int id = System.Convert.ToInt32 (rbList [index].Attributes ["ID"].InnerText);
			
			float x = (float)System.Convert.ToDouble (rbList [index].Attributes ["x"].InnerText);
			float y = (float)System.Convert.ToDouble (rbList [index].Attributes ["y"].InnerText);
			float z = (float)System.Convert.ToDouble (rbList [index].Attributes ["z"].InnerText);
			
			float qx = (float)System.Convert.ToDouble (rbList [index].Attributes ["qx"].InnerText);
			float qy = (float)System.Convert.ToDouble (rbList [index].Attributes ["qy"].InnerText);
			float qz = (float)System.Convert.ToDouble (rbList [index].Attributes ["qz"].InnerText);
			float qw = (float)System.Convert.ToDouble (rbList [index].Attributes ["qw"].InnerText);
			
			//== coordinate system conversion (right to left handed) ==--
			
			z = -z;
			qz = -qz;
			qw = -qw;
			
			//== bone pose ==--
			
			Vector3 position = new Vector3 (x, y, z);
			Quaternion orientation = new Quaternion (qx, qy, qz, qw);
			
			// Check xml for elems..
			XmlNodeList routeList = xmlRoutes.GetElementsByTagName (rbList [index].Attributes ["Name"].InnerText);
			//			if(routeList.Count == 0){
			//				Debug.Log ("No routes found for rigid body: "+rbList [index].Attributes ["Name"]);
			//			}
			for (int j = 0; j < routeList.Count; j++) {
				GameObject go = GameObject.Find (routeList [j].Attributes ["target"].InnerText);
				if (go == null) {
					continue;
				}
				go.transform.position = position;
				go.transform.rotation = orientation;
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	// Public methods
	public bool requestRigidBodyUpdates (string rigidbodyname, string targetobjname)
	{
		
		Debug.Log ("RequestRigidBodyUpdates: " + rigidbodyname + " => " + targetobjname);
		
		if (xmlRoutes == null) {
			Debug.LogWarning (" -> _NatNetParser not ready yet, return false...");
			return false;
		}
		
		XmlNode rootNode = getRoutesRootNode ();
		
		XmlAttribute attr1 = xmlRoutes.CreateAttribute ("rigidbody");
		attr1.Value = rigidbodyname;
		
		XmlAttribute attr2 = xmlRoutes.CreateAttribute ("target");
		attr2.Value = targetobjname;
		
		XmlElement newELem = xmlRoutes.CreateElement (rigidbodyname);
		newELem.Attributes.Append (attr1);
		newELem.Attributes.Append (attr2);
		
		rootNode.AppendChild (newELem);
		
		
		//xmlRoutes.AppendChild (newELem);
		
		string filepath = Application.dataPath + @"/NatNetParser-dump.xml"; //.dataPath + @"/StreamingAssets/gamexmldata.xml"
		xmlRoutes.Save (filepath);
		
		return true;
	}
	
	private XmlNode getRoutesRootNode ()
	{
		XmlNodeList rootNodes = xmlRoutes.GetElementsByTagName ("RigidBodyTargets");
		
		if (rootNodes.Count == 1) {
			return rootNodes [0]; // return the one-and-only root-node (this is the most likely scenario)
		}
		
		if (rootNodes.Count > 1) { // this is pretty much impossible
			Debug.Log ("Invalid XML: More than one root node in XML, this should not be possible");
			return rootNodes [0]; // let's be flexible; just return the first root node and pretend the others don't exist
		}
		
		// no root node yet, let's create it, add it to our xmlRoutes structure and call this function again so it'll find the just created element
		XmlElement newRootEl = xmlRoutes.CreateElement ("RigidBodyTargets");
		xmlRoutes.AppendChild (newRootEl);
		return getRoutesRootNode (); // let's try that again
	}
}






















