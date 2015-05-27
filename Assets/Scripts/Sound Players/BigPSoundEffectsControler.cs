using UnityEngine;
using System.Collections;

public class BigPSoundEffectsControler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public AudioClip weaponSwing;
	public AudioClip kame;
	public AudioClip jumpCroak;
	
	private AudioSource source;
	private float lowPitchRange = .75F;
	private float highPitchRange = 1.5F;
	private float velToVol = .2F;
	private float velocityClipSplit = 10F;
	
	private float volLowRange = .5f;
	private float volHighRange = 1.0f;
	
	void Awake () {
		source = GetComponent<AudioSource>();
	}
	
	public void PlayWeaponSwing() {
		PlaySound(weaponSwing);
	}
	
	public void PlayKame() {
		PlaySound(kame);
	}
	
	public void PlayJump() {
		PlaySound(jumpCroak);
	}
	
	

	
	private void PlaySound (AudioClip playMe )
	{
		source.pitch = Random.Range (lowPitchRange,highPitchRange);
		float hitVol = Random.Range (volLowRange, volHighRange);
		source.PlayOneShot(playMe,hitVol);
	}
	
}
