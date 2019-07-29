using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public int Speed;
    public int thrust;
    public bool InAir;
    public bool Pause;
    public int score;
    public Text Score;
    public bool pause = true;
    private GameObject[] coins;
    public GameObject manager;
    public GameObject player;
    public Game_Manager GM;
    public Data data;
    private float timer;
    public Text timerText;
    public GameObject timer_;
    public GameObject particle;
    private ParticleSystem exp; 
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(-10.0f, 3.0f, 0f);
        transform.localScale = new Vector3(.5f, .5f, 1f);
        rb.gravityScale = 0;
        manager = GameObject.FindWithTag("GAME MANAGER");
        GM = manager.GetComponent<Game_Manager>();
        data = manager.GetComponent<Data>();
        player = GameObject.FindWithTag("Player");
        timer = 3.0f;
    }

    public void PointAdd()
    {
        score += 100;
        Score.text = "Score: " + score.ToString();
    }
    void FixedUpdate()
    {
        
        if (pause)
        {

        }
        else
        {
            Timer();
            if (timer <= 0)
            {
                timer_.SetActive(false);
                float speed = 0f;
                float Thrust = 0f;
                

                if (data.grasping_force < 20)
                {
                    float prop_size = (data.grasping_force * 0.05f);
                    transform.localScale = new Vector3(prop_size + .5f, prop_size + .75f, 0);
                }

                if (data.grasping_force >= 20)
                {
                    particle.transform.position = transform.position;
                    rb.velocity = new Vector3(0f, 0f, 0f);
                    particle.SetActive(true);
                    player.SetActive(false);
                    exp = particle.GetComponent<ParticleSystem>();
                    exp.Play();
                    
                    GM.Invoke("GameOver", 1);
                }
                              

                if (data.lifting_force > 0.2 && !InAir)
                {
                    Thrust = thrust;
                }

                else
                {
                    Thrust = rb.velocity.y;
                }

                if (!InAir)
                {
                    speed = Speed;
                }

                else
                {
                    speed = rb.velocity.x;
                }

                rb.velocity = new Vector2(speed, Thrust);

                if (rb.position.y < -8)
                {
                    FindObjectOfType<Game_Manager>().GameOver();
                }

            }
        }
        /*// Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0f, transform.localScale.y, 0f), -Vector2.up, transform.localScale.y/2.0f);

        // If it hits something...
        if (hit.collider != null)
        {
            float distance = Mathf.Abs(hit.point.y - transform.position.y);
            if (distance < transform.position.y)
            {
                // Calculate the distance from the surface and the "error" relative
                // to the floating height.
                Debug.DrawRay(transform.position - new Vector3(0f, transform.localScale.y, 0f), Vector3.down * distance, Color.yellow);
                Debug.Log("Did Hit");
            }
            
        }
        else
        {
            Debug.DrawRay(transform.position - new Vector3(0f, transform.localScale.y, 0f), transform.TransformDirection(Vector3.down) * (transform.localScale.y / 2.0f), Color.red);
            Debug.Log("Did Not Hit");
        }*/
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Ground")
        {
            foreach(var ContactPoint in col.contacts)
            {
                if(Mathf.Abs(transform.position.x - ContactPoint.point.x) < 0.1f)
                {
                    InAir = false;
                }
            }            
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        InAir = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            PointAdd();
        }
    }

    public void GameStart()
    {
        pause = false;
        data.start_thing = true;
        rb.gravityScale = 1;
        transform.localScale = new Vector3(.5f, .5f, 0);
        transform.position = new Vector3(-10.0f, 3.0f, 0f);        
        coins = GameObject.FindGameObjectsWithTag("Coin");
        timer = 3.0f;
        timer_.SetActive(true);
        particle.gameObject.SetActive(false);
    }
    public void GameEnd()
    {
        pause = true;
        rb.gravityScale = 0;
        foreach(GameObject coin in coins)
        {
            coin.SetActive(true);
        }
        rb.velocity = new Vector3(0f, 0f, 0f);
    }
    void Timer()
    {
        timer -= Time.deltaTime;
        timerText.text = timer.ToString("0");
    }
}
