using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Knife")]
public class Knife : ScriptableObject {

	public int id;
	public Sprite sprite;
	public Sprite shineSprite;

	public int Id { get { return id; } }
	public Sprite Sprite { get { return sprite; } }
	public Sprite ShineSprite { get { return shineSprite; } }
}
