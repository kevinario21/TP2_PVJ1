using UnityEngine;

public class LetalGas : MonoBehaviour
{
    [Header("Configuración de Gas")]
    [SerializeField] private float damagePerSecond = 5f; // Cuánta vida quita por segundo

    private void OnTriggerStay(Collider other)
    {
        // Mientras el jugador permanezca dentro de la zona de gas
        if (other.CompareTag("Player"))
        {
            HealthSystem health = other.GetComponent<HealthSystem>();
            if (health != null)
            {
                // Restamos vida de forma continua basada en el tiempo transcurrido (Time.deltaTime)
                health.TakeDamage(damagePerSecond * Time.deltaTime);

            }
            else
            {
                Debug.LogWarning("El objeto con tag 'Player' entró al gas, pero le falta el componente HealthSystem.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("<color=orange>¡El jugador ha entrado en la zona de gas letal!</color>");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("<color=green>El jugador ha salido a salvo de la zona de gas.</color>");
        }
    }
}
