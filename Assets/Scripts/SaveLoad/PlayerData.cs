using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //character stats
    public float maxHealth;
    public float currentHealth;
    public string element;
    public float armor;
    public float power;
    public float movementSpeed;

    //player stats
    public float upgradeLimit;
    public float lifetime;
    public float size;
    public float speed;
    public float instances;
    public float unique;
    public float cooldownreduction;

    public float[] position;

    public PlayerData(PlayerStats player)
    {
        //player stats
        maxHealth = player.maxHealth.GetValue();
        currentHealth = player.GetCurrentHP();
        element = player.element.ElementName;
        armor = player.armor.GetValue();
        power = player.power.GetValue();
        movementSpeed = player.movementspeed.GetValue();

        upgradeLimit = player.upgradeLimit.GetValue();
        lifetime = player.lifetime.GetValue();
        size = player.size.GetValue();
        speed = player.speed.GetValue();
        instances = player.instances.GetValue();
        unique = player.unique.GetValue();
        cooldownreduction = player.cooldownReduction.GetValue();

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
