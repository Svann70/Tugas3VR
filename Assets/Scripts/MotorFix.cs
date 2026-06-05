using UnityEngine;
using UnityEngine.InputSystem; // Pastikan package 'Input System' terinstall

[RequireComponent(typeof(Rigidbody))]
public class KendaliScooter : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 100f;
    public float groundCheckDistance = 0.25f;
    public LayerMask groundLayers = ~0;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Setting Fisika Dasar
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        // Input
        float move = 0;
        if (kb.wKey.isPressed) move = 1;
        else if (kb.sKey.isPressed) move = -1;

        float turn = 0;
        if (kb.aKey.isPressed) turn = -1;
        else if (kb.dKey.isPressed) turn = 1;

        // Gerak (Gunakan Velocity agar tidak melayang)
        Vector3 vel = transform.forward * move * speed;
        vel.y = rb.linearVelocity.y; // Membiarkan gravitasi bekerja pada sumbu Y
        rb.linearVelocity = vel;

        // Rotasi
        if (Mathf.Abs(move) > 0.1f)
        {
            float rot = turn * turnSpeed * Time.fixedDeltaTime;
            if (move < 0) rot = -rot;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rot, 0));
        }
    }
}