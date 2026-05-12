using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TMP_Text clockText;
    public TMP_Text pointsText;
    public PlayerPoints playerPoints;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        clockText.text = "Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");

        pointsText.text = "Points: " + playerPoints.points;
    }
}
