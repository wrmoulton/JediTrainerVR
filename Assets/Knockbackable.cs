using UnityEngine;
using System.Collections;

public class Knockbackable : MonoBehaviour
{
    public float knockbackTime = 0.20f;

    private FlyingChase chase;
    private Coroutine routine;

    void Awake()
    {
        chase = GetComponent<FlyingChase>();
    }

    public void Knockback(Vector3 direction, float distance)
    {
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.0001f) direction = transform.forward;
        direction.Normalize();

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(KnockbackRoutine(direction, distance));
    }

    private IEnumerator KnockbackRoutine(Vector3 dir, float distance)
    {
        if (chase) chase.enabled = false;

        Vector3 start = transform.position;
        Vector3 end = start + dir * distance;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.01f, knockbackTime);
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        if (chase) chase.enabled = true;
        routine = null;
    }
}
