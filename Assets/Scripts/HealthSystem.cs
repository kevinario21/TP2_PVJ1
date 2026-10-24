using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    public float CurrentHealth => currentHealth; // Getter de sólo lectura

    [SerializeField] private bool isDead;
    public bool IsDead => isDead;

    [SerializeField] private GameObject vfx;

    private Rigidbody rb;
    private Collider col;
    private Attack attack;

    // Usamos Renderer general para evitar problemas si usa MeshRenderer o SkinnedMeshRenderer
    private Renderer characterRenderer;

    private void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        attack = GetComponent<Attack>();

        characterRenderer = GetComponentInChildren<Renderer>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"{gameObject.name} recibe daño. Salud actual: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Dead();
        }
    }

    private void Dead()
    {
        isDead = true;

        // Notificamos al GameManager según el Tag del objeto que muere
        if (gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.NotifyPlayerDead();
            }
            Debug.Log("¡El Jugador ha muerto! Game Over.");
        }
        else if (gameObject.CompareTag("Character"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.NotifyEnemyDefeat();
            }
            Debug.Log("Enemigo derrotado.");
        }

        // Iniciamos la transición para apagar componentes y destruir el objeto
        StartCoroutine(DestroyTransition());
    }

    IEnumerator DestroyTransition()
    {
        // Desactivamos físicas y colisiones
        if (col != null) col.enabled = false;
        if (rb != null) rb.isKinematic = true;

        // Desactivamos el ataque si lo tiene
        if (attack != null) attack.enabled = false;

        // Ocultamos la malla visual
        if (characterRenderer != null) characterRenderer.enabled = false;

        // Esperamos 5 segundos antes de destruir el objeto
        yield return new WaitForSeconds(5f);

        // Destruimos el objeto de la escena
        Destroy(this.gameObject);
    }
}