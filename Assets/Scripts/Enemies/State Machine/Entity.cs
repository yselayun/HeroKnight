using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public FiniteStateMachine stateMachine;

    public int facingDirection { get; private set; }

    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    public GameObject aliveGO { get; private set; }

    private Vector2 velocityWorkSpace;

    public virtual void Start()
    {
        aliveGO = transform.Find("Alive").gameObject;
        rb = aliveGO.GetComponent<Rigidbody2D>();
        anim = aliveGO.GetComponent<Animator>();

        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }
    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity)
    {
        velocityWorkSpace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkSpace;
    }

}
