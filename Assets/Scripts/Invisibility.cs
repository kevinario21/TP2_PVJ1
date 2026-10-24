using System.Collections;
using UnityEngine;

public class InvisibilityPowerUp : MonoBehaviour
{
    [Header("Configuración de Invisibilidad")]
    [SerializeField] private float invisibilityDuration = 20f;

    private void OnTriggerEnter(Collider other)
    {
        // Si el jugador toca el ítem
        if (other.CompareTag("Player"))
        {
            // Buscamos si el jugador tiene el componente para volverse invisible
            PlayerInvisibility playerInvis = other.GetComponent<PlayerInvisibility>();
            if (playerInvis == null)
            {
                playerInvis = other.gameObject.AddComponent<PlayerInvisibility>();
            }

            playerInvis.ActivateInvisibility(invisibilityDuration);

            // Destruimos el ítem del suelo
            Destroy(gameObject);
        }
    }
}
