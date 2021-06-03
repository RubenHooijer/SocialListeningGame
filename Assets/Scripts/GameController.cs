using Cinemachine;
using DG.Tweening;
using Dialogue;
using FMOD.Studio;
using FMODUnity;
using Oasez.Extensions;
using Oasez.Extensions.Generics.Singleton;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : GenericSingleton<GameController, GameController> {

    private SceneInformation currentSceneInformation;
    private DialogueGraph currentGraph;

    private EventInstance musicEvent;
    private StudioListener myListener;

    private void Start() {
        if (!gameObject.HasComponent<StudioListener>()) {
            myListener = gameObject.AddComponent<StudioListener>();
            myListener.attenuationObject = gameObject;
        } else {
            myListener = gameObject.GetComponent<StudioListener>();
        }
        Debug.Log(myListener);
    }

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
            case StringEvent stringEventNode:
                ProcessInvokeStringEventNode(stringEventNode);
                break;
            case CharacterIntStringEvent characterIntStringEventNode:
                ProcessInvokeCharacterIntStringEventNode(characterIntStringEventNode);
                break;
            case Dialogue.Camera cameraNode:
                ProcessCameraNode(cameraNode);
                break;
            case StartStop startStopNode:
                ProcessStartStopNode(startStopNode);
                break;
            case LoadScene loadSceneNode:
                ProcessLoadSceneNode(loadSceneNode);
                break;
            case PlaySound playSound:
                ProcessPlaySound(playSound);
                break;
            case PlayMusic playMusic:
                ProcessPlayMusic(playMusic);
                break;
            default:
                Debug.LogWarning($"<color=orange>{current.GetType()} type was not defined.</color>");
                break;
        }
    }

    private void ProcessChatNode(Chat chatNode) {
        Debug.Log("Chat");
        chatNode.text.GetLocalizedStringAsync().Completed += x => OnDialogueLoaded(x, chatNode);
    }

    private void OnDialogueLoaded(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<string> obj, Chat chat) {
        if (obj.IsDone) {
            DialogueScreen.Instance.ShowSpeech(chat);
            if (string.IsNullOrEmpty(chat.sound) == false) {
                EventInstance soundInstance = RuntimeManager.CreateInstance(chat.sound);
                soundInstance.setProperty(EVENT_PROPERTY.MAXIMUM_DISTANCE, 1000);
                soundInstance.set3DAttributes(transform.To3DAttributes());
                soundInstance.start();
            }
        } else {
            Debug.Log("String was not done loading");
        }
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


    private void ProcessInvokeCharacterIntStringEventNode(CharacterIntStringEvent eventNode) {
        Debug.Log("Invoke CIS event");
        eventNode.trigger.Raise(eventNode.character, eventNode.number, eventNode.guid);
        eventNode.Next();

        HandleCurrentNode();
    }

    private void ProcessInvokeStringEventNode(StringEvent stringEventNode) {
        Debug.Log("String event");
        stringEventNode.trigger.Raise(stringEventNode.data);
        stringEventNode.Next();

        HandleCurrentNode();
    }

    private void ProcessCameraNode(Dialogue.Camera cameraNode) {
        Debug.Log("Camera");
        CinemachineBrain cinemachineBrain = UnityEngine.Camera.main.GetComponent<CinemachineBrain>(); 
        ICinemachineCamera cinemachineCamera = cinemachineBrain.ActiveVirtualCamera;
        CinemachineVirtualCamera virtualCamera = cinemachineCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();

        switch (cameraNode.action) {
            case Dialogue.Camera.Action.LookAt:
                cinemachineCamera.LookAt = StaticView.GetView(cameraNode.lookAtGuid).transform;
                break;
            case Dialogue.Camera.Action.TrackingOffset:
                CinemachineComposer cinemachineComposer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
                DOTween.To(() => cinemachineComposer.m_TrackedObjectOffset,
                    x => cinemachineComposer.m_TrackedObjectOffset = x,
                    cameraNode.offset,
                    cameraNode.duration);
                break;
            case Dialogue.Camera.Action.PositionOffset:
                CinemachineTrackedDolly cinemachineTrackDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
                DOTween.To(() => cinemachineTrackDolly.m_AutoDolly.m_PositionOffset,
                    x => cinemachineTrackDolly.m_AutoDolly.m_PositionOffset = x,
                    cameraNode.positionOffset,
                    cameraNode.duration);
                break;
            case Dialogue.Camera.Action.FollowOffset:
                CinemachineTransposer cinemachineTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
                DOTween.To(() => cinemachineTransposer.m_FollowOffset,
                    x => cinemachineTransposer.m_FollowOffset = x,
                    cameraNode.offset,
                    cameraNode.duration);
                break;
        }
        cameraNode.Next();

        HandleCurrentNode();
    }

    private void ProcessPlaySound(PlaySound playSoundNode) {
        Debug.Log("Play Sound");
        StudioListener activeListener = GetActiveListener();
        if (activeListener != null) {
            RuntimeManager.PlayOneShot(playSoundNode.sound, activeListener.transform.position);
        } else {
            Debug.Log("No Listener found");
        }

        playSoundNode.Next();
        HandleCurrentNode();
    }

    private void ProcessPlayMusic(PlayMusic playMusicNode) {
        Debug.Log("Play Music");
        //if (musicEvent.isValid()) {
        //    musicEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //}
        //musicEvent = RuntimeManager.CreateInstance(playMusicNode.music);
        //musicEvent.start();
        FadeMusic(playMusicNode.music);

        playMusicNode.Next();
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

    private void FadeMusic(string newMusicPath) {
        EventInstance newMusicEvent = RuntimeManager.CreateInstance(newMusicPath);
        newMusicEvent.start();

        DOTween.To(
            () => {
                newMusicEvent.getVolume(out float volume);
                return volume;
            },
            x => newMusicEvent.setVolume(x),
            1,
            10).SetEase(Ease.InOutSine)
            .ChangeStartValue(0)
            .OnComplete(() => musicEvent = newMusicEvent);

        DOTween.To(
            () => {
                musicEvent.getVolume(out float volume);
                return volume;
            },
            x => musicEvent.setVolume(x),
            0,
            9f).SetEase(Ease.InOutSine);
    }

    private StudioListener GetActiveListener() {
        return myListener;
    }

}