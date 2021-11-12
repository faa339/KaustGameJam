using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float shootDelay;
    private float delayMax;
    public bool canShoot;
    public Vector3 rotationDir; //TODO: consider better name? 
    public Rigidbody enemyRB;
    public AudioClip enemySound;
    public GameObject bullet;
    public GameObject player;
    public Transform fireD; //where the projectile will come out
    // Start is called before the first frame update
    void Start()
    {
        //Initialization code 
        speed = 5.0f;
        delayMax = Random.Range(2, 3.5f);
        shootDelay = delayMax;
        canShoot = true;
        rotationDir = new Vector3(0, 0, 0);
        enemyRB = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        rotationDir = new Vector3(0, 0, 0);
        /*
        enemySound = initialize when ready
        bullet = will be set from within the editor 
        */
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Rotate towards player, then shoot
        Vector3 pos = player.transform.position + new Vector3(0, 1, 0);
        transform.LookAt(pos, Vector3.up);
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        if (Vector3.Distance(transform.position, player.transform.position) < 5.0f)
        {
            Shoot();
        }
        else {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player") {
            //Reduce player time 
            //Replicate another enemy nearby 
            Vector3 newPos = transform.position + 
                new Vector3(Random.Range(-4, 4), 0, Random.Range(-4, 4));
            Enemy newEnemy = Instantiate(this, newPos, transform.rotation);
            //Prevent instant shooting
            newEnemy.canShoot = false;
            newEnemy.shootDelay = 1.99f; 
            //And dont forget to reduce player time left < time
        }
        //Bullet collision with enemy will do nothing, safety feature lol
    }

    private void Shoot() {
        //Shoot a projectile in the rotationDir direction
        //if within shooting range (some fixed constant) 
        if (canShoot)
        {
            //instantiate a projectile 
            Instantiate(bullet, fireD.position, fireD.rotation);
            Debug.Log("Shooting!");
            canShoot = false;
        }
        else {
            //Bit hacky, but I didnt want to add a bool lol 
            shootDelay -= Time.deltaTime;
            if (shootDelay <= 0) {
                shootDelay = delayMax;
                canShoot = true;
            }
        }
    }
}
