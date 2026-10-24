using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField] private bool isStop = false;
    public bool IsStop
    {
        get { return isStop; }
        set { isStop = value; }
    }

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float doubleJumpForce = 5f;
    [SerializeField] private int maxJumps = 1; // 1 para salto normal, 2 para doble salto
    private int jumpsPerformed = 0;

    private float moveHorizontal;
    private float moveVertical;
    private Rigidbody rb;

    [Header("Ground & Platform Settings")]
    [SerializeField] private bool isGrounded;          // Detectado por Raycast (para el terreno general)
    [SerializeField] private bool isOnPlatform;        // Detectado por Colisión (para la plataforma móvil)
    [SerializeField] private float rayDistance = 1.5f;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        if (isStop)
        {
            moveHorizontal = 0f;
            moveVertical = 0f;
            return;
        }

        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        // 1. Comprobamos el suelo general con Raycast
        CheckGround();

        // Propiedad unificada: Estamos en el suelo si el Raycast toca el terreno O si estamos sobre la plataforma
        bool canJump = isGrounded || isOnPlatform;

        // 2. Lógica de Salto
        if (Input.GetButtonDown("Jump"))
        {
            if (canJump)
            {
                // Primer salto
                ExecuteJump(jumpForce);
                jumpsPerformed = 1;

                // Si salta desde la plataforma, nos desvinculamos de ella
                transform.SetParent(null);
                isOnPlatform = false;
            }
            else if (jumpsPerformed < maxJumps)
            {
                // Segundo salto en el aire (Doble salto)
                ExecuteJump(doubleJumpForce);
                jumpsPerformed++;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isStop) return;

        // Movimiento relativo a la rotación de la cámara
        Vector3 movement = (transform.forward * moveVertical + transform.right * moveHorizontal).normalized;
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.z * speed);
    }

    private void ExecuteJump(float force)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    private void CheckGround()
    {
        Vector3 origin = transform.position;
        isGrounded = Physics.Raycast(origin, Vector3.down, rayDistance, groundLayer);

        Color colorRay = isGrounded ? Color.green : Color.red;
        Debug.DrawRay(origin, Vector3.down * rayDistance, colorRay);
    }

    // --- DETECCIÓN DE LA PLATAFORMA MÓVIL (COMO ASCENSOR) ---
    private void OnCollisionStay(Collision collision)
    {
        // Si el objeto con el que colisionamos tiene la etiqueta "Plataforma"
        if (collision.gameObject.CompareTag("Plataforma"))
        {
            foreach (ContactPoint contacto in collision.contacts)
            {
                // Verificamos que lo estemos pisando desde arriba
                if (contacto.normal.y > 0.5f)
                {
                    isOnPlatform = true;
                    transform.SetParent(collision.transform); // Nos hacemos hijos para movernos con ella
                    break;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Cuando salimos de la plataforma móvil
        if (collision.gameObject.CompareTag("Plataforma"))
        {
            transform.SetParent(null); // Nos desvinculamos
            isOnPlatform = false;      // Ya no estamos en la plataforma
        }
    }

    // Métodos públicos de control
    public void RotateCharacter(float rotateY)
    {
        transform.rotation = Quaternion.Euler(0f, rotateY, 0f);
    }

    public void EnableDoubleJump() => maxJumps = 2;
    public void DisableDoubleJump()
    {
        maxJumps = 1;
        if (jumpsPerformed > 1) jumpsPerformed = 1;
    }
}