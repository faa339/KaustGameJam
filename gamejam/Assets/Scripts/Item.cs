using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public Rigidbody itemRb;
    public GameObject player;
    public ParticleSystem itemPs;
 

    public float pickUpRange;
    public bool equipped;

    public AudioClip itemS;


    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem itemPs = GetComponent<ParticleSystem>();
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        // check if player is in range of the item
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if(dist <= pickUpRange)
        {
            itemPs.Play();
            //Debug.Log("im here!");
            if (Input.GetKeyDown(KeyCode.R))
            {
                PickUp();
                Debug.Log("picked up");
                
            }

        }
        else
        {
            itemPs.Stop();
        }
    }

    void PickUp()
    {
        equipped = true;
        //player.GetComponent<PlayerMovement>().backPack++;
        

    }

}
