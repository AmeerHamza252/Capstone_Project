using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Entity
{
    //Variables

    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    [SerializeField] private float jumpHeight;

    //References

    private CharacterController controller;
    private Animator anim;

	private void Start()
	{
        // Player Stats
        InstantiateEntity(100, 10, 2);

        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        
    }

    private void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            StartCoroutine(Attack());
        }
    }

	private void Move()
	{
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        // make sure if player is fully grounded

        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -2.0f;
        }


        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(0.0f,0.0f, moveZ);

        //Setting player forward direction

        moveDirection = transform.TransformDirection(moveDirection);

        // checking if player is grounded

        if (isGrounded) 
        {
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.RightShift))
            {
                Walk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.RightShift))
            {
                Run();
            }

            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }
            moveDirection *= moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                Jump();
            }
        }
        
        controller.Move(moveDirection * Time.deltaTime);


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Idle() 
    {
        anim.SetFloat("PlayerSpeed", 0.0f, 0.1f, Time.deltaTime);
    }

    private void Walk() 
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("PlayerSpeed", 0.5f, 0.1f, Time.deltaTime);
    }

    private void Run() 
    {
        moveSpeed = runSpeed;
        anim.SetFloat("PlayerSpeed", 1.0f,0.1f, Time.deltaTime);
    }

    private void Jump() 
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        float check = velocity.y;
        Debug.Log(check);
    }

    private IEnumerator Attack() 
    {
        // Setting player weight in attack layer so that it can attack smoothly
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 1 );
        anim.SetTrigger("Attack");
        Attack(20);

        yield return new WaitForSeconds(0.9f);

        //disabling the attack layer
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 0);
    }
}