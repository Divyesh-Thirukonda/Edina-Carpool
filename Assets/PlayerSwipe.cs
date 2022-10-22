using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerSwipe : MonoBehaviour
{
    // Touch variables
    Vector2 startTouchPosition;
    Vector2 currentTouchPosition;
    Vector2 endTouchPosition;
    bool stopTouch = false;
    public float swipeRange = 2;
    public float tapRange;

    // amount of gold collected
    float gold = 0;

    // UI
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI scoreMultiplierText;
    public TextMeshProUGUI scoreText;

    // Audio
    public AudioSource coinAudio;
    public AudioSource powerupAudio;

    // The jump-boost LIST
    public GameObject[] jumpBoostCollection;

    
    void Update()
    {
        Swipe();

        // display Spawn.points and gold in UI
        coinText.text = Mathf.Floor(gold).ToString();
        scoreMultiplierText.text = "X " + UIupdate(Spawn.scoreMultiplier);
        scoreText.text = UIupdate(Spawn.points);
    }

    string UIupdate(float numToConvert) {

        string[] suffix = {"K", "M", "B"};
        for(int i = 2; i >= 0; i--) {
            if (numToConvert < 1000) {
                return Mathf.Floor(numToConvert).ToString();
                break;
            }
            if(numToConvert / (1000 * Mathf.Pow(10, i*3)) > 1) {
                return Mathf.Floor(numToConvert / (1000 * Mathf.Pow(10, i*3))) + suffix[i];
                break;
            }
        }
        return "";

        // BELOW IS AN ALTERNATE WAY TO WRITE THE CODE ABOVE
        // if (numToConvert > 1000f && numToConvert < 1000000f) {
        //     return Mathf.Floor(numToConvert/1000f).ToString() + "K";
        // } else if (numToConvert > 1000000f && numToConvert < 1000000000f) {
        //     return Mathf.Floor(numToConvert/1000000f).ToString() + "M";
        // } else if (numToConvert > 1000000000f && numToConvert < 1000000000000f) {
        //     return Mathf.Floor(numToConvert/1000000000f).ToString() + "B";
        // } else {
        //     return Mathf.Floor(numToConvert).ToString();
        // }
    }

    void OnTriggerEnter(Collider collision) {

        // if you collide with a gameobject and its tag is 'X' then do its corrosponding action
        if (collision.gameObject.tag == "destroyPlayer")
        {
            ResetGame();
        }
        if (collision.gameObject.tag == "gold")
        {
            IncreaseGold();
            coinAudio.Play();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "multiplierIncrease")
        {
            powerupAudio.Play();
            IncreaseMultiplier();
            Destroy(collision.gameObject);
        }
    }


    void ResetGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Spawn.points = 0;
        Spawn.scoreMultiplier = 1;
        Spawn.timeForNextBlock = 0;
        Spawn.playerSpeed = 1000f;
        Spawn.numOfBlocksSpawned = 0;
    }


    void IncreaseGold() {
        gold++;
    }
    void IncreaseMultiplier() {
        Spawn.scoreMultiplier *= (float)2;
    }


    void Swipe()
    {
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            startTouchPosition = Input.GetTouch(0).position;
        }
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
            currentTouchPosition = Input.GetTouch(0).position;
            Vector2 Distance = currentTouchPosition - startTouchPosition;

            if(!stopTouch) {
                // if swipe up more than 5 on Y, and the fingers stay between 3 and -3 on the X, it's up swipe. Try jumping. If player doesn't have the jump-boost, then it swipes left or right
                if(Distance.y > swipeRange && ((Distance.x < 3) | (Distance.x > -3))) {
                    // up
                    try {
                        // GameObject.Find("UI Script").GetComponent<UiScript>().playerJump();

                    } catch {
                        if(Distance.x < -.2f && ((Distance.y < 2) | (Distance.y > -2)) && transform.position.x != (float)-1.4) {
                            // left
                            transform.position += new Vector3((float)-1.4, 0, 0);
                            stopTouch = true;
                        } else if(Distance.x > .2f && ((Distance.y < 2) | (Distance.y > -2)) && transform.position.x != (float)1.4) {
                            // right
                            transform.position += new Vector3((float)1.4, 0, 0);
                            stopTouch = true;
                        }
                    }
                    stopTouch = true;
                } else if(Distance.x < -.2f && ((Distance.y < 2) | (Distance.y > -2)) && transform.position.x != (float)-1.4) {
                    // left
                    transform.position += new Vector3((float)-1.4, 0, 0);
                    stopTouch = true;
                } else if(Distance.x > .2f && ((Distance.y < 2) | (Distance.y > -2)) && transform.position.x != (float)1.4) {
                    // right
                    transform.position += new Vector3((float)1.4, 0, 0);
                    stopTouch = true;
                }
                // if(Distance.y < -swipeRange) {
                //     // down (shrink)
                //     stopTouch = true;
                // }
            }
        }

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
            stopTouch = false;
            endTouchPosition = Input.GetTouch(0).position;
            Vector2 Distance = endTouchPosition - startTouchPosition;
            if(Mathf.Abs(Distance.x) < tapRange && Mathf.Abs(Distance.y) < tapRange) {
                //tap
            }
        }
    }


    IEnumerator Jumpy()
    {
        // move it up, wait 1.15 seconds, then move it back down (creating the illusion of a jump)
        transform.position += new Vector3(0, 1, 0);

        yield return new WaitForSeconds(1.15f);

        transform.position += new Vector3(0, -1, 0);
    }
    
    public void physicallyJump() {
        // Jump
        StartCoroutine(Jumpy());
    }
}
