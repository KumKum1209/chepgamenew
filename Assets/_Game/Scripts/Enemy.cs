using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float attackrange;
    [SerializeField] private float movespeed;
    [SerializeField] private GameObject Attackarea;
    [SerializeField] Rigidbody2D rb;

    private IState currenState;

    private bool IsRight = true;
    private Character target;
    public Character Target => target;
    private void Update()
    {
        if (currenState != null && !IsDead)
        {
            currenState.OnExecute(this);
        }
    }
    public override void OnInit()
    {
        base.OnInit();
        
        ChangeState(new IdleState());
        DeActiveAttack();
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(HealthBar.gameObject);
        Destroy(gameObject);
    }
    protected override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
    }

    public void ChangeState(IState newstate)
    {
        if (currenState != null)
        {
            currenState.OnExit(this);
        }
        currenState = newstate;
        if (currenState != null)
        {
            currenState.OnEnter(this);
        }

    }
    internal void SetTarget(Character character)
    {
        this.target = character;
        if (IsTargetInRange())
        {
            ChangeState(new AttackState());
        }
        else
            if(Target!= null) 
            {
                ChangeState(new PatrolState());
            }
            else
            {
                ChangeState(new IdleState());
            }
    }
    public void Moving()
    {
        ChangeAnim("run");
        rb.velocity = transform.right * movespeed;
    }
    public void StopMoving()
    {
        ChangeAnim("idle");
        rb.velocity = Vector2.zero;
    }
    public void Attack()
    {
        ChangeAnim("attack");
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }
    public bool IsTargetInRange()
    {
        if (Target != null && Vector2.Distance(target.transform.position, transform.position) <= attackrange)
        {
            return true;
        }
        else 
            return false;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemywall")
        {
            ChangeDirection(!IsRight);
        }
    }
    public void ChangeDirection(bool isright)
    {
        this.IsRight = isright;
        transform.rotation = IsRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }
    private void ActiveAttack()
    {
        Attackarea.SetActive(true);
    }
    private void DeActiveAttack()
    {
        Attackarea.SetActive(false);
    }


}   
