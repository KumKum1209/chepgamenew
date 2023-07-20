using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public GameObject hitVFX;
    public Rigidbody2D rb;
    private float speed = 5f;
    
    
    void Start()
    {      
        OnInit();
    }

    // Update is called once per frame
    private void OnInit()
    {
        rb.velocity = transform.right * speed;
        Invoke(nameof(OnDespawn), 4f);
    }
    private void OnDespawn()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<Character>().OnHit(30f);
            GameObject obj = Instantiate(hitVFX,transform.position,transform.rotation);
            DestroyObject(obj, 0.75f);
            /*Debug.Log("-30")*/
            ;
            OnDespawn();
        }
    }
    
}
