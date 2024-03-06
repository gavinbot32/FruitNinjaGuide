using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCursor : MonoBehaviour
{
    [Header("Components")]
    public GameObject particleObject;
    public Collider2D gameCollider;


    [Header("Game Vars")]
    public bool mouseDown; //Bool for if mouse button is clicked

    public float cooldownDur;
    float cooldown;
    public AudioClip[] swooshSounds;
    private AudioSource audioSrc;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    private void CursorPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Converts the cursors screen position to world position
        transform.position = new Vector3(mousePos.x, mousePos.y,0); //Sets position to mouse position
    }

    // Update is called once per frame
    void Update()
    {
        CursorPosition();
        
        if (mouseDown)
        {
            selfActive(true);
        }
        else
        {
            selfActive(false);
        }

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

    }

    private void selfActive(bool active)
    {
        //This method toggles collision and particle system;
        gameCollider.enabled = active;
        particleObject.SetActive(active);
    }

    private void OnMouseDown()
    {
        mouseDown = true;
    }

    private void OnMouseUp()
    {
        mouseDown = false;    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (mouseDown)
        {

            if (collision.CompareTag("Fruit"))
            {
                if (cooldown <= 0)
                {


                    cooldown = cooldownDur;

                    if (collision.GetComponent<Fruit>().sliced == false)
                    {
                        AudioClip clip = swooshSounds[randomChoice(swooshSounds.Length)];
                        audioSrc.PlayOneShot(clip);
                    }
                    collision.GetComponent<Fruit>().damageFruit();


                }
            }
           
        }
    }

    public int randomChoice(int length)
    {
        return Random.Range(0, length);
    }

}
