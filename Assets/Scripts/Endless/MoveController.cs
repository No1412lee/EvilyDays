using UnityEngine;

public class MoveController : MonoBehaviour
{
    //public float minLength;
    //public float speed;
    //public float minSpeed;
    //private CharacterController controller;
    //public void JoyStickControlMove(Vector2 direction)
    //{
    // //   Debug.Log("Move");
    //    //获取摇杆中心偏移的坐标  

    //    if (direction.magnitude > minLength)
    //    {

    //        //设置角色的朝向（朝向当前坐标+摇杆偏移量）  
            
    //        transform.LookAt(new Vector3(transform.position.x + direction.x, transform.position.y, transform.position.z + direction.y));
    //        //移动玩家的位置（按朝向位置移动）  

    //        //transform.Translate(Vector3.forward * Time.deltaTime * speed);
    //        //Debug.Log(Vector3.forward);
    //        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
    //        controller.SimpleMove(moveDirection * speed + moveDirection.normalized * minSpeed);
    //        //播放奔跑动画
    //        GetComponent<Animator>().SetBool("idle", false);
    //        GetComponent<Animator>().SetBool("run", true);
    //    }
    //}

    //public void JoyStickControlIdle( )
    //{
    //    GetComponent<Animator>().SetBool("idle", true);
    //    GetComponent<Animator>().SetBool("run", false);
    //}

    //void Awake()
    //{
    //    GetComponent<Animator>().SetBool("idle", true);
    //    controller = GetComponent<CharacterController>();
    //}



}