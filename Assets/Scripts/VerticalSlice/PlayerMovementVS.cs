using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementVS : MonoBehaviour
{
    public static PlayerMovementVS Instance;

    private BalanceMinigame balanceMinigame;

    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private float speed, stepTime, groundCheckHeight, jumpForce;

    [SerializeField] private GameObject stepEffect;

    [HideInInspector] public Animator animator;

    private Rigidbody rigidbody;

    private InputManager inputManager;

    public bool canWalkDepth;

    public bool canWalk;

    [SerializeField] private Collider collider;

    [Header("Jump Parameters")]

    private float tParam;

    [SerializeField] private float jumpSpeed;

    [SerializeField] private Vector2 bezierOffsetPlayer, bezierOffsetPlatform;

    public bool canJump;

    private float translationX;

    private bool walkingToEustachius;

    private bool jumping;

    protected virtual void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        //collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        canWalk = true;
    }

    protected virtual void Start()
    {
        if(BalanceMinigame.Instance != null)
        {
            balanceMinigame = BalanceMinigame.Instance;
        }
        inputManager = InputManager.Instance;
        inputManager.enabled = true;
        //inputManager.JumpPerformed.AddListener(Jump);
        canWalkDepth = false;
    }

    private void Update()
    {
        if (canWalk)
        {
            Movement();
        }
    }

    private void Movement()
    {
        if (!walkingToEustachius)
        {
            //translationX = inputManager.GetMovement().x;
            translationX = 0;
        }
        //float translationY = inputManager.GetMovement().y;
        float translationY = 0;

        if (!canWalkDepth)
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

        if (translationX < 0)
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

    protected virtual IEnumerator StartJump(Vector2 landPosition)
    {
        if (jumping)
        {
            yield break;
        }
        jumping = true;
        tParam = 0;
        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(0.25f);

        //Use Bezier curve to jump to position.
        Vector2 p0 = transform.localPosition;
        Vector2 p1 = p0 + bezierOffsetPlayer;
        Vector2 p3 = landPosition;
        Vector2 p2 = p3 + bezierOffsetPlatform;

        Vector2 newPosition;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * jumpSpeed;

            newPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            transform.localPosition = newPosition;

            //Make sure no falling happens when jumping
            animator.SetBool("FallDown", false);
            
            yield return new WaitForEndOfFrame();
        }

        jumping = false;

        StartCoroutine(CheckGrounded());
    }


    private bool IsGrounded(float extraHeight)
    {
        bool hit = Physics.Raycast(collider.bounds.center, Vector2.down, collider.bounds.extents.y + groundCheckHeight + extraHeight, groundLayerMask);
        return hit;
    }

    private IEnumerator CheckGrounded()
    {
        if (IsGrounded(1.4f))
        {
            animator.SetTrigger("Fall");
            animator.SetBool("Balance", true);

            yield break;
        }
        yield return new WaitForEndOfFrame();
        StartCoroutine(CheckGrounded());
    }

    public void StepEffect()
    {
        if (transform.rotation.eulerAngles.y == 90)
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
        stepEffectObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void EnableInput()
    {
        //inputManager.EnableInput();
    }

    public IEnumerator WalkRight()
    {
        walkingToEustachius = true;
        float distanceTraveled = 0;
        if(BalanceMinigame.Instance.currentPlatform > 0)
        {
            distanceTraveled = 2;
        }
        Vector2 startPosition = transform.localPosition;
        while (distanceTraveled < 3.3f)
        {
            canWalk = true;
            distanceTraveled = Vector2.Distance(startPosition, transform.localPosition);
            translationX = 1;
            if(gameObject.name == "Eustachius")
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
                translationX = 0.6f;
            }
            yield return new WaitForEndOfFrame();
        }
        canWalk = false;
        walkingToEustachius = false;

        //Jump to next platform
        BalanceMinigame balanceMinigame = BalanceMinigame.Instance;

        if(gameObject.name != "Eustachius")
        {
            Jump((Vector2)balanceMinigame.balancePlatforms[balanceMinigame.currentPlatform].position + balanceMinigame.balancePlatformLandOffset);
            BalanceMinigame.Instance.currentPlatform++;
        }
    }

    public void DisableCollider()
    {
        collider.enabled = false;
    }

    public void StartBalanceAnimation()
    {
        animator.SetBool("Balance", true);
    }

    public void StopBalanceAnimation()
    {
        animator.SetBool("Balance", false);
    }

    //public IEnumerator MoveToDoor()
    //{
    //    Vector2 doorPosition = DoorMinigame.Instance.DoorPushSpot.position;
    //    while(Vector2.Distance(transform.position, doorPosition) > 0.5f)
    //    {
    //        walkingToEustachius = true;
    //        inputManager.DisableInput();
    //        translationX = 1;
    //        yield return new WaitForEndOfFrame();
    //    }
    //    walkingToEustachius = false;
    //    canWalk = false;
    //    animator.SetBool("Walking", false);
    //    animator.SetBool("Pushing", true);
    //    inputManager.EnableInput();

    //    DoorMinigame.Instance.gameObject.SetActive(true);
    //    DoorMinigame.Instance.StartMinigame();
    //}
}
