using System.Collections;
using UnityEngine;

public class PlayerInvisibility : MonoBehaviour
{
    private bool isInvisible = false;
    public bool IsInvisible => isInvisible;

    public void ActivateInvisibility(float duration)
    {
        StartCoroutine(InvisibilityRoutine(duration));
    }

    IEnumerator InvisibilityRoutine(float duration)
    {
        isInvisible = true;
        Debug.Log("¡Jugador invisible! Los enemigos no te detectarán.");

        // Opcional: Aquí puedes cambiar la opacidad del material del jugador para que se vea transparente.

        yield return new WaitForSeconds(duration);

        isInvisible = false;
        Debug.Log("El efecto de invisibilidad ha terminado.");
    }
}


