using Dialogue;
using Oasez.Extensions.Generics.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : GenericSingleton<GameController, GameController> {

    private SceneInformation currentSceneInformation;
    private DialogueGraph currentGraph;

    public void SetNewGraph(SceneInformation sceneInformation) {
        currentSceneInformation = sceneInformation;
        currentGraph = sceneInformation.sceneGraph;

        currentGraph.Restart();
        HandleCurrentNode();
    }

    private void OnEnable() {
        DialogueScreen.Instance.OnSkipButtonClicked.AddListener(OnDialogueSkipButtonClicked);
        DialogueScreen.Instance.OnAnswerButtonClicked.AddListener(OnDialogueAnswerButtonClicked);
    }

    private void OnDisable() {
        DialogueScreen.Instance.OnSkipButtonClicked.RemoveListener(OnDialogueSkipButtonClicked);
        DialogueScreen.Instance.OnAnswerButtonClicked.RemoveListener(OnDialogueAnswerButtonClicked);
    }

    private void OnDialogueSkipButtonClicked() {
        ((Chat)currentGraph.current).AnswerQuestion(0);
        DialogueScreen.Instance.Hide();
        HandleCurrentNode();
    }

    private void OnDialogueAnswerButtonClicked(int index) {
        Debug.Log($"You answered with {index}");
        ((Chat)currentGraph.current).AnswerQuestion(index);
        DialogueScreen.Instance.Hide();
        HandleCurrentNode();
    }

    private void OnWaitNodeEventTriggered() {
        Wait waitNode = ((Wait)currentGraph.current);
        waitNode.trigger.OnEventRaised -= OnWaitNodeEventTriggered;
        waitNode.Next();
        HandleCurrentNode();
    }

    private void HandleCurrentNode() {
        DialogueBaseNode current = currentGraph.current;

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
            case InvokeEvent invokeEventNode:
                ProcessInvokeEventNode(invokeEventNode);
                break;
            case StartStop startStopNode:
                ProcessStartStopNode(startStopNode);
                break;
            case LoadScene loadSceneNode:
                ProcessLoadSceneNode(loadSceneNode);
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
                CoroutineHelper.Delay(waitNode.Time, () => {
                    waitNode.Next();
                    HandleCurrentNode();
                });
                break;
            case WaitFor.PathEnd:
                PathView pathView = PathView.GetView(waitNode.PathGuid);

                CoroutineHelper.Delay(() => pathView.Trs.DistanceRatio >= 1, () => {
                    waitNode.Next();
                    HandleCurrentNode();
                });
                break;
            case WaitFor.EventRaise:
                waitNode.trigger.OnEventRaised += OnWaitNodeEventTriggered;
                break;
            default:
                break;
        }
    }

    private void ProcessFollowPathNode(FollowPath followPathNode) {
        Debug.Log("Follow path");
        CharacterView characterView = CharacterView.GetView(followPathNode.character);
        PathView pathView = PathView.GetView(followPathNode.pathGuid);

        if (followPathNode.useCharacterSpeed) {
            characterView.FollowPath(pathView);
        } else {
            characterView.FollowPath(pathView, followPathNode.speed);
        }

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

    private void ProcessInvokeEventNode(InvokeEvent invokeEventNode) {
        Debug.Log("Invoked event");
        invokeEventNode.trigger.Raise();
        invokeEventNode.Next();

        HandleCurrentNode();
    }

    private void ProcessStartStopNode(StartStop startStopNode) {
        Debug.Log("Start stop");
        if (startStopNode.function == StartStop.StartStopEnum.Start) {
            startStopNode.Next();
            HandleCurrentNode();
        }
    }

    private void ProcessLoadSceneNode(LoadScene loadSceneNode) {
        Debug.Log("Load scene");
        SceneManager.UnloadSceneAsync(currentSceneInformation.currentScene);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(loadSceneNode.nextScene, LoadSceneMode.Additive);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        SceneManager.SetActiveScene(scene);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}