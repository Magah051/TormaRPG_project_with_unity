using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Int32 CalculateHealth(Player player)
    {
        // Formula: (resistence * 10) + (level * 4) + 10
        Int32 result = (player.entity.resistence * 10) + (player.entity.level * 4) + 10;
        Debug.LogFormat("CalculateHealth: {0}", result);
        return result;
    }

    public Int32 CalculateMana(Player player)
    {
        // Formula: (intelligence * 10) + (level * 4) + 5
        Int32 result = (player.entity.intelligence * 10) + (player.entity.level * 4) + 5;
        Debug.LogFormat("CalculateMana: {0}", result);
        return result;
    }

    public Int32 CalculateStamina(Player player)
    {
        // Formula: (intelligence * 10) + (level * 4) + 5
        Int32 result = (player.entity.resistence + player.entity.willpower) + (player.entity.level * 2) + 5;
        Debug.LogFormat("CalculateStamina: {0}", result);
        return result;
    }

    public Int32 CalculateDamage(Player player, int weaponDamage)
    {
        // Formula: (str * 2) + (weapon dmg * 2) + (level * 3)  + random (1-20)
        System.Random rnd = new System.Random();
        Int32 result = (player.entity.strength * 2) + (weaponDamage * 2) + (player.entity.level * 3) + rnd.Next(1, 20);
        Debug.LogFormat("CalculateDamage: {0}", result);
        return result;
    }

    public Int32 CalculateDefense(Player player, int armorDefense)
    {
        // Formula: (endurance * 2) + (level * 3) + armorDefense;
        Int32 result = (player.entity.resistence * 2) + (player.entity.level * 3) + armorDefense;
        Debug.LogFormat("CalculateDefense: {0}", result);
        return result;
    }

}