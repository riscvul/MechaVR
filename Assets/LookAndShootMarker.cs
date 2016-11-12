using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LookAndShootMarker : MonoBehaviour {

    //Variables
    public LayerMask raycastMask;
    private bool cursorVisible = false;

    //Components
    [SerializeField]
    private AnimationCurve cursorScaling;

    //System

    // Use this for initialization
    void Start () {
        foreach(Transform t in GameObject.Find("TestCockpit").transform) {
            if(t.tag.Equals("Weapon")) {
                UpdateWeaponRotationEvent += t.GetComponent<MechaWeapon>().UpdateWeaponRotation;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        float distance = 500.0f;

        if(Physics.Raycast(new Ray(transform.parent.position, transform.parent.forward), out hit, 500f, raycastMask)) {
            //Debug.Log("hit: " + hit.collider.name);
            transform.position = hit.point;
            float newScale = cursorScaling.Evaluate(hit.distance);
            transform.localScale = new Vector3(newScale, newScale, 1);
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Player")) {               
                if(cursorVisible) {
                    foreach(Transform t in transform) {
                        t.gameObject.SetActive(false);
                    }
                }
                cursorVisible = false;
                //Distance needs to be tested again in this case while adding player to the mask.
            } else {                
                if (!cursorVisible) {
                    foreach (Transform t in transform) {
                        t.gameObject.SetActive(true);
                    }
                }
                cursorVisible = true;
                distance = hit.distance;
            }
        } else {
            transform.position = transform.parent.position + transform.parent.forward * 500.0f;
            float newScale = cursorScaling.Evaluate(500.0f);
            transform.localScale = new Vector3(newScale, newScale, 1);
            if (cursorVisible) {
                foreach (Transform t in transform) {
                    t.gameObject.SetActive(true);
                }
            }
            cursorVisible = true;
        }

        FireUpdateWeaponRotationEvent(transform.position, cursorScaling.Evaluate(distance), distance, cursorVisible);
	}

    #region Events

    public delegate void UpdateWeaponRotation(Vector3 pCursorPosition, float pCursorScale, float pDistance, bool pCursorVisible);
    public event UpdateWeaponRotation UpdateWeaponRotationEvent;
    public void FireUpdateWeaponRotationEvent(Vector3 pCursorPosition, float pCursorScale, float pDistance, bool pCursorVisible) {
        if (UpdateWeaponRotationEvent != null)
            UpdateWeaponRotationEvent(pCursorPosition, pCursorScale, pDistance, pCursorVisible);
    }

    #endregion
}
