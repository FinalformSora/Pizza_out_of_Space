using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }

    public Sprite AttractSprite;
    public Sprite RepelSprite;
    public Sprite BatterySprite;
    public Sprite TrapSprite;
    public Sprite PizzaSprite;
    public Sprite PillsSprite;
}
