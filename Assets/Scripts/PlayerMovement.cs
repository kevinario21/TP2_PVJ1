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

    [Header("Raycast Ground Settings")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float rayDistance = 1.1f;
    [SerializeField] private LayerMask groundLayer;

    private GameManager gm; // Referencia opcional por si la usas luego

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Evita que el personaje se caiga de lado al chocar
    }

    void Update()
    {
        // Si el personaje está detenido, no procesamos entradas de movimiento ni salto
        if (isStop)
        {
            moveHorizontal = 0f;
            moveVertical = 0f;
            return;
        }

        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        // Detección de suelo por Raycast (del primer código)
        CheckGround();

        // Lógica de Salto y Doble Salto unificada
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                // Primer salto
                ExecuteJump(jumpForce);
                jumpsPerformed = 1;
                transform.SetParent(null); // Desvincula de plataformas móviles al saltar
            }
            else if (jumpsPerformed < maxJumps)
            {
                // Segundo salto en el aire
                ExecuteJump(doubleJumpForce);
                jumpsPerformed++;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isStop) return;

        // Movimiento relativo a la rotación de la cámara (tomado del primer script pero mejorado)
        Vector3 movement = (transform.forward * moveVertical + transform.right * moveHorizontal).normalized;
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.z * speed);
    }

    private void ExecuteJump(float force)
    {
        // Resetea la velocidad vertical para un impulso limpio y consistente
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

    // --- Métodos de interacción con Plataformas (del segundo script) ---
    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contacto in collision.contacts)
        {
            if (contacto.normal.y > 0.5f)
            {
                if (collision.gameObject.CompareTag("Plataforma"))
                {
                    transform.SetParent(collision.transform);
                }
                break;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plataforma"))
        {
            transform.SetParent(null);
        }
    }

    // --- Métodos públicos para habilidades o control externo ---
    public void RotateCharacter(float rotateY)
    {
        // Método indispensable para que funcione con tu script CameraFollow
        transform.rotation = Quaternion.Euler(0f, rotateY, 0f);
    }

    public void EnableDoubleJump()
    {
        maxJumps = 2;
    }

    public void DisableDoubleJump()
    {
        maxJumps = 1;
        if (jumpsPerformed > 1)
        {
            jumpsPerformed = 1;
        }
    }
}