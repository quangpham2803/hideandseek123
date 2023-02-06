using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 position;
    public float speed = 15;

    private void Start()
    {
        position = transform.position;
    }

    private void Update()
    {
        position.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.position = position;
    }
}
