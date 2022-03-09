using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public GameObject slapPrefab;
    public List<Slap> allSlap;
    private List<Material> currentColors = new List<Material>();
    [Tooltip("Количество ладоней")]
    public Vector2 countSlap;
    [Tooltip("Расстояние между ладонями")]
    public Vector2 offcet = new Vector2(5, 5);
    [Tooltip("Таймер включения ладони")]
    public float timerToEnabledSlap = 5f;
    public Transform[] exitPoints;
    public int minLevelToExit = 3;
    private int numberColor = 0;

    private void Awake()
    {
        //StartCoroutine(SpawnAllSlap());
        if (allSlap.Count == 0)
            SpawnSlap();
    }

    private void Start()
    {
        GetComponent<BoxCollider>().enabled = true;
    }

    [ExecuteAlways]
    [ContextMenu("SpawnSlap")]
    void SpawnSlap ()
    {
        for (int i = 0; i < allSlap.Count; i++)
            DestroyImmediate(allSlap[i].gameObject);
        allSlap = new List<Slap>();
        for (int x = 0; x < countSlap.x; ++x)
        {
            for (int y = 0; y < countSlap.y; ++y)
            {
                Vector3 newPos = new Vector3(transform.position.x + x * offcet.x, transform.position.y, transform.position.z - y * offcet.y);
                Slap newSlap = Instantiate(slapPrefab, newPos, Quaternion.identity).GetComponent<Slap>();
                allSlap.Add(newSlap);
                newSlap.transform.SetParent(transform);                
                newSlap.SetColor();
                newSlap.gameObject.SetActive(false);
            }
        }

        System.Random rand = new System.Random();

        for (int i = 0; i < allSlap.Count-1; i++)
        {
            int j = rand.Next(i + 1);

            Slap tmp = allSlap[j];
            allSlap[j] = allSlap[i];
            allSlap[i] = tmp;
        }

    }

    void SetRandomSlapColor (Material slapColor)
    {
        int count = allSlap.Count / 3; int start = count * currentColors.Count;
        for (int i = start; i > start - count; i--)
        {
            Slap slap = allSlap[i];
            slap.SetColor(slapColor);  
            if (!slap.gameObject.activeSelf)
            {                          
                slap.gameObject.SetActive(true);
                
            }
                        
        }
    }

    IEnumerator TimerToCheckAllSlap()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            CheckAllSlap();
        }
    }

    void SwitchMaterial(Slap currentSlap)
    {
        Material c = currentColors[getNextColor()];
        currentSlap.SetColor(c);
    }

    int getNextColor ()
    {
        numberColor++;
        if (numberColor > currentColors.Count - 1)
            numberColor = 0;
        return numberColor;
    }

    public void AddNewColor (Material currentColor)
    {
        if (currentColors.Count == 0)
            StartCoroutine(TimerToCheckAllSlap());
        if (currentColors.IndexOf(currentColor) < 0)
        {
            currentColors.Add(currentColor);
            SetRandomSlapColor(currentColor);
        }
    }


    void CheckAllSlap ()
    {        
        foreach (Slap slap in allSlap)
        {
            if (slap.gameObject.activeSelf == false)
            {
                
                if (slap.timer > 0)
                    slap.timer--;
                else
                {
                    if (slap.timer > -1)
                    {
                        slap.gameObject.SetActive(true);
                        slap.timer = timerToEnabledSlap;
                        SwitchMaterial(slap);
                    }
                }
            }
            

        }
    }

    public Transform GetSlapDeciredColor(Material slapColor)
    {
        Transform slap = null;
        for (int i = Random.Range(0, allSlap.Count/2); i < allSlap.Count-1; i++)
        {
            if (allSlap[i].gameObject.activeSelf && (allSlap[i].currentMaterial == slapColor || allSlap[i].currentMaterial == allSlap[i].deffaultMaterial))
            {
                slap = allSlap[i].transform;
                break;
            }
        }

        return slap;
    }

    public Transform GetExitPoint ()
    {
        return exitPoints[Random.Range(0, exitPoints.Length)];
    }
}
