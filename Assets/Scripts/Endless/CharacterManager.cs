using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CharacterState
{
    Idle,
    Run,
    Talking
}



public class CharacterManager : MonoBehaviour {

    public float minLength;
    public float speed;
    public float minSpeed;
    private CharacterController controller;
    public CharacterState characterState;
    private UIManager UI;
    public List<AudioClip> audioClip;
    private AudioSource audioSource;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag == "Item")
        {
            Rigidbody body = hit.collider.attachedRigidbody;

            if (body == null || body.isKinematic)
            {
                return;
            }
            else
            {
                audioSource.clip = audioClip[0];
                audioSource.volume = 0.5f;
                audioSource.Play();
            
                hit.collider.gameObject.GetComponent<ItemManager>().PickUp();
                //  Debug.Log("touch gameObject： " + hit.collider.gameObject.name);
                //给物体一个移动的力
                //body.velocity = new Vector3(hit.moveDirection.x,0,hit.moveDirection.z) * 30.0f;

            }
        }
    }

    // Use this for initialization
    void Awake()
    {
        characterState = CharacterState.Idle;
        GetComponent<Animator>().SetBool("idle", true);
        controller = GetComponent<CharacterController>();
    }
	void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UI = GameObject.Find("MainCanvas").GetComponent<UIManager>();
        
	}

    public void JoyStickControlMove(Vector2 direction)
    {
        if(characterState==CharacterState.Idle)
        {
            GameObject.Find("GameManager").GetComponent<BuildManager>().mapCubesClear();
            UI.updateUI();
            characterState = CharacterState.Run;
        }
   
        if (direction.magnitude > minLength)
        {

            //设置角色的朝向（朝向当前坐标+摇杆偏移量）  

            transform.LookAt(new Vector3(transform.position.x + direction.x, transform.position.y, transform.position.z + direction.y));
            //移动玩家的位置（按朝向位置移动）  

            //transform.Translate(Vector3.forward * Time.deltaTime * speed);
            //Debug.Log(Vector3.forward);
            Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
            controller.SimpleMove(moveDirection * speed + moveDirection.normalized * minSpeed);
            //播放奔跑动画
            GetComponent<Animator>().SetBool("idle", false);
            GetComponent<Animator>().SetBool("run", true);
        }
    }

    public void JoyStickControlIdle()
    {
        characterState = CharacterState.Idle;
        GetComponent<Animator>().SetBool("idle", true);
        GetComponent<Animator>().SetBool("run", false);
    }


}
