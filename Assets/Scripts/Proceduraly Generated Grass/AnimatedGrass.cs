﻿using UnityEngine;
using System.Collections;

public class AnimatedGrass : MonoBehaviour {
	public int columns = 2;
	public int rows = 2;
	public float framesPerSecond = 10f;
	
	//the current frame to display
	private int index = 0;
	private int direction = 1;
	void Start()
	{
		StartCoroutine(updateTiling());
		
		//set the tile size of the texture (in UV units), based on the rows and columns
		Vector2 size = new Vector2(1f / columns, 1f / rows);
		GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex", size);
	}
	
	private IEnumerator updateTiling()
	{
		while (true)
		{
			//move to the next index
			index+=direction;
			if (index >= (rows * columns) || index < 0){
				direction *=-1;
				index+=direction;
			}
			
			//split into x and y indexes
			Vector2 offset = new Vector2((float)index / columns - (index / columns), //x index
			                             (index / columns) / (float)rows);          //y index
			
			GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);

			yield return new WaitForSeconds(1f / framesPerSecond);

		}
		
	}
}

