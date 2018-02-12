using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
	private static System.Random rng = new System.Random();  

	public static float CameraScale {
		get {
			return Camera.main.GetComponent<CameraFix>().pixelScale;;
		}
	}


	public static void Shuffle<T>(this IList<T> list) {  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = RandomInt(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

	public static ColorType NextRandomColor(ColorType specialColor = ColorType.None) {
		List<ColorType> shuffledColors = new List<ColorType>{ ColorType.Blue, ColorType.Green, ColorType.Red, ColorType.Teal, ColorType.Yellow };
		if (specialColor != ColorType.None) {
			shuffledColors.Add(specialColor);
		}
		shuffledColors.Shuffle();
		return shuffledColors[0];
	}

	public static int RandomInt(int max) {
		return rng.Next(max);
	}

	public static string ColorTypeAsString(ColorType c) {
		switch (c) {
		case ColorType.Blue:
			return "Blue";
		case ColorType.Green:
			return "Green";
		case ColorType.Red:
			return "Red";
		case ColorType.Teal:
			return "Teal";
		case ColorType.Yellow:
			return "Yellow";
		case ColorType.Block:
			return "Block";
		case ColorType.None:
		default:
			return "Empty";
		}
	}

	public static string CharacterTypeAsString(CharacterType c) {
		switch (c) {
		case CharacterType.Cat:
			return "Cat";
		case CharacterType.Fox:
			return "Fox";
		case CharacterType.Whale:
			return "Whale";
		case CharacterType.Lizard:
			return "Lizard";
		case CharacterType.Penguin:
			return "Penguin";
		case CharacterType.None:
		default:
			return "Empty";
		}
	}

	public static void DestroyEntity(GameObject g) {
		BaseComponent[] allComponents = g.GetComponents<BaseComponent>();
		foreach(BaseComponent c in allComponents) {
			Pool.Instance.RemoveComponent(c);
		}

		GameObject.Destroy(g);
	}

}
