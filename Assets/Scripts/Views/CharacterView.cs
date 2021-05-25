using Dialogue;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour {

    private readonly static List<CharacterView> CharacterViews = new List<CharacterView>();

    [SerializeField] private CharacterType character;
    [SerializeField] private MoverComponent mover;

    public CharacterType Character => character;

    public static CharacterView GetView(CharacterType character) {
        return CharacterViews.Find(x => x.Character == character);
    }

    public void FollowPath(PathView pathView) {
        FollowPath(pathView, mover.Speed);
    }

    public void FollowPath(PathView pathView, float speed = 1) {
        mover.SetPath(pathView.Path);
        mover.SetSpeed(speed);
        mover.EnableMovement(true);
    }

    public void EnableMovement(bool isMoving) {
        mover.EnableMovement(isMoving);
    }

    private void OnEnable() {
        CharacterViews.Add(this);
    }

    private void OnDisable() {
        CharacterViews.Remove(this);
    }

}