using UnityEngine;

public class MapCube : MonoBehaviour
{
    public GameObject chooseEffect;
    //[HideInInspector]
    public GameObject turretGo;
    //[HideInInspector]
    public TurretData turret;
    public bool Buildable = true;
    public GameObject destroyEffect;
    public GameObject fanwei;
    private GameObject tempEffect;
    private Renderer renderer;
    private bool haveEffect = false;
    public static bool isBuildStage = true;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        turretGo = null;
        turret = null;
    }

    public GameObject ReBuild(GameObject turretPrefab)
    {
        if (turretGo == null)
            return null;
        Destroy(turretGo);
        turretGo = Instantiate(turretPrefab, transform.position, Quaternion.identity);
        turret = turretGo.GetComponent<TurretDataLink>().data;
        return turretGo;
    }

    public GameObject BuildTurret(GameObject turretPrefab)
    {
        turretGo = Instantiate(turretPrefab, transform.position, Quaternion.identity);
        turret = turretGo.GetComponent<TurretDataLink>().data;
        AstarPath.active.Scan();

        Buildable = false;
        return turretGo;
    }

    public void DestroyTurret()
    {
        if (turretGo == null)
            return;

        Vector3 tempV3 = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);
        GameObject.Instantiate(destroyEffect, tempV3, transform.rotation);
        GameObject.Find("AudioSource/Environment").GetComponent<AudioManager>().EnvAudioDestroy();
        turretGo.transform.FindChild("ob").gameObject.layer = 0;
        if (turretGo.name == "TurretBrain(Clone)")
            turretGo.GetComponent<TurretBrain>().DisEffect();
        Destroy(turretGo);
        turretGo = null;
        turret = null;
        AstarPath.active.Scan();
        Buildable = true;
    }

    public void highLight()
    {

        // renderer.material.color = new Color(255/255f, 111/255f, 105/255f, 0.8f);
        if (haveEffect == false)
        {
            Vector3 effectPostion = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);
            tempEffect = Instantiate(chooseEffect, effectPostion, transform.rotation);
            haveEffect = true;
            //if (turretGo == null)
            //    return;
            //turretGo.GetComponent<AttackDataManager>().ShowFanwei();
        }
    }

    public void normalLight()
    {
        //   renderer.material.color = new Color(255 / 255f, 111 / 255f, 105 / 255f, 0.0f);
        Destroy(tempEffect, 0.5f);
        haveEffect = false;
        //if (turretGo == null)
        //    return;
        //turretGo.GetComponent<AttackDataManager>().HideFanwei();
    }


    public void ShowFanwei()
    {
        if (turretGo == null)
            return;
        turretGo.GetComponent<AttackDataManager>().ShowFanwei();
    }
    public void HideFanwei()
    {
        if (turretGo == null)
            return;
        turretGo.GetComponent<AttackDataManager>().HideFanwei();
    }
}
