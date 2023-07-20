using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected CombatText combatTextPrefab;
    [SerializeField] protected CombatText combatTextRecoverPrefab;
    private float timeRecoverCount;
    private float hpRecover = 20;
    private float timeRecover = 3f;


    private float hp;
    public bool IsDead => hp <= 0;
    public float TimeRecoverCount { get => timeRecoverCount; set => timeRecoverCount = value; }
    public float TimeRecover { get => timeRecover; set => timeRecover = value; }
    public float HpRecover { get => hpRecover; set => hpRecover = value; }
    // Start is called before the first frame update
    private string currentAnimName;
    public HealthBar HealthBar { get => healthBar; set => healthBar = value; }
    private void Start()
    {
        OnInit();
    }
    public virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(100, transform);
    }

    public virtual void OnDespawn()
    {

    }
    protected virtual void OnDeath()
    {
        ChangeAnim("die");
        Invoke(nameof(OnDespawn),2f);
    }
    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);

            currentAnimName = animName;

            anim.SetTrigger(currentAnimName);
        }
    }
    public void OnHit(float damage)
    {
        //GameObject obj = Instantiate(blood, transform.position, transform.rotation);
        //DestroyObject(obj, 0.75f);
        
        if (!IsDead)
        {
            hp -= damage;
            if (IsDead)
            {
                hp = 0;
                OnDeath();
            }
            healthBar.SetNewHP(hp);
            Instantiate(combatTextPrefab,transform.position + Vector3.up,Quaternion.identity).OnInit(damage);
        }
    }
   public void Recover(float hpRecover)
    {
        if (this.healthBar.IsFull())
        {
            
            return;
        }
        this.hp += hpRecover;
        this.healthBar.SetNewHP(this.hp);
        Instantiate(combatTextRecoverPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(hpRecover);
    }






}
