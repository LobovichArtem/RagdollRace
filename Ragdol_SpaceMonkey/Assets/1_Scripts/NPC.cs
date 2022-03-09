using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : DamageBase
{
    public Animator animator;

    private void Awake()
    {
        base.Awake();
    }

    public override void Damage(Transform go)
    {
        DamageBase _dmgParams = go.root.GetComponentInChildren<DamageBase>();
        if (_dmgParams.currentMaterial == base.currentMaterial)
        {
            StartCoroutine(GetDown());//анимация исчезновения или что-то такое
            
        }
        else
        {            
            if (_dmgParams.level > level)
            {
                base.InstantiateRagdoll(transform.position);
                GetComponent<CharacterController>().enabled = false;
                _dmgParams.LowerLevel(level);
                currentMaterial = _dmgParams.currentMaterial;
                SwapColor();                
                if (_myRecources != null)
                    _myRecources.InstantiateDamageParticle(transform.position + new Vector3(0, 5, 0));
                if (_dmgParams._isPlayer)
                    _levelController.StartSlowmo();

                transform.root.position += new Vector3(0, 100, 0);
            }
            else
            {                
                _dmgParams.LowerLevel(level);
                _dmgParams.Damage(transform);
                
                StartCoroutine(MoveBack(_dmgParams.transform.root));
                if (animator != null)
                    animator.SetTrigger("attack");
            }
        }
    }

    IEnumerator MoveBack(Transform tr)
    {
        float t = 3;
        while (t > 0)
        {
            tr.Translate(Vector3.back * 9 * Time.deltaTime, Space.World);
            t -= Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator GetDown()
    {
        Vector3 newPos = transform.root.position;
        float t = 1.5f;
        while (t > 0)
        {
            transform.root.Translate(0, -25 * Time.deltaTime, 0);
            t -= Time.deltaTime;
            yield return null;
        }
        transform.root.position = newPos;
    }
}
