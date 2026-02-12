using UnityEngine;
using System.Collections.Generic;

public class SaberDamage : MonoBehaviour
{
    public float damage = 25f;
    public float hitCooldown = 0.25f;

    private readonly Dictionary<int, float> nextHitTime = new();

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[SABER] Trigger ENTER with {other.name}", other);
        TryHit(other);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"[SABER] Trigger STAY with {other.name}", other);
        TryHit(other);
    }

    private void TryHit(Collider other)
    {
        Debug.Log($"[SABER] Checking for EnemyHealth on {other.name}");

        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
        if (enemy == null)
        {
            Debug.Log($"[SABER] No EnemyHealth found on {other.name} or its parents");
            return;
        }

        int id = enemy.gameObject.GetInstanceID();
        float now = Time.time;

        if (nextHitTime.TryGetValue(id, out float allowedAt) && now < allowedAt)
        {
            Debug.Log($"[SABER] Hit blocked by cooldown on {enemy.name}");
            return;
        }

        nextHitTime[id] = now + hitCooldown;

        Debug.Log($"[SABER] HIT enemy {enemy.name} for {damage} damage!");
        enemy.TakeDamage(damage);
    }
}
