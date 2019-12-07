using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{

     private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    public Text wintext;
    public Text livesText;
    public Text timelimitText;
    public Text powerText;
    private float timeLimit = 60.0f;
    private int scoreValue = 0;
    private int livesValue = 3;
    public AudioSource musicSource;
    public AudioSource musicScource2;
    public AudioClip musicClipsong;
    public AudioClip musicClipwin;
    public AudioClip musicClipcoin;
    public AudioClip musicClipjump;
    public AudioClip musicClipoof;
    public AudioClip musicClippowerup;
    private bool faceRight;
    private bool powered = false;
    private SpriteRenderer flip;
    private IEnumerator coroutine;
    private IEnumerator powerup;
    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: "+ scoreValue.ToString();
        wintext.text = " ";
        livesText.text = "Lives: "+livesValue;
        powerText.text = "";
        timelimitText.text = "" + timeLimit;
        musicSource.clip = musicClipsong;
        musicSource.Play();
        anim = GetComponent<Animator>();
        flip = GetComponent<SpriteRenderer>();
        faceRight = true;
        coroutine = StartCountdown();
        StartCoroutine(coroutine);
        powerup = Powerup();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        float hozMovement = Input.GetAxisRaw("Horizontal");
        float verMovement = Input.GetAxisRaw("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * (speed/2)));

        if (Input.GetKey(KeyCode.A))
        {
            anim.SetInteger("state", 1);
            if (faceRight == true)
            {
                flip.flipX = true;
                faceRight = false;
            }
        }


        if (Input.GetKey(KeyCode.D))
        {
            anim.SetInteger("state", 1);
            if (faceRight == false)
            {
                flip.flipX = false;
                faceRight = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("state", 0);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("state", 0);
        }

        if(Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            anim.SetInteger("state", 0);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetInteger("state", 3);
        }

       



        if (scoreValue == 8)
        {
            wintext.text = "You Win! Game Created by Christopher Alfian".ToString();
            if (musicSource.clip == musicClipsong) { 
            musicSource.clip = musicClipwin;
                musicSource.Play();
                musicSource.volume = 0.75f;
                musicSource.loop = false;
                StopCoroutine(coroutine);
            }

        }

        if (livesValue == 0 )
        {
            wintext.text = "You Lose! Game Created by Christopher Alfian".ToString();
            gameObject.SetActive(false);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

    }
    public IEnumerator StartCountdown(float countdownValue =30)
    {
        timeLimit = countdownValue;
        
        
            while (timeLimit >= 0)
            {
                timelimitText.text = "" + timeLimit;
                yield return new WaitForSeconds(1.0f);
                timeLimit--;

                if (timeLimit == 0)
                {
                    wintext.text = "You Lose! Game Created by Christopher Alfian".ToString();
                    gameObject.SetActive(false);
                }
            }
        
 
    }
    public IEnumerator Powerup(float powerupTime=2)
    {
        while (powerupTime >= 0)
        {
            powerText.text = "Double Jump Height!";
            yield return new WaitForSeconds(1.0f);
            powerupTime--;
        }
        
        powerText.text = "";
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("coin"))
        {
            musicScource2.clip = musicClipcoin;
            musicScource2.Play();
            collision.gameObject.SetActive(false);
            scoreValue++;
            score.text ="Score: "+ scoreValue.ToString();

            if (scoreValue == 4)
            {
                transform.position = new Vector2(74.0f, 1.97f);
                livesValue = 3;
                livesText.text = "Lives: " + livesValue.ToString();
                score.text = "Score: "+ scoreValue.ToString();
            }

        }

        if (collision.gameObject.CompareTag("power"))
        {
            musicScource2.clip = musicClippowerup;
            musicScource2.Play();
            collision.gameObject.SetActive(false);
            powered = true;
            StartCoroutine(powerup);
        }

        }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {

           
            collision.gameObject.SetActive(false);
            livesValue--;
            livesText.text = "Lives: " + livesValue.ToString();
            musicScource2.clip = musicClipoof;
            musicScource2.Play();

        }
    }



    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("floor"))
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                anim.SetInteger("state", 1);
            }
            else { anim.SetInteger("state", 0); }

            anim.SetBool("ground", true);

          

            if (Input.GetKey(KeyCode.W))
            {
                if (powered == true)
                {
                    rd2d.AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
                    musicScource2.clip = musicClipjump;
                    musicScource2.Play();
                    anim.SetInteger("state", 2);
                    anim.SetBool("ground", false);
                }
                else
                {
                    rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                    musicScource2.clip = musicClipjump;
                    musicScource2.Play();
                    anim.SetInteger("state", 2);
                    anim.SetBool("ground", false);
                }
            }

           
        }
       

    }

}
