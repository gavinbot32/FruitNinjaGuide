using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{

    public int health;
    public int bounty;

    public float lifeTime;

    public bool sliced;


    [Header("Components")]
    protected private Rigidbody2D rig;
    public ParticleSystem ps;
    protected private SpriteRenderer sr;
    protected private GameManager manager;
    public SwordCursor player;
    public AudioSource audioSrc;
    public AudioClip[] sliceSounds;

    [Header("Physics Vars")]
    public float minVelx;
    public float maxVelx;
    public float minVely;
    public float maxVely;

    
    [Header("Generation Vars")]
    public int genIndex;
    public int genLength;
    public Sprite[] spriteList;


    protected private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        player = FindObjectOfType<SwordCursor>();
        manager = FindObjectOfType<GameManager>();
        audioSrc = GetComponentInChildren<AudioSource>();
        sliced = false;

    }
    // Start is called before the first frame update
    protected void Start()
    {
        StartPhysics();
    }
    void StartPhysics()
    {
        //Get a random y velocity for the fruit
        float velY = Random.Range(minVely, maxVely);

        //( Sets Min/Max x velocity values based on whichever side of the screen the fruits on
        if (genIndex > 0)
        {

            minVelx = -5;
            maxVelx = 5;
            minVely = 0;
            maxVely = 8;
            velY = Random.Range(minVely, maxVely);

        }
        else
        {
            if (transform.position.x < -2)
            {
                minVelx = 5;
                maxVelx = 8;
            }
            else if (transform.position.x > 2)
            {
                minVelx = -8;
                maxVelx = -5;
            }
            else
            {
                minVelx = -7;
                maxVelx = 7;
            }
        }
        // )

        //Get a random x velocity for the fruit using the min/max velx we defined above
        float velX = Random.Range(minVelx, maxVelx);

        //Add an impulse force of both random values.
        rig.AddForce(new Vector2(velX, velY), ForceMode2D.Impulse);

        // gives fruit a little spin in the air
        rig.angularVelocity = ((velX * velY) / 2) * 10;
    }

    protected void FixedUpdate()
    {
        DeathCheck();
    }

    void DeathCheck()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
        if (transform.position.y < -15)
        {
            Destroy(gameObject);
        }

        if (transform.position.y <= -8)
        {
            if (genIndex == 0 && sliced == false)
            {
                manager.looseLife();
                Destroy(gameObject);
            }
        }
    }

    public void initialize(int index)
    {
        genIndex = index + 1;
        sr.sprite = spriteList[genIndex];
        if (genIndex >= genLength)
        {
            Destroy(GetComponent<Collider2D>());

        }
    }

    public virtual void sliceFruit()
    {
        sliced = true;
        audioSrc.PlayOneShot(sliceSounds[randomChoice(sliceSounds.Length)]);
        if (genIndex < genLength)
        {
            Instantiate(this.gameObject, transform.position, Quaternion.identity).GetComponent<Fruit>().initialize(genIndex);
            Instantiate(this.gameObject, transform.position, Quaternion.identity).GetComponent<Fruit>().initialize(genIndex);
            psBurst();
            manager.addPoints(bounty);
            audioSrc.transform.SetParent(null);
            Destroy(gameObject);
        }
        Collider2D thisCol = GetComponent<Collider2D>();
        Destroy(thisCol);
        audioSrc.transform.SetParent(null);
        Destroy(audioSrc.gameObject, 2f);
        Destroy(gameObject, 3f);
    }
    public void damageFruit()
    {
        health -= 1;
        if (health <= 0)
        {
            sliceFruit();
        }
    }

    public void psBurst()
    {
        if (ps == null)
        {
            return;
        }
        ps.transform.parent = null;
        ps.Stop();
        ps.Play();
        Destroy(ps.gameObject, 2f);
    }

    public int randomChoice(int length)
    {
        return Random.Range(0, length);
    }
}
