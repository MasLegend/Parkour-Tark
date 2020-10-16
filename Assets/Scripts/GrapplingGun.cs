using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{

    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public LayerMask whatBlocksGrapple;
    public Transform gunTip, camera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;
    private int NumberPush;
    public int MaxPush;
    public float pullForce;
    public GameObject bullet;
    public float speed = 100f;
    private RaycastHit hit1;
    private RaycastHit hit2;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canGrapple())
            {
                StartGrapple();
            }
        }

        //condition to stop grappling if an object gets between
        //the grapple point and the player (not finished)

        //else if (Input.GetMouseButtonUp(0) || !stillCanGrapple()) 
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();

        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            CloseJoints();

        }

    }

        //Called after Update
        void LateUpdate()
        {
            DrawRope();
        }

        /// <summary>
        /// Call whenever we want to start a grapple
        /// </summary>
        void StartGrapple()
        {
            GameObject instaBullet = Instantiate(bullet, gunTip.position, Quaternion.identity) as GameObject;

            Rigidbody instaBulletRigidBody = instaBullet.GetComponent<Rigidbody>();

            instaBulletRigidBody.velocity = camera.forward * speed;

            FindObjectOfType<AudioManager>().Play("GrapplingShot");

            grapplePoint = hit1.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = 4.5f;
            joint.damper = 15f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;


        }

        /// <summary>
        /// Call whenever we want to get joint closer
        /// </summary>
        void CloseJoints()
        {
            if (NumberPush > MaxPush - 1)
            {
                return;
            }
            if (joint)
            {
                NumberPush++;
                joint.connectedAnchor = grapplePoint;

                float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

                //The distance grapple will try to keep from grapple point. 

                joint.maxDistance = 10f;
                joint.minDistance = distanceFromPoint * 0.25f;

                //Adjust these values to fit your game.
                joint.spring = pullForce;
                joint.damper = 7f;
                joint.massScale = 4.5f;

                lr.positionCount = 2;
                currentGrapplePosition = gunTip.position;
            }
        }

        /// <summary>
        /// Call whenever we want to stop a grapple
        /// </summary>
        void StopGrapple()
        {
            lr.positionCount = 0;
            NumberPush = 0;
            Destroy(joint);
        }
    

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }

    public bool canGrapple()
    {
        if (Physics.Raycast(camera.position, camera.forward, out hit1, maxDistance, whatIsGrappleable))
        {
            if (Physics.Raycast(camera.position, camera.forward, out hit2, maxDistance, whatBlocksGrapple))
            {
                if (hit1.distance < hit2.distance)
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }
    public bool stillCanGrapple()
    {
        Vector3 fromGun = gunTip.transform.position;
        Vector3 direction = fromGun - grapplePoint;

        if (Physics.Raycast(gunTip.position, direction, out hit1, maxDistance, whatIsGrappleable))
        {
            if (Physics.Raycast(gunTip.position, direction, out hit2, maxDistance, whatBlocksGrapple))
            {
                if (hit1.distance < hit2.distance)
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

}