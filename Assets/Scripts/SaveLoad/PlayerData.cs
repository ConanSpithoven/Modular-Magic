[System.Serializable]
public class PlayerData
{
    public float currentHealth;

    public float[] position;

    public PlayerData(PlayerStats player)
    {
        currentHealth = player.GetCurrentHP();

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
