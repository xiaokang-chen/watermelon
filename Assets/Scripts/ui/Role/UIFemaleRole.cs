public class UIFemaleRole : SpineRoleComp
{
    private int feelingsDelta = 10;
    protected new void Awake()
    {
        spineAnimDict = new()
        {
            { TextEmoticonsType.Happy, "2.高兴地说"},
            { TextEmoticonsType.Calm, "3.正常地说"},
            { TextEmoticonsType.Sad, "4.悲伤地说"},
            { TextEmoticonsType.Angry, "5.愤怒地说"},
            { TextEmoticonsType.Surprise, "7.惊讶"},
            { TextEmoticonsType.ActingCute, "6.卖萌"},
            { TextEmoticonsType.Confused, "8.疑惑"},
            { TextEmoticonsType.Idle, "1.idle"}
        };
        base.Awake();
    }

    protected new void OnPromptSuccess(PromptResponse response)
    {
        base.OnPromptSuccess(response);
        if (TextEmoticonsType.Happy == textEmoticonsType)
        {
            DataStorageMgr.Instance.Feelings += feelingsDelta;
        }
        else if (TextEmoticonsType.Angry == textEmoticonsType || TextEmoticonsType.Sad == textEmoticonsType)
        {
            if (feelingsDelta < DataStorageMgr.Instance.Feelings)
            {
                DataStorageMgr.Instance.Feelings -= feelingsDelta;
            }
            else
            {
                DataStorageMgr.Instance.Feelings = 0;
            }
        }
    }
}