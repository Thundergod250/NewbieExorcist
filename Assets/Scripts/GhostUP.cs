using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostUP : MonoBehaviour
{
    public float liftSpeed = 2f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        StartCoroutine(LiftAndDestroy());
    }

    private IEnumerator LiftAndDestroy()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 2f)
        {
            rb.MovePosition(rb.position + liftSpeed * Time.deltaTime * Vector3.up);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
