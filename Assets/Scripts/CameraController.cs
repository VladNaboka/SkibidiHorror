using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 3f;

    private float mouseX, mouseY;

    private void Update()
    {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;

        mouseY = Mathf.Clamp(mouseY, -90f, 90f);

        transform.eulerAngles = new Vector3(mouseY, mouseX, 0.0f);
    }
}
