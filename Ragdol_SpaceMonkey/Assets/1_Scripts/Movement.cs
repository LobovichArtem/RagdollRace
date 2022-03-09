using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //public bool ai = false;
    public float speedMove = 8;
    public float speedRotation = 5;
    public Animator animator;

    private Vector3 direction;
    private Vector3 moveDirection;
    private CharacterController characterController;
    private Transform tr;
    private Transform autoMoveTarget;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        tr = transform;
        
    }

    public void SetDirection(Vector3 _dir)
    {
        if (autoMoveTarget == null)
            direction = _dir;
    }

    private void Update()
    {
        if (direction != Vector3.zero)
        {
            //Vector3 relativePos = new Vector3(direction.x, 0, direction.y);// (tr.position + new Vector3(direction.x, 0, direction.y)) - tr.position;
            Quaternion targetDir = Quaternion.LookRotation(direction, Vector3.up);
            tr.rotation = Quaternion.Lerp(tr.rotation, targetDir, speedRotation*Time.deltaTime);

        }
    }

    void FixedUpdate()
    {        
		//direction = new Vector3(Joystick.direction.x, 0, Joystick.direction.y);                        
		
        moveDirection = new Vector3(0, -1, direction.magnitude*speedMove);
        moveDirection = tr.TransformDirection(moveDirection);
        characterController.Move(moveDirection);
        
        if (animator == null)
            return;
        float anim = characterController.velocity.magnitude / speedMove;
        animator.SetFloat("move", anim);
    }

    private void OnTriggerEnter(Collider myCol)
    {
        if (myCol.CompareTag("GameController"))
        {
            autoMoveTarget = myCol.transform;
        }
    }

    private void OnTriggerStay(Collider myCol)
    {
        if (myCol.CompareTag("GameController"))
        {
            Vector3 _dir = Vector3.ClampMagnitude(autoMoveTarget.position - tr.position, 1).normalized;
            _dir.y = 0;
            direction = _dir;
        }
    }

    private void OnTriggerExit(Collider myCol)
    {
        if (myCol.CompareTag("GameController"))
        {
            if (autoMoveTarget == myCol.transform)
                autoMoveTarget = null;
        }
    }

    private void OnEnable()
    {
        autoMoveTarget = null;
    }

    /*

    void Update()
    {
        //direction = new Vector3(Joystick.direction.x, 0, Joystick.direction.y);                        
        if (direction != Vector3.zero)
        {
            //handTarget.position = tr.position;
            Quaternion targetDir = Quaternion.LookRotation(direction, Vector3.up);
            //handTarget.rotation = Quaternion.Lerp(handTarget.rotation, targetDir, speedRotation * Time.deltaTime);
            tr.rotation = Quaternion.Lerp(tr.rotation, targetDir, speedRotation * Time.deltaTime);
            //tr.rotation = Quaternion.Euler(handTarget.eulerAngles + new Vector3(-90, 0, -90));

            animator.SetFloat("move", 1);
        }
        else
            animator.SetFloat("move", 0);

        if (animator == null)
            return;
        float anim = direction.magnitude / speedMove;
        
    }

    /*private void FixedUpdate()
    {
        //moveDirection = new Vector3(0, 0, direction.magnitude * speedMove);
        if (direction != Vector3.zero)
        {
            rb.AddForce(tr.forward * speedMove, ForceMode.VelocityChange);
            //footLeft.AddForce((targetFootLeft.position - footLeft.transform.position).normalized * force); 
            //footRight.AddForce((targetFootRight.position-footRight.transform.position).normalized * force);
        }
    }*/

}

