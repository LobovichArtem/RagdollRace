using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[Serializable]
public class Slap : MonoBehaviour
{
    public Material deffaultMaterial;
    public Material currentMaterial;
    public float timer = -1;
    void Awake ()
    {
        
    }

   /* public static Color GetColor (SlapColor color)
    {
        Color newColor = new Color();
        switch (color)
        {
            case SlapColor.blue:
                newColor = Color.blue;
                break;
            case SlapColor.green:
                newColor = Color.green;
                break;
            case SlapColor.grey:
                newColor = Color.grey;
                break;
            case SlapColor.yellow:
                newColor = Color.yellow;
                break;
            case SlapColor.purple:
                newColor = Color.magenta;
                break;
            case SlapColor.red:
                newColor = Color.red;
                break;
            default:
                newColor = Color.grey;
                break;

        }
        return newColor;
    }
   */
    public void SetColor(Material color)
    {
        GetComponent<Renderer>().material = color;
        currentMaterial = color;
        if (timer < 0)
            timer = 1;
    }

    //set deffault color
    [ExecuteAlways]
    public void SetColor ()
    {
        timer = -1;
        GetComponent<Renderer>().material = deffaultMaterial;
        currentMaterial = deffaultMaterial;
    }


}
