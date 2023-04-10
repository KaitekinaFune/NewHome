using UnityEngine;
using UnityEngine.UI;

public class MainBuilding : Building
{
    [SerializeField] private Slider HealthSlider;

    public int StartingHealth;
    public int Health { get; set; }

    protected override void Start()
    {
        Health = StartingHealth;
        float healthAmount = (float)Health / StartingHealth;
        HealthSlider.value = healthAmount;
    }

    public void Damage(int damage)
    {
        Health -= damage;
        HealthSlider.value = (float)Health / StartingHealth;

        if (Health < 0)
        {
            GameManager.Instance.Lose();
        }
    }
}