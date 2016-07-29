﻿using UnityEngine;
using System.Collections;
using Player;
using Explosion;

public class BubbleController : MonoBehaviour {

    public Material shieldMaterial;
    public Material speedMaterial;
    public Material damageMaterial;

    public float speedBoost;
	public float dmgMultiplier;

	public GameObject explosionPrefab;
	private ExplosionDamage expScript;

    private Energy playerEnergy;
    private PlayerController playControl;
    private Renderer bubbleRenderer;
	private Collider col;
    private AudioSource audioSrc;
    private float originalSpeed;
	private float energyLastUsed;

	public float originalDmg;
    
	public bool key1Toggled;
    public bool key2Toggled;
    public bool key3Toggled;

    void Start() {
        //Get our player's energy script
        playerEnergy = GetComponentInParent<Energy>();
        playControl = GetComponentInParent<PlayerController>();
        originalSpeed = playControl.speed;
        bubbleRenderer = GetComponent<Renderer>();

        audioSrc = GetComponent<AudioSource>();
        //Start bubble invisible
        ShrinkBubble();
        key1Toggled = key2Toggled = key3Toggled = false;

		col = GetComponent<Collider> ();
		col.enabled = false;

		expScript = explosionPrefab.GetComponent<ExplosionDamage> ();
		originalDmg = expScript.maxDamage;
		//Set an initial time for the last used energy
		energyLastUsed = Time.time;
    }

    void Update() {
		if (playerEnergy.GetEnergy () != 0f) {
			CheckKeys ();
			if (key1Toggled) {
				col.enabled = true;
			} else {
				col.enabled = false;
			}
			if (key2Toggled) {
				playControl.speed = speedBoost;
			} else {
				playControl.speed = originalSpeed;
			}
			if (key3Toggled) {
				expScript.maxDamage = originalDmg * dmgMultiplier;
			} else {
				expScript.maxDamage = originalDmg;
			}

			if (key1Toggled || key2Toggled || key3Toggled) {
				if (Time.time >= energyLastUsed + 1) {
					playerEnergy.UsePlayerEnergy (0.01f);
				}
			}
		} else {
			ShrinkBubble ();
			expScript.maxDamage = originalDmg;
			playControl.speed = originalSpeed;
			col.enabled = false;
		}
    }

    private void ExpandBubble(Material bubbleMaterial) {
        transform.localScale = new Vector3(9f, 9f, 9f);
        bubbleRenderer.material = bubbleMaterial;
        audioSrc.Play();
    }

    private void ShrinkBubble() {
        transform.localScale = new Vector3(0f, 0f, 0f);
        audioSrc.Stop();
    }

    private void CheckKeys() {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            if (key1Toggled)
            {
                ShrinkBubble();
                key1Toggled = false;
            }
            else
            {
                ExpandBubble(shieldMaterial);
                key1Toggled = true;
                key2Toggled = key3Toggled = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            if (key2Toggled)
            {
                ShrinkBubble();
                key2Toggled = false;
            }
            else
            {
                ExpandBubble(speedMaterial);
                key2Toggled = true;
                key1Toggled = key3Toggled = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            if (key3Toggled)
            {
                ShrinkBubble();
                key3Toggled = false;
            }
            else
            {
                ExpandBubble(damageMaterial);
                key3Toggled = true;
                key2Toggled = key1Toggled = false;
            }
        }
    }
}
