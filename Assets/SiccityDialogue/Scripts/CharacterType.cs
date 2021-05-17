using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {

	[CreateAssetMenu(menuName = "Dialogue/CharacterType")]
	public class CharacterType : ScriptableObject {

		public string Name;
		public Color Color;
		public Sprite Icon;

	}

}