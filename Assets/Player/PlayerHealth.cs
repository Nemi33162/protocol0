using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health & Damage")]
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int AmountOfDamage)
    {
        if (isDead) return;

        currentHealth -= AmountOfDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthBar.value = currentHealth;

        Debug.Log("Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        Debug.Log("Player has died.");

        var controller = GetComponent<CharacterController>();
        if (controller != null)
            controller.enabled = false;

        var fps = GetComponent<FirstPersonController>();
        if (fps != null)
            fps.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}