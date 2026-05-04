using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    public int points = 1000;

    public bool SpendPoints(int cost)
    {
        if (points >= cost)
        {
            points -= cost;
            Debug.Log("Points left: " + points);
            return true;
        }

        Debug.Log("Not enough points");
        return false;
    }
}