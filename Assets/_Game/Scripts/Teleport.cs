using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float timeWait;
    private float timeCount;
    private PlayerControler playerControler;
    private void Start()
    {
        timeCount = timeWait;
    }
    private void Update()
    {
        if (timeCount < 0)
        {
            playerControler.TeleTo(target.transform.position.x, target.transform.position.y);
            timeCount = timeWait;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            timeCount -= Time.deltaTime;
            playerControler = collision.GetComponent<PlayerControler>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            timeCount = timeWait;
            playerControler = null;
        }
    }
}
