using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

    public static GameController instance;
    public Text percentage;
    public float PercentageMultiplier = 0.5f;
    public float PercentageMultiplierHelper = 1.0f;

    void Awake()
    {
        instance = this;
        percentage.text = 0 + "%";
    }

    void Start()
    {
    }

    void Update()
    {
        if (MrJump.instance != null)
        {
            if (MrJump.instance.state == MrJump.GameState.Playing)
            {
                if (MrJump.instance.transform.position.x >= 0.0f)
                {
                    float Vals = Mathf.Clamp(MrJump.instance.transform.position.x * PercentageMultiplier * PercentageMultiplierHelper, 0, 99);
                    percentage.text = (int)Vals + "%";
                }
            }
        }
    }
}
