using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public GrapplingGun grappling;

    private Quaternion desiredRotation;
    private float rotationSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        if (!grappling.IsGrappling())
            desiredRotation = transform.parent.rotation;
        else
            transform.LookAt(grappling.GetGrapplePoint());

        transform.rotation = Quaternion.Lerp(a: transform.rotation, b: desiredRotation, t: Time.deltaTime * rotationSpeed);
    }
}
