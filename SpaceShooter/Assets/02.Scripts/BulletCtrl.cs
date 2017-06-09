using UnityEngine;
using System.Collections;

public class BulletCtrl : MonoBehaviour {
    //총알의 파괴력
    public int damage = 20;
    //총알 발사 속도
    public float speed  = 1000.0f;
    
    public GameObject bullethole;
    
    void Start () {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }
    
    void OnCollisionEnter( Collision coll )
    {
//        ContactPoint contact = coll.contacts[0];
//        GameObject hole = (GameObject)Instantiate(bullethole);
//        hole.transform.position = contact.point;
//        hole.transform.rotation = Quaternion.LookRotation (contact.normal);
    }
     
}
