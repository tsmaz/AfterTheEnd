using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects {
	[CreateAssetMenu(fileName = "RecipeBase", menuName = "Recipes", order = 0)]
	public class RecipeBase : ScriptableObject { 
		public List<String> requiredItems;
		public String resultingItem;
	}
}