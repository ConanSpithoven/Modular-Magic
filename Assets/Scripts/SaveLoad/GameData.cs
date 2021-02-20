[System.Serializable]
public class GameData
{
    public int score;
    public int totalTime;
    public int lives;

    public GameData(int score, int time, int lives)
    {
        this.score = score;
        totalTime = time;
        this.lives = lives;
    }
}
