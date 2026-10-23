using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadCaminar = 5f;


    [Header("Configuración de Salto")]
    public float fuerzaSalto = 10f;
    public float fuerzaDoble = 5f;

    private int maximosSaltos = 1;
    private int saltosRealizados = 0;

    private Rigidbody rb;
    private bool enSuelo = true;
    private float horizontal;
    private float vertical;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // SALTO Y DOBLE SALTO
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (enSuelo)
            {
                // Primer salto desde el suelo
                EjecutarSalto(fuerzaSalto);
                enSuelo = false;
                saltosRealizados = 1;
                transform.SetParent(null);
            }
            else if (saltosRealizados < maximosSaltos)
            {
                // Segundo salto en el aire (usando la fuerza de doble salto)
                EjecutarSalto(fuerzaDoble);
                saltosRealizados++;
            }
        }
    }

    void EjecutarSalto(float fuerza)
    {
        // Reseteamos la velocidad vertical para un impulso limpio y consistente
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * fuerza, ForceMode.Impulse);
    }

    void FixedUpdate()
    {

        Vector3 velocidadMovimiento = new Vector3(-horizontal * velocidadCaminar, rb.linearVelocity.y, -vertical * velocidadCaminar);
        rb.linearVelocity = velocidadMovimiento;
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contacto in collision.contacts)
        {
            if (contacto.normal.y > 0.5f)
            {
                enSuelo = true;

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

        enSuelo = false;
    }
    public void HabilitarDobleSalto()
    {
        maximosSaltos = 2;
    }

    public void DeshabilitarDobleSalto()
    {
        maximosSaltos = 1;
        if (saltosRealizados > 1)
        {
            saltosRealizados = 1;
        }
    }
}