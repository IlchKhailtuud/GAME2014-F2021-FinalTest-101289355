////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: ShrinkingPlatformController.cs
//Author: Yiliqi
//Student Number: 101289355
//Last Modified On : 12/18/2021
//Description : Class for floating & shrinking platform
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingPlatformController : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public bool isActive;
    public float platformTimer;
    public float threshold;
    private BoxCollider2D col;

    public PlayerBehaviour player;
    private Vector3 distance;
    
    //time counter for playing colliding with the platform
    private float collisionTime;
    
    //check if collisionTime has reached certain value
    private float checkTime;
    
    //check if platform start to shrink
    public bool isShrink;

    //original scale of the platform
    private Vector3 objectScale;
    //original scale of the collider
    private Vector2 colScale;
    //the platform scale shrinks by this factor
    float shrinkFactor = 0.75f;

    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>();

        platformTimer = 0.1f;
        platformTimer = 0;
        isActive = true;
        distance = end.position - start.position;

        audio = GetComponent<AudioSource>();
        col = GetComponent<BoxCollider2D>();
        objectScale = transform.localScale;
        colScale = col.size;

        isShrink = false;
        checkTime = 0.5f;
        collisionTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            platformTimer += Time.deltaTime;
            _Move();
        }
        else
        {
            if (Vector3.Distance(player.transform.position, start.position) <
                Vector3.Distance(player.transform.position, end.position))
            {
                if (!(Vector3.Distance(transform.position, start.position) < threshold))
                {
                    platformTimer += Time.deltaTime;
                    _Move();
                }
            }
            else
            {
                if(!(Vector3.Distance(transform.position, end.position) < threshold))
                {
                    platformTimer += Time.deltaTime;
                    _Move();
                }
            }
        }
        
        if (isShrink)
        {
            //starting counting time for collision with the platform
            collisionTime += Time.deltaTime;
            
            if (collisionTime >= checkTime)
            {
                Shrink();
                
                //clear collisionTime
                collisionTime = 0;
            }
        }
        else
        {
            //ResetPlatform();
        }
    }
    
    private void Shrink()
    {
        //if the size of the platform & collider is smaller than a certain size, then destroy the platform
        if (transform.localScale.x <= objectScale.x * 0.35 && transform.localScale.y <= objectScale.y * 0.35)
        {
            Destroy(gameObject);
        }
        
        //change platform & collider size by multiplying with a certain value
        transform.localScale = new Vector3(objectScale.x * shrinkFactor, objectScale.y * shrinkFactor, objectScale.z);
        col.size = new Vector2(colScale.x * shrinkFactor, colScale.y * shrinkFactor);
        
        //play soundFX
        audio.Play();
        
        //decrement shrinkfactor everytime this function is called
        shrinkFactor -= 0.2f;
    } 

    private void ResetPlatform()
    {
        transform.localScale = objectScale;
        col.size = colScale;
    }

    private void _Move()
    {
        var distanceX = (distance.x > 0) ? start.position.x + Mathf.PingPong(platformTimer, distance.x) : start.position.x;
        var distanceY = (distance.y > 0) ? start.position.y + Mathf.PingPong(platformTimer, distance.y) : start.position.y;

        transform.position = new Vector3(distanceX, distanceY, 0.0f);
    }

    public void Reset()
    {
        transform.position = start.position;
        platformTimer = 0;
    }
}
