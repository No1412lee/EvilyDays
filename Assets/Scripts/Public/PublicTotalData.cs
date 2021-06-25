using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Vec2Int
{
    public static int xSize;
    public static int ySize;
    public int x, y;
    public Vec2Int() { }
    public Vec2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public bool Equals(Vec2Int other)
    {
        return x == other.x && y == other.y;
    }
    public bool legal()
    {
        return x >= 0 && x < xSize && y >= 0 && y < ySize;
    }
}

[System.Serializable]
public class AttackData
{
    public TurretType turretType;
    public Transform firePosition;
    public Transform head;
    //   public float rotateSpeed;
    public float attackSpeed;        //x秒攻击一次
    public float attack;
    public AttackType attackType;
    public int attackNumber;        //最多攻击数
  //  public DeBuff debuff;
 //   public Buff buff;
    public BulletData bulletData;
    public GreenTurretData greenData;
}
[System.Serializable]
public class GreenTurretData
{
    public float greenAttack;
    public float greenSpeed;
    public float greenRadius;
    public float attackNumber;
}

public enum AttackType
{
    AD,
    AP,
    MIX
}

[System.Serializable]
public class BulletData
{
    public GameObject bulletPrefab; //子弹的模型
    public float bulletSpeed;
    public float distanseArrive;
}

[System.Serializable]
public class DeBuff
{
    public string DeBuffId;
    public float keepTime;
    //   public float keepCount = 0;
    public float dotAttack;
    public float slowSpeed;
    public float breakArmor;
}

[System.Serializable]
public class Buff
{
    public string buffId;
  //  public int buffId;
    public float keepTime;
    //   public float keepCount = 0;
    public float buffAttack;
    public float buffSpeed;

}

[System.Serializable]
public class BuffCount
{
    public float keepCount;
    public Buff buff;
}

[System.Serializable]
public class AudioData
{
    public AudioType audioType;
    public List<AudioClip> audioClips;
}

public enum AudioType
{
    BGM,
    Environment,
    UI,
    Character,
    Enemy,
    Turret
}

[System.Serializable]
public class TurretData
{
    public GameObject turretPrefab;  //塔的模型 
    public List<GameObject> upgradeTurretPrefab;
    //public TurretGrade grade;
    public TurretType attackType;
    public int buildCost;
    public int recover;
    public GameObject UI;
    [HideInInspector]
    public int ID;
    public int hunger;
    //public GameObject upTurret;
    //public GameObject composeUI;
    // AP/AD
    // effect 技能
    //public TurretData downTurret;
}

public enum TurretGrade
{
    PrimaryTurret,
    UpgradeTurret,
    MergeTurret
}

public enum TurretType
{
    NormalTurret,
    GravityTurret,
    ElecTurret,
    RayTurret,
    SlowTurret,
    DotTurret,
    BuffTurret,
    ResourceTurret,
    obstacleTurret,
    aoeTurret
}

[System.Serializable]
public class DrugData
{
    public GameObject drug;
    //    public GameObject drugIco;
    public int value;
    //    public float scopen;
    private List<GameObject> reactionDrug;
}


