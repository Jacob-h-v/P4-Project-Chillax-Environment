using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualInput : MonoBehaviour
{
    private float moveAD;
    private float moveWS;
    //private float gravity = -20f;
    //private float jumpHeight = 1f;
    public float speed = 1f;
    //private Vector3 velocity;
    //public CharacterController charC;
    public Transform playerObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        moveAD = Input.GetAxis("Horizontal");
        moveWS = Input.GetAxis("Vertical");

        //Vector3 movePlayer = transform.right * moveAD + transform.forward * moveWS;

        //charC.Move(movePlayer * speed * Time.deltaTime);

        //{
         //   velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        //}

        //velocity.y += gravity * Time.deltaTime;
        //charC.Move(velocity * Time.deltaTime);

        playerObject.Rotate(Vector3.up * speed * moveAD);
        playerObject.Rotate(Vector3.right * speed * moveWS);
    }
}