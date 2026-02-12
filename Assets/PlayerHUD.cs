using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public PlayerStats stats;

    [Header("UI Refs (Fill Images)")]
    public Image healthFill;
    public Image forceFill;

    void Update()
    {
        if (!stats) return;

        if (healthFill) healthFill.fillAmount = stats.Health01();
        if (forceFill) forceFill.fillAmount = stats.Force01();
    }
}
