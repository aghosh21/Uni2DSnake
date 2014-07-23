using UnityEngine;
using System.Collections;

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
	

	// Update is called once per frame
	void Update () {
	
	}
}
