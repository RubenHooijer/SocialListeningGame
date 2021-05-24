using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using FMODUnity;

public class AnimatedWidget : MonoBehaviour, IAnimatingWidget {

    public static List<IAnimatingWidget> AnimatingWidgets = new List<IAnimatingWidget>();
    public static List<string> OtherAnimators = new List<string>();
    public static bool IsAnimating { get { return AnimatingWidgets.Count + OtherAnimators.Count > 0; } }

    public bool IsShowing { get; private set; }
    public float StartOpacity { get; set; }
    
    [Header("General")]
    [SerializeField] public bool ShowOnEnable = true;
    [SerializeField] public bool PreventShowAnimationOnFastReenable;
    [SerializeField] public bool DisableInteractions = true;
    [Space]
    [SerializeField, EventRef] private string showSound;
    [SerializeField, EventRef] private string hideSound;
    [SerializeField] public Transform TargetOverride = null;

    [Header("Show")]
    [SerializeField] public float ShowDuration = .2f;
    [SerializeField] public float ShowDelay = 0.0f;
    [SerializeField] public Ease ShowEase = Ease.OutBack;

    [Header("Hide")]
    [SerializeField] public float HideDuration = .1f;
    [SerializeField] public float HideDelay = 0.0f;
    [SerializeField] public Ease HideEase = Ease.InSine;
    [Space]
    [SerializeField] public float HideScale = 1.0f;
    [SerializeField] public float HideOpacity = 1.0f;
    [SerializeField] public float HideFill = 1.0f;
    [SerializeField] public Vector3 HideOffset = Vector3.zero;
    [SerializeField] public Vector2 HideAnchoredOffset = Vector2.zero;
    [Space]
    [SerializeField] public bool HideAnchorSizeX = false;
    [SerializeField] public bool HideAnchorSizeY = false;

    private bool isInitialized;

    private Vector3 startLocalPosition;
    private Vector2 startAnchoredPosition;
    private Image fadeImage;
    private CanvasGroup fadeCanvasGroup;
    private float startFill;
    private RectTransform sizeDeltaTransform;
    private Vector2 startSizeDelta;
    private Coroutine showFrameDelayRoutine;
    private Coroutine disableTrackRoutine;
    private bool isDisabledLongEnough = true;

	public static void AddAnimating(IAnimatingWidget animatingWidget) {
        AnimatingWidgets.AddDistinct(animatingWidget);
    }

    public static void RemoveAnimating(IAnimatingWidget animatingWidget) {
        AnimatingWidgets.Remove(animatingWidget);
    }

    public static void AddAnimating(string animationKey) {
        OtherAnimators.AddDistinct(animationKey);
    }

    public static void RemoveAnimating(string animationKey) {
        OtherAnimators.Remove(animationKey);
    }

    public void Show(bool instant = false) {
        InitializeIfNeeded();

        IsShowing = true;

        StopShowFrameDelayRoutine();
        GetTarget().DOKill();
        if (fadeImage != null) { fadeImage.DOKill(); }
        if (fadeCanvasGroup != null) { fadeCanvasGroup.DOKill(); }
        AnimatingWidgets.Remove(this);

        Vector2 targetSizeDelta = Vector2.zero;
        if (sizeDeltaTransform != null) {
            targetSizeDelta = new Vector2(HideAnchorSizeX ? startSizeDelta.x : sizeDeltaTransform.sizeDelta.x,
                                          HideAnchorSizeY ? startSizeDelta.y : sizeDeltaTransform.sizeDelta.y);
        }

        if (instant || ShowDuration <= 0.0f) {
            if (HideOffset != Vector3.zero) {
                GetTarget().localPosition = startLocalPosition;
            }
            if(HideAnchoredOffset != Vector2.zero) {
                GetRectTransform().anchoredPosition = startAnchoredPosition;
            }
            GetTarget().localScale = Vector3.one;
            if (fadeImage != null) {
                fadeImage.SetAlpha(StartOpacity);
                fadeImage.fillAmount = startFill;
            }
            if (fadeCanvasGroup != null) {
                fadeCanvasGroup.alpha = StartOpacity;
            }
            if (sizeDeltaTransform != null) {
                sizeDeltaTransform.sizeDelta = targetSizeDelta;
            }
            if (!instant) {
                if (string.IsNullOrEmpty(showSound) == false) {
                    RuntimeManager.PlayOneShot(showSound);
                }
            }
        } else {
            showFrameDelayRoutine = CoroutineHelper.DelayFrame(() => {
                if (HideOffset != Vector3.zero) {
                    GetTarget().DOLocalMove(startLocalPosition, ShowDuration)
                        .SetDelay(ShowDelay)
                        .SetEase(ShowEase);
                }
                if (HideAnchoredOffset != Vector2.zero) {
                    GetRectTransform().DOAnchorPos(startAnchoredPosition, ShowDuration)
                        .SetDelay(ShowDelay)
                        .SetEase(ShowEase);
                }
                if (fadeImage != null) {
                    if (HideOpacity != StartOpacity) {
                        fadeImage.DOFade(StartOpacity, ShowDuration)
                            .SetDelay(ShowDelay)
                            .SetEase(ShowEase);
                    }
                    if (HideFill != startFill) {
                        fadeImage.DOFillAmount(startFill, ShowDuration)
                            .SetDelay(ShowDelay)
                            .SetEase(ShowEase);
                    }
                }
                if (fadeCanvasGroup != null) {
                    if (HideOpacity != StartOpacity) {
                        fadeCanvasGroup.DOFade(StartOpacity, ShowDuration)
                            .SetDelay(ShowDelay)
                            .SetEase(ShowEase);
                    }
                }
                if (sizeDeltaTransform != null) {
                    if (HideAnchorSizeX || HideAnchorSizeY) {
                        sizeDeltaTransform.DOSizeDelta(targetSizeDelta, ShowDuration)
                            .SetDelay(ShowDelay)
                            .SetEase(ShowEase);
                    }
                }

                if (DisableInteractions) {
                    AnimatingWidgets.Add(this);
                }

                GetTarget().DOScale(1.0f, ShowDuration)
                    .SetDelay(ShowDelay)
                    .SetEase(ShowEase)
                    .OnStart(() => {
                        if (string.IsNullOrEmpty(showSound) == false) {
                            RuntimeManager.PlayOneShot(showSound);
                        }
                    })
                    .OnComplete(() => {
                        AnimatingWidgets.Remove(this);
                    });
            });
        }
    }

    public void Hide(bool instant = false) {
        InitializeIfNeeded();

        IsShowing = false;

        StopShowFrameDelayRoutine();
        GetTarget().DOKill();
        if (fadeImage != null) { fadeImage.DOKill(); }
        if (fadeCanvasGroup != null) { fadeCanvasGroup.DOKill(); }
        AnimatingWidgets.Remove(this);

        Vector2 targetSizeDelta = Vector2.zero;
        if (sizeDeltaTransform != null) {
            targetSizeDelta = new Vector2(HideAnchorSizeX ? 0.0f : sizeDeltaTransform.sizeDelta.x,
                                          HideAnchorSizeY ? 0.0f : sizeDeltaTransform.sizeDelta.y);
        }

        if (instant || HideDuration <= 0.0f) {
            if (HideOffset != Vector3.zero) {
                GetTarget().localPosition = startLocalPosition + HideOffset;
            }
            if (HideAnchoredOffset != Vector2.zero) {
                GetRectTransform().anchoredPosition = startAnchoredPosition + HideAnchoredOffset;
            }
            GetTarget().localScale = Vector3.one * HideScale;
            if (fadeImage != null) {
                fadeImage.SetAlpha(HideOpacity);
                fadeImage.fillAmount = HideFill;
            }
            if (fadeCanvasGroup != null) {
                fadeCanvasGroup.alpha = HideOpacity;
            }
            if (sizeDeltaTransform != null) {
                sizeDeltaTransform.sizeDelta = targetSizeDelta;
            }
            if (!instant) {
                if (string.IsNullOrEmpty(hideSound) == false) {
                    RuntimeManager.PlayOneShot(hideSound);
                }
            }
        } else {
            if (HideOffset != Vector3.zero) {
                GetTarget().DOLocalMove(startLocalPosition + HideOffset, HideDuration)
                .SetDelay(HideDelay)
                .SetEase(HideEase);
            }
            if(HideAnchoredOffset != Vector2.zero) {
                GetRectTransform().DOAnchorPos(startAnchoredPosition + HideAnchoredOffset, HideDuration)
                    .SetDelay(HideDelay)
                    .SetEase(HideEase);
            }
            if (fadeImage != null) {
                if (HideOpacity != StartOpacity) {
                    fadeImage.DOFade(HideOpacity, HideDuration)
                        .SetDelay(HideDelay)
                        .SetEase(HideEase);
                }
                if (HideFill != startFill) {
                    fadeImage.DOFillAmount(HideFill, HideDuration)
                        .SetDelay(HideDelay)
                        .SetEase(HideEase);
                }
            }
            if (fadeCanvasGroup != null) {
                if (HideOpacity != StartOpacity) {
                    fadeCanvasGroup.DOFade(HideOpacity, HideDuration)
                        .SetDelay(HideDelay)
                        .SetEase(HideEase);
                }
            }
            if (sizeDeltaTransform != null) {
                if (HideAnchorSizeX || HideAnchorSizeY) {
                    sizeDeltaTransform.DOSizeDelta(targetSizeDelta, HideDuration)
                        .SetDelay(HideDelay)
                        .SetEase(HideEase);
                }
            }

            if (DisableInteractions) {
                AnimatingWidgets.Add(this);
            }

            GetTarget().DOScale(HideScale, HideDuration)
                .SetDelay(HideDelay)
                .SetEase(HideEase)
                .OnStart(() => {
                    if (string.IsNullOrEmpty(hideSound) == false) {
                        RuntimeManager.PlayOneShot(hideSound);
                    }
                })
                .OnComplete(() => {
                    AnimatingWidgets.Remove(this);
                });
        }
    }

    private void Start() {
        InitializeIfNeeded();
    }

    private void InitializeIfNeeded() {
        if (isInitialized) { return; }
        isInitialized = true;

        startLocalPosition = GetTarget().localPosition;
        startAnchoredPosition = GetRectTransform().anchoredPosition;

        fadeImage = GetTarget().GetComponent<Image>();
        if (fadeImage != null) {
            StartOpacity = fadeImage.color.a;
            startFill = fadeImage.fillAmount;
        } else {
            fadeCanvasGroup = GetTarget().GetComponent<CanvasGroup>();
            if (fadeCanvasGroup != null) {
                StartOpacity = fadeCanvasGroup.alpha;
            }
        }

        sizeDeltaTransform = GetTarget().GetComponent<RectTransform>();
        if (sizeDeltaTransform != null) {
            startSizeDelta = sizeDeltaTransform.sizeDelta;
        }

        if (ShowOnEnable) {
            Hide(true);
        }
    }

    private void OnEnable() {
        if (!ShowOnEnable) { return; }
        Hide(true);

        if (PreventShowAnimationOnFastReenable && !isDisabledLongEnough) {
            Show(true);
        } else {
            Show(false);
        }
        
    }

    private void OnDestroy() {
        GetTarget().DOKill();
        RemoveAnimating(this);
        StopShowFrameDelayRoutine();
        StopDisableTrackRoutine();
    }

    private void OnDisable() {
        GetTarget().DOKill();
        RemoveAnimating(this);
        StopShowFrameDelayRoutine();

        if (PreventShowAnimationOnFastReenable) {
            isDisabledLongEnough = false;
            StopDisableTrackRoutine();
            disableTrackRoutine = CoroutineHelper.DelayFrame(() => {
                isDisabledLongEnough = true;
            });
        }
    }

    private Transform GetTarget() {
        return TargetOverride == null ? transform : TargetOverride;
    }

    private RectTransform GetRectTransform() {
        return GetComponent<RectTransform>();
    }

    private void StopShowFrameDelayRoutine() {
        if (showFrameDelayRoutine != null) {
            CoroutineHelper.Stop(showFrameDelayRoutine);
            showFrameDelayRoutine = null;
        }
    }

    private void StopDisableTrackRoutine() {
        if (disableTrackRoutine != null) {
            CoroutineHelper.Stop(disableTrackRoutine);
            disableTrackRoutine = null;
        }
    }

}