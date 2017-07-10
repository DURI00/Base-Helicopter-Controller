using UnityEngine;
using UnityEngine.UI;

public class HelicopterController : MonoBehaviour
{
    public AudioSource HelicopterSound;
    public ControlPanel ControlPanel;
    public Rigidbody HelicopterModel;

    public float TurnForce = 3f;
    public float ForwardForce = 10f;
    public float SwingForce = 10f;
    public float ForwardTiltForce = 20f;
    public float TurnTiltForce = 30f;
    public float EffectiveHeight = 100f;

    public float turnTiltForcePercent = 1.5f;
    public float turnForcePercent = 1.3f;

    [SerializeField]
    private float _engineForce;
    public float EngineForce
    {
        get { return _engineForce; }
        set
        {
            HelicopterSound.pitch = Mathf.Clamp(value / 40, 0, 1.2f);
    //        if (UIGameController.runtime.EngineForceView != null)
   //             UIGameController.runtime.EngineForceView.text = string.Format("Engine value [ {0} ] ", (int)value);

            _engineForce = value;
        }
    }

    public Vector2 hMove = Vector2.zero;
    private Vector2 hTilt = Vector2.zero;
    private float hTurn = 0f;
    public bool IsOnGround = true;
    public bool IsKeyInput = false;

    // Use this for initialization
	void Start ()
	{
        ControlPanel.KeyPressed += OnKeyPressed;
	}

	void Update () {
	}
  
    void FixedUpdate()
    {
        LiftProcess();
        MoveProcess();
        //SwingProcess();
        TiltProcess();
    }


    //앞으로 가면서 턴
    public float turn;
    private void MoveProcess()
    {
        turn = TurnForce * Mathf.Lerp(hMove.x, hMove.x * (turnTiltForcePercent - Mathf.Abs(hMove.y)), Mathf.Max(0f, hMove.y));
        hTurn = Mathf.Lerp(hTurn, turn, Time.fixedDeltaTime * TurnForce);
        HelicopterModel.AddRelativeTorque(0f, hTurn * HelicopterModel.mass, 0f);
        //이동
        HelicopterModel.AddRelativeForce(Vector3.forward * Mathf.Max(-1f, hMove.y * ForwardForce * HelicopterModel.mass));
    }

    //위아래
    public float upForce;
    private void LiftProcess()
    {
        //Clamp -> 범위 지정(?) 0~1
        upForce = 1 - Mathf.Clamp(HelicopterModel.transform.position.y / EffectiveHeight, -1f, 1f);
        upForce = Mathf.Lerp(-1, EngineForce, upForce) * HelicopterModel.mass;
        HelicopterModel.AddRelativeForce(Vector3.up * upForce);
    }

    //회전하는 연출
    private void TiltProcess()
    {   //Lerp --> 점진적인 변화
        //z축 기준으로 좌우로 기울기(여기선 x로 씌여져 있는데, 씬 환경이 달라서 그런듯)
        hTilt.x = Mathf.Lerp(hTilt.x, hMove.x * TurnTiltForce, Time.deltaTime);
        //x축 기준으로 앞뒤로 기울기
        hTilt.y = Mathf.Lerp(hTilt.y, hMove.y * ForwardTiltForce, Time.deltaTime);
        HelicopterModel.transform.localRotation = Quaternion.Euler(hTilt.y, HelicopterModel.transform.localEulerAngles.y, -hTilt.x);
    }

    //턴과 관련없이 좌우 이동
    private void SwingProcess()
    {
        var turn = TurnForce * Mathf.Lerp(hMove.x, hMove.x * (turnTiltForcePercent - Mathf.Abs(hMove.x)), Mathf.Max(0f, hMove.x));
        hTurn = Mathf.Lerp(hTurn, turn, Time.fixedDeltaTime * TurnForce);
        HelicopterModel.AddRelativeTorque(0f, hTurn * HelicopterModel.mass, 0f);
        HelicopterModel.AddRelativeForce(Vector3.right * hMove.x * SwingForce * HelicopterModel.mass);

    }
    private void OnKeyPressed(PressedKeyCode[] obj)
    {
        if (!IsKeyInput) return;

        float tempY = 0;
        float tempX = 0;

        // stable forward
        if (hMove.y > 0)
            tempY = - Time.fixedDeltaTime;
        else
            if (hMove.y < 0)
                tempY = Time.fixedDeltaTime;

        // stable lurn
        if (hMove.x > 0)
            tempX = -Time.fixedDeltaTime;
        else
            if (hMove.x < 0)
                tempX = Time.fixedDeltaTime;


        foreach (var pressedKeyCode in obj)
        {
            switch (pressedKeyCode)
            {
                case PressedKeyCode.SpeedUpPressed:
                    EngineForce += 0.1f;
                    break;

                case PressedKeyCode.SpeedDownPressed:
                    EngineForce -= 0.12f;
                    if (EngineForce < 0) EngineForce = 0;
                    break;

                case PressedKeyCode.ForwardPressed:
                    if (IsOnGround) break;
                    tempY = Time.fixedDeltaTime;
                    break;

                case PressedKeyCode.BackPressed:
                    if (IsOnGround) break;
                    tempY = -Time.fixedDeltaTime;
                    break;

                case PressedKeyCode.LeftPressed:
                    if (IsOnGround) break;
                    tempX = -Time.fixedDeltaTime;
                    break;

                case PressedKeyCode.RightPressed:
                    if (IsOnGround) break;
                    tempX = Time.fixedDeltaTime;
                    break;

                case PressedKeyCode.TurnRightPressed:
                    {
                        if (IsOnGround) break;
                        var force = (turnForcePercent - Mathf.Abs(hMove.y)) * HelicopterModel.mass;
                        HelicopterModel.AddRelativeTorque(0f, force, 0);
                    }
                    break;

                case PressedKeyCode.TurnLeftPressed:
                    {
                        if (IsOnGround) break;

                        var force = -(turnForcePercent - Mathf.Abs(hMove.y)) * HelicopterModel.mass;
                        HelicopterModel.AddRelativeTorque(0f, force, 0);
                    }
                    break;
            }
        }

        hMove.x += tempX;
        hMove.x = Mathf.Clamp(hMove.x, -1, 1);

        hMove.y += tempY;
        hMove.y = Mathf.Clamp(hMove.y, -1, 1);

    }

    private void OnCollisionEnter()
    {
        IsOnGround = true;
    }

    private void OnCollisionExit()
    {
        IsOnGround = false;
    }
}