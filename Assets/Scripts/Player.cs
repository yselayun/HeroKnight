using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float _speed = 500f, _maxSpeed = 90, _powJump = 5000f;
    public bool _falled = true, _grounded = false, _faceRight = true, _doubleJump = false;
    public Animator _anim;

    public Rigidbody2D rigidbody2D;
    void Start()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _anim = gameObject.GetComponent<Animator>();
      
    }

    // Update is called once per frame
    void Update()
    {
        _anim.SetBool("Falled", _falled);
        _anim.SetBool("Grounded", _grounded);
        _anim.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (_grounded                                                                                                                                                                   )
            {
                _falled = false;
                _grounded = false;
                rigidbody2D.AddForce(Vector2.up * _powJump);
                _falled = true;
            }
        }

    }
    void FixedUpdate()
    {
        float _move = Input.GetAxis("Horizontal");
        rigidbody2D.AddForce((Vector2.right) * _speed * _move);

        if (rigidbody2D.velocity.x > _maxSpeed)
        {
            rigidbody2D.velocity = new Vector2(_maxSpeed, rigidbody2D.velocity.y);
        }
        if (rigidbody2D.velocity.x < -_maxSpeed)
        {
            rigidbody2D.velocity = new Vector2(-_maxSpeed, rigidbody2D.velocity.y);
        }

        if (_move > 0 && !_faceRight)
        {
            Flip();
        }
        if (_move < 0 && _faceRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        _faceRight = !_faceRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }
}
