using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensitivity = 100f;
    private float yRotation;
    private Quaternion originalRotation;

    private void Start()
    {
        yRotation = transform.localEulerAngles.y;
        originalRotation = transform.localRotation;
    }

    private void Update()
    {
        float rotationInput = 0f;

        if (Input.GetKey(KeyCode.A))
            rotationInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            rotationInput = 1f;

        yRotation += rotationInput * sensitivity * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(originalRotation.eulerAngles.x, yRotation, originalRotation.eulerAngles.z);
    }
}
