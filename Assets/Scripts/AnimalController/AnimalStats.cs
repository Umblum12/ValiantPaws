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

    // M�todo para recibir da�o
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Comprueba si la salud ha llegado a cero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // M�todo para recuperar salud
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        // Limita la salud m�xima al valor m�ximo establecido
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    // M�todo para consumir energ�a
    public void ConsumeEnergy(float energyAmount)
    {
        currentEnergy -= energyAmount;

        // Comprueba si la energ�a ha llegado a cero
        if (currentEnergy <= 0)
        {
            currentEnergy = 0;
            // Aqu� puedes agregar acciones adicionales cuando la energ�a se agote, como desactivar habilidades, etc.
            if (!isRecharging)
            {
                energyRechargeCoroutine = StartCoroutine(RechargeEnergyAfterDelay(energyRechargeDelay));
            }
        }
    }

    // M�todo para recuperar energ�a
    public void RechargeEnergy(float rechargeAmount)
    {
        currentEnergy += rechargeAmount;

        // Limita la energ�a m�xima al valor m�ximo establecido
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
    }

    // Coroutine para esperar un tiempo antes de comenzar a recargar la energ�a
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

    // M�todo para manejar la muerte del jugador
    private void Die()
    {
        // Aqu� puedes agregar acciones adicionales al morir, como reiniciar el nivel, mostrar un mensaje de game over, etc.
        Debug.Log("Player died.");
        // Por ejemplo, puedes reiniciar el nivel:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
