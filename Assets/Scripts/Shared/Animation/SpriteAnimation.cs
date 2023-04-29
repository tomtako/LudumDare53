using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class AnimationListItem
{
    public string animationName;
    public SpriteAnimationData animation;
    public SpriteAnimationAsset file;
}

[DisallowMultipleComponent]
public class SpriteAnimation : MonoBehaviour
{
    public bool playing;
    public bool paused;
    public float timer;
    public int animIdx = -1;
    public int currentIdx;
    public float speedRatio = 1;
    public int playFrom;
    public bool overrideTimeScale;
    public float timeScaleOverride;

    public new SpriteRenderer renderer
    {
        get
        {
            if (_renderer == null)
                _renderer = GetComponent<SpriteRenderer>();
            return _renderer;
        }
    }

    public Image image
    {
        get
        {
            if (_image == null)
                _image = GetComponent<Image>();

            return _image;
        }
    }

    public bool IsDone => CurrentFrame >= CurrentFrameCount - 1 || !playing;

    public List<AnimationListItem> list;
    public SpriteAnimationAsset animationAsset;
    public SpriteAnimationTimeMode mode;
    public Dictionary<string, SpriteAnimationData> animationsByName;

    public event Action AnimationComplete;
    public event Action<int> AnimationFrameUpdate;

    private SpriteRenderer _renderer;
    private Image _image;
    private SpriteAnimationData currentAnimation;

    private Action _animComplete;
    private Action<int> _animFrameUpdate;

    public int AnimationCount
    {
        get { return list.Count; }
    }

    protected SpriteAnimationData CurrentAnimation
    {
        get { return currentAnimation; }
    }

    public string CurrentAnimationName
    {
        get { return CurrentAnimation != null ? CurrentAnimation.name : string.Empty; }
    }

    public int CurrentAnimationIdx
    {
        get { return animIdx; }
    }

    public int CurrentFrame
    {
        get { return currentIdx; }
        set { currentIdx = value; }
    }

    public int CurrentFrameCount
    {
        get
        {
            if (animationsByName.ContainsKey(CurrentAnimationName))
            {
                return animationsByName[CurrentAnimationName].frameDatas.Count;
            }

            return 0;
        }
    }

    public int GetFrameCount()
    {
        return CurrentFrameCount;
    }

    void OnEnable()
    {
        UpdateAnimations();

        if (!playing && animIdx >= 0 && list.Count > 0)
        {
            if (animIdx >= list.Count || animIdx < 0)
            {
                animIdx = list.Count - 1;
            }

            Play(list[animIdx].animationName, playFrom);
        }
    }

    private void OnDisable()
    {
        playing = false;
    }

    void Update()
    {
        if (!playing || paused || CurrentAnimation == null)
        {
            return;
        }

        if (overrideTimeScale)
        {
            timer += Time.deltaTime * timeScaleOverride;
        }
        else
        {
            timer += Time.deltaTime * speedRatio;
        }

        if (timer >= CurrentAnimation.frameDatas[CurrentFrame].time)
        {
            timer = 0;
            CurrentFrame++;

            if (CurrentFrame >= CurrentAnimation.frameDatas.Count)
            {
                if (_animComplete != null)
                {
                    AnimationComplete?.Invoke();
                }

                try
                {
                    if (CurrentFrame >= CurrentAnimation.frameDatas.Count)
                    {
                        switch (CurrentAnimation.loop)
                        {
                            case SpriteAnimationLoopMode.NOLOOP:
                                Stop();
                                return;
                            case SpriteAnimationLoopMode.LOOPTOSTART:
                                CurrentFrame = 0;
                                break;
                            case SpriteAnimationLoopMode.LOOPTOFRAME:
                                CurrentFrame = CurrentAnimation.frameToLoop;
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                   Debug.LogError($"{gameObject.name} is NOT WORKING!!!");
                }
            }

            SetCurrentFrame();

            if (_animFrameUpdate != null)
            {
                AnimationFrameUpdate?.Invoke(CurrentFrame);
            }
        }
    }

    public float GetCurrentAnimationLength()
    {
        float length = 0f;

        if (animationsByName.ContainsKey(CurrentAnimationName))
        {
            for (int i = 0; i < CurrentAnimation.frameDatas.Count; i++)
            {
                length += CurrentAnimation.frameDatas[i].time;
            }
        }

        return length;
    }

    public void SetCurrentAnimation(string aName, bool editor, int frame = 0)
    {
        if (editor)
        {
            UpdateAnimations();
        }

        currentIdx = frame;
        currentAnimation = GetAnimationData(aName);
    }


    public bool Play(string animName, int startFrame = 0, bool showDebug = false)
    {
        if (!this)
        {
           // Debug.LogWarning("null script reference");
            return false;
        }

        if (gameObject == null)
        {
            if (showDebug)Debug.LogWarning("gameobject is null");
            return false;
        }

        if (string.IsNullOrEmpty(animName))
        {
            if (showDebug)Debug.LogWarning($"animation name is not valid! '{animName}'");
            return false;
        }

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        if (animationsByName == null || list == null)
        {
            UpdateAnimations();
        }

        RemoveCallbacks();

        SetCurrentAnimation(animName, false, startFrame);

        if (CurrentAnimation != null && CurrentAnimation.Valid())
        {
            PlayCurrentAnim();
            return true;
        }

        if (showDebug) Debug.LogWarningFormat("current animation is null or not valid {0} {1}", name, animName);
        return false;
    }

    public void Play(string animName, Action callBack)
    {
        Play(animName);

        _animComplete = callBack;
        AnimationComplete += _animComplete;
    }

    public void Play(string animName, Action<int> callBack)
    {
        Play(animName);

        _animFrameUpdate = callBack;
        AnimationFrameUpdate += _animFrameUpdate;
    }

    public void Stop()
    {
        playing = false;
        RemoveCallbacks();
    }

    public void SetCurrentAnimation(int idx)
    {
        animIdx = idx;
        currentIdx = 0;
    }

    public SpriteAnimationData GetAnimationData(string animName)
    {
        if (animationsByName == null)
        {
            UpdateAnimations();
        }

        return animationsByName.ContainsKey(animName) && animationsByName[animName].Valid() ? animationsByName[animName] : null;
    }

    protected SpriteAnimationData GetAnimationData(int idx)
    {
        if (list == null)
        {
            UpdateAnimations();
        }

        return list.Count > idx && list[idx].animation.Valid() ? list[idx].animation : null;
    }

    protected void PlayCurrentAnim()
    {
        timer = 0;
        playing = true;
        SetCurrentFrame();
    }

    public void SetFrame(string animName, int frame)
    {
        if (animationsByName == null || list == null)
        {
            UpdateAnimations();
        }

        SetCurrentAnimation(animName, false, frame);

        if (CurrentAnimation != null && CurrentAnimation.Valid())
        {
            PlayCurrentAnim();
            playing = false;
        }
    }

    protected void SetCurrentFrame()
    {
        // if (CurrentAnimation?.frameDatas == null)
        // {
        //     return;
        // }

        if (currentIdx >= CurrentAnimation.frameDatas.Count)
        {
            Debug.LogWarningFormat("trying to {0} at frame {1} but only {2} frame(s) on {3}", CurrentAnimation.name, currentIdx, CurrentAnimation.frameDatas.Count, transform.parent == null ? name : transform.parent.name);
            return;
        }

        if (renderer != null)
        {
            renderer.sprite = CurrentAnimation.frameDatas[currentIdx].sprite;
        }
        else if (image != null)
        {
            image.sprite = CurrentAnimation.frameDatas[currentIdx].sprite;
        }
    }

    public void UpdateAnimations()
    {
        if (animationsByName == null)
        {
            animationsByName = new Dictionary<string, SpriteAnimationData>();
        }

        if (list == null)
        {
            list = new List<AnimationListItem>();
        }

        animationsByName.Clear();
        list.Clear();

        if (animationAsset != null && animationAsset.animations != null && animationAsset.animations.Count > 0)
        {
            for (int i = 0; i < animationAsset.animations.Count; i++)
            {
                var data = animationAsset.animations[i];

                animationsByName[data.name] = data;

                AnimationListItem item = new AnimationListItem
                {
                    animation = data,
                    animationName = data.name,
                    file = animationAsset
                };

                list.Add(item);
            }
        }

    }

    public void SortAnimations()
    {
        if (list != null && list.Count > 0)
        {
            list = list.OrderBy(x => x.animationName).ToList();
        }
    }

    void Reset()
    {
        UpdateAnimations();
    }

    void RemoveCallbacks()
    {
        if (_animComplete != null)
        {
            AnimationComplete -= _animComplete;
            _animComplete = null;
        }

        if (_animFrameUpdate != null)
        {
            AnimationFrameUpdate -= _animFrameUpdate;
            _animFrameUpdate = null;
        }
    }

    public bool HasAnimation(string anim)
    {
        return !string.IsNullOrEmpty(anim) && animationsByName.ContainsKey(anim);
    }


    private void OnDestroy()
    {
        RemoveCallbacks();
    }
}
