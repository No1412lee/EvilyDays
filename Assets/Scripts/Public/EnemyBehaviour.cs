using Pathfinding;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    private EnemyData enemyData;
    public Slider hpSlider;
    public GameObject dieEffect;
    public GameObject arriveEffect;
    private float hp;
    private float maxhp;
    public float x = 0.05f;
    //private int money;
    //private Text moneyText;

    // 【寻路】
    private FindPath findPath;
    private Path path;
    private int currentPath;
    private int currentPathPoint;
    private CharacterController controller;
    // 【寻路】

    // Use this for initialization
    void Awake ()
    {
        enemyData = GetComponent<EnemyDataManager>().enemyData;
        findPath = GameObject.Find("A*").GetComponent<FindPath>();
        controller = GetComponent<CharacterController>();
        currentPath = 0;
        path = findPath.paths[currentPath];
        currentPathPoint = 0;
        maxhp = enemyData.maxHitPoint;
        //moneyText = GameObject.Find("Canvas/MoneyPanel/MoneyText").GetComponent<Text>();
        //money = Convert.ToInt32(moneyText.text);
    }
    public void endlessADD(EndlessAddEnemy _add, int Overwave)
    {
        enemyData.maxHitPoint += _add.addHp * Overwave;
        enemyData.hitPoint += _add.addHp * Overwave;
        enemyData.speed += _add.addspeed * Overwave;
        enemyData.spell += _add.addspell * Overwave;
        enemyData.armor += _add.addarmor * Overwave;
        enemyData.hot += _add.addarmor * Overwave;
    }
    public void reachBase()     // 怪物到达终点
    {
        //也播放死亡特效
        Vector3 tempPosition = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
        Instantiate(arriveEffect, tempPosition, transform.rotation);
        GameObject.Find("AudioSource/Environment").GetComponent<AudioManager>().EnvAudioEnemyHit();

        if (SceneManager.GetActiveScene().name == "Endless")
        {
            BuildManager.BaseHpChange(enemyData.hitPoint / enemyData.maxHitPoint * enemyData.attack);
        }
        else
        {
            ChapterBuildManager.BaseHpChange(enemyData.hitPoint / enemyData.maxHitPoint * enemyData.attack);
        }
        Destroy(gameObject);
    }

    public void FixedUpdate()
    {
        if (currentPathPoint >= path.vectorPath.Count)
        {
            currentPath++;
            if (currentPath < findPath.paths.Count)
            {
                path = findPath.paths[currentPath];
                currentPathPoint = 0;
            }
            else
            {
                reachBase();
            }
            return;
        }

        Vector3 end = path.vectorPath[currentPathPoint];
        end.y = transform.position.y;
        if (Vector3.Distance(transform.position, end) < findPath.nextWayPointDistance)
        {
            currentPathPoint++;
            return;
        }

        Vector3 dir = (end - transform.position).normalized;
        float speed = enemyData.speed + enemyData.greenEnemyData.speed;

        //移速最小是10
        if (speed > 10)
        {
            dir *= speed * Time.fixedDeltaTime;
        }
        else
        {
            dir *= 10.0f* Time.fixedDeltaTime;
        }
        enemyData.enemyPrefab.transform.forward = Vector3.Lerp(enemyData.enemyPrefab.transform.forward, dir, 0.1f); 
        controller.SimpleMove(dir);

        //Hot回血
        if (enemyData.hitPoint < enemyData.maxHitPoint)
            enemyData.hitPoint += enemyData.hot * Time.deltaTime;

        //DOT掉血
        enemyData.hitPoint -= (enemyData.greenEnemyData.dot * Time.deltaTime);
        hpSlider.value = enemyData.hitPoint / enemyData.maxHitPoint;
        //DOT死亡
        if (enemyData.hitPoint <= 0)
        {
            //moneyText = GameObject.Find("Canvas/MoneyPanel/MoneyText").GetComponent<Text>();
            //money = Convert.ToInt32(moneyText.text);
            //ChangeMoney(enemyData.reward);
            BuildManager.ChangeMoney(-enemyData.reward);
            Die();
        }
    }

    public void TakeDamager(float damage, AttackType attactType)
    {
        
        float realDamage=0;
        if (enemyData.hitPoint < 0)    // 安全监测
            return;
        
        
        //TODO 伤害计算公式
        // War3:damage=attack/（1+x%*amor），amor＞0   damage=attack*（1+x%*amor），amor＜0
        if (attactType == AttackType.AD)  
        {
            float armor = enemyData.armor + enemyData.greenEnemyData.armor;
            if(armor>0)
                realDamage = damage / (1 + x * armor);
            else
                realDamage = damage * (1 - x * armor);

            
            if (realDamage > 0)
                enemyData.hitPoint -= realDamage;
            hpSlider.value = enemyData.hitPoint / enemyData.maxHitPoint;
        }
        else if (attactType == AttackType.AP)
        {
            float spell = enemyData.spell + enemyData.greenEnemyData.spell;
            if (spell > 0)
                realDamage = damage / (1 + x * spell);
            else
                realDamage = damage * (1 - x * spell);
            if (realDamage > 0)
                enemyData.hitPoint -= realDamage;
          
            hpSlider.value = enemyData.hitPoint / enemyData.maxHitPoint;
        }
        else if (attactType == AttackType.MIX)
        {
            float armor = enemyData.armor + enemyData.greenEnemyData.armor;
            float spell = enemyData.spell + enemyData.greenEnemyData.spell;

            if (armor > 0)
                realDamage = 0.5f*damage / (1 + x * armor);
            else
                realDamage = 0.5f*damage * (1 -x * armor);

            if (spell > 0)
                realDamage += 0.5f*damage / (1 + x * spell);
            else
                realDamage += 0.5f*damage * (1 - x * spell);

            if (realDamage > 0)
                enemyData.hitPoint -= realDamage;

            hpSlider.value = enemyData.hitPoint / enemyData.maxHitPoint;
        }

        else    // 异常
        {
            enemyData.hitPoint -= 1;
            hpSlider.value = 0;
        }

        // 死亡处理
        if (enemyData.hitPoint<=0)
        {
            if (SceneManager.GetActiveScene().name == "Endless")
            {
                BuildManager.ChangeMoney(-enemyData.reward);
            }
            else
            {
                ChapterBuildManager.ChangeMoney(-enemyData.reward);
            }
            Die();
        }
    }
    void Die()
    {
        // 播放死亡特效
        Vector3 tempPosition = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
        Instantiate(dieEffect, tempPosition, transform.rotation);

        if (SceneManager.GetActiveScene().name == "Endless")
        {


            foreach (DropDate tempDrop in enemyData.dropDates)
            {
                if (tempDrop.probability > Random.value)
                {
                    // TODO: 掉落

                    //TODO:判断是否要叮
                    //Vector3 tempDropPosition = new Vector3(transform.position.x + Random.value * 2 - 1.0f, transform.position.y, transform.position.z + Random.value * 2 - 1.0f);
                    Vector3 tempDropPosition = new Vector3(Random.value * 2 - 1.0f, 1.0f, Random.value * 2 - 1.0f);
                    GameObject dropItem = ToolsManager.instance.InstantiateItem(tempDrop.itemID, tempPosition, transform.rotation);
                    //Debug.Log(tempDropPosition);
                    dropItem.GetComponent<Rigidbody>().AddForce(tempDropPosition, ForceMode.Impulse);
                }
            }
        }

        //Destroy(effect,1.5f);
        Destroy(gameObject);
    }
}
