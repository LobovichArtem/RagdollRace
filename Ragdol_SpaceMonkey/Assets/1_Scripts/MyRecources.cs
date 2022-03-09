using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRecources : MonoBehaviour
{
    public List<GameObject> slapRigidbody;
    public List<GameObject> damageParticle;
    public List<GameObject> slapParticle;


    public void InstantiateSlapParticle (Vector3 newPos)
    {
        InstantiateNeedParticle(slapParticle, newPos);
    }

    public void InstantiateDamageParticle (Vector3 newPos)
    {
        InstantiateNeedParticle(damageParticle, newPos);
    }

    public void InstantiateSlap (Vector3 newPos, int count)
    {
        int k = 0;
        newPos.y += 5f;

        InstantiateDamageParticle(newPos);
        for (int i = 0; i < count; i ++)
        {
            foreach (GameObject g in slapRigidbody)
            {
                if (!g.activeSelf)
                {
                    g.transform.position = newPos;
                    g.GetComponent<Slap>().SetColor();
                    g.SetActive(true);
                    g.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-300, 300), 75, Random.Range(-300, 300)));
                    k++;
                    break;
                }
            }
        }

        for (; k<count; )
        {
            slapRigidbody.Add(Instantiate(slapRigidbody[0], newPos, Quaternion.identity));
            k++;
        }

    }

    private void InstantiateNeedParticle (List<GameObject> currentList, Vector3 newPos)
    {
        GameObject go = null;
        foreach (GameObject g in currentList)
        {
            if (!g.activeSelf)
            {
                go = g;
                break;
            }
        }
        if (go == null)
        {
            go = Instantiate(currentList[0], newPos, Quaternion.identity);
            currentList.Add(go);
        }
        go.transform.position = newPos;
        go.SetActive(true);
    }
}
