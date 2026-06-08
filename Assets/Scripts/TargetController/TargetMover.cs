using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class TargetMover : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float rotationSpeed = 100f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float moveHorizontal = 0f;
        float moveVertical = 0f;
        float rotation = 0f;

        if (Keyboard.current != null)
        {

            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveVertical = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveVertical = -1f;

            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveHorizontal = 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveHorizontal = -1f;


            if (Keyboard.current.qKey.isPressed) rotation = -1f;
            if (Keyboard.current.eKey.isPressed) rotation = 1f;
        }


        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
        rb.linearVelocity = movement * speed;
        rb.angularVelocity = new Vector3(0f, rotation * rotationSpeed * Mathf.Deg2Rad, 0f);
    }
}