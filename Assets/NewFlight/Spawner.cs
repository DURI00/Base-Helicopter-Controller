using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    
    public Rigidbody bullet;
    public float speed = 10.0f;

    public Transform Spawn;
    public Rigidbody Missle;
    public int waitTime;
    public int speed_missile;
    private Ray ray = new Ray();

    private RaycastHit hit = new RaycastHit();

    void Update()
    {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //ray : 직선
        //해당 컴포넌트에서 마우스 포지션까지의 직선임!
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("맞은애 : " + hit.transform.name);
            Vector3 point = hit.point;
            this.transform.LookAt(point);
        }
        else
        {
            Spawn.transform.localRotation = Quaternion.identity;
        }

        if (Input.GetButtonDown("Fire1")) //마우스입력, Edit - Project Setting - input 에 마우스 좌클릭으로 설정되어있는 Fire1
        {
            Rigidbody instance = Instantiate(bullet, Spawn.position,
                                                   Spawn.rotation); //instantiate : 게임 동작 중에 prefab(여기선 bullet)불러옴
            instance.velocity = Spawn.forward * speed;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)) // Project Setting - Input 을 사용하지 않고 keycode를 사용
        {
            Debug.Log(Spawn.rotation);
            Rigidbody instance = Instantiate(Missle, Spawn.position, Spawn.rotation);
            instance.velocity = Spawn.forward * speed_missile;
        }
    }
}