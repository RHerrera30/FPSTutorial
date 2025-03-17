using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 500f;
    
    float xRotation = 0f;
    float yRotation = 0f;
    
    public float topClamp = -90f;
    public float bottomClamp = 90f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Getting the mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Rotation around the x-axis (look up and down)
        xRotation -= mouseY;
        
        //Clamp the rotation so you don't look at your feet
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);
        
        //Rotation around the y-axis (look left and right)
        yRotation += mouseX;
        
        //Apply rotations to our transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
