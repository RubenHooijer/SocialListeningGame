using BansheeGz.BGSpline.Components;
using Dialogue;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField] private DialogueGraph startingDialogue;

    [Button]
    public void StartScene() {
        startingDialogue.Restart();

        DialogueScreen.Instance.OnSkipButtonClicked.AddListener(OnDialogueSkipButtonClicked);
        DialogueScreen.Instance.OnAnswerButtonClicked.AddListener(OnDialogueAnswerButtonClicked);

        HandleCurrentNode();
    }

    private void OnDialogueSkipButtonClicked() {
        Debug.Log(startingDialogue.current.GetType());
        ((Chat)startingDialogue.current).AnswerQuestion(0);
        DialogueScreen.Instance.Hide();
        HandleCurrentNode();
    }

    private void OnDialogueAnswerButtonClicked(int index) {
        Debug.Log($"You answered with {index}");
        ((Chat)startingDialogue.current).AnswerQuestion(index);
        DialogueScreen.Instance.Hide();
        HandleCurrentNode();
    }

    private void HandleCurrentNode() {
        DialogueBaseNode current = startingDialogue.current;

        switch (current) {
            case Chat chat:
                ProcessChatNode(chat);
                break;
            case Wait wait:
                ProcessWaitNode(wait);
                break;
            case FollowPath followPath:
                ProcessFollowPathNode(followPath);
                break;
            case PauseCharacter pauseCharacter:
                ProcessPauseCharacterNode(pauseCharacter);
                break;
            case PlayAnimationEvent playAnimationNode:
                ProcessPlayAnimationNode(playAnimationNode);
                break;
            default:
                Debug.LogWarning($"<color=orange>{current.GetType()} type was not defined.</color>");
                break;
        }
    }

    private void ProcessChatNode(Chat chatNode) {
        Debug.Log("Chat");
        DialogueScreen.Instance.ShowSpeech(chatNode);
    }

    private void ProcessWaitNode(Wait waitNode) {
        Debug.Log("Wait");
        switch (waitNode.waitFor) {
            case WaitFor.Time:
                CoroutineHelper.Instance.StartCoroutine(() => {
                    waitNode.Next();
                    HandleCurrentNode();
                }, waitNode.Time);
                break;
            case WaitFor.PathEnd:
                PathView pathView = PathView.GetView(waitNode.PathGuid);

                CoroutineHelper.Instance.StartCoroutine(() => {
                    waitNode.Next();
                    HandleCurrentNode();
                }, () => pathView.Trs.DistanceRatio >= 1);
                break;
            default:
                break;
        }

    }

    private void ProcessFollowPathNode(FollowPath followPathNode) {
        Debug.Log("Follow path");
        CharacterView characterView = CharacterView.GetView(followPathNode.character);
        PathView pathView = PathView.GetView(followPathNode.pathGuid);

        characterView.FollowPath(pathView);

        followPathNode.Next();
        HandleCurrentNode();
    }

    private void ProcessPauseCharacterNode(PauseCharacter pauseCharacter) {
        Debug.Log("Pause path");
        CharacterView characterView = CharacterView.GetView(pauseCharacter.character);
        characterView.EnableMovement(pauseCharacter.pause);

        pauseCharacter.Next();
        HandleCurrentNode();
    }

    private void ProcessPlayAnimationNode(PlayAnimationEvent playAnimationNode) {
        Debug.Log("Play animation");
        playAnimationNode.trigger.Raise(playAnimationNode.animationTrigger);
        playAnimationNode.Next();

        HandleCurrentNode();
    }

}