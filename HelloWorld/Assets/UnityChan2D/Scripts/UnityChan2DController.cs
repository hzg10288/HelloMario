using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class UnityChan2DController : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float jumpSpeed = 500f;
    public Vector2 backwardForce = new Vector2(-4.5f, 5.4f);

    public LayerMask whatIsGround;

    private Animator m_animator;
    private BoxCollider2D m_boxcollier2D;
    private Rigidbody2D m_rigidbody2D;
    private SpriteRenderer m_renderer;
    private bool m_isGround;
    private const float m_centerY = 1.5f;
    private int isjumponce = 0;

    public GameObject bullet;
    public float shootCooldown = 0.2f;
    private float time = 0;

    private State m_state = State.Normal;

    void Reset()
    {
        Awake();

        // UnityChan2DController
        maxSpeed = 10f;
        jumpSpeed = 500;
        backwardForce = new Vector2(-4.5f, 5.4f);
        whatIsGround = 1 << LayerMask.NameToLayer("Ground");

        // Transform
        transform.localScale = new Vector3(1, 1, 1);

        // Rigidbody2D
        m_rigidbody2D.gravityScale = 3.5f;
        m_rigidbody2D.fixedAngle = true;

        // BoxCollider2D
        m_boxcollier2D.size = new Vector2(1, 2.5f);
        m_boxcollier2D.offset = new Vector2(0, -0.25f);

        // Animator
        m_animator.applyRootMotion = false;
    }

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_boxcollier2D = GetComponent<BoxCollider2D>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (m_state != State.Damaged)
        {
            float x = Input.GetAxis("Horizontal");
            bool jump = Input.GetButtonDown("Jump");
            Move(x, jump);
        }
        if (Input.GetKey(KeyCode.Z) && time > shootCooldown)
        {
            GameObject obj = Instantiate(bullet, transform);
            obj.transform.parent = null;
            Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
            if (!m_renderer.flipX)
            {
                rigidbody.velocity = new Vector2(10, 0);
            }
            else
            {
                rigidbody.velocity = new Vector2(-10, 0);
            }
            
            time = 0;
        }
        time += Time.deltaTime;
    }

    void Move(float move, bool jump)
    {
        if (Mathf.Abs(move) > 0)
        {
            Quaternion rot = transform.rotation;
        //    transform.rotation = Quaternion.Euler(rot.x, Mathf.Sign(move) == 1 ? 0 : 180, rot.z);
            if (Mathf.Sign(move) == 1)
            {
                m_renderer.flipX = false;
            }
            else
            {
                m_renderer.flipX = true;
            }
        }

        m_rigidbody2D.velocity = new Vector2(move * maxSpeed, m_rigidbody2D.velocity.y);

        m_animator.SetFloat("Horizontal", move);
        m_animator.SetFloat("Vertical", m_rigidbody2D.velocity.y);
        m_animator.SetBool("isGround", m_isGround);

        // 两段跳
        if (jump && m_isGround)
        {
            m_animator.SetTrigger("Jump");
            SendMessage("Jump", SendMessageOptions.DontRequireReceiver);
            m_rigidbody2D.velocity = Vector2.up * jumpSpeed;
            isjumponce = 1;
            jump = false;
        }
        if (jump && isjumponce == 1)
        {
            m_animator.SetTrigger("Jump");
            SendMessage("Jump", SendMessageOptions.DontRequireReceiver);
            m_rigidbody2D.velocity = Vector2.up * jumpSpeed;
            isjumponce = 0;
        }
    }

    void FixedUpdate()
    {
        Vector2 pos = transform.position;
        Vector2 groundCheck = new Vector2(pos.x, pos.y - (m_centerY * transform.localScale.y));
        Vector2 groundArea = new Vector2(m_boxcollier2D.size.x * 0.49f, 0.05f);

        m_isGround = Physics2D.OverlapArea(groundCheck + groundArea, groundCheck - groundArea, whatIsGround);
        m_animator.SetBool("isGround", m_isGround);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "DamageObject" && m_state == State.Normal)
        {
            m_state = State.Damaged;
            StartCoroutine(INTERNAL_OnDamage());
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "DamageObject" && m_state == State.Normal)
        {
            m_state = State.Damaged;
            StartCoroutine(INTERNAL_OnDamage());
        }
    }

    IEnumerator INTERNAL_OnDamage()
    {
        m_animator.Play(m_isGround ? "Damage" : "AirDamage");
        m_animator.Play("Idle");

        SendMessage("OnDamage", SendMessageOptions.DontRequireReceiver);

        m_rigidbody2D.velocity = new Vector2(transform.right.x * backwardForce.x, transform.up.y * backwardForce.y);

        yield return new WaitForSeconds(.2f);

        while (m_isGround == false)
        {
            yield return new WaitForFixedUpdate();
        }
        m_animator.SetTrigger("Invincible Mode");
        m_state = State.Invincible;
    }

    void OnFinishedInvincibleMode()
    {
        m_state = State.Normal;
    }

    enum State
    {
        Normal,
        Damaged,
        Invincible,
    }
}
