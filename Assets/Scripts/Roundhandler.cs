using TMPro;
using UnityEngine;

public class Roundhandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI rounds_count;
    [SerializeField]
    private TextMeshProUGUI points_count;

    [SerializeField]
    private int _points = 2;
    public int points => _points;
    private int _rounds = 0;
    public int rounds => _rounds;

    void Awake()
    {
        updateUI();
    }
    private void updateUI()
    {
        points_count.text = "Points; " + _points;
        rounds_count.text = "No. Turns; " + _rounds;
    }

    public bool spendPoints(int amount)
    {
        if (amount > _points || amount <= 0) return false;

        _points -= amount;
        updateUI();

        return true;
    }

    public void nextRound()
    {
        BotsHandler.botsTurn();
        _rounds += 1;
        updateUI();
    }

    public void skipRound()
    {
        BotsHandler.botsTurn();
        _rounds += 1;
        _points += 2;
        updateUI();
    }

    
    public void addPoints(int amt)
    {
        _points += amt;
        updateUI();
    }
}
