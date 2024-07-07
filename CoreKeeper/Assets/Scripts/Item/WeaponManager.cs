using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : SingletonBehaviour<WeaponManager>
{
    public GameObject[] projectilePrefabs;

    private Dictionary<string, GameObject> findProjectiles = new Dictionary<string, GameObject>();

    private new void Awake()
    {
        base.Awake();

        for(int i = 0; i < projectilePrefabs.Length; i++) 
        {
            findProjectiles.Add(projectilePrefabs[i].GetComponent<Projectile>().rangeWeaponName, projectilePrefabs[i]);
        }
    }

    public GameObject GetProjectile(string _rangeWeaponName)
    {
        return findProjectiles[_rangeWeaponName];
    }
}
