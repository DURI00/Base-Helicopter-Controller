using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTypes: MonoBehaviour {

    enum Direction { North, East, South, West };

    //private으로 하면 유니티의 inspector창엔 안나옴,
    //밑에 Seriali~ 하면, inspector창에 나옴.
    //private으로 하고싶은데 디자이너나 기획자들이 볼 수 있게 하려고.
    [SerializeField]
    private GameObject[] buildings;

    public GameObject[] enemies;
    public Transform playerTransform;

    
//참고.. 태그의 사용 예시 - 충돌 처리
private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;
        //tag는 이렇게 충돌처리하는데에 종종 사용된다고 하심.
        if (tag == "Enemy")
        {
            DataTypes enemy = collision.gameObject.GetComponent<DataTypes>();
            enemy.ReverseDirection(Direction.East);

            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.mass += 10;
            rb.Sleep();//이 부분을 주석처리 한다면, 
            //rigidbody를 넣은 Enemy들이 충돌 후에 멈추지 않고, 날라갈것이다.
            //참고로 밑에 TranformRefValue()로 위치가 재조정되면서 충돌나게됨.ㅎㅎ
            Debug.Log("rigid enter");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
    //Enumerate points
    //여기선 yield 부분을 중점적으로 보자...
    [System.Serializable]
    public class Polygon
    {
        public Vector2[] points;

        public IEnumerator PointEnumerator()
        {
            foreach (Vector2 eachPoint in points) yield return eachPoint;
            yield break;
        }
    }
    public Polygon polygon;

    //코루틴
    IEnumerator Example()
    {
        print(Time.time);
        //~~에너지 모으는 연출 부분~~//
        yield return new WaitForSeconds(5);
        print(Time.time);
        //~~5초뒤 발사하는 연출 부분~~//
    }


    void Start()
    {
        //코루틴
        StartCoroutine(Example());


        //value & ref
        Transform tran = TransformRefValue(transform);

        //enum
        Direction myDirection = Direction.North;
        Direction reversedDirection = ReverseDirection(myDirection);

        //arrays
        //Enemy라는 이름으로 Tag를 만들고, 'Enemy' GameObj들을 
        //Enemy로 태깅한다. 그리고 Find~Tag함수를 이용해
        //Enemy라는 이름의 태그를 가진 GameObj들을 반환받아 연결한다. 
        //연결 되었는지는 유니티에서 실행후 해당 스크립트를 넣은 오브젝트에서 확인가능.
        enemies = GameObject.FindGameObjectsWithTag("Enemy");


        for(int i=0; i< enemies.Length; i++)
        {
            Debug.Log("Enemy Number " + i + " is named " + enemies[i].name);
        }

        //Enumerate points
        IEnumerator pointEnumerator = polygon.PointEnumerator();
        while (pointEnumerator.MoveNext())
        {
            Debug.Log(pointEnumerator.Current);
        }
    }

    Transform TransformRefValue(Transform trans)
    {
        //Value type variable
        Vector3 pos = transform.position;
        //pos에 현재 위치를 복사한 값이 담긴다.
        pos = new Vector3(0, 2, 0);
        //pos에 새로운 위치값(Vector3)를 넣는다 하더라도,
        //transform.position의 값은 변하지 않는다.

        //Reference type variable
        Transform tran = transform;
        tran.position = new Vector3(0, 2, 0);
        //transfrom.position = new Vector3(0, 2, 0); 도 가능함!
        return tran;
    }

    Direction ReverseDirection(Direction dir)
    {
        if (dir == Direction.North)
            dir = Direction.South;
        else if (dir == Direction.South)
            dir = Direction.North;
        else if (dir == Direction.East)
            dir = Direction.West;
        else if (dir == Direction.West)
            dir = Direction.East;

        return dir;
    }
}
