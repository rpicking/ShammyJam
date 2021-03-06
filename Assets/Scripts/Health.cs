﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    public RectTransform healthTransform;
    public int player;
    public float cachedY;
    public float minXValue;
    public float maxXValue;
    public int currentHealth;
    public int maxHealth;
    public Image visualHealth;
    float heartcount;
    GameObject heart;
	//bool hit = false;

	// Use this for initialization
	void Start () {
        cachedY = healthTransform.position.y;
        if (player == 2) {
            maxXValue = healthTransform.position.x;
            minXValue = healthTransform.position.x + healthTransform.rect.width*1.8f;
        }
        else {
            maxXValue = healthTransform.position.x;
            minXValue = healthTransform.position.x - healthTransform.rect.width*1.8f;
        }
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        heart = GameObject.Find("Heartbeat");

        
        if ((currentHealth <= (float)maxHealth / 2) && !heart.GetComponent<AudioSource>().isPlaying) {
            heart.GetComponent<AudioSource>().Play();
            heartcount = (float)currentHealth / ((float)maxHealth / 2); // percent of last 50% health
            heart.GetComponent<AudioSource>().volume =  1.0f - heartcount;
            heart.GetComponent<AudioSource>().pitch = 3.0f - (2 * heartcount);
        }
        else if ((currentHealth <= (float)maxHealth / 2) && heart.GetComponent<AudioSource>().isPlaying) {
            heartcount = (float)currentHealth / ((float)maxHealth / 2); // percent of last 50% health
            heart.GetComponent<AudioSource>().volume = 1.0f - heartcount;
            heart.GetComponent<AudioSource>().pitch = 3.0f - (2 * heartcount);
        }

		if (currentHealth <= 0) {
			Time.timeScale = 0;
			GameObject.Find("Lucian").GetComponent<Steve>().enabled = false;
			GameObject.Find("Bob").GetComponent<Bob>().enabled = false;
			if (Input.GetButtonDown("Submit")) {
				Application.LoadLevel(Application.loadedLevel);
				Time.timeScale = 1;
				GameObject.Find("Lucian").GetComponent<Steve>().enabled = true;
				GameObject.Find("Bob").GetComponent<Bob>().enabled = true;
			}
		}
        healthBar();
		//print ("min" + minXValue);

    }

    private void healthBar()
    {
        if (currentHealth < 0) {
            currentHealth = 0;
        }

        float currentXValue = mapValues(currentHealth, maxHealth, minXValue, maxXValue);
		//print (currentXValue);
        healthTransform.position = new Vector3(currentXValue, cachedY);

        if (currentHealth > maxHealth / 2) {
            visualHealth.color = new Color32((byte)mapColor(currentHealth, maxHealth, true), 255, 0, 255);
        }
        else {
            visualHealth.color = new Color32(255, (byte)mapColor(currentHealth, maxHealth, false), 0, 255);
        }
    }

    private float mapValues(float currentHealth, float maxHealth, float outMin, float outMax)
    {
        if (player == 2) {
            return outMin - ((currentHealth / maxHealth) * (outMin - outMax));
        }
        return (currentHealth / maxHealth) * (outMax + Mathf.Abs(outMin)) + outMin;
    }

    private float mapColor(float currentHealth, float maxHealth, bool half)
    {
        if (!half) {
            return (currentHealth / maxHealth) * 255;
        }
        return 255 - ((currentHealth / maxHealth) * 255);
    }

    public void changeHealth(int change) {
		//hit = true;
        this.gameObject.GetComponent<Animator>().SetTrigger("hit");
        currentHealth += change;
        if (heart.GetComponent<AudioSource>().isPlaying) {
            if (currentHealth > (maxHealth / 2)) {
                heart.GetComponent<AudioSource>().Stop();
            }
        }
    }
}
