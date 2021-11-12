using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody projectileRB;
    public GameObject player;
    public GameObject enemy;
    public float speed;
    public float lifeTime;
    public int damage;
    public bool damagePlayer;

    // Start is called before the first frame update
    void Start()
    {
        //vars inti , initial position ( enemy ) end position ( player ) 
        projectileRB = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");

    }

    // Update is called once per frame
    private void Update()
    {
        // on hit effect ( player ) gets damaged.
        // if enemy can shoot, projectile will move towards player, if hits reduce hp/timer by 2
            //projectileRB.velocity = transform.forward * speed;
            lifeTime -= Time.deltaTime;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

}   
    
