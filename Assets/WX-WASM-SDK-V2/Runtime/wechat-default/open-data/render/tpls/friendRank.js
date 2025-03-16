/**
 * 下面的内容分成两部分，第一部分是一个模板，模板的好处是能够有一定的语法
 * 坏处是模板引擎一般都依赖 new Function 或者 eval 能力，小游戏下面是没有的
 * 所以模板的编译需要在外部完成，可以将注释内的模板贴到下面的页面内，点击 "run"就能够得到编译后的模板函数
 * https://wechat-miniprogram.github.io/minigame-canvas-engine/playground.html
 * 如果觉得模板引擎使用过于麻烦，也可以手动拼接字符串，本文件对应函数的目标仅仅是为了创建出 xml 节点数
 */
/*
<view class="container" id="main">
  <view class="rankList">
        <scrollview class="list" scrollY="true">
            {{~it.data :item:index}}
                {{? index % 2 === 0 }}
                <view class="listItem listItemEven">
                {{?}}
                {{? index % 2 === 1 }}
                <view class="listItem">
                {{?}}
                    <bitmaptext font="fnt_number-export" class="listItemNum" value="{{= index + 1}}"></bitmaptext>
                    <image class="rankAvatar" src="{{= item.avatarUrl }}"></image>
                  <text class="rankName" value="{{= item.nickname}}"></text>
                  <text class="listScoreUnit" value="好感度："></text>
                  <text class="listItemScore" value="{{= item.score}}"></text>
                </view>
            {{~}}
        </scrollview>
    </view>
</view>
*/
/**
 * xml经过doT.js编译出的模板函数
 * 因为小游戏不支持new Function，模板函数只能外部编译
 * 可直接拷贝本函数到小游戏中使用
 */
export  function getFriendFeelingsRankXML(it) {
    var out = '<view class="container" id="main"> <view class="rankList"> <scrollview class="list" scrollY="true"> ';
    var arr1 = it.data;
    if (arr1) {
        var item, index = -1,
            l1 = arr1.length - 1;
        while (index < l1) {
            item = arr1[index += 1];
            out += ' ';
            if (index % 2 === 0) {
                out += ' <view class="listItem listItemEven"> ';
            }
            out += ' ';
            if (index % 2 === 1) {
                out += ' <view class="listItem"> ';
            }
            out += ' <text class="listItemNum" value="' + (index + 1) + '"/> <image class="rankAvatar" src="' + (item.avatarUrl) + '"></image> <text class="rankName" value="' + (item.nickname) + '"></text> <text class="listScoreUnit" value="好感度："></text>                   <text class="listItemScore" value="' + (item.score) + '"></text> </view> ';
        }
    }
    out += ' </scrollview> </view></view>';
    return out;
}

export function getFriendScoreRankXML(it) {
    var out = '<view class="container" id="main"> <view class="rankList"> <scrollview class="list" scrollY="true"> ';
    var arr1 = it.data;
    if (arr1) {
        var item, index = -1,
            l1 = arr1.length - 1;
        while (index < l1) {
            item = arr1[index += 1];
            out += ' ';
            if (index % 2 === 0) {
                out += ' <view class="listItem listItemEven"> ';
            }
            out += ' ';
            if (index % 2 === 1) {
                out += ' <view class="listItem"> ';
            }
            out += ' <text class="listItemNum" value="' + (index + 1) + '"/> <image class="rankAvatar" src="' + (item.avatarUrl) + '"></image> <text class="rankName" value="' + (item.nickname) + '"></text> <text class="listScoreUnit" value="分数："></text>                   <text class="listItemScore" value="' + (item.score) + '"></text> </view> ';
        }
    }
    out += ' </scrollview> </view></view>';
    return out;
}