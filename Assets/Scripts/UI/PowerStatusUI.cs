using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerStatusUI : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI durationText;

    public float Cooldown { get; set; }
    public void Setup(PowerUpdata data)
    {
        image.sprite = data.icon;
        Cooldown = data.duration;
    }
    private void Update()
    {
        durationText.text = $"{Cooldown:F1}s";
    }
}
