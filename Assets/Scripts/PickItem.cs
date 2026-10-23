using UnityEngine;

public class PickItem : MonoBehaviour
{
    [Header("Manos del Personaje")]
    [SerializeField] private Transform rightHand; // Para armas ("Weapon")
    [SerializeField] private Transform leftHand;  // Para ítems ("Item")

    [Header("Configuración de Recogida")]
    [SerializeField] private float pickupRange = 2.0f; // Radio para buscar objetos cercanos

    // Separamos las referencias para cada mano
    private GameObject currentLeftItem = null;
    private GameObject currentRightItem = null;

    private void Update()
    {
        // Si presionamos E, buscamos qué agarrar (siempre que al menos una mano esté libre)
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickUpNearestItem();
        }
    }

    private void TryPickUpNearestItem()
    {
        // Lanzamos una esfera alrededor del jugador para detectar objetos cercanos
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRange);

        GameObject nearestItem = null;
        float minDistance = Mathf.Infinity;

        // Buscamos cuál de todos los objetos cercanos es el más próximo
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Item") || col.CompareTag("Weapon"))
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestItem = col.gameObject;
                }
            }
        }

        // Si encontramos un objeto, verificamos qué tag tiene y si tenemos la mano correspondiente libre
        if (nearestItem != null)
        {
            if (nearestItem.CompareTag("Item"))
            {
                if (currentLeftItem == null)
                {
                    Pick(nearestItem, leftHand, true); // True indica que va a la izquierda
                }
                else
                {
                    print("¡La mano izquierda ya está ocupada!");
                }
            }
            else if (nearestItem.CompareTag("Weapon"))
            {
                if (currentRightItem == null)
                {
                    Pick(nearestItem, rightHand, false); // False indica que va a la derecha
                }
                else
                {
                    print("¡La mano derecha ya está ocupada!");
                }
            }
        }
    }

    private void Pick(GameObject item, Transform targetHand, bool isLeftHand)
    {
        if (targetHand == null)
        {
            Debug.LogError("¡La mano seleccionada no está asignada en el Inspector!");
            return;
        }

        // Guardamos la referencia en la mano que corresponda
        if (isLeftHand)
        {
            currentLeftItem = item;
        }
        else
        {
            currentRightItem = item;
        }

        // Emparentar el ítem a la mano correspondiente
        item.transform.SetParent(targetHand);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        // Desactivar físicas mientras se lleva equipado
        Rigidbody rb = item.GetComponent<Rigidbody>();
        Collider col = item.GetComponent<Collider>();

        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = false;
    }

    // Funciones opcionales por si en el futuro quieres soltarlos por separado
    public GameObject DropLeftItem()
    {
        GameObject temp = currentLeftItem;
        currentLeftItem = null;
        return temp;
    }

    public GameObject DropRightItem()
    {
        GameObject temp = currentRightItem;
        currentRightItem = null;
        return temp;
    }

    public GameObject DropItem()
    {
        // Por defecto, decide soltar el de la derecha (o cámbialo a izquierda si prefieres)
        if (currentRightItem != null)
        {
            return DropRightItem();
        }
        else if (currentLeftItem != null)
        {
            return DropLeftItem();
        }
        return null;
    }

}