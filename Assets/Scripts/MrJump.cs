using UnityEngine;
using System.Collections;

public class MrJump : MonoBehaviour
{
    public static MrJump instance;
    public Transform ThrughRay;
    public float SpeedX = 5.0f;
    public float SpeedY = 20.0f;
    private float ForceY = 0.0f;
    public float AmountOfForce = 1000.0f;
    public float DeadAmountForce = 100.0f;
    public string JumpLayerName = "Jump";
    public bool IsJump = false;
    public float TempPressedTime = 0.0f;
    public float MinPressTimes = 0.075f;
    public float MaxPressTimes = 0.225f;
    public bool Jump = false;
    public Vector2 Velocity = Vector2.zero;
    public Transform UpperCollider;
    public Animator anim;
    public enum GameState
    {
        Starting,
        Playing,
        LevelComplete,
        Dead
    };
    public GameState state = GameState.Starting;

    void Awake()
    {
        state = GameState.Starting;
        instance = this;
    }

    void Start()
    {
        state = GameState.Playing;
    }

    void FixedUpdate()
    {
        if (state == GameState.Playing)
        {
            Velocity = rigidbody2D.velocity;
            if (rigidbody2D != null)
            {
                rigidbody2D.velocity = new Vector2(SpeedX, rigidbody2D.velocity.y);
            }
        }
    }

    void Update()
    {
        if (state == GameState.Dead)
        {
            if (rigidbody2D.velocity.y < 0)
            {
                rigidbody2D.gravityScale = 5;
            }
        }
        if (state == GameState.Playing)
        {
            if (rigidbody2D.velocity.y > SpeedY)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, SpeedY);
            }
            if (ThrughRay != null)
            {
                IsJump = false;
                RaycastHit2D[] hitAll = Physics2D.RaycastAll(ThrughRay.transform.position, -Vector2.up, 0.1f);
                Debug.DrawLine(ThrughRay.transform.position, new Vector3(ThrughRay.transform.position.x, ThrughRay.transform.position.y - 0.1f,
                    ThrughRay.transform.position.z), Color.green);
                foreach (RaycastHit2D hit in hitAll)
                {
                    if (hit.collider != null)
                    {
                        //Debug.LogWarning("Collide Something: " + hit.collider);
                        if (LayerMask.LayerToName(hit.transform.gameObject.layer).ToLower().ToString().Equals(JumpLayerName.ToLower().ToString()))
                        {
                            //Debug.LogWarning("Jump Got");
                            IsJump = true;
                            break;
                        }
                        else
                        {
                            IsJump = false;
                        }
                    }
                }
                DesktopInput();
                AndroidInput();
            }
        }
    }

    void DesktopInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump = true;
            TempPressedTime = MinPressTimes;
        }
        else if (Input.GetKey(KeyCode.Space) && Jump)
        {
            TempPressedTime += Time.deltaTime;
            if (TempPressedTime >= MaxPressTimes)
            {
                //Debug.LogError("Max");
                TempPressedTime = MaxPressTimes;
                ApplyJumpForce();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space) && Jump)
        {
            ApplyJumpForce();
        }
    }

    void AndroidInput()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Jump = true;
                TempPressedTime = MinPressTimes;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Stationary && Jump)
            {
                TempPressedTime += Time.deltaTime;
                if (TempPressedTime >= MaxPressTimes)
                {
                    TempPressedTime = MaxPressTimes;
                    ApplyJumpForce();
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended && Jump)
            {
                ApplyJumpForce();
            }
        }
    }

    void ApplyJumpForce()
    {
        if (IsJump)
        {
            Debug.LogWarning("Apply Jump: " + TempPressedTime);
            ForceY = TempPressedTime * AmountOfForce * rigidbody2D.gravityScale;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0.0f);
            rigidbody2D.AddForce(new Vector2(0.0f, ForceY * Time.deltaTime));
            TempPressedTime = 0.0f;
            IsJump = false;
            Jump = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col2D)
    {
        if (col2D.transform.tag == "Dead" && state == GameState.Playing)
        {
            Debug.LogWarning("Dead");
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.AddForce(new Vector2(0.0f, DeadAmountForce * Time.deltaTime * rigidbody2D.gravityScale));
            state = GameState.Dead;
            collider2D.enabled = false;
            UpperCollider.collider2D.enabled = false;
            anim.SetTrigger("Dead");
            StartCoroutine(LoadLevelTime(2.0f));
        }
    }

    IEnumerator LoadLevelTime(float time)
    {
        yield return new WaitForSeconds(time);
        Application.LoadLevel(Application.loadedLevel);
    }

    void OnTriggerEnter2D(Collider2D col2D)
    {
        if (col2D.transform.name == "LevelComplete")
        {
            state = GameState.LevelComplete;
            rigidbody2D.isKinematic = true;
            rigidbody2D.velocity = Vector2.zero;
            if (GameController.instance != null)
            {
                GameController.instance.percentage.text = 100 + "%";
            }
        }
    }

    void OnTriggerExit2D(Collider2D col2D)
    {
        if (col2D.transform.name == "Jumpo")
        {

        }
    }
}
