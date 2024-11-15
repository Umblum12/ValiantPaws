using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public float maxEnergy = 100;
    public float currentEnergy;
    public float energyRechargeAmount = 0.1f;
    public float energyRechargeDelay = 35f;

    private Coroutine energyRechargeCoroutine;
    private bool isRecharging = false;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    // Método para recibir daño
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Comprueba si la salud ha llegado a cero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Método para recuperar salud
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        // Limita la salud máxima al valor máximo establecido
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    // Método para consumir energía
    public void ConsumeEnergy(float energyAmount)
    {
        currentEnergy -= energyAmount;

        // Comprueba si la energía ha llegado a cero
        if (currentEnergy <= 0)
        {
            currentEnergy = 0;
            // Aquí puedes agregar acciones adicionales cuando la energía se agote, como desactivar habilidades, etc.
            if (!isRecharging)
            {
                energyRechargeCoroutine = StartCoroutine(RechargeEnergyAfterDelay(energyRechargeDelay));
            }
        }
    }

    // Método para recuperar energía
    public void RechargeEnergy(float rechargeAmount)
    {
        currentEnergy += rechargeAmount;

        // Limita la energía máxima al valor máximo establecido
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
    }

    // Coroutine para esperar un tiempo antes de comenzar a recargar la energía
    private IEnumerator RechargeEnergyAfterDelay(float delayTime)
    {
        isRecharging = true;
        yield return new WaitForSeconds(delayTime);
        while (currentEnergy < maxEnergy)
        {
            RechargeEnergy(energyRechargeAmount);
            yield return new WaitForSeconds(energyRechargeAmount);
        }
        isRecharging = false;
    }

    // Método para manejar la muerte del jugador
    private void Die()
    {
        // Aquí puedes agregar acciones adicionales al morir, como reiniciar el nivel, mostrar un mensaje de game over, etc.
        Debug.Log("Player died.");
        // Por ejemplo, puedes reiniciar el nivel:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
