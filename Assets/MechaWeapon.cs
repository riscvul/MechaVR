using UnityEngine;
using System.Collections;

public class MechaWeapon : MonoBehaviour {

    //Variables
    public float maxLeftRotation = -15;
    public float maxRightRotation = 15;
    public float maxUpRotation = 45;
    public float maxDownRotation = -45;
    public enum FireMode { Single, Semi, Auto };
    public FireMode firemode;
    public float rpm; 
    public float damage = 1;
    public int totalAmmo = 100;
    public int ammoPerMag = 10;
    public int currentAmmoPerMag = 10;
    public float gunID;
    public LayerMask collisionMask;
    public string fireSoundGroup;

    //Components
    private GameObject cannotFireIndicator;
    private LineRenderer laserPointer;
    public Transform spawn;

    //System
    Vector3 originalRotation;
    private float secondsBetweenShots;
    private float timeTillNextShot;
    private float muzzleFlashDuration = 0.1f;
    private float lastMuzzleFlashTime;

    // Use this for initialization
    void Start () {
        cannotFireIndicator = GameObject.Instantiate(Resources.Load("CannotFireIndicator"), Vector3.zero, Quaternion.identity) as GameObject;
        cannotFireIndicator.SetActive(false);
        //cannotFireIndicator.transform.SetParent(Camera.main.transform);

        //Determine seconds between shots for this gun
        secondsBetweenShots = 60 / rpm;
        if (GetComponent<LineRenderer>()) {
            laserPointer = GetComponent<LineRenderer>();
            //laserPointer.SetPosition(0, spawn.position);
        }
    }
	
    public void Update() {
        //Clumsy Weapon Fire Test Animation
        if (laserPointer.enabled && (lastMuzzleFlashTime + muzzleFlashDuration) < Time.time) {
            laserPointer.enabled = false;
        } else if(laserPointer.enabled) {
            laserPointer.SetPosition(0, spawn.position);
            laserPointer.SetPosition(1, cannotFireIndicator.transform.position);
        }
    }

    public virtual void UpdateWeaponRotation(Vector3 pCursorPosition, float pCursorScale, float pDistance, bool pCursorVisible) {
        //Calculate the direction the weapon wants to point in before clamping
        Quaternion oldRotation = transform.rotation;
        Vector3 compareRotation;
        Vector3 targetRotation;
        transform.rotation = Quaternion.LookRotation(pCursorPosition - transform.position, Vector3.up);
        targetRotation = compareRotation = transform.localRotation.eulerAngles;
        transform.rotation = oldRotation;
        targetRotation.y = Utils.ClampAngle(targetRotation.y, maxLeftRotation, maxRightRotation);
        targetRotation.x = Utils.ClampAngle(targetRotation.x, maxDownRotation, maxUpRotation);
        targetRotation.z = 0;
        //small slerp to smooth out jumps
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(targetRotation), Time.deltaTime * 15);

        //Now check if the aiming markers should be visible before performing calculations
        if (pCursorVisible) {
            //Check if end result had to be clamped. This means the weapon is not pointing at the cursor and needs to display its cannotFireIndicator
            cannotFireIndicator.transform.position = spawn.position + spawn.forward * pDistance;
            cannotFireIndicator.transform.localScale = new Vector3(pCursorScale, pCursorScale, 1);
            cannotFireIndicator.transform.LookAt(Camera.main.transform);

            float percentageDifferenceAllowed = 0.001f;
            if ((targetRotation - compareRotation).magnitude >= (targetRotation * percentageDifferenceAllowed).magnitude) {
                cannotFireIndicator.SetActive(true);
                //if (compareRotation != targetRotation) {
                //Set Indicator position

                //Find screen point to see if weapon arc is out of view
                //Vector3 screenPos = Camera.main.WorldToScreenPoint(cannotFireIndicator.transform.position);

                //Code to show indicators for items that are out of sight
                //if(screenPos.z > 0 && (screenPos.x > 0 && screenPos.x < Screen.width) && (screenPos.y > 0 && screenPos.y < Screen.height)) {
                //    //Indicator is in view make sure the canvas reflects that
                //    float newScale = go.GetComponent<LookAndShootMarker>().cursorScaling.Evaluate(distance);
                //    cannotFireIndicator.transform.localScale = new Vector3(newScale, newScale, 1);
                //} else {
                //    //Indicator is out of view, put a marker on the edge of the screen
                //    if(screenPos.z < 0) {
                //        screenPos *= -1;
                //    }

                //    Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2.0f;

                //    float angle = Mathf.Atan2(screenPos.y, screenPos.x);
                //    angle -= 90 * Mathf.Deg2Rad;

                //    float cos = Mathf.Cos(angle);
                //    float sin = -Mathf.Sin(angle);

                //    screenPos = screenCenter + new Vector3(sin * 150, cos * 150, 0);

                //    float m = cos / sin;

                //    Vector3 screenBounds = screenCenter * 0.9f;

                //    //if(cos>0) {
                //    //    screenPos = new Vector3(screenBounds.y / m, screenBounds.y, 0);
                //    //} else {
                //    //    screenPos = new Vector3(-screenBounds.y/m, -screenBounds.y, 0);
                //    //}

                //    screenPos += screenCenter;
                //    Vector3 tempPosition = Camera.main.ScreenToWorldPoint(screenPos) + Camera.main.transform.forward * 500.0f;
                //    float newScale = go.GetComponent<LookAndShootMarker>().cursorScaling.Evaluate(500.0f);
                //    cannotFireIndicator.transform.localScale = new Vector3(newScale, newScale, 1);

                return;
            }
        }
        cannotFireIndicator.SetActive(false);

    }


    public void Shoot() {
        if (CanShoot()) {
            FireBullet();
        }
    }

    public void ShootContinuous() {
        if (firemode == FireMode.Auto) {
            Shoot();
        }
    }

    private bool CanShoot() {
        bool canShoot = true;

        if (Time.time < timeTillNextShot) {
            canShoot = false;
        }

        if (currentAmmoPerMag <= 0) {
            canShoot = false;
        }

        return canShoot;
    }

    protected void FireBullet() {

        //Vector3 ejectionPosition = ejectionPoint.position;

        //currentAmmoPerMag = Mathf.Clamp(currentAmmoPerMag - 1, 0, ammoPerMag);
        //Debug.Log(currentAmmoPerMag);

        lastMuzzleFlashTime = Time.time;
        Debug.Log("pointer enabled?");
        laserPointer.enabled = true;

        //laserPointer.transform.localRotation = Quaternion.Euler(0, 180, Random.Range(0, 359));

        //Now to shoot the bullet and see what we hit
        Vector3 targetPoint = spawn.position + spawn.forward;//
        Ray ray = new Ray(spawn.position, spawn.forward);//targetPoint - spawn.position);
        RaycastHit hit;



        float shotDistance = 1000;

        //GameObject bulletDecal = null;
        if (Physics.Raycast(ray, out hit, shotDistance, collisionMask)) {
            shotDistance = hit.distance;

            //if (hit.collider.GetComponent<Damageable>()) {
            //    hit.collider.GetComponent<Damageable>().TakeDamage(damage);
            //}

            //if (hit.transform.gameObject.layer == LayerMask.NameToLayer("LevelGeometry") || hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor")) {
            //    bulletDecal = Instantiate(bulletHitDecal, hit.point + (hit.normal * Utils.FLOAT_IN_FRONT_OF_WALL_DISTANCE), Quaternion.LookRotation(-hit.normal)) as GameObject;
            //    Instantiate(bulletHitParticles, hit.point + (hit.normal * Utils.FLOAT_IN_FRONT_OF_WALL_DISTANCE), Quaternion.LookRotation(Vector3.Reflect(spawn.position, hit.normal) * -1));
            //}
        }

        timeTillNextShot = Time.time + secondsBetweenShots;
        //aimAccumulation += MaxRecoilForce;
        //if (aimAccumulation.x > aimVarianceMax.x)
        //    aimAccumulation.x = aimVarianceMax.x;
        //if (aimAccumulation.y > aimVarianceMax.y)
        //    aimAccumulation.y = aimVarianceMax.y;

        //MasterAudio.PlaySound3DAtTransform(fireSoundGroup, spawn);

        //TracerEffect.ShowTracerEffect(spawn.position, ray.direction, shotDistance);

        //GameObject newCasing = Instantiate(casingType, ejectionPosition, Quaternion.Euler(90, 0, 0)) as GameObject;
        //newCasing.GetComponent<Rigidbody>().AddForce(ejectionPoint.forward * Random.Range(30f, 40f) + spawn.forward * Random.Range(-10, 10));
        //newCasing.GetComponent<Rigidbody>().AddTorque(0, Random.Range(0f, 30f), Random.Range(0f, 30f));
        ////Add objects to casing manager which should probably be renamed
        //GameObject.FindGameObjectWithTag("Player").GetComponent<CasingManager>().AddCasing(newCasing);
        //if (bulletDecal != null) {
        //    GameObject.FindGameObjectWithTag("Player").GetComponent<CasingManager>().AddBulletHole(bulletDecal);
        //}

        //if (recoilOn) {
        //    GameObject.FindGameObjectWithTag("Player").GetComponent<RootMotion.FinalIK.Recoil>().Fire(1.0f);
        //}

    }
}


