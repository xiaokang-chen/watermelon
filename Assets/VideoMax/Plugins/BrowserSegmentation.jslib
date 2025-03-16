mergeInto(LibraryManager.library, {
    isWechat : function() {   
       var ua = navigator.userAgent.toLowerCase()
       var isWXWork = ua.match(/wxwork/i) == 'wxwork'
       var isWeixin = !isWXWork && ua.match(/MicroMessenger/i) == 'micromessenger'
       return isWeixin
    },
    changeSpeed : function(speed) {
          let myVideo = document.getElementById('myVideo')
          myVideo.playbackRate = speed
    }
});