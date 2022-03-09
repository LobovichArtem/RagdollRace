using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossFight : MonoBehaviour
{
    public GameObject bossFightPanel;
    public GameObject [] damageParticleBoss;
    public GameObject damageParticlePlayer;

    public Image hpBossImage;
    public Image hpPlayerImage;
    public int hpBoss = 7;
    private float _hpBoss;
    public int hpPlayer = 10;
    private float _hpPlayer;
    public Animator bossAnimator;
    public Animator playerAnimator;
    private CameraController _cameraController;

    public void StartBossFight (GameObject go)
    {
        if (!go.GetComponentInChildren<DamageBase>()._isPlayer)
        {
            FindObjectOfType<LevelController>().LevelFail();
            return;
        }

        _hpPlayer = hpPlayer;
        _hpBoss = hpBoss;
        _cameraController = FindObjectOfType<CameraController>();
        bossFightPanel.SetActive(true);
        playerAnimator = go.GetComponent<Animator>();
        playerAnimator.enabled = true;
        FindObjectOfType<LevelController>().StartBossFight();
        StartCoroutine(BossFightCor());
        StartCoroutine(UpdateCor());

        if (bossAnimator == null)
            bossAnimator = GetComponent<Animator>();
    }

    IEnumerator BossFightCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            bossAnimator.SetTrigger("attack");
            yield return new WaitForSeconds(.42f);
            damageParticlePlayer.SetActive(true);
            _hpPlayer -= 1;
            playerAnimator.SetTrigger("damage");
            CheckStatus();
            yield return new WaitForSeconds(.7f);
        }
    }

    void CheckStatus ()
    {
        hpBossImage.fillAmount = ( _hpBoss / (float)hpBoss);
        hpPlayerImage.fillAmount = (_hpPlayer / (float)hpPlayer);
        if (_hpBoss <= 0)
        {
            bossAnimator.SetTrigger("dead");
            FindObjectOfType<LevelController>().LevelWin();
            StopAllCoroutines();
            damageParticleBoss[1].SetActive(true);
            bossFightPanel.SetActive(false);
            return;
        }
        if (_hpPlayer <= 0)
        {
            FindObjectOfType<LevelController>().LevelFail();
            StopAllCoroutines();
            return;
        }

    }

    private bool _hit;
    private IEnumerator UpdateCor()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !_hit)
            {
                playerAnimator.SetTrigger("attack");
                _hit = true;
                yield return new WaitForSeconds(.42f);
                _hpBoss--;
                bossAnimator.SetTrigger("damage");
                damageParticleBoss[0].SetActive(true);
                CheckStatus();
                _cameraController.CameraShake();
                yield return new WaitForSeconds(1f);                
                _hit = false;
            }
            yield return null;
        }
    }
}
