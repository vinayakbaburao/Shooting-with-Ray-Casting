using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastShooting : MonoBehaviour
{
    //Public variables

    public int GunDamage = 1;                           //For initiliazing Gun Damage

    public float FireRate = .25f;                       //For the rate of fire per second

    public float WeaponRange = 50f;                     //For the range of the gun

    public float HitForce = 100f;                       //For the Damage given by the fire

    public Transform GunEnd;                            //For holding the gun at which the laser line begins

    
    //Private variables

    private Camera FpsCam;                              //For the Camera being used in FPS

    private WaitForSeconds ShotDuration = new WaitForSeconds(.07f);     //For how long the lase remains visible

    private AudioSource GunAudio;                       //For storing the sound file

    private LineRenderer LaserLine;                     //For displaying a line in game view

    private float NextFire;                             //For storing the time for next fire 

    //For the Particle System 

    public ParticleSystem MuzzleFlash;                  //For the Muzzle flash

    public GameObject ImpactEffect;                     //For when bullet strikes objects


    void Start()
    {
        
        LaserLine = GetComponent<LineRenderer> ();      //For storing the Laser line 

        GunAudio = GetComponent<AudioSource> ();        //For storing the Gun Audio

        FpsCam = GetComponentInParent<Camera> ();       //For storing the Fps cam 

    }

    
    void Update()
    {
        //To check if the player has pressed fire button and check if the time has changed after fire

        if(Input.GetButtonDown ("Fire1") && Time.time >NextFire)
        {
            //For updating Next fire
            NextFire = Time.time + FireRate;            
        
            //For Starting the Coroutine
            StartCoroutine(ShotEffect());                   

            //For displaying the camera in world space
            Vector3 RayOrigin = FpsCam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0));

            //For storing the information about the collider hit
            RaycastHit Hit;                             

            //For the initial position for the Lase line
            LaserLine.SetPosition(0, GunEnd.position);        

            /*For utilizing Physics.Raycast   Point in which it originates, the direction, the information
            about the object being hit and finally the range for the ray. */
            if(Physics.Raycast(RayOrigin, FpsCam.transform.forward, out Hit, WeaponRange))
            {
                //For if the ray hits something
                LaserLine.SetPosition(1, Hit.point);

                //For checking if there is a shootable object 
                ShootableBox health = Hit.collider.GetComponent<ShootableBox> ();

                if(health != null)
                {
                    health.Damage(GunDamage);
                }

                if(Hit.rigidbody != null)
                {
                    Hit.rigidbody.AddForce(-Hit.normal * HitForce);
                }
            }
            else
            {
                //For if the ray doesn't hit anything
                LaserLine.SetPosition(1, RayOrigin + (FpsCam.transform.forward * WeaponRange));
            }

            GameObject ImpactGobj = Instantiate(ImpactEffect, Hit.point, Quaternion.LookRotation(Hit.normal));
            Destroy(ImpactGobj, 2f);
        }
    }

    private IEnumerator ShotEffect()
    {
        MuzzleFlash.Play();                             //For displaying the muzzle flash when firing 
        
        GunAudio.Play();                                //For playing the gun audio file

        LaserLine.enabled = true;                       //For rendering the lase line

        yield return ShotDuration;                      //For the laser line to wait for .07 seconds

        LaserLine.enabled = false;                      //For disabeling the laser line
    }

}
