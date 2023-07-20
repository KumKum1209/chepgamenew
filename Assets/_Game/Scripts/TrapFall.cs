using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFall : MonoBehaviour
{
    [SerializeField] private PlayerControler player;
    [SerializeField] private float damage = 30;
    [SerializeField] private LayerMask playerLayer;
    private void Update()
    {
        //if (transform.position.x < player.transform.position.x + 3 && transform.position.x > player.transform.position.x - 3 && transform.position.y > player.transform.position.y)
        if(CheckDrop())
        {
            Fall();
        }
    }
    private void Fall()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;
    }
    private bool CheckDrop()
    {

        //Debug.DrawLine(transform.position, transform.position + Vector3.down * 6.5f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 6.5f,playerLayer);
        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
        // return hit.collider != null
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            float dame = damage;
            player.OnHit(dame);
            Destroy(gameObject);
        }
        else if ((collision.tag == "ground"))
        {
            Destroy(gameObject);
        }
    }
}
