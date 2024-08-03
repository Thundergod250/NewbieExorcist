using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("HIT: " + other.name);
        if (other.GetComponent<AIScript>() != null)
        {
            other.GetComponent<AIScript>().KillThisAI();
            Debug.LogWarning("TESTOR");
        }
        if (other.TryGetComponent(out AIScript aiScript))
        {
            aiScript.KillThisAI();
            Debug.LogWarning("SINDIECOMPONTENT"); 
        }
    }
}
