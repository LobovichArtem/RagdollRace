using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    private Movement _movement;
    private DamageBase _damageBase;    

    private Transform _target;
    private Transform tr;
    private int _deciredLevel;

    private void Awake()
    {
        tr = transform;
        _movement = GetComponent<Movement>();
        _damageBase = GetComponent<DamageBase>();
        _damageBase.updateLevelEvent.AddListener(UpdateLevel);
        _damageBase.damageEvent.AddListener(DamageLevel);
        _damageBase.newArenaEvent.AddListener(NewArena);
        
    }

    private void Start()
    {
        


        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        yield return new WaitForSeconds(1.5f);
        GetRandomLevel();
        _target = _damageBase._arenaController.GetSlapDeciredColor(_damageBase.currentMaterial);
        while(true)
        {
            if (_target != null)
            {
                if (_target.gameObject.activeSelf)
                {
                    Vector3 _direction = Vector3.ClampMagnitude(_target.position - tr.position, 1).normalized;
                    if (_movement != null)
                        _movement.SetDirection(new Vector3(_direction.x, 0, _direction.z));
                }
                else
                {
                    yield return new WaitForSeconds(Random.Range(.1f, .4f));
                    _target = _damageBase._arenaController.GetSlapDeciredColor(_damageBase.currentMaterial);

                }
            }
            else
            {
                _movement.SetDirection(Vector3.zero);
                _target = _damageBase._arenaController.GetSlapDeciredColor(_damageBase.currentMaterial);
            }

            yield return new WaitForSeconds(.2f);
        }
    }


    void UpdateLevel ()
    {
        if (_damageBase.level >= _deciredLevel)
        {
            _target = _damageBase._arenaController.GetExitPoint();
        }
    }

    void DamageLevel ()
    {
        GetRandomLevel();
    }

    void GetRandomLevel()
    {
        _deciredLevel = _damageBase._arenaController.minLevelToExit + Random.Range(-2, 3);        
    }

    void NewArena ()
    {
        _target = _damageBase._arenaController.GetSlapDeciredColor(_damageBase.currentMaterial);
    }

}
