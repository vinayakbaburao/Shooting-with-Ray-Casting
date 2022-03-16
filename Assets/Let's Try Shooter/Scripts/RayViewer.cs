using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayViewer : MonoBehaviour
{
 
    public float WeaponRange = 50f;

    private Camera FpsCam;

    void Start()
    {
        
        FpsCam = GetComponentInParent<Camera>();
        
    }

    
    void Update()
    {
        Vector3 LineOrigin = FpsCam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0));
        Debug.DrawRay (LineOrigin, FpsCam.transform.forward * WeaponRange, Color.green);
    }
}
