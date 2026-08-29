/**
 * AppAjax
 * لایه‌ی مشترک برای همه‌ی درخواست‌های AJAX در پنل ادمین.
 * شامل: مدیریت CSRF Token، Loading State، و مدیریت یکپارچه‌ی خطا.
 *
 * استفاده:
 *   AppAjax.post(url, data).then(res => ...).catch(err => ...);
 *   AppAjax.get(url).then(res => ...);
 */
const AppAjax = (function () {
    // خواندن توکن CSRF از فرم مخفی موجود در Layout
    function getAntiForgeryToken() {
        const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
        return tokenInput ? tokenInput.value : null;
    }

    // تنظیم پیش‌فرض هدر CSRF برای تمام درخواست‌های jQuery Ajax
    function setup() {
        $.ajaxSetup({
            beforeSend: function (xhr) {
                const token = getAntiForgeryToken();
                if (token) {
                    xhr.setRequestHeader('RequestVerificationToken', token);
                }
            }
        });
    }

    // استخراج پیام خطا از پاسخ سرور (چه متنی، چه JSON استاندارد)
    function extractErrorMessage(xhr, defaultMessage) {
        if (!xhr) return defaultMessage;

        try {
            const json = JSON.parse(xhr.responseText);
            return json.message || json.title || defaultMessage;
        } catch {
            return xhr.responseText && xhr.responseText.length < 200
                ? xhr.responseText
                : defaultMessage;
        }
    }

    // درخواست عمومی — پایه‌ی همه‌ی متدهای دیگر
    function request(method, url, data, options) {
        options = options || {};
        const defaultErrorMessage = options.errorMessage || 'خطایی در ارتباط با سرور رخ داد';
        if (options.showLoading !== false) {
            toggleLoading(true, options.loadingTarget);
        }

        const isGet = method === 'GET'

        return new Promise((resolve, reject) => {
            $.ajax({
                url: url,
                method: method,
                contentType: isGet ? false : 'application/json',
                data: data ? JSON.stringify(data) : null,
                success: function (response) {
                    resolve(response);
                },
                error: function (xhr) {
                    console.error('Status:', xhr.status);
                    console.error('Response:', xhr.responseText);
                    const message = extractErrorMessage(xhr, defaultErrorMessage);
                    if (options.showToastOnError !== false) {
                        toastr.error(message);
                    }
                    reject({ xhr, message });
                },
                complete: function () {
                    if (options.showLoading !== false) {
                        toggleLoading(false, options.loadingTarget);
                    }
                }
            });
        });
    }

    // نمایش/مخفی کردن Loading روی یک دکمه یا کل صفحه (اختیاری)
    function toggleLoading(isLoading, targetSelector) {
        if (!targetSelector) return;

        const $target = $(targetSelector);
        if (isLoading) {
            $target.data('original-html', $target.html());
            $target.prop('disabled', true)
                .html('<span class="spinner-border spinner-border-sm ms-1"></span> در حال پردازش...');
        } else {
            $target.prop('disabled', false)
                .html($target.data('original-html'));
        }
    }

    return {
        setup: setup,
        get: (url, options) => request('GET', url, null, options),
        post: (url, data, options) => request('POST', url, data, options),
        put: (url, data, options) => request('PUT', url, data, options),
        delete: (url, options) => request('DELETE', url, null, options)
    };
})();

// راه‌اندازی خودکار به محض بارگذاری فایل
AppAjax.setup();