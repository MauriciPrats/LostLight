using UnityEngine;
using System.Collections;


public class AnimationEventBroadcast : MonoBehaviour {

//In charge of informing subscribed attacks of animation events
	
	ComboAttack subscriber;
	public void subscribe (ComboAttack attack) {
	if (subscriber) 
	{}
	else {
		subscriber = attack;
		}
	}
	
	public void unsubscribe (ComboAttack attack) {
		subscriber = null;
	}

	public void informAttack(string msg) {
	print ("recived msg" + msg);
		if (subscriber) {
			switch (msg) {
				case "start": 
				subscriber.enableHitbox();
				break;
				case "end":
				subscriber.dissableHitbox();
				break;
				case "done":
				subscriber.endCombo();
				break;
				case "combo":
				subscriber.allowChaining();
				break;
				default: 
			
			break;
			
			
			}
		}
	}

}
