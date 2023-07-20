using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRainbowForm : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject hitVFX;
    public float initialSpeed;
    public float launchAngle;

    private Vector3 initialVelocity;
    void Start()
    {
        OnInit();
        Launch();
    }

    
    private void Launch()
    {
      
        rb.velocity = initialVelocity;
        rb.AddForce(Vector3.forward, (ForceMode2D)ForceMode.Acceleration);
        Invoke(nameof(OnDespawn), 4f);
    }
    private void OnInit()
    {
        float launchAngleRad = launchAngle * Mathf.Deg2Rad;


        float initialVelocityX = initialSpeed * Mathf.Cos(launchAngleRad);
        float initialVelocityY = initialSpeed * Mathf.Sin(launchAngleRad);


        initialVelocity = new Vector3(initialVelocityX, initialVelocityY, 0f);
        
    }
    private void OnDespawn()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Character>().OnHit(30f);
            GameObject obj = Instantiate(hitVFX, transform.position, transform.rotation);
            DestroyObject(obj, 0.75f);
            /*Debug.Log("-30")*/
            ;
            OnDespawn();
        }
    }
}
