using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SaberDamage : MonoBehaviour
{
    public float damage = 25f;
    public float hitCooldown = 0.25f;

    [Header("Audio")]
    public AudioClip hitSound;
    public float hitVolume = 1f;

    private readonly Dictionary<int, float> nextHitTime = new();
    private AudioSource audioSrc;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("[SABER] Trigger ENTER with: " + other.name, other);
        TryHit(other);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("[SABER] Trigger STAY with: " + other.name, other);
        TryHit(other);
    }

    private void TryHit(Collider other)
    {
        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
        if (enemy == null) return;

        int id = enemy.gameObject.GetInstanceID();
        float now = Time.time;

        if (nextHitTime.TryGetValue(id, out float allowedAt) && now < allowedAt)
            return;

        nextHitTime[id] = now + hitCooldown;

        enemy.TakeDamage(damage);

        // 🔊 Play hit sound ONCE per valid hit
        if (hitSound && audioSrc)
        {
            audioSrc.PlayOneShot(hitSound, hitVolume);
        }

        Debug.Log($"[SABER] Hit {enemy.name} for {damage}");
    }
}
