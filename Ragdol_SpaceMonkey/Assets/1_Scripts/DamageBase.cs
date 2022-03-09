using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DamageBase : MonoBehaviour
{
    public int level = 1;
    public int hp = 0;
    [Tooltip("Количество хп необходимое до увеличения уровня")]
    public int hpToNextLevel = 5;
    public float force = 200f;
    public float timeStanLock = 3f;
    public float slapScale = .2f;
    public Material currentMaterial;

    public GameObject ragdoll;
    public GameObject slapHand;
    public GameObject levelUpParticles;
    
    protected GameObject _currentRagdol;
    public ArenaController _arenaController;
    public LevelController _levelController;
    private Movement _movement;
    protected MyRecources _myRecources;
    public bool _isPlayer = false;
    public static int score;
    [HideInInspector]
    public UnityEvent damageEvent = new UnityEvent();
    [HideInInspector]
    public UnityEvent updateLevelEvent = new UnityEvent();
    [HideInInspector]
    public UnityEvent newArenaEvent = new UnityEvent();
    [HideInInspector]
    public UnityEvent finishEvent = new UnityEvent();

    protected void Awake()
    {
        _levelController = FindObjectOfType<LevelController>();
        _myRecources = FindObjectOfType<MyRecources>();
        try {_movement = GetComponent<Movement>(); }
        catch { }
        if (GetComponent<Joystick>())
            _isPlayer = true;
        if (currentMaterial == null)
            currentMaterial = transform.root.GetComponentInChildren<Renderer>().sharedMaterial;
        SwapColor();
        UpdateSlapHand();
    }

    private void Start()
    {
        if (!_isPlayer)
            FindObjectOfType<Joystick>().gameObject.GetComponent<DamageBase>().finishEvent.AddListener(Finish);
    }

    protected void SwapColor(GameObject go)
    {
        Renderer[] mshr = go.transform.GetComponentsInChildren<Renderer>();
        
        foreach (Renderer mr in mshr)
            mr.material = currentMaterial;
    }

    protected void SwapColor ()
    {
        Renderer[] mshr = transform.root.GetComponentsInChildren<Renderer>();
        foreach (Renderer mr in mshr)
            mr.material = currentMaterial;
    }

    public virtual void Damage(Transform go)
    {
        DamageBase _dmgParams = go.root.GetComponentInChildren<DamageBase>();
        if (_dmgParams.level > level)
        {
            InstantiateRagdoll(go.transform.position);
            LowerLevel(1);            
            if (_movement != null)
                _movement.enabled = false;
            GetComponent<CharacterController>().enabled = false;
            SwapColor(_currentRagdol);
            _myRecources.InstantiateSlap(transform.position, level);
            if (damageEvent != null)
                damageEvent.Invoke();
            if (_dmgParams._isPlayer)
                _levelController.StartSlowmo();
            transform.root.position += new Vector3(0, 100, 0);  //= new Vector3(transform.position.x, transform.position.y - 100, transform.position.z);            
        }
    }

    public void LowerLevel (int damageLvl)
    {
        level -= damageLvl;
        
        UpdateLevel();
                
        
    }

    void UpdateLevel ()
    {
        if (slapHand == null)
            return;        
        if (hp >= hpToNextLevel)
        {
            level++;
            hp = 0;
            if (_isPlayer)
            {
                levelUpParticles.SetActive(true);
                score += 2;
            }
        }
        UpdateSlapHand();

        if (updateLevelEvent != null)
            updateLevelEvent.Invoke();

    }

    void UpdateSlapHand()
    {
        if (slapHand == null)
            return;
        float l = (level * slapScale) + 1;
        slapHand.transform.localScale = new Vector3(l, l, l);
    }

    protected void InstantiateRagdoll (Vector3 newPos)
    {
        
        _currentRagdol = Instantiate(ragdoll, transform.position, transform.rotation);
        Vector3 dir = (transform.position - newPos) * force;
        Rigidbody[] rb = _currentRagdol.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody r in rb)
        {            
            r.AddForce(dir, ForceMode.Impulse);
        }
        _levelController.Timer(this);

    }

    public void EnablePlayer()
    {
        level = Mathf.Max(level, 1);
        enabled = true;
        transform.root.position -= new Vector3(0, 100, 0);
        Destroy(_currentRagdol);
        if (_movement != null)
            _movement.enabled = true;
        GetComponent<CharacterController>().enabled = true;
    }

    private void OnTriggerEnter(Collider myCol)
    {
        switch (myCol.tag)
        {
            case "Player":
                if (this == GetComponent<NPC>())
                    return;
                Material slapColor = myCol.GetComponent<Slap>().currentMaterial;
                if (slapColor == currentMaterial || slapColor.name == "gray")
                {
                    hp++;
                    myCol.gameObject.SetActive(false);
                    UpdateLevel();
                    if (_isPlayer)
                    {
                        _myRecources.InstantiateSlapParticle(myCol.transform.position);
                        score++;
                    }
                    
                }
                break;
            case "Arena":        
                _arenaController = myCol.GetComponent<ArenaController>();
                _arenaController.AddNewColor(currentMaterial);
                if (newArenaEvent != null)
                    newArenaEvent.Invoke();
                break;
            case "Slap":
                Damage(myCol.transform);
                break;
            case "Finish":
                myCol.SendMessage("StartBossFight", transform.root.gameObject);
                _movement.enabled = false;
                Rigidbody[] rg = transform.root.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody r in rg)
                    r.isKinematic = true;
                transform.root.position = transform.position;
                transform.localPosition = Vector3.zero;
                if (finishEvent != null)
                    finishEvent.Invoke();                    
                enabled = false;
                break;

        }
    }

    void Finish ()
    {
        if (_movement == null)
            return;
        _movement.enabled = false;
        enabled = false;
    }
    


    /*
    
    public void Damage(DamageParams damageParams)
    {
        if (damageParams.level > level)
        {
            level--;
            level = Mathf.Max(level, 1);
            targetHand.SetActive(false);
            Vector3 dir = (transform.position - damageParams.newPosition).normalized * force;
            root.GetComponent<Rigidbody>().isKinematic = false;            
            root.GetComponentInChildren<Rigidbody>().AddForce(dir, ForceMode.Impulse);
            StartCoroutine(Timer());
        }
    }

    IEnumerator Timer ()
    {
        yield return new WaitForSeconds(timeStanLock);
        root.GetComponent<Rigidbody>().isKinematic = true;
        root.transform.localPosition = Vector3.zero;
        root.transform.localEulerAngles = Vector3.zero;
        targetHand.SetActive(true);


    }
    */
    /*
    public void Damage(DamageParams _dmgParams)
    {
        if (_dmgParams.level > level)
        { 
            level--;
            level = Mathf.Max(level, 1);
            _currentRagdol = Instantiate(ragdoll, transform.position, Quaternion.identity);
            foreach (MonoBehaviour mb in scripts)
                mb.enabled = false;
            GetComponent<CharacterController>().enabled = false;
            transform.GetChild(0).localPosition = new Vector3(0, -100, 0);
            _currentRagdol.SetActive(true);
            StartCoroutine(Timer(_dmgParams.newPosition));
            if (_dmgParams.moveBack)
                StartCoroutine(MoveBack());

            if (_dmgParams.player)
                _levelController.StartSlowmo();

            if (returnDamage > 0)
            {
                _dmgParams.attackingPerson.SendMessage("Damage", returnDamage);
            }
        }
        else
        {
            if (returnDamage > 0)
            {
                _dmgParams.attackingPerson.SendMessage("Damage", new DamageParams(gameObject, returnDamage, transform.position, false, true));
            }
        }
    }

    public void Damage (int damage)
    {
        level--;
        level = Mathf.Max(level, 1);
    }

    IEnumerator Timer(Vector3 newPos)
    {
        yield return new WaitForSeconds(.1f);
        Rigidbody[] rb = _currentRagdol.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody r in rb)
        {
            Vector3 dir = (r.transform.position - newPos).normalized * force;
            r.AddForce(dir, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(timeStanLock);
        transform.GetChild(0).localPosition = Vector3.zero;
        Destroy(_currentRagdol);
        foreach (MonoBehaviour mb in scripts)
            mb.enabled = true;

        GetComponent<CharacterController>().enabled = true;
    }

    IEnumerator MoveBack ()
    {
        while (_currentRagdol != null)
        {
            transform.Translate(Vector3.back*8*Time.deltaTime, Space.World);
            yield return null;
        }
    }
    */




}
