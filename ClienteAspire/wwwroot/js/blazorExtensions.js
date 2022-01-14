window.blazorExtensions = {
    WriteCookie: function (clave, value) {
        var date = new Date();
        date.setTime(date.getTime() + (60 * 60 * 1000));
        var expires = "; expires=" + date;
        var httpOnly = "; httpOnly=true";
        document.cookie = clave + "=" + value + "; Secure";
    },

    ReadCookie: function (cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    },

    DeleteCookie: function (cname) {
        document.cookie = cname + "=; expires = Thu, 01 Jan 1970 00:00:00 GMT"
    }
}