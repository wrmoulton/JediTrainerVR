using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SaberSwingAudio : MonoBehaviour
{
    [Header("Swing Settings")]
    public AudioClip swingClip;
    public float swingSpeedThreshold = 1.2f;   // meters/sec
    public float swingCooldown = 0.15f;

    Vector3 lastPos;
    float nextAllowedTime;
    AudioSource audioSrc;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        lastPos = transform.position;
    }

    void Update()
    {
        Vector3 velocity = (transform.position - lastPos) / Time.deltaTime;
        float speed = velocity.magnitude;

        lastPos = transform.position;

        if (speed >= swingSpeedThreshold && Time.time >= nextAllowedTime)
        {
            PlaySwing();
            nextAllowedTime = Time.time + swingCooldown;
        }
    }

    void PlaySwing()
    {
        float speed = ((transform.position - lastPos) / Time.deltaTime).magnitude;
        float vol = Mathf.InverseLerp(swingSpeedThreshold, swingSpeedThreshold * 2f, speed);

        audioSrc.pitch = Random.Range(0.9f, 1.1f);
        audioSrc.PlayOneShot(swingClip, Mathf.Clamp01(vol));
    }

}
