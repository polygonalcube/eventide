using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerLogic : MonoBehaviour
{
    // The player class.

    // The input & movement of the player GameObject.
    
    [SerializeField] InputAction movement;
    public Vector2 movValue;

    [SerializeField] InputAction shining;
    public float shineValue;

    [SerializeField] MoveComponent mover;

    [SerializeField] Rigidbody2D rb;

    [SerializeField] Transform flashlight;
    [SerializeField] GameObject lightCone;

    // Values for flashlight SmoothDampAngle().
    float refVelocity = 0f;
    [SerializeField] float smoothTime = .3f; // The time it will take for the camera to settle at it's final position, in seconds.
    [SerializeField] float maxSpeed = Mathf.Infinity;

    public int sanity = 180;
    [SerializeField] int maxSanity = 180;
    public bool debugSiren = false;

    void OnEnable()
    {
        movement.Enable();
        shining.Enable();
    }

    void OnDisable()
    {
        movement.Disable();
        shining.Disable();
    }
    
    void Update()
    {
        ReceiveInput();
        Flashlight();
    }

    void FixedUpdate()
    {
        Movement();
        Sanity();
    }

    void ReceiveInput()
    {
        movValue = movement.ReadValue<Vector2>(); // Storing the value of movement input.
        shineValue = shining.ReadValue<float>(); // Storing the value of flashlight input.
    }

    void Movement()
    {
        rb.velocity = mover.Move(movValue, 1f);
    }

    void Flashlight()
    {
        // If the player is currently moving, rotate the flashlight in accordance with player movement direction.
        if (movValue != new Vector2(0f, 0f)) flashlight.eulerAngles = new Vector3(0f, 0f, 
        Mathf.SmoothDampAngle(flashlight.eulerAngles.z, (Mathf.Atan2(movValue.y, movValue.x) * Mathf.Rad2Deg), ref refVelocity, smoothTime, maxSpeed));
        lightCone.SetActive(shineValue == 1f); // If there is flashlight input, turn on the flashlight. Else, it will be off.
    }

    void Sanity()
    {
        if (debugSiren && shineValue == 1f) sanity--;
        if (!debugSiren) sanity = maxSanity;
        if (sanity <= 0) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        LayerMask camLockZoneLayer = LayerMask.NameToLayer("Camera Lock Zones");
        if (col.gameObject.layer == camLockZoneLayer)
        {
            Vector2 colPos = (Vector2)col.gameObject.transform.position;
            string[] axes = {colPos.x.ToString(), colPos.y.ToString()};
            MessengerBus.messenger.Broadcast("LockZoneEntered", new List<string>(axes));
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        LayerMask camLockZoneLayer = LayerMask.NameToLayer("Camera Lock Zones");
        if (col.gameObject.layer == camLockZoneLayer) MessengerBus.messenger.Broadcast("LockZoneExited");
    }
}
