using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ContactDamageToPlayer : MonoBehaviour
{
    public float damage = 10f;
    public float damageInterval = 0.5f;

    [Header("Audio")]
    public AudioClip hitSound;

    float nextDamageTime;
    AudioSource audioSrc;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    void OnTriggerStay(Collider other)
    {
        var ps = other.GetComponentInParent<PlayerStats>();
        if (ps == null) return;

        if (Time.time >= nextDamageTime)
        {
            nextDamageTime = Time.time + damageInterval;

            ps.TakeDamage(damage);

            if (hitSound && audioSrc)
            {
                audioSrc.PlayOneShot(hitSound);
            }
        }
    }
}
