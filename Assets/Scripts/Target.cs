using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Target")]
public class Target : ScriptableObject {

	public int id;
	public string name;
	public Sprite sprite;
	public Sprite shineSprite;
	public Color particleSystemColor;

	public int Id { get { return id; } }
	public string Name { get { return name; } }
	public Sprite Sprite { get { return sprite; } }
	public Sprite ShineSprite { get { return shineSprite; } }
	public Color ParticleSystemColor { get { return particleSystemColor; } }
}
