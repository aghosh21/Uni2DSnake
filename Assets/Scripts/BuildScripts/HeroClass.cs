using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroClass : MonoBehaviour {
	
	public string Name;
	public int Heart;
	public int Sword;
	public int Shield;
	public bool isHero;
	public Texture2D Icon;



	public void SetUpAvatar(string avatar, int life, int attack, int defense, bool hero, Texture2D icon) {

		Heart = life;
		Sword = attack;
		Shield = defense;
		isHero = hero;
		Icon = icon;
	}

	public void SetUpAvatar(int ID) {

		Name = ID.ToString ();
		Heart = randomInt ();
		Sword = randomInt ();
		Shield = randomInt ();
		isHero = (ID < 17);
		Icon = setUpIcon (ID);
	}

	
	private int randomInt() {
		int randomvalue = (int) Random.Range (0, 50);
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
