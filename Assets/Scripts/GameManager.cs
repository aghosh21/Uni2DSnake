using UnityEngine;
using System.Collections;



public class GameManager : MonoBehaviour 
{

	void Start () 
	{

		// build our Food object
		Food.Instance.Initialize();
		
		// build our Snake object
		Snake.Instance.Initialize ();

		// build our SnakeGame object
		SnakeGame.Instance.Initialize();



	}	
}
