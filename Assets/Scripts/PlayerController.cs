using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    private void Update()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * Time.deltaTime;

        transform.position += new Vector3(x * speed, 0, z * speed);
    }
}
