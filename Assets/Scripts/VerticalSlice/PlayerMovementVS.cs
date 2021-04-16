using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementVS : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private float speed, stepTime, groundCheckHeight, jumpForce;

    [SerializeField] private GameObject stepEffect;

    [SerializeField] private Animator animator;

    private Rigidbody rigidbody;

    private InputManager inputManager;

    public bool canWalkDepth;

    private Collider collider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
        inputManager.JumpPerformed.AddListener(Jump);
        canWalkDepth = false;
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float translationX = inputManager.GetMovement().x;
        float translationY = inputManager.GetMovement().y;

        if(!canWalkDepth)
        {
            translationY = 0;
        }

        if (translationX != 0 || translationY != 0)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        if(translationX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else if (translationY < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (translationY > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        transform.position += new Vector3(translationX, 0, translationY) * speed * Time.deltaTime;
    }

    private void Jump()
    {
        if(IsGrounded())
        {
            Debug.Log(IsGrounded());
            animator.SetBool("Jumping", true);
            rigidbody.AddForce(new Vector3(0, jumpForce, 0));
        }
    }

    private bool IsGrounded()
    {
        bool hit = Physics.Raycast(collider.bounds.center, Vector2.down, collider.bounds.extents.y + groundCheckHeight, groundLayerMask);
        Debug.Log(hit);
        return hit;
    }

    public void StepEffect()
    {
        if(transform.rotation.eulerAngles.y == 90)
        {
            Instantiate(stepEffect, transform.position + new Vector3(0.4f, -0.03f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(stepEffect, transform.position + new Vector3(-0.4f, -0.03f, 0), Quaternion.identity);
        }
    }
}
