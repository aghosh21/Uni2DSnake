using UnityEngine;
using System.Collections;

public class SnakeGame : MonoBehaviour 
{
	// prvate fields
	private static SnakeGame instance = null;
	//private GameGUI displayGUI;
	private GUIText displayLives;
	private GUIText displayScore;
	private GUIText title;
	
	// fields
	public int gameScore = 0;
	public int gameLives = 1;
	public int scoreMultiplier = 100;
	

	public static SnakeGame Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new GameObject("SnakeGame").AddComponent<SnakeGame>();
			}
			
			return instance;
		}
	}
	

	public void OnApplicationQuit()
	{
		DestroyInstance();
	}
	

	public void DestroyInstance()
	{
		print("Snake Game Instance destroyed");
		
		instance = null;
	}
	

	public void UpdateScore(int additive)
	{
		// add to our current game score
		gameScore += additive * scoreMultiplier;
		
		// update our display
		displayScore.text = "Score " + gameScore.ToString();
	}



	public void UpdateLives(int additive)
	{
		// add to our current game score
		gameLives += additive;
		
		// clamp to 0 if lower
		gameLives = Mathf.Clamp(gameLives, 0, gameLives);

		// update our display
		displayLives.text = "Lives " + gameLives;
	}
	

	public void Initialize()
	{
		print("SnakeGame initialized");
		
		// initialize transform information
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		
		// initialize SnakeGame variables
		gameScore = 0; // no score initially
		gameLives = 1; // 1 lives to start with
		scoreMultiplier = 100; // adjusts score display		
		//Food.Instance.UpdateFood ();
		// setup our snake game border background
		GUIHelper.CreateGUITexture(new Rect(0,0,1024,768), Color.grey, "ScreenBorder", 0);
		
		// setup our snake game playing field
		GUIHelper.CreateGUITexture(new Rect(27,70,970,600), Color.black, "ScreenField", 1);
		
		// create and initialize our score GUIText
		displayScore = GUIHelper.CreateGetGUIText(new Vector2(10,758), "GameScore", "Score", 1);
		
		// update our integer score and display score
		UpdateScore(0);
		
		// create and initialize our lives GUIText
		displayLives = GUIHelper.CreateGetGUIText(new Vector2(944,758), "GameLives", "Lives", 1);

		// create and post title
		title = GUIHelper.CreateGetGUIText (new Vector2 (250, 758), "Title", "Pocket Adventure", 1, 70);

		// update our integer lives and display lives
		UpdateLives(0);
	}
}