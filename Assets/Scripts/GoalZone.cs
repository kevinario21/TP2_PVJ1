using UnityEngine;

public class GoalZone : MonoBehaviour
{
    [SerializeField] private Transform goal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickItem pickItem = other.GetComponent<PickItem>();

            if (pickItem != null)
            {
                // Extrae el ítem de las manos del jugador
                GameObject item = pickItem.DropItem();

                if (item != null)
                {
                    // Coloca el ítem visualmente en la zona de meta
                    ItemInZone(item);

                    // ¡CONEXIÓN CLAVE! Notificamos al GameManager que se alcanzó la victoria
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.NotifyGoalZone();
                    }

                    Debug.Log("¡Completaste el nivel y entregaste el ítem!");
                }
                else
                {
                    Debug.Log("Llegaste a la meta, pero no llevas ningún ítem encima.");
                    // Opcional: Aquí podrías manejar alguna penalización o mensaje en pantalla si llega sin el ítem
                }
            }
        }
    }

    private void ItemInZone(GameObject item)
    {
        // Emparentamos el objeto a la zona de meta
        item.transform.SetParent(goal);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        // Reactivamos componentes opcionales
        Collider col = item.GetComponent<Collider>();
        Rigidbody rb = item.GetComponent<Rigidbody>();

        if (col != null) col.enabled = true;
        if (rb != null) rb.isKinematic = false;
    }
}