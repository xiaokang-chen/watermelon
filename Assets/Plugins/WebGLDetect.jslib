var LibraryGL = {
 IsWeChatPlatform: function() {
    if (typeof wx !== 'undefined' && typeof wx.getSystemInfoSync !== 'undefined')
    {
        return true;
    }
    else
    {
        return false;
    }
}
};
mergeInto(LibraryManager.library, LibraryGL);