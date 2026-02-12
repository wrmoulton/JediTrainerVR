using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHp = 50f;
    public float hp = 50f;

    public System.Action<EnemyHealth> OnDied;

    void Awake() => hp = maxHp;

    public void TakeDamage(float dmg)
    {
        if (hp <= 0) return;

        hp = Mathf.Max(0, hp - dmg);

        if (hp <= 0)
        {
            OnDied?.Invoke(this);
            Destroy(gameObject, 0.05f);
        }
    }
}
