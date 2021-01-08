﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    public bool combatEnabled;
    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage;
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private LayerMask whatIsDamageAble;

    private bool gotInput, isAttacking, isFirstAttack, isSecondAttack = false;

    private float lastInputTime = Mathf.NegativeInfinity;

    private float[] attackDetails = new float[2];

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);

        
    }
    private void Update()
    {
        checkCombatInput();
        checkAttacks();

        //Debug.Log(Input.GetMouseButton(0));
        Debug.Log(combatEnabled);
    }
    private void checkCombatInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            if (combatEnabled)
            {
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
       
        
    }

    private void checkAttacks()
    {
        if (gotInput)
        {
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);
                
            }
        }

        if(Time.time >= lastInputTime + inputTimer)
        {
            gotInput = false;
        }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedOjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius,whatIsDamageAble);

        attackDetails[0] = attack1Damage;
        attackDetails[1] = transform.position.x;
        foreach (Collider2D collider in detectedOjects)
        {
            collider.transform.parent.SendMessage("Damage", attackDetails);

        }
    }

    private void FinishAttack1()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }
}
