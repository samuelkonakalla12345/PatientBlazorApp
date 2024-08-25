(
    function (publicMethod, $) {
        // #region Sweet Alert

        publicMethod.hello = function () {
            console.log("hello");
        }


        publicMethod.redirectTo = function (path) {
            window.location.replace(path);
        }

        publicMethod.showSweetAlertPopup = function (status, title, message) {

            console.log(status + title + message);

            Swal.fire(
                {
                    title: title,
                    text: message,
                    icon: status,
                })
        }

        publicMethod.showSweetAlertPopupWithDelay = function (status, title, message, delayInMilliSec) {
            setTimeout(
                function () {
                    Swal.fire(
                        {
                            icon: status,
                            title: title,
                            text: message,
                        })
                }, delayInMilliSec);
        }

        publicMethod.showSweetToastNotification = function (status, message, timeout) {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: timeout,
                timerProgressBar: true,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })

            Toast.fire({
                icon: status,
                title: message
            })
        }

        publicMethod.showSweetToastNotificationWithDelay = function (status, message, timeout, delayInMilliSec) {
            setTimeout(
                function () {
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top-end',
                        showConfirmButton: false,
                        timer: timeout,
                        timerProgressBar: true,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })

                    Toast.fire({
                        icon: status,
                        title: message
                    })
                }, delayInMilliSec);
        }

        // #endregion Sweet Alert

        // #region Browser Storage & Cookies

        publicMethod.createLocalStorage = function (localStorageName, value) {
            localStorage.setItem(localStorageName, value);
        }

        publicMethod.readLocalStorage = function (localStorageName) {
            var value = localStorage.getItem(localStorageName);

            if (value == undefined) {
                return "";
            }
            return value;
        }

        publicMethod.clearLocalStorage = function (localStorageName) {
            localStorage.removeItem(localStorageName);
        }

        publicMethod.deleteAllLocalStorage = function () {
            localStorage.clear();
        }

        publicMethod.createCookieMinutes = function (cookieName, value, minutes) {
            if (minutes) {
                var date = new Date();
                date.setTime(date.getTime() + (minutes * 60 * 1000));
                var expires = "; expires=" + date.toGMTString();
            }
            else var expires = "";

            document.cookie = cookieName + "=" + value + expires + "; path=/";
        }

        publicMethod.createCookie = function (cookieName, value, hours) {
            if (hours) {
                var date = new Date();
                date.setTime(date.getTime() + (hours * 60 * 60 * 1000));
                var expires = "; expires=" + date.toGMTString();
            }
            else var expires = "";

            document.cookie = cookieName + "=" + value + expires + "; path=/";
        }

        publicMethod.readCookie = function (cookieName) {
            var name = cookieName + "=";
            var decodedCookie = decodeURIComponent(document.cookie);
            var ca = decodedCookie.split(';');
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
        }

        publicMethod.eraseCookie = function (cookieName) {
            document.cookie = cookieName + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';

            $.ajax({
                type: "POST",
                url: eraseAppCookieURL,
                data: { cookieName: cookieName },
                datatype: "json",
                headers: {
                    "RequestVerificationToken": $('input[name = __RequestVerificationToken]').val()
                },
                begin: function () {
                },
                complete: function () {
                },
                success: function (data) {
                },
                error: function (xMLHttpRequest, textStatus, errorThrown) {
                }
            });
        }

        publicMethod.deleteAllCookies = function () {
            var cookies = document.cookie.split(";");

            for (var i = 0; i < cookies.length; i++) {
                var cookie = cookies[i];
                var eqPos = cookie.indexOf("=");
                var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
                document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
            }
        }

        // #endregion Browser Storage & Cookies


    }(window.appScripts = window.appScripts || {}));

