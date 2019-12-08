using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject playerBullet;
    public GameObject BulletPosition;

    float speed = 5f;

    void Start() { }
    void Update()
    {


        if (Input.GetButtonDown("Jump"))
        {
            GameObject bullet = (GameObject)Instantiate(playerBullet);
            bullet.transform.position = BulletPosition.transform.position;


            // For Touch Code
            //if (Input.touchCount > 0)
            //{
            //    Touch touch = Input.GetTouch(0);
            //    Vector3 touchdirection = Camera.main.ScreenToWorldPoint(touch.position);
            //    touchdirection.z = 0f;
            //    transform.position = touchdirection;
            //}

        }
        Vector2 direction = transform.position;
        direction.y += Input.GetAxis("Vertical") * speed * Time.deltaTime;
        direction.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.position = direction;


    }
}
