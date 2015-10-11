using UnityEngine;
using System.Collections;

public class IntroductionTrigger : MonoBehaviour 
{
	public GameManager manager;
	
	void OnTriggerEnter(Collider col)
	{
		if(col.GetComponent<CharacterController2D>() != null)
		{
			manager.StartIntroductionConversation();
			gameObject.SetActive(false);
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
	}
}
