using UnityEngine;
using System.Collections;

public class CreateHero : HeroClass {

	public HeroClass avatar = new HeroClass ();

	public CreateHero() {

		int ID = Random.Range(1, 77);
		avatar.Name = ID.ToString ();
		avatar.Heart = randomInt ();
		avatar.Sword = randomInt ();
		avatar.Shield =randomInt ();
		avatar.isHero = setUpisHero (ID);
		avatar.Icon = setUpIcon (ID);

	}
	
	public HeroClass returnHero() {
		return avatar;
	}

	private int randomInt() {
		int randomvalue = Random.Range(0, 50);
		return randomvalue;
	}
	
	
	
	private bool setUpisHero(int ID) {
		if (ID < 17) {
			return true;
		} else {
			return false;
		}
	}
	
	private Texture2D setUpIcon(int ID) {
		Texture2D temp = Resources.Load ("" + ID) as Texture2D;
		return temp;
	}

}
