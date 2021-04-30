using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementVS : MonoBehaviour
{
    public static PlayerMovementVS Instance;

    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private float speed, stepTime, groundCheckHeight, jumpForce;

    [SerializeField] private GameObject stepEffect;

    [SerializeField] private Animator animator;

    private Rigidbody rigidbody;

    private InputManager inputManager;

    public bool canWalkDepth;

    private Collider collider;

    [Header("Jump Parameters")]

    private float tParam;

    [SerializeField] private float jumpSpeed;

    [SerializeField] private Vector2 bezierOffsetPlayer, bezierOffsetPlatform;


    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
        inputManager.enabled = true;
        //inputManager.JumpPerformed.AddListener(Jump);
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
        else if (translationX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (translationY < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (translationY > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        transform.position += new Vector3(translationX, 0, translationY) * speed * Time.deltaTime;
    }

    public void Jump(Vector2 landPosition)
    {
        StartCoroutine(StartJump(landPosition));
    }

    private IEnumerator StartJump(Vector2 landPosition)
    {
        if(IsGrounded(0f))
        {
            tParam = 0;
            animator.SetTrigger("Jump");
            yield return new WaitForSeconds(0.25f);

            //Use Bezier curve to jump to position.
            Vector2 p0 = transform.localPosition;
            Vector2 p1 = p0 + bezierOffsetPlayer;
            Vector2 p3 = landPosition;
            Vector2 p2 = p3 + bezierOffsetPlatform;

            Vector2 newPosition;

            while(tParam < 1)
            {
                tParam += Time.deltaTime * jumpSpeed;

                newPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                    3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                    3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                    Mathf.Pow(tParam, 3) * p3;

                transform.localPosition = newPosition;

                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(0.25f);
            StartCoroutine(CheckGrounded());
        }
    }


    private bool IsGrounded(float extraHeight)
    {
        bool hit = Physics.Raycast(collider.bounds.center, Vector2.down, collider.bounds.extents.y + groundCheckHeight + extraHeight, groundLayerMask);
        return hit;
    }

    private IEnumerator CheckGrounded()
    {
        if(IsGrounded(1.4f))
        {
            animator.SetTrigger("Fall");
            yield break;
        }
        yield return new WaitForEndOfFrame();
        StartCoroutine(CheckGrounded());
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

    public void LandEffect()
    {
        GameObject stepEffectObject = Instantiate(stepEffect, transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
        stepEffectObject.transform.localScale = new Vector3(1,1,1);
    }
}
