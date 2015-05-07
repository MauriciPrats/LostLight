using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OutlineChanging : MonoBehaviour {

	private Color outlineColor;

	private List<Material> materialsList;

	public void setOutlineColor(Color color){
		foreach(Material material in materialsList){
			if(material.HasProperty("_OutlineColor")){
				material.SetColor("_OutlineColor",color);
				material.SetColor("_TintColor",color);
			}
		}
	}

	void Start(){
		materialsList = new List<Material> (0);
		Renderer rend = GetComponent<Renderer> ();
		if (rend != null) {
			Material[] materials = rend.materials;
			materialsList.AddRange (materials);
		}
		foreach(Renderer renderer in GetComponentsInChildren<Renderer>()){
			materialsList.AddRange(renderer.materials);
		}
	}
}
