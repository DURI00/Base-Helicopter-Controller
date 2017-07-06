using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Class : MonoBehaviour
{
    //This is the base class which is
    //also known as the Parent class.
    public class Fruit
    {
        public string color;

        //This is the first constructor for the Fruit class
        //and is not inherited by any derived classes.
        public Fruit()
        {
            color = "orange";
            Debug.Log("1st Fruit Constructor Called");
        }

        //This is the second constructor for the Fruit class
        //and is not inherited by any derived classes.
        public Fruit(string newColor)
        {
            color = newColor;
            Debug.Log("2nd Fruit Constructor Called");
        }

        public void Chop()
        {
            Debug.Log("The " + color + " fruit has been chopped.");
        }

        virtual public void SayHello()
        {
            Debug.Log("Hello, I am a fruit.");
            //코드 상에서 Object 생성하기
            GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            capsule.transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-5f, 5f), Random.Range(-10f, 10f));
        }
    }
    //This is the derived class whis is
    //also know as the Child class.
    public class Apple : Fruit
    {
        //This is the first constructor for the Apple class.
        //It calls the parent constructor immediately, even
        //before it runs.
        public Apple()
        {
            //Notice how Apple has access to the public variable
            //color, which is a part of the parent Fruit class.
            color = "red";
            Debug.Log("1st Apple Constructor Called");
        }

        //This is the second constructor for the Apple class.
        //It specifies which parent constructor will be called
        //using the "base" keyword.
        public Apple(string newColor) : base(newColor)
        {
            //Notice how this constructor doesn't set the color
            //since the base constructor sets the color that
            //is passed as an argument.
            Debug.Log("2nd Apple Constructor Called");
        }
        override public void SayHello()
        {
            Debug.Log("Hello, I am a fruit.");
            //코드 상에서 Object 생성하기
            GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.transform.position = new Vector3(2, -1, 0);
            //생성된 실린더 오브젝트에서 렌더러 컴포넌트를 가져와서, 렌더러의 색상을 변경한다.
            cylinder.GetComponent<Renderer>().material.color = Color.red;

            //Player라는 이름을 가진 GameObject의 렌더러 컴포넌트 색상을 변경
            GameObject.Find("Player").GetComponent<Renderer>().material.color = Color.blue;


            //랜덤 범위에 에너미들 출현 
            for (int i = 0; i < 10; i++)
            {
                GameObject.Find("Enemies").transform.GetChild(i).position = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            }



            //Enemies의 자식 오브젝트들의 컴포넌트들 색상 변경
            Renderer[] enemyRd = GameObject.Find("Enemies").GetComponentsInChildren<Renderer>();
            for (int i = 0; i < enemyRd.Length; i++)
            {
                enemyRd[i].material.color = Color.cyan;
            }

            Rigidbody[] enemyRdb = GameObject.Find("Enemies").GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < enemyRdb.Length; i++)
            {
                enemyRdb[i].useGravity = false;
            }
            new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);

            //실린더형에 박스형 콜라이더를 추가해 봄...(컴포넌트 추가 명령어도 있다!)
            cylinder.AddComponent<BoxCollider>();
            cylinder.AddComponent<Rigidbody>();
            
            //monobehavior을 상속받는 모든 클래스들은 컴포넌트로 취급할 수 있다.
            //이와 같이 ColliderChecker 클래스를 컴포넌트로 추가하여 사용할 수 있음을 보인다.
            cylinder.AddComponent<ColliderChecker>();


            //태그 별로 처리
            
        }
    }
    /// \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ interface \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public interface IKillable
    {
        void Kill();
    }

    //This is a generic interface where T is a placeholder
    //for a data type that will be provided by the 
    //implementing class.
    public interface IDamageable<T>
    {
        void Damage(T damageTaken);
    }

    public class Avatar : MonoBehaviour, IKillable, IDamageable<float>
    {
        //The required method of the IKillable interface
        public void Kill()
        {
            //Do something fun
        }

        //The required method of the IDamageable interface
        public void Damage(float damageTaken)
        {
            //Do something fun
        }
    }
    /// \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ /interface \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

  

    void Start()
    {


        //Let's illustrate inheritance with the 
        //default constructors.
        Debug.Log("Creating the fruit");
        Fruit myFruit = new Fruit(); //public Fruit() 호출
        Debug.Log("Creating the apple");
        Apple myApple = new Apple(); //public Fruit() ->  public Apple() 순으로 호출

        //Call the methods of the Fruit class.
        myFruit.SayHello(); //public class Fruit.SayHello() 호출
        myFruit.Chop(); //public class Fruit.Chop() 호출

        //Call the methods of the Apple class.
        //Notice how class Apple has access to all
        //of the public methods of class Fruit.
        myApple.SayHello();//public class Fruit.SayHello() 호출
        myApple.Chop(); //public class Fruit.Chop() 호출 #color는 Apple() 호출시 red를 넣었기 때문에 red가 출력

        //Now let's illustrate inheritance with the 
        //constructors that read in a string.
        Debug.Log("Creating the fruit");
        myFruit = new Fruit("yellow");// public Fruit(string newColor) 호출
        Debug.Log("Creating the apple");
        myApple = new Apple("green");// : base(newColor)-->(public Fruit(string newColor)) 호출 후, public Apple(string newColor) 호출

        //Call the methods of the Fruit class.
        myFruit.SayHello();//public class Fruit.SayHello() 호출
        myFruit.Chop();//public class Fruit.Chop() 호출 (yellow)

        //Call the methods of the Apple class.
        //Notice how class Apple has access to all
        //of the public methods of class Fruit.
        myApple.SayHello();//public class Fruit.SayHello() 호출
        myApple.Chop();//public class Fruit.Chop() 호출 #color는 Apple(string newColor) 호출시 green 넣었기 때문에 green 출력


        //과일 생성.. but 난 이미 에너미로 했으니 주석처리

/*        List<Fruit> fruits = new List<Fruit>();
        for (int i = 0; i < 10; i++) 
            fruits.Add(new Fruit("black"));
        foreach(Fruit fruit in fruits)
        {
            fruit.SayHello();
        }
        this.gameObject.GetComponent<Renderer>().material.color = Color.black;
*/
    }
}
