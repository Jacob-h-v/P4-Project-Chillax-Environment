using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualInput : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    private float moveAD;
    private float moveWS;
    public float speed = 1f;
    public Transform playerObject;

    // Update is called once per frame
    void Update()
    {
        moveAD = Input.GetAxis("Horizontal");
        moveWS = Input.GetAxis("Vertical");

        playerObject.Rotate(Vector3.up * speed * moveAD);
        playerObject.Rotate(Vector3.right * speed * moveWS);
        // Open volume controls when pressing escape
        if (Input.GetKey("escape"))
        {
            canvas.SetActive(true);
        }

        // Reset player object's rotation when clicking "0"
        if (Input.GetKeyDown(KeyCode.R))
        {
            playerObject.localRotation = Quaternion.identity;
        }
    }
}