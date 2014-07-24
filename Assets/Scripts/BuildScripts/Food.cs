using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Food : MonoBehaviour {
	// public fields
	public Rect foodPos = new Rect(0,0,20,20);
	
	// private fields
	private static Food instance = null;
	public static int[] initXPos = new int[] {22,42,62,82,102,122,142,162,182,202,222,242,262,282,302,322,342,362,382,402,422,442,462,482,502,522,542,562,582,602,622,642,662,682,702,722,742,762,782,802,822,842,862,882,902,922,942,962,982};
	public static int[] initYPos = new int[] {94,114,134,154,174,194,214,234,254,274,294,314,334,354,374,394,414,434,454,474,494,514,534,554,574,594,614,634,654};

	public Texture2D foodTexture;
	public List<Texture2D> SpriteList = new List<Texture2D>();
	public Texture2D oldFoodTexture;
	public List<HeroClass> FoodList = new List<HeroClass>();
	public Dictionary<int, HeroClass> NewFoodList = new Dictionary<int, HeroClass> ();
	public HeroClass newHero = new HeroClass ();

	public static Food Instance

	{
		get 
		{
			if (instance == null)
			{
				instance = new GameObject("Food").AddComponent<Food>();
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
		print("Food Instance destroyed");
		
		instance = null;
	}



	public void LoadSprites() {			
		for (int i = 1; i < 78; i++) {
			setupnewfood(i);
			SpriteList.Add(newHero.Icon);
			FoodList.Add( newHero);
		}

	}

	private void setupnewfood(int ID) {
		newHero.Name = ID.ToString ();
		newHero.Heart = Random.Range (0, 50);
		newHero.Sword = Random.Range (0, 50);
		newHero.Shield = Random.Range (0, 50);
		newHero.isHero = (ID < 17);
		newHero.Icon = setUpIcon (ID);
	}

	private Texture2D setUpIcon(int ID) {
		Texture2D temp = Resources.Load ("" + ID) as Texture2D;
		return temp;
	}

	private Texture2D getTexture(HeroClass avatar) {
		return avatar.Icon;

	}

	public void UpdateFood()
	{
		print("Food updated");
		// play our food pickup sound
		// initialize pixelInset random positions
		int ranX = Random.Range(0, initXPos.Length);
		int ranY = Random.Range(0, initYPos.Length);

		// assign a random position to the pixelInset
		foodPos = new Rect(initXPos[ranX],initYPos[ranY],20,20);

	}
	

	void OnGUI()
	{
		if (Food.Instance != null)
		{
			GUI.DrawTexture(foodPos, foodTexture);
		}
	}

	public void setFoodTexture() {
	
		foodTexture = SpriteList[(int)Random.Range(0,77)];
		oldFoodTexture = foodTexture;

	}

	public void Initialize()
	{
		print("Food initialized");

		// make sure our localScale is correct for a GUItexture
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;

		// we create a texture for our GUITexture mainTexture
		//foodTexture = TextureHelper.CreateTexture (20, 20, Color.yellow);

		LoadSprites ();
		setFoodTexture();
		// set a random position
		// initialize pixelInset random positions

		int ranX = Random.Range(0, initXPos.Length);
		int ranY = Random.Range(0, initYPos.Length);
		// assign a random position to the pixelInset

		foodPos = new Rect(initXPos[ranX],initYPos[ranY],20,20);
	}



	/*
	public HeroClass GetHeroTypeFromTexture(Texture2D avatar) {
		string answer = avatar.ToString ();
		if (answer.Contains("Bad")) {
			if (answer.Contains("Thief")) {
				return Thief;
			} else {
				return Monster;
			}
		} 
		if (answer.Contains("Wizard")) {
			return Wizard;
		} else {
			return Warrior;
		}
	}
	*/

}
