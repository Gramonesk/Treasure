using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header(header: "Player Configurations")]
    public GameObject Facedirection;
    public FloatValue PlayerRotateSpeed;
    public FloatValue speed;
    public FloatValue jumpspeed;
    public FloatValue gravity;
    public FloatValue dashMagnitude;
    public FloatValue dashStartMoveDuration;
    public LayerMask layermask;
    [Tooltip("Set this to 0 if you dont want any friction")]
    public FloatValue frictionspeed;
    [Space]
    [Header(header: "External Force")]
    public FloatValue forceDamp;
    public FloatValue forcespeed;
    public Vector3 forceToApply;
    [Space]
    [Header("Other settings")]
    public FloatValue pushPower;
    public FloatValue groundedDist;
    
    private CharacterController characterController;
    private Vector3 speedchara;
    [HideInInspector]
    public Vector3 MoveSpeed;
    [HideInInspector]
    public bool canMove = true;
    private void OnEnable()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {   
        if (isgrounded())
        {
            #region Dash
            if (Input.GetKeyDown(KeyCode.R))
            {
                ApplyForce(dashMagnitude.value);
                canMove = false;
                Invoke("StopDash", dashStartMoveDuration.value);
            }
            #endregion
            #region InternalForce
            speedchara.z = canMove ? Input.GetAxis("Vertical") : 0;
            speedchara.x = canMove ? Input.GetAxis("Horizontal") : 0;

            if (speedchara.magnitude > 1)
            {
                speedchara = speedchara.normalized;
            }
            MoveSpeed = speed.value * speedchara;

            if (MoveSpeed != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(MoveSpeed);
                Facedirection.transform.rotation = Quaternion.RotateTowards(Facedirection.transform.rotation, targetRotation, PlayerRotateSpeed.value);
            }

            if (Input.GetButtonDown("Jump") && canMove)
            {
                MoveSpeed.y = jumpspeed.value;
            }
            #endregion
            #region ExternalForce
            MoveSpeed += forceToApply;
            forceToApply = Vector3.Lerp(forceToApply, Vector3.zero, forceDamp.value * Time.deltaTime);

            if(forceToApply.magnitude <= forcespeed.value)
            {
                forceToApply = Vector3.zero;
            }
            #endregion
            #region Friction (Optional)
            if (Mathf.Abs(MoveSpeed.x) + Mathf.Abs(MoveSpeed.z) <= frictionspeed.value)
            {
                MoveSpeed.x = 0;
                MoveSpeed.z = 0;
            }
            #endregion
           
 
        }
        MoveSpeed.y -= gravity.value * Time.deltaTime;
        characterController.Move(MoveSpeed * Time.deltaTime);
    }
    public void OnCollisionEnter(Collision collision)
    {
        //if collided with something that has to do with ExternalForce
        //forceToApply = 
        // Opt 1 : collision.GetComponent<Item>().ApplyForceSpeed;
        // Opt 2 : new Vector3(-X, -Y, -Z)
    }
    public void ApplyForce(float force)
    {
        forceToApply = Facedirection.transform.forward * force;
    }
    public void StopDash()
    {
        canMove = true;
    }
    public bool isgrounded()
    { 
        RaycastHit hit;
        Physics.Raycast(gameObject.transform.position, new Vector3(0, -1, 0), out hit, 100, layermask);
        if (hit.distance > groundedDist.value || hit.distance == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Item obj = hit.collider.GetComponent<Item>();
        if (obj != null)
        {
            obj.ApplyForce(new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z) * pushPower.value * MoveSpeed.magnitude);
        }
    }
}
