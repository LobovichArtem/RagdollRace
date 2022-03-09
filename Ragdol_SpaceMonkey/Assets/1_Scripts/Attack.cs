using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Animator animator;
    private bool isPlayer = false;
    public DamageBase damageBase;
    private Rigidbody rb;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        if (damageBase.gameObject.GetComponent<Joystick>())
            isPlayer = true;
        rb = GetComponent<Rigidbody>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.collider.CompareTag("Interactable"))
        {
            DamageBase _damageBase = collision.collider.GetComponent<DamageBase>();
            _damageBase.Damage(new DamageParams(damageBase, rb.velocity)); //, isPlayer));
            if (damageBase.level <= _damageBase.level)
                return;
            
            if (isPlayer)
                damageBase._levelController.StartSlowmo();
            
            if (!animator)
                return;
            animator.SetTrigger("attack");
            
        }
        */
    }
}
