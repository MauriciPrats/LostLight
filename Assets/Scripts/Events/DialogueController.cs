using UnityEngine;
using System.Collections;

public class DialogueController : MonoBehaviour {
	
	public Vector3 positionOffset;

	public void createNewDialogue(string text,float timeToLast,bool bouncing,bool fadeOut){
		GameObject newDialogue = GameManager.dialogueManager.getDialogue ();
		newDialogue.transform.parent = transform;
		newDialogue.transform.localPosition =  positionOffset;
		newDialogue.transform.parent = null;

		newDialogue.GetComponent<Dialogue> ().initialize (text, gameObject, timeToLast,bouncing,fadeOut);

		newDialogue.transform.up = transform.up;
	}

	public void createNewExpression(string text,float timeToLast,bool bouncing,bool fadeOut){
		GameObject newExpression = GameManager.dialogueManager.getExpression ();
		newExpression.transform.parent = transform;
		newExpression.transform.localPosition =  positionOffset;
		newExpression.transform.parent = null;
		
		newExpression.GetComponent<Expression> ().initialize (text, gameObject, timeToLast,bouncing,fadeOut);
		
		newExpression.transform.up = transform.up;
	}

	void Update(){
		if(Input.GetKeyUp(KeyCode.V)){

		}
	}

}
