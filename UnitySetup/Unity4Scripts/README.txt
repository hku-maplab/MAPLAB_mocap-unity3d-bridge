
A QUICK HOWTO

*!!! USE AN ETHERNET CABLE !!!*

- NATNET-UNITY BRIDGE
1. Start UnitySample.bat (just double click it)
2. Provide remote IP (computer running Motiv, 10.200.200.14)
3. Provide local IP (NOT localhost or 127.0.0.1 but (ex) 10.200.200.27)

- IN UNITY
1. Drag "_NatNet" prefab into scene
2. Drag "RigidBodyListener" script onto object you want to control
3. In the Inspector type the name of the (natnet) rigidbody you want the object to be controlled by

- OPTIONAL: Flip axis
You may flip an axis of choice (pos and/or quat); select the _NatNet object in your hierarchy and use the toggles in the script's inspector

- TROUBLESHOOTING
Check the console log for warnings, errors, etc