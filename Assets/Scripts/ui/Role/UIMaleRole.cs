public class UIMaleRole : SpineRoleComp
{
    protected new void Awake()
    {
        spineAnimDict = new()
        {
            { TextEmoticonsType.Calm, "talk"},
            { TextEmoticonsType.Sad, "talk_sad"},
            { TextEmoticonsType.Angry, "talk_anger"},
            { TextEmoticonsType.Idle, "idle"},

            { TextEmoticonsType.Happy, "talk"},
            { TextEmoticonsType.ActingCute, "talk"},
            { TextEmoticonsType.Confused, "talk"},
            { TextEmoticonsType.Surprise, "talk"},
        };
    }


    // Update is called once per frame
    new void Update(){}

    public void PlayTTSAnim(TextEmoticonsType emoticonsType, string word)
    {
        inputField.enabled = false;
        textEmoticonsType = emoticonsType;
        aiText.text = word;
        var req = new TTSRequest(ttsAppKey, word);
        /*{
            formmat = TTSAudioFormmat.WAV
        };*/
        TTSService.Instance.PostTTS(req, OnTTSSuccess, OnPromptError);
    }
}
