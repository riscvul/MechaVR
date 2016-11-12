using UnityEngine;
using System.Collections;

public class CockpitManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        UnityEngine.VR.InputTracking.Recenter();
        SteamVR.instance.hmd.ResetSeatedZeroPose();
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Space)) {
            SteamVR.instance.hmd.ResetSeatedZeroPose();
        }
	}
}
