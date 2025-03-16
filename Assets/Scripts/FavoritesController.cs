using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FavoritesController : MonoBehaviour
{
    public int FavoritesNum = 15;
    public Image TitleImage;
    public GameObject[] Favorites = new GameObject[20];
    private Button[] favoritesButtons = new Button[20];

    public GameObject TipsPanel;
    public TextMeshProUGUI TipsText;
    public TextMeshProUGUI TitleText;

    private string[] favoritesText = {
        "金玲做的局，金玲的手和车祸现场的太像了",
        "对不起，对不起",
        "40码的鞋",
        "这看起来像是固定什么柜子的",
        "不知道这是谁的血，但一定是个该死的混蛋...",
        "这个螺丝钉装在书柜上，刚好。",
        "那本日记...到底去哪了？",
        "妈妈在日记中对赵明轩父母的死感到非常愧疚",
        "这张收据上的日期，正好和日记中被撕去的几页相吻合。这一定不是巧合。我们必须去这家医院看看",
        "死者死于实验事故，暴露过量辐射死于----",
        "",
        "对不起，那个孩子他不应该",
        "把小雅和她的商业合同交出来",
        "赵明轩故意泄露明天科技机密，盗窃了阿尔法人技术，还对缺陷做了恶意改造",
        "阿尔法人执行语义识别有原始不稳定性，缺陷未修复增多，并且持续复制。代码后与相关记录一并被封存",
        "赵明轩邮件：没人在乎谁是开发者，人们只会记得产品经理。牺牲品总会有，就算李智雅倒霉罢了",
        "爸爸，大恨将雪。李家自有李家的报应",
        "李：最后警告，停止现在的一切，现在的一切正义吗 赵：想想十多年前的那场实验吧，真是荒唐，谁才是正义？李：实验？",
        "视频监控：车祸现场，赵明轩和金玲",
        "提交记录未见异常   UNKNOWN:打包未通过 UNKNOWN:安全自检：代码疑遭泄露"
    };
/*
A.李智恩观察报告---图标一份 介绍一份：金玲做的局，金玲的手和车祸现场的太像了
B.母亲的喃喃自语---头像一份 介绍一份：对不起，对不起
C.房间内脚印--图标一份，介绍一份：40码的鞋
D.螺丝钉---图标一份，介绍一份：这看起来像是固定什么柜子的
E.血迹---图标一份，介绍一份：不知道这是谁的⾎，但⼀定是个该死的混蛋...
F.书柜--图标一份， 介绍一份：这个螺丝钉装在书柜上，刚好。
G.打开的书柜---图标一份， 介绍一份：那本⽇记...到底去哪了？
H.日记本---图标一份介绍一份：妈妈在日记中对赵明轩父母的死感到非常愧疚
I.收据----图标一份介绍一份：这张收据上的日期，正好和日记中被撕去的几页相吻合。这一定不是巧合。我们必须去这家医院看看
J.死亡记录---图标一份，介绍一份：死者死于实验事故，暴露过量辐射死于----
K.病历本---图标一份，介绍一份：
L.母亲的楠楠自语：对不起，那个孩子他不应该
M.赵明轩---头像一份，介绍一份：把小雅和她的商业合同交出来
N.智雅---头像一份，介绍一份：赵明轩故意泄露明天科技机密，盗窃了阿尔法人技术，还对缺陷做了恶意改造
O.技术资料----图标一份，介绍一份：阿尔法人执行语义识别有原始不稳定性，缺陷未修复增多，并且持续复制。代码后与相关记录一并被封存
P.子类通话记录---介绍一份：赵明轩邮件：没人在乎谁是开发者，人们只会记得产品经理。牺牲品总会有，就算李智雅倒霉罢了
Q.子类家书---介绍一份：爸爸，大恨将雪。李家自有李家的报应
R.子类信息记录：李智雅与赵明轩：李：最后警告，停止现在的一切，现在的一切正义吗 赵：想想十多年前的那场实验吧，真是荒唐，谁才是正义？李：实验？
S.第22-1被公司藏匿的监控记录---介绍一份：视频监控：车祸现场，赵明轩和金玲
T.子类代码提交记录：李智雅---提交记录未见异常   UNKNOWN:打包未通过 UNKNOWN:安全自检：代码疑遭泄露
*/
    private string[] titleTexts = {
        "李智恩观察报告",
        "母亲的喃喃自语",
        "房间内脚印",
        "螺丝钉",
        "血迹",
        "书柜",
        "打开的书柜",
        "日记本",
        "收据",
        "死亡记录",
        "病历本",
        "母亲的楠楠自语",
        "赵明轩",
        "智雅",
        "技术资料",
        "子类通话记录",
        "子类家书",
        "子类信息记录",
        "第22-1被公司藏匿的监控记录",
        "子类代码提交记录"
    };

    // Start is called before the first frame update
    void Start()
    {
        int level = PlayerPrefs.GetInt("level", 1);
        if(level == 0)
        {
            FavoritesNum = 0;
        }
        else if(level > 0 && level <= 3)
        {
            FavoritesNum = 1;
        }
        else if(level == 4){
            FavoritesNum = 2;
        }
        else if(level == 5){
            FavoritesNum = 5;
        }
        else if(level >= 6 && level <= 7){
            FavoritesNum = 7;
        }
        else if(level == 8){
            FavoritesNum = 9;
        }
        else if(level == 9){
            FavoritesNum = 11;
        }
        else if(level >= 10 && level <= 12){
            FavoritesNum = 12;
        }
        else if(level == 13){
            FavoritesNum = 13;
        }
        else if(level >= 14 && level <= 17){
            FavoritesNum = 14;
        }
        else if(level >= 18 && level < 28){
            FavoritesNum = 15;
        }
        else if(level >= 28){
            FavoritesNum = 20;
        }
        

        for (int i = 0; i < Favorites.Length; i++)
        {
            Favorites[i].SetActive(false);
        }
        for (int i = 0; i < FavoritesNum; i++)
        {
            Favorites[i].SetActive(true);
        }

        for (int i = 0; i < favoritesButtons.Length; i++)
        {
            int index = i; // Capture the current value of i
            favoritesButtons[i] = Favorites[i].GetComponent<Button>();
            favoritesButtons[i].onClick.AddListener(delegate {
                OnFavoritesButtonClicked(index);
            });
        }

        TitleImage.gameObject.SetActive(false);
    }

    private void OnFavoritesButtonClicked(int index)
    {
        if (index >= favoritesText.Length)
        {
            Debug.LogError("Index out of range: " + index);
            TipsText.text = "";
            return;
        }
        Debug.Log(" " + index);
        TipsPanel.SetActive(true);
        TipsText.text = favoritesText[index];
        if (favoritesText[index].Length > 10)
        {
            TipsText.fontSize = 35;
        }
        else
        {
            TipsText.fontSize = 45;
        }

        TitleImage.gameObject.SetActive(true);
        TitleImage.sprite = Favorites[index].GetComponent<Image>().sprite;

        TitleText.text = titleTexts[index];
    }

    // Update is called once per frame
    void Update()
    {

    }
}