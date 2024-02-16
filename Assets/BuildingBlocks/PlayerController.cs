using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] protected float _movementSpeed = 20;
    [SerializeField] protected float _jumpForce = 20;
    [SerializeField] protected float _fallMultiplier = 2f;

    private Rigidbody2D _rb;
    private Animator _animator;

    private bool _isGrounded = false;
    private bool _shouldJump = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (WasJumpPressed() && _isGrounded)
            _shouldJump = true;
    }

    private void FixedUpdate()
    {
        if (_shouldJump)
        {
            _rb.AddForce(Vector2.up * _jumpForce);
            OnPlayerGroundExit();
            _shouldJump = false;
        }

        Vector2 velocity = new Vector2(Input.GetAxisRaw("Horizontal") * _movementSpeed, _rb.velocity.y);
        _rb.velocity = new Vector2(velocity.x * Time.fixedDeltaTime, velocity.y);

        if (_rb.velocity.y < 0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity * (_fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    public void OnPlayerGroundEnter()
    {
        _isGrounded = true;
        _animator.SetBool("IsGrounded", true);
    }

    public void OnPlayerGroundExit()
    {
        _isGrounded = false;
        _animator.SetBool("IsGrounded", false);
    }

    private bool WasJumpPressed()
    {
        return (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W));
    }

    /// <summary>
    /// There is a bug when using OnCollisionEnter. When player very quickly moves to one platform and
    /// out it dosent change color back. Using OnCollisionStay fix this bug eventhough its more costy.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        IInteractable interactable = collision.gameObject.GetComponentInParent<IInteractable>();
        if (interactable != null)
        {
            interactable.Interact(this);
        }
    }
}
