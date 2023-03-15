using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PS4Controller : MonoBehaviour
{
    public float jumpSpeed;


    GameObject player = null;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
       for (int i = 0; i < Gamepad.all.Count; i++)
       {
            Debug.Log(Gamepad.all[i].name);
       } 

       player = GameObject.Find("Player");
       rb =  player.GetComponent<Rigidbody2D>();

        if (player != null)
            Debug.Log("Found player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all.Count > 0)
        {
            if (Gamepad.all[0].leftStick.left.isPressed)
            {
                player.transform.position += Vector3.left * Time.deltaTime * 5f;
            }

            if (Gamepad.all[0].leftStick.right.isPressed)
            {
                player.transform.position += Vector3.right * Time.deltaTime * 5f;
            }

            if (Gamepad.all[0].buttonSouth.wasPressedThisFrame)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            }
            if (Gamepad.all[0].buttonWest.wasPressedThisFrame) {
                player.GetComponent<Character>().StartAttack(1);
            }
        }  
    }
}
