using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderChecker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
    //#참고, 만약 collider컴포넌트에 is Trigger를 true로 한다면 리지드바디 영향을 안받고 충돌만 체크함
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");
        if (collision.gameObject.tag == "Enemy")// && gameObject.tag == "Player")
            collision.gameObject.SetActive(false);

        // 운석이 벽에 충돌한 경우, 폭발처리
        if(this.gameObject.tag == "Meteor")
        {
            //this.gameObject.transform.GetChild(0).gameObject.SetActive(true); //SetActive로 제어하면, 단발성이기 때문에,,, 변경
            
            //폭발 오브젝트 생성
            GameObject explosion = GameObject.Instantiate(GameObject.Find("Explosion"), this.transform.position, Quaternion.identity);

            //운석 제거
            this.gameObject.SetActive(false);

        }

    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("onTriggerEnter");
    }
}
