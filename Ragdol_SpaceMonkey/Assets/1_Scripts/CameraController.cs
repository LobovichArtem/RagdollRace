using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera baseCamera;
    public CinemachineVirtualCamera bossFightCamera;
    private Transform player;
    private CinemachineBasicMultiChannelPerlin _shake;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Joystick>().transform;
        player.gameObject.GetComponent<DamageBase>().damageEvent.AddListener(PlayerOff);
        //player.gameObject.GetComponent<Movement>().brigeEnterExit.AddListener(BrigeCamera);
        baseCamera.Priority = 99;
        _shake = bossFightCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void PlayerOff()
    {
        baseCamera.Priority = 99;
        baseCamera.Follow = null;
        baseCamera.LookAt = null;
        Invoke("PlayerOn", 4f);
    }

    void PlayerOn ()
    {
        baseCamera.Priority = 99;
        baseCamera.Follow = player;
        baseCamera.LookAt = player;
    }


    public void StartBossFight ()
    {
        baseCamera.Priority = 1;
        bossFightCamera.Priority = 99;
    }


    public void CameraShake ()
    {
        StartCoroutine(CameraShakeCor());
    }

    IEnumerator CameraShakeCor ()
    {
        _shake.m_AmplitudeGain = 2;
        _shake.m_FrequencyGain = 2;
        yield return new WaitForSeconds(.15f);
        _shake.m_AmplitudeGain = 0;
        _shake.m_FrequencyGain = 0;
    }

    /*
    private Transform _myCamera;
    public Transform player;
    private bool explosion;
    public Vector3 startPos;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindObjectOfType<Joystick>().transform;
        _myCamera = Camera.main.transform;
        startPos = player.position - _myCamera.position;
        player.GetComponent<DamageBase>().damageEvent.AddListener(PlayerDamage);
    }

    private void Update()
    {
        if (player == null)
            return;
        if (!explosion)
            _myCamera.position = player.position - startPos; //Vector3.MoveTowards(myCamera.position, player.position - startPos, 5 * Time.deltaTime);
    }

    void PlayerDamage ()
    {
        explosion = true;
        Invoke("E", 3f);
    }

    void E ()
    {
        explosion = false;
    }

    public void Explosion()
    {        
        if (!explosion)
        {
            explosion = true;
            List<Vector3> positions = new List<Vector3>(0);
            for (int i = 0; i < 4; i++)
            {
                Vector3 newPos = _myCamera.position + new Vector3(Random.Range(-.2f, .2f), Random.Range(-.25f, .25f), Random.Range(-.2f, .2f));
                positions.Add(newPos);
            }
            StartCoroutine(ExplosionInterpolation(positions));
        }
    }

    IEnumerator ExplosionInterpolation (List<Vector3> positions)
    {
        float speed = 20f;
        positions.Add(_myCamera.position);
        
        for (int i = 0; i < positions.Count; i++)
        {
            while (_myCamera.position != positions[i])
            {
                _myCamera.position = Vector3.MoveTowards(_myCamera.position, positions[i], speed * Time.deltaTime);
                yield return null;                
            }
        }
        explosion = false;
    }

    IEnumerator CameraZoom(Vector3 newTransform, Vector3 newRotate)
    {
        while (_myCamera.localPosition != newTransform && _myCamera.localEulerAngles != newRotate)
        {
            _myCamera.localPosition = Vector3.MoveTowards(_myCamera.localPosition, newTransform, 5 * Time.deltaTime);
            _myCamera.localEulerAngles = Vector3.MoveTowards(_myCamera.localEulerAngles, newRotate, 7 * Time.deltaTime);
            yield return null;
        }
    }
    */


}
