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

public class Body : MonoBehaviour {
	

    private SlipStream stream;
    private XmlDocument xmlDoc;
    private bool hasWritten = false;

    public string myDrive = "C:";
    public string remoteIP = "10.200.200.14";
    public string myIp = "10.200.200.29";

    public GameObject SlipStreamObject;
    public GameObject prefabBlock;
    void OnEnable()
    {
        
        stream.PacketNotification += new PacketReceivedHandler(OnPacketReceived);
    }

    void OnDisable()
    {
        stream.PacketNotification -= new PacketReceivedHandler(OnPacketReceived);
    }

	// Use this for initialization
	void Awake () 
	{
        // we start the connection to the server from Unity by running the .bat file named UnitySample
        string path = Application.dataPath.Substring(2) + "/Server";
        path = path.Replace('/', '\\');
        Debug.Log(Application.dataPath + "/Server/UnitySample.bat " + remoteIP+ " " + myIp+ " " + myDrive + " " + '"' + path + '"');
        System.Diagnostics.Process.Start(Application.dataPath + "/Server/UnitySample.bat ", remoteIP + " " + myIp + " "  + myDrive + " "+'"'+ path+'"');

        xmlDoc = new XmlDocument();
        if (SlipStreamObject == null)
        {
            stream = GetComponent<SlipStream>();
        }
        else
        {
            stream = SlipStreamObject.GetComponent<SlipStream>();
        }
        
	}
	
	// packet received
	void OnPacketReceived(object sender, string Packet)
	{
		
		xmlDoc.LoadXml(Packet);

        if (!hasWritten)
        {
            Debug.Log("started writing...");
            xmlDoc.Save(Application.dataPath + "/XMLDoc/xmlDoc.xml");
            hasWritten = true;
        }


        //== skeletons ==--

		XmlNodeList boneList = xmlDoc.GetElementsByTagName("Bone");

		for(int index=0; index<boneList.Count; index++)
		{
		
			int id = System.Convert.ToInt32(boneList[index].Attributes["ID"].InnerText);
			
			float x = (float) System.Convert.ToDouble(boneList[index].Attributes["x"].InnerText);
			float y = (float) System.Convert.ToDouble(boneList[index].Attributes["y"].InnerText);
			float z = (float) System.Convert.ToDouble(boneList[index].Attributes["z"].InnerText);
			
			float qx = (float) System.Convert.ToDouble(boneList[index].Attributes["qx"].InnerText);
			float qy = (float) System.Convert.ToDouble(boneList[index].Attributes["qy"].InnerText);
			float qz = (float) System.Convert.ToDouble(boneList[index].Attributes["qz"].InnerText);
			float qw = (float) System.Convert.ToDouble(boneList[index].Attributes["qw"].InnerText);
			
			//== coordinate system conversion (right to left handed) ==--
			
			z = -z;
			qz = -qz;
			qw = -qw;
			
			//== bone pose ==--
			
			Vector3    position    = new Vector3(x,y,z);
			Quaternion orientation = new Quaternion(qx,qy,qz,qw);
			
			//== locate or create bone object ==--
			
			string objectName = "Bone"+id.ToString();
				
			GameObject bone;
			
			bone = GameObject.Find(objectName);
			
            if(bone==null)
			{
                bone = (GameObject)Instantiate(prefabBlock);// GameObject.CreatePrimitive(PrimitiveType.Cube);
				//Vector3 scale = new Vector3(0.1f,0.1f,0.1f);
				//bone.transform.localScale = scale;
				bone.name = objectName;
			}		
			
			//== set bone's pose ==--
			
			bone.transform.position = position;
			bone.transform.rotation = orientation;
		}

        //== rigid bodies ==--

        XmlNodeList rbList = xmlDoc.GetElementsByTagName("RigidBody");

        for (int index = 0; index < rbList.Count; index++)
        {

            int id = System.Convert.ToInt32(rbList[index].Attributes["ID"].InnerText);

            float x = (float)System.Convert.ToDouble(rbList[index].Attributes["x"].InnerText);
            float y = (float)System.Convert.ToDouble(rbList[index].Attributes["y"].InnerText);
            float z = (float)System.Convert.ToDouble(rbList[index].Attributes["z"].InnerText);

            float qx = (float)System.Convert.ToDouble(rbList[index].Attributes["qx"].InnerText);
            float qy = (float)System.Convert.ToDouble(rbList[index].Attributes["qy"].InnerText);
            float qz = (float)System.Convert.ToDouble(rbList[index].Attributes["qz"].InnerText);
            float qw = (float)System.Convert.ToDouble(rbList[index].Attributes["qw"].InnerText);

            //== coordinate system conversion (right to left handed) ==--

            z = -z;
            qz = -qz;
            qw = -qw;

            //== bone pose ==--

            Vector3 position = new Vector3(x, y, z);
            Quaternion orientation = new Quaternion(qx, qy, qz, qw);

            //== locate or create bone object ==--

            string objectName = "RigidBody" + id.ToString();

            GameObject bone;

            bone = GameObject.Find(objectName);

            if (bone == null)
            {
                bone = (GameObject)Instantiate(prefabBlock);// GameObject.CreatePrimitive(PrimitiveType.Cube);
                                                            //Vector3 scale = new Vector3(0.1f,0.1f,0.1f);
                                                            //bone.transform.localScale = scale;
                bone.name = objectName;
            }

            //== set bone's pose ==--

            bone.transform.position = position;
            bone.transform.rotation = orientation;
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    void OnApplicationQuit()
    {

        //Stop the stream from the Server by Closing it's process
        System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcesses();
        foreach (System.Diagnostics.Process p in procs)
        {
            if(p.ProcessName == "UnitySample")
            {
                p.CloseMainWindow();
                p.Close();
                Debug.Log("Process: " + p.ProcessName + " Stopped Succesfully!");

            }
            
        }

        
    }
}
