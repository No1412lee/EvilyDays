using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public List<GameObject> materials;
    public GameObject goalTurret;
    public float possibility = 1.0f;
    public bool Equals(List<MapCube> mapCubes)
    {
        if (mapCubes.Count != materials.Count)
            return false;
        // 从空格建新塔
        //if (materials.Count == 1 && materials[0] == null && mapCubes[0].turretGo == null)
        //    return true;

        bool[] found = new bool[materials.Count];
        foreach (GameObject turret in materials)
        {
            TurretData turretData = turret.GetComponent<TurretDataLink>().data;
            int index = -1;
            for (int i = 0; i < mapCubes.Count; i++)
                if (!found[i] && mapCubes[i].turret.ID == turretData.ID)
                {
                    index = i;
                    found[i] = true;
                    break;
                }
            if (index == -1)
                return false;
        }
        return true;
    }
}

public class UpgradeData : MonoBehaviour
{
    public List<Upgrade> upgrades;
    public List<GameObject> primaryTurret;
    public int mergeCost;
}
