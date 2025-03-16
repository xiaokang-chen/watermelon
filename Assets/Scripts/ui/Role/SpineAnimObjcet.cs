using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;


/*public class SpineAnimObjcet
{
    public TextEmoticonsType emoticonsType;
    public string animName;
}*/


public class SpineRoleComp : MonoBehaviour
{
    public AudioSource bgAudioSource;
    public SkeletonGraphic spineAnimation;
    public Text aiText;
    public InputField inputField;
    public GameObject uiLoading;

    public string wordModelAppId = "4b432593e6804206bbeb1a943d08a478";
    public string ttsAppKey = "bPaLYkY6kg0fP0ki";

    public delegate void DialogueCompleteDelegate();

    public event DialogueCompleteDelegate DialogueTTSCompete, DialogueCompete;

    public delegate void PromptCompleteDelegate(PromptOut output);

    public event PromptCompleteDelegate PromptComplete;

    // 卖萌检测间隔
    public int actingCuteAnimationCheckInterval = 10;

    protected Dictionary<TextEmoticonsType, string> spineAnimDict = new(){};

    protected List<string> needListenSpineEventAnimationNames;

    protected long spineIdleStartTimestamp = 0;

    protected TextEmoticonsType textEmoticonsType;

    protected void Awake()
    {
        needListenSpineEventAnimationNames = new()
        {
            spineAnimDict[TextEmoticonsType.ActingCute],
            spineAnimDict[TextEmoticonsType.Confused],
            spineAnimDict[TextEmoticonsType.Surprise],
        };
        spineAnimation.AnimationState.Complete += OnSpineAnimationCompleteEvent;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        spineIdleStartTimestamp = DateTimeUtils.GetTimestampToMilliseconds();
        uiLoading.SetActive(false);
        aiText.text = "";
        var com = inputField.GetComponent<WechatInputField>();
#if UNITY_WEBGL
        if(Application.platform == RuntimePlatform.WebGLPlayer && WeChatDetector.IsWeChat())
        {
            com.enabled = true;
        }
        else { 
            com.enabled = false;
        }
#else
        com.enabled = false;
#endif
    }

    public void OnEditComplete()
    {
        var inputText = inputField.text;
        Debug.Log("OnEditComplete: " + inputText);
        if (0 != inputText.CompareTo(aiText.text))
        {
            uiLoading.SetActive(true);
            RequestPrompt(inputField.text);
        }
    }

    protected void OnPromptSuccess(PromptResponse response)
    {
        PromptComplete?.Invoke(response.output);
        inputField.enabled = false;
        textEmoticonsType = TextEmoticons.GetEmoticons(response.output.text);
        var word = TextEmoticons.TrimTextEmoticons(response.output.text);
        aiText.text = word;
        var req = new TTSRequest(ttsAppKey, word);
        /*{
            formmat = TTSAudioFormmat.WAV
        };*/
        TTSService.Instance.PostTTS(req, OnTTSSuccess, OnPromptError);
    }

    protected void OnPromptError(string error)
    {
        inputField.enabled = true;
        uiLoading.SetActive(false);
    }


    protected void RequestPrompt(string prompt)
    {
        HttpService.Instance.PostPrompt(new PromptRequest(wordModelAppId, new PromptInput(prompt)), OnPromptSuccess, OnPromptError);
    }


    protected void OnTTSSuccess(byte[] data)
    {
        DialogueTTSCompete?.Invoke();
        uiLoading.SetActive(false);
        var type = textEmoticonsType;
        var anim = spineAnimDict[type];
        Debug.Log("OnTTSSuccess type: " + type + " anim " + anim);
        bool loop = true;
        if (TextEmoticonsType.Surprise == type || TextEmoticonsType.Confused == type)
        {
            loop = false;
        }
        spineAnimation.AnimationState.SetAnimation(0, anim, loop);
        AudioClip audioClip = WavUtility.ToAudioClip(data);
        PlayClip(audioClip);
    }

    protected void PlayClip(AudioClip clip)
    {
        if (!TryGetComponent<AudioSource>(out var audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = clip;
        audioSource.Play();
        StartCoroutine(MonitorAudioPlayback(clip));
    }

    protected IEnumerator MonitorAudioPlayback(AudioClip audioClip)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        while (audioSource.isPlaying)
        {
            float playbackProgress = audioSource.time / audioClip.length;

            if (playbackProgress >= 0.99f)
            {
                // Audio is almost finished playing
                Debug.Log("Audio is almost finished playing");

                // Perform actions when audio finishes playing
                var anim = spineAnimDict[TextEmoticonsType.Idle];
                var spineAnimationName = spineAnimation.AnimationState.GetCurrent(0).Animation.Name;
                if (0 != spineAnimationName.CompareTo(anim))
                {
                    spineAnimation.AnimationState.SetAnimation(0, anim, true);
                }

                inputField.enabled = true;

                DialogueCompete?.Invoke();
               
                // Stop the coroutine
                yield break;
            }

            yield return null; // Wait for next frame
        }
    }

    protected void OnPromptError(TTSResponseError error)
    {
        inputField.enabled = true;
        uiLoading.SetActive(false);
    }

    protected void OnSpineAnimationCompleteEvent(TrackEntry trackEntry)
    {
        var name = trackEntry.Animation.Name;
        if (-1 != needListenSpineEventAnimationNames.IndexOf(name))
        {
            var cuteAnimName = spineAnimDict[TextEmoticonsType.ActingCute];
            if (0 != name.CompareTo(cuteAnimName))
            {
                var anim = spineAnimDict[TextEmoticonsType.Calm];
                spineAnimation.AnimationState.SetAnimation(0, anim, true);
            }
            else
            {
                var anim = spineAnimDict[TextEmoticonsType.Idle];
                spineAnimation.AnimationState.SetAnimation(0, anim, true);
            }
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!uiLoading.activeInHierarchy)
        {
            var spineAnimationName = spineAnimation.AnimationState.GetCurrent(0).Animation.Name;
            var idleAnimationName = spineAnimDict[TextEmoticonsType.Idle];
            if (0 != spineAnimationName.CompareTo(idleAnimationName))
            {
                spineIdleStartTimestamp = DateTimeUtils.GetTimestampToMilliseconds();
            }
            else
            {
                var now = DateTimeUtils.GetTimestampToMilliseconds();
                if (actingCuteAnimationCheckInterval * 1000 < now - spineIdleStartTimestamp)
                {
                    spineIdleStartTimestamp = now;
                    if (0 == Random.Range(0, 2))
                    {
                        var anim = spineAnimDict[TextEmoticonsType.ActingCute];
                        spineAnimation.AnimationState.SetAnimation(0, anim, false);
                    }
                }
            }
        }
    }
}
