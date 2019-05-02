using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public Transform attachedCamera;
    public bool isCursorHidden = true;
    public float minPitch = -80f, maxPitch = 80f;
    public Vector2 speed = new Vector2(120f, 120f);

    private Vector2 euler; // Current rotation of the camera

    // Start is called before the first frame update
    void Start()
    {
        // Is the cursor supposed to be hidden?
        if (isCursorHidden)
        {
            // Lock and Hide it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; // ... Invisible!
        }
        // Get current camera euler
        euler = transform.eulerAngles;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Rotate the euler with Mouse movement
        euler.y += Input.GetAxis("Mouse X") * speed.x * Time.deltaTime;
        euler.x -= Input.GetAxis("Mouse Y") * speed.y * Time.deltaTime;

        // Clamp the camera on pich
        euler.x = Mathf.Clamp(euler.x, minPitch, maxPitch);

        // Rotate the Player and Transform seperately
        transform.localEulerAngles = new Vector3(0f, euler.y, 0f);
        attachedCamera.localEulerAngles = new Vector3(euler.x, 0f, 0f);
    }
}
