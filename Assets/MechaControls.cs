using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MechaControls : MonoBehaviour {

    //Variables
    public AnimationCurve rightControlStickSensitivity;
    public AnimationCurve leftControlStickSensitivity;
    public float maxSpeed = 5;
    public float rotationalMaxSpeed = 1;
    //To be replaced
    public List<MechaWeapon> LeftWeapons;
    public List<MechaWeapon> RightWeapons;

    //Components
    private Transform rightControlStickOrigin;
    private Transform leftControlStickOrigin;
    private Transform rightControlStickPosition;
    private Transform leftControlStickPosition;
    private NewtonVR.NVRInteractableItem rightInteractableControlStick;
    private NewtonVR.NVRInteractableItem leftInteractableControlStick;

    //System
    float desiredYRotation = 0;
    int leftController;
    int rightController;


    // Use this for initialization
    void Start () {
        leftController = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
        leftController = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
        rightControlStickOrigin = transform.FindChild("RightControlStickReference");
        leftControlStickOrigin = transform.FindChild("LeftControlStickReference");
        rightControlStickPosition = transform.FindChild("RightControlStick");
        leftControlStickPosition = transform.FindChild("LeftControlStick");
        rightInteractableControlStick = transform.FindChild("RightControlStick").GetComponent<NewtonVR.NVRInteractableItem>();
        leftInteractableControlStick = transform.FindChild("LeftControlStick").GetComponent<NewtonVR.NVRInteractableItem>();

        desiredYRotation = transform.parent.rotation.eulerAngles.y;

        Debug.Log(transform.parent.name);
    }
	
	// Update is called once per frame
	void Update () {
        float rightControlStickLinearDeviation = (rightControlStickOrigin.localPosition.z - rightControlStickPosition.localPosition.z);
        float leftControlStickLinearDeviation = (leftControlStickOrigin.localPosition.z - leftControlStickPosition.localPosition.z);
        rightControlStickLinearDeviation = (Mathf.Round(rightControlStickLinearDeviation * 100f) / 100f) / 0.25f;
        leftControlStickLinearDeviation = (Mathf.Round(leftControlStickLinearDeviation * 100f) / 100f) / 0.25f;

        desiredYRotation = (leftControlStickLinearDeviation - rightControlStickLinearDeviation) * rotationalMaxSpeed * Time.deltaTime;
        transform.parent.Rotate(0, desiredYRotation, 0);

        transform.parent.position += transform.parent.forward * ((rightControlStickLinearDeviation + leftControlStickLinearDeviation) / 2) * maxSpeed * Time.deltaTime;

        if(rightInteractableControlStick.AttachedHand != null) {
            if(rightInteractableControlStick.AttachedHand.Inputs[Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger].IsPressed) { 
                foreach(MechaWeapon mw in RightWeapons) {
                    mw.ShootContinuous();
                }
            }
        }
        if(leftInteractableControlStick.AttachedHand != null) {
            if (leftInteractableControlStick.AttachedHand.Inputs[Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger].IsPressed) {
                foreach (MechaWeapon mw in LeftWeapons) {
                    mw.ShootContinuous();
                }
            }
        }

    }
}
