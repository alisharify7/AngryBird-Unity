using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AngieBird : MonoBehaviour
{

    private Rigidbody2D _rb;
    private CircleCollider2D _circleCollider;

    private bool _hasBeenLunched;
    private bool _shouldFaceVelocityDirection = false;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    public void Start()
    {
        _rb.isKinematic = true;
        _circleCollider.enabled = false;
    }
    public void LunchBird(Vector2 direction, float force)
    {
        _hasBeenLunched = true;
        _rb.isKinematic = false;
        _circleCollider.enabled = true;
        _rb.AddForce(direction * force, ForceMode2D.Impulse);
        //_shouldFaceVelocityDirection = true;
    }

    private void FixedUpdate()
    {
        // 50 times per second
        //if (_hasBeenLunched && _shouldFaceVelocityDirection)
        //{
        //    this.transform.right = _rb.velocity;
        //}

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _shouldFaceVelocityDirection = false;
    }


}
