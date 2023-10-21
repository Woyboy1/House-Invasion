using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ZombieStats : NetworkBehaviour
{
    [field: SerializeField] public int Health { get; set; } = 20;

    [SerializeField] private int zombieDamage = 40;

    public int ZombieDamage => zombieDamage;

}
