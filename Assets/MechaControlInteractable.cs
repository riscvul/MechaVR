using UnityEngine;
using System.Collections;
using NewtonVR;

public class MechaControlInteractable : NVRInteractableItem {

    //public override void UseButtonDown() {
    //    base.UseButtonDown();

    //    transform.parent.GetComponent<MechaControls>().

    //    AttachedHand.TriggerHapticPulse(500, Valve.VR.EVRButtonId.k_EButton_Axis0);
    //}

    //public override void BeginInteraction(NVRHand hand) {
    //    base.BeginInteraction(hand);

    //    PickupTransform = new GameObject(string.Format("[{0}] NVRPickupTransform", this.gameObject.name)).transform;
    //    PickupTransform.parent = hand.transform;
    //    PickupTransform.position = this.transform.position;
    //    PickupTransform.rotation = this.transform.rotation;

    //    ClosestHeldPoint = (PickupTransform.position - this.transform.position);
    //}

    //public override void EndInteraction() {
    //    base.EndInteraction();

    //    if (PickupTransform != null) {
    //        Destroy(PickupTransform.gameObject);
    //    }

    //    if (VelocityHistory != null) {
    //        this.Rigidbody.velocity = GetMeanVector(VelocityHistory);
    //        this.Rigidbody.angularVelocity = GetMeanVector(AngularVelocityHistory);

    //        VelocityHistoryStep = 0;

    //        for (int index = 0; index < VelocityHistory.Length; index++) {
    //            VelocityHistory[index] = null;
    //            AngularVelocityHistory[index] = null;
    //        }
    //    }
    //}

}
