using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Players
{
    public DamageBase damageBase;
    public Transform tr;
    public Image _image;
    public Text _text;

    public Players(DamageBase dmgBase)
    {
        damageBase = dmgBase;
    }

}

[System.Serializable]
public class MyUnityEventClass : UnityEvent<int, string> { }

public class LevelController : MonoBehaviour
{
    public GameObject hpBarPrefab;
    public Transform root;
    public GameObject panelWin;
    public Text moneyText;
    public GameObject panelFail;
    public float offcet = 6;
    private Players[] players;
    private Camera _camera;

    public float timeToKnockdown = 3f;
    public float timeToSlowmo = 1f;
    [Tooltip("Скорость времени в замедлении")]
    [Range(0.001f, 1)]
    public float timeSlowmoScale = 0.5f;    
    private bool _slowmo = false;
    [HideInInspector]
    public static MyUnityEventClass levelWinEvent = new MyUnityEventClass();
    [HideInInspector]
    public static MyUnityEventClass levelFailEvent = new MyUnityEventClass();
    [HideInInspector]
    public static MyUnityEventClass levelStartEvent = new MyUnityEventClass();


    public void StartSlowmo ()
    {
        if (!_slowmo)
        {
            StartCoroutine(SlowmoCor());
            _slowmo = true;
        }
    }

    IEnumerator SlowmoCor ()
    {
        Time.timeScale = timeSlowmoScale;
        yield return new WaitForSeconds(timeToSlowmo);
        Time.timeScale = 1;
        _slowmo = false;
    }


    public void Timer (DamageBase _damageBase)
    {
        StartCoroutine(TimerCor(_damageBase));
       
    }

    IEnumerator TimerCor (DamageBase _damageBase)
    {
        _damageBase.enabled = false;
        CheckAllUI(_damageBase);
        yield return new WaitForSeconds(timeToKnockdown);

        _damageBase.EnablePlayer();
        CheckAllUI(_damageBase);
    }

    void CheckAllUI(DamageBase _dmg)
    {
        foreach (Players player in players)
        {
            if (player.damageBase == _dmg)
            {
                player.tr.gameObject.SetActive(_dmg.enabled);
            }
        }
    }

    private void Awake()
    {
        DamageBase[] all = FindObjectsOfType<DamageBase>();
        players = new Players[all.Length];
        
        for (int i = 0; i < all.Length; i++)
        {
            players[i] = new Players(all[i]);
            GameObject hpBar = Instantiate(hpBarPrefab);
            players[i].tr = hpBar.transform;
            players[i].tr.SetParent(root);
            players[i]._text = players[i].tr.GetComponentInChildren<Text>();
            if (players[i].damageBase.hpToNextLevel > 0)
            {
                players[i]._image = players[i].tr.GetChild(0).GetComponent<Image>();
            }
            else
            {
                players[i].tr = players[i]._text.transform;
                players[i].tr.SetParent(root);
                players[i]._text.text = players[i].damageBase.level.ToString();
                Destroy(hpBar);
            }
        }
        _camera = Camera.main;
        StartCoroutine(UpdateBar());
    }

    private IEnumerator UpdateBar()
    {
        while (true)
        {
            foreach (Players player in players)
            {
                if (player.damageBase.enabled)
                {
                    player.tr.position = _camera.WorldToScreenPoint(player.damageBase.transform.position + new Vector3(0, offcet, 0));
                    if (player._image != null)
                    {
                        //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
                        float fill = (float)player.damageBase.hp / (float)player.damageBase.hpToNextLevel;
                        player._image.fillAmount = fill;
                        player._text.text = player.damageBase.level.ToString();
                    }
                }

            }
            yield return null;
        }
    }

    private void Start()
    {
        if (levelStartEvent != null)
            levelStartEvent.Invoke(MySceneManager.currentLevelNum, MySceneManager.currentSceneName);
    }

    public void StartBossFight ()
    {
        FindObjectOfType<CameraController>().StartBossFight();
        root.gameObject.SetActive(false);
        foreach (Players player in players)
        {
            if (player.damageBase.enabled)
            {
                player.damageBase.enabled = false;
            }
        }
    }

    public void LevelWin ()
    {
        StartSlowmo();
        Invoke("PanelWin", timeToSlowmo*2.5f);
        
        if (levelWinEvent != null)
            levelWinEvent.Invoke(MySceneManager.currentLevelNum, MySceneManager.currentSceneName);
    }

    void PanelWin()
    {
        moneyText.text = DamageBase.score.ToString();
        panelWin.SetActive(true);
        MySceneManager.AddMoney(DamageBase.score);
        MySceneManager.AddNextLevel();
        
    }

    public void LevelFail ()
    {
        panelFail.SetActive(true);
        Movement[] a = FindObjectsOfType<Movement>();
        foreach (Movement d in a)
        {
            d.enabled = false;
        }


        if (levelFailEvent != null)
            levelFailEvent.Invoke(MySceneManager.currentLevelNum, MySceneManager.currentSceneName);
    }

    public static void Vibration ()
    {
        //MMVibrationManager.Haptic();
    }
}
