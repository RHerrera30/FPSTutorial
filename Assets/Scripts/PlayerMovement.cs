using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;
    
    public Transform groundCheck;
    public Transform cam;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    
    Vector3 velocity;

    bool isGrounded;
    bool isMoving;
    
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //Reset the default velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        //Calculating the movement
        Vector3 move = cam.transform.right * x + cam.transform.forward * z; //(right/left - red axis, forward/backward - blue axis)
        
        //SHMOOVIN'!
        controller.Move(move * (speed * Time.deltaTime));
        
        //Check if player can jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //Going up
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        //Falling down
        velocity.y += gravity * Time.deltaTime;
        
        //JUMPIN'!
        controller.Move(velocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded)
        {
            isMoving = true;
            // for later use
        }
        else
        {
            isMoving = false;
            //for later use
        }

        lastPosition = gameObject.transform.position;
    }
}
