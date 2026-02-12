using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float health = 100f;

    [Header("Force")]
    public float maxForce = 100f;
    public float force = 100f;
    public float forceRegenPerSecond = 12f;

    void Update()
    {
        // Passive Force regen
        force = Mathf.Min(maxForce, force + forceRegenPerSecond * Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        health = Mathf.Max(0f, health - amount);
        if (health <= 0f)
        {
            Debug.Log("Player died");
            // TODO: trigger fail state
        }
    }

    public bool TrySpendForce(float amount)
    {
        if (force < amount) return false;
        force -= amount;
        return true;
    }

    public float Health01() => maxHealth <= 0 ? 0 : health / maxHealth;
    public float Force01() => maxForce <= 0 ? 0 : force / maxForce;
}
