using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Snake : MonoBehaviour
{
	// private static instance of class
	public static Snake instance = null;
	
	public Direction mState = Direction.LEFT;
	// private fields
	private List<Rect> snakePos = new List<Rect>();
	public List<Texture2D> snakeIcon = new List<Texture2D> ();
	public List<HeroClass> snakeHeros = new List<HeroClass> ();
	public int snakeLength = 1;
	private float moveDelay = 0.1f;
	private bool didCollide = false;
	public HeroClass starter = new HeroClass();
	public int Health = 0;

	// direction enum for clarification
	public enum Direction
	{
		UP,
		DOWN,
		LEFT,
		RIGHT
	}
	
	
	
	public static Snake Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject("Snake").AddComponent<Snake>();
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
		print("Snake Instance destroyed");
		
		instance = null;
	}
	
	
	void Start ()
	{
		// start our SnakeUpdate loop
		
		StartCoroutine(UpdateSnake());
	}
	
	IEnumerator UpdateSnake ()
	{
		while(true)
		{
			// handle multi key presses
			if (InputHelper.GetStandardMoveMultiInputKeys())
			{
				Debug.Log ("We are pressing multiple keys for direction");
				yield return null;
				continue;
			}
			
			if (InputHelper.GetStandardMoveUpDirection())
			{
				mState = Direction.UP;
			}
			
			if (InputHelper.GetStandardMoveLeftDirection())
			{
				mState = Direction.LEFT;
			}
			
			if (InputHelper.GetStandardMoveDownDirection())
			{
				mState = Direction.DOWN;
			}
			
			if (InputHelper.GetStandardMoveRightDirection())
			{
				mState = Direction.RIGHT;
			}

			yield return StartCoroutine(MoveSnake(mState));

			// here we check for snake collision (it can only collide with itself)
			if (SnakeCollidedWithSelf() == true)
			{
				break;
			}
			
			yield return new WaitForSeconds(moveDelay);
		}
		
		if (instance.SnakeCollidedWithSelf()) {
			yield return StartCoroutine (ScreenHelper.FlashDeathScreen (6, 0.1f, new Color (1, 0, 0, 0.5f)));
			SnakeGame.Instance.UpdateLives(-1);
			
			if (SnakeGame.Instance.gameLives == 0) {
				Application.LoadLevel("Snake");
			} else {
				Initialize();
				Start();
			}
		}
	}
	
	
	
	public IEnumerator MoveSnake(Direction moveDirection)
	{
		// define a temp List of Rects to our current snakes List of Rects
		List<Rect> tempRects = new List<Rect>();
		Rect segmentRect = Food.Instance.foodPos;
		
		// initialize
		for (int i = 0; i < snakePos.Count; i++)
		{
			tempRects.Add(snakePos[i]);
		}
		
		switch(moveDirection)
		{
		case Direction.UP:
			if (snakePos[0].y > 94)
			{
				// we can move up
				snakePos[0] = new Rect(snakePos[0].x, snakePos[0].y - 20, snakePos[0].width, snakePos[0].height);
				
				// now update the rest of our body
				UpdateMovePosition(tempRects);
				
				// check for food
				if (CheckForFood() == true)
				{
					// check for valid build segment position and add a segment
					
					// create a temporary check position (this one is below the last segment in snakePos[])
					segmentRect = CheckForValidDownPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						
						// increment our snake length
						snakeLength++;

						// decrement our moveDelay
						moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f);
						
						// give control back to our calling method
						yield break;
					}
					
					// create a temporary check position (this one is to the left the last segment in snakePos[])
					segmentRect = CheckForValidLeftPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						
						// increment our snake length
						snakeLength++;
						
						// decrement our moveDelay
						moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f);
						
						// give control back to our calling method
						yield break;
					}
					
					// create a temporary check position (this one is to the right the last segment in snakePos[])
					segmentRect = CheckForValidRightPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						
						// increment our snake length
						snakeLength++;
						
						// decrement our moveDelay
						moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f);
					}
					
					// no need to check Up, because we are pressing the Up key, we do not want a segment above us
				}
				
			} else {
				SnakeGame.Instance.UpdateLives(-1);
				snakeIcon.RemoveAt(0);
				if (SnakeGame.Instance.gameLives != 0) {
					mState = Direction.LEFT;
				} else {
					yield return StartCoroutine (ScreenHelper.FlashDeathScreen (6, 0.1f, new Color (1, 0, 0, 0.5f)));
					if (SnakeGame.Instance.gameLives == 0) {
						Application.LoadLevel("Snake");
					}
				}
			}
			break;
		case Direction.LEFT:
			if (snakePos[0].x > 22)
			{
				// we can move left
				snakePos[0] = new Rect(snakePos[0].x - 20, snakePos[0].y, snakePos[0].width, snakePos[0].height);
				
				// now update the rest of our body
				UpdateMovePosition(tempRects);
				
				// check for food
				if (CheckForFood() == true)
				{
					// check for valid build segment position and add a segment
					
					// create a temporary check position (this one is to the right the last segment in snakePos[])
					segmentRect = CheckForValidRightPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						
						// increment our snake length
						snakeLength++;
						
						// decrement our moveDelay
						moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f);
						
						// give control back to our calling method
						yield break;
					}
					
					// create a temporary check position (this one is above the last segment in snakePos[])
					segmentRect = CheckForValidUpPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						
						// increment our snake length
						snakeLength++;
						
						// decrement our moveDelay
						moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f);
						
						// give control back to our calling method
						yield break;
					}
					
					// create a temporary check position (this one is below the last segment in snakePos[])
					segmentRect = CheckForValidDownPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						
						// increment our snake length
						snakeLength++;
						
						// decrement our moveDelay
						moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f);
						
						// give control back to our calling method
						yield break;
					}
					
					// no need to check Left, because we are pressing the Left key, we do not want a segment ahead of us
				}
				
				
			}  else {
				SnakeGame.Instance.UpdateLives(-1);
				snakeIcon.RemoveAt(0);
				if (SnakeGame.Instance.gameLives != 0) {
					mState = Direction.DOWN;
				} else {
					yield return StartCoroutine (ScreenHelper.FlashDeathScreen (6, 0.1f, new Color (1, 0, 0, 0.5f)));
					if (SnakeGame.Instance.gameLives == 0) {
						Application.LoadLevel("Snake");
					}
				}
			}
			break;
		case Direction.DOWN:
			if (snakePos[0].y < 670)
			{
				// we can move down
				snakePos[0] = new Rect(snakePos[0].x, snakePos[0].y + 20, snakePos[0].width, snakePos[0].height);
				
				// now update the rest of our body
				UpdateMovePosition(tempRects);
				
				// check for food
				if (CheckForFood() == true)
				{
					// check for valid build segment position and add a segment
					
					// create a temporary check position (this one is above the last segment in snakePos[])
					segmentRect = CheckForValidUpPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						
						// increment our snake length
						snakeLength++;
						
						// decrement our moveDelay
						moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f);
						
						// give control back to our calling method
						yield break;
					}
					
					// create a temporary check position (this one is to the left the last segment in snakePos[])
					segmentRect = CheckForValidLeftPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						
						// increment our snake length
						snakeLength++;
						
						// decrement our moveDelay
						moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f);
						
						// give control back to our calling method
						yield break;
					}
					
					// create a temporary check position (this one is to the right the last segment in snakePos[])
					segmentRect = CheckForValidRightPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						
						// increment our snake length
						snakeLength++;
						
						// decrement our moveDelay
						moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f);
					}
					
					// no need to check Down, because we are pressing the Down key, we do not want a segment below us
				}
				
			} else {
				SnakeGame.Instance.UpdateLives(-1);
				snakeIcon.RemoveAt(0);
				
				if (SnakeGame.Instance.gameLives != 0) {
					mState = Direction.RIGHT;
				} else {
					yield return StartCoroutine (ScreenHelper.FlashDeathScreen (6, 0.1f, new Color (1, 0, 0, 0.5f)));
					if (SnakeGame.Instance.gameLives == 0) {
						Application.LoadLevel("Snake");
					}
				}
			}
			break;
		case Direction.RIGHT:
			if (snakePos[0].x < 982)
			{
				// we can move right
				snakePos[0] = new Rect(snakePos[0].x + 20, snakePos[0].y, snakePos[0].width, snakePos[0].height);
				
				// now update the rest of our body
				UpdateMovePosition(tempRects);
				
				// check for food
				if (CheckForFood() == true)
				{
					// check for valid build segment position and add a segment
					
					// create a temporary check position (this one is left of the last segment in snakePos[])
					segmentRect = CheckForValidLeftPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						
						// increment our snake length
						snakeLength++;
						
						// decrement our moveDelay
						moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f);
						
						// give control back to our calling method
						yield break;
					}
					
					// create a temporary check position (this one is to the left the last segment in snakePos[])
					segmentRect = CheckForValidUpPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						
						// increment our snake length
						snakeLength++;
						
						// decrement our moveDelay
						moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f);
						
						// give control back to our calling method
						yield break;
					}
					
					// create a temporary check position (this one is below the last segment in snakePos[])
					segmentRect = CheckForValidDownPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						
						// increment our snake length
						snakeLength++;
						
						// decrement our moveDelay
						moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f);
					}
					
					// no need to check Right, because we are pressing the Right key, we do not want a segment ahead of us
				}
			}  else {
				SnakeGame.Instance.UpdateLives(-1);
				snakeIcon.RemoveAt(0);
				
				if (SnakeGame.Instance.gameLives != 0) {
					mState = Direction.UP;
				} else {
					yield return StartCoroutine (ScreenHelper.FlashDeathScreen (6, 0.1f, new Color (1, 0, 0, 0.5f)));
					if (SnakeGame.Instance.gameLives == 0) {
						Application.LoadLevel("Snake");
					}
				}
			}
			break;
		}
		
		yield return null;
	}
	
	
	private void UpdateMovePosition(List<Rect> tmpRects)
	{
		// update our snakePos Rect with the tmpRect positions
		for (int i = 0; i < tmpRects.Count - 1; i++)
		{
			// exe. size of 3, assign 1,2,3 to 0,1,2 - snakePos[0] is already assigned
			snakePos[i+1] = tmpRects[i];
		}
	}
	
	
	private bool CheckForFood()
	{
		if(Food.Instance != null)
		{
			Rect foodRect = Food.Instance.foodPos;
			if (snakePos[0].Contains(new Vector2(foodRect.x,foodRect.y)))
			{
				
				Debug.Log ("We hit the food");
				
				// we re-position the food
				Food.Instance.UpdateFood();
				
				// we add to our score
				SnakeGame.Instance.UpdateScore(1);
				SnakeGame.Instance.UpdateLives(1);
				snakeLength++;
				return true;
			}
		}
			
			return false;
	}


	/*
	 * Avatar’s Sword - Enemy’s Shield = Avatar’s Damage
	 * Avatar’s Damage - Enemy’s Heart
	 * If Enemy heart is not = 0, Enemy attack using the same formula
	 * If Avatar’s Heart = 0, change to next Avatar in the line
	 * If there is no Avatar left, ends the game and start over
	 * Lowest Damage possible is 1
	 * When a Hero attack an Enemy of the same Type, the Hero’s Sword value will be multiplied by 2
	 * 
	 *
	 private bool Combat(HeroClass attacker, int attackerhealth, int currentBattle) {

		int attackerHealth = attackerhealth;
		int damage = Mathf.Max(starter.Sword - attacker.Shield,1);
		int hit = Mathf.Max(attacker.Sword - starter.Shield, 1);

		attackerHealth = attackerHealth - damage;

		if (attackerHealth <= 0) {
			Food.Instance.UpdateFood ();
			Food.Instance.setFoodTexture ();
			return false;
		} else {
			Health = Health - hit;
		}

		if (Health < 0) {
			SnakeGame.Instance.UpdateLives(-1);
			snakeIcon.RemoveAt(0);
			snakeHeros.RemoveAt(0);
			snakeLength--;
			if (snakeLength != 0) {
				starter = GetHeroTypeFromTexture(snakeIcon[0]);
				Health = starter.Heart;
			} else {
				DestroyInstance();
				Application.LoadLevel("Snake");
			}
		}

		if (starter.Heart > 0 && attackerHealth > 0) {
			return Combat(attacker, attackerHealth,currentBattle++);
		}
		return false;
	}
	*/

	private Rect CheckForValidDownPosition()
	{
		if (snakePos[snakePos.Count-1].y != 654)
		{
			return new Rect(snakePos[snakePos.Count-1].x, snakePos[snakePos.Count-1].y - 20, 20, 20);
		}
		
		return new Rect(0,0,0,0);
	}
	
	
	private Rect CheckForValidUpPosition()
	{
		if (snakePos[snakePos.Count-1].y != 94)
		{
			return new Rect(snakePos[snakePos.Count-1].x, snakePos[snakePos.Count-1].y + 20, 20, 20);
		}
		
		return new Rect(0,0,0,0);
	}
	
	
	private Rect CheckForValidLeftPosition()
	{
		if (snakePos[snakePos.Count-1].x != 22)
		{
			return new Rect(snakePos[snakePos.Count-1].x - 20, snakePos[snakePos.Count-1].y, 20, 20);
		}
		
		return new Rect(0,0,0,0);
	}
	
	
	private Rect CheckForValidRightPosition()
	{
		if (snakePos[snakePos.Count-1].x != 982)
		{
			return new Rect(snakePos[snakePos.Count-1].x + 20, snakePos[snakePos.Count-1].y, 20, 20);
		}
		
		return new Rect(0,0,0,0);
	}
	
	
	private void BuildSnakeSegment(Rect rctPos)
	{
		Texture2D temp = Food.Instance.oldFoodTexture;
		//snakeHeros.Add(Food.Instance.GetHeroTypeFromTexture(temp));
		snakeIcon.Add (temp);
		snakePos.Add(rctPos);
		snakeLength++;
	}
	
	private bool SnakeCollidedWithSelf()
	{
		didCollide = false;
		
		if (snakePos.Count <= 3)
		{
			return false;
		}
		
		for (int i = 0; i < snakePos.Count; i++)
		{
			if (i > 0)
			{
				if (snakePos[0].x == snakePos[snakePos.Count - i].x && snakePos[0].y == snakePos[snakePos.Count - i].y)
				{
					// we have collided
					didCollide = true;
					
					break;
				}
			}
		}
		
		return didCollide;
	}
	
	void OnGUI()
	{
		for (int i = 0; i < SnakeGame.Instance.gameLives; i++)
		{
			GUI.DrawTexture(snakePos[i], snakeIcon[i]);
		}
	}
	
	public void SetUpStarter() {
		starter.SetUpAvatar ("Starter", 50, 20, 15, true, Resources.Load ("Starting") as Texture2D);
		Health = starter.Heart;
	}
	/*
	public HeroClass GetHeroTypeFromTexture(Texture2D avatar) {
		string answer = avatar.ToString ();
		if (answer.Contains("Bad")) {
			if (answer.Contains("Thief")) {
				return Food.Instance.Thief;
			} else {
				return Food.Instance.Monster;
			}
		} 
		if (answer.Contains("Wizard")) {
			return Food.Instance.Wizard;
		} else {
			return Food.Instance.Warrior;
		}
	}
	*/

	public void Initialize()
	{
		
		print("Snake initialized");

		// clear our Lists
		snakePos.Clear();
		snakeIcon.Clear();

		// initialize our length to start length
		snakeLength = 1;
		
		// intialize our moveDelay
		moveDelay = 0.1f;
		
		
		// make sure our localScale is correct for a GUItexture
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;

		snakeIcon.Add (Resources.Load ("Starting") as Texture2D);
		SetUpStarter ();
		// define our snake head and tail texture
		//snakeIcon.Add(TextureHelper.CreateTexture(20, 20, Color.blue));
		
		// define our snake head and tail GUI Rect
		snakePos.Add(new Rect(Screen.width * 0.5f - 10, Screen.height * 0.5f - 10, snakeIcon[0].width, snakeIcon[0].height));
	}
}
