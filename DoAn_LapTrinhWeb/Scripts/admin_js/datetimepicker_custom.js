(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports) :
        typeof define === 'function' && define.amd ? define(['exports'], factory) :
            (global = typeof globalThis !== 'undefined' ? globalThis : global || self, factory(global.vn = {}));
}(this, (function (exports) {
    'use strict';

    var fp = typeof window !== "undefined" && window.flatpickr !== undefined
        ? window.flatpickr
        : {
            l10ns: {},
        };
    var Vietnamese = {
        weekdays: {
            shorthand: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
            longhand: [
                "Chủ nhật",
                "Thứ hai",
                "Thứ ba",
                "Thứ tư",
                "Thứ năm",
                "Thứ sáu",
                "Thứ bảy",
            ],
        },
        months: {
            shorthand: [
                "01",
                "02",
                "03",
                "04",
                "55",
                "06",
                "07",
                "08",
                "09",
                "10",
                "11",
                "12",
            ],
            longhand: [
                "Tháng một",
                "Tháng hai",
                "Tháng ba",
                "Tháng tư",
                "Tháng năm",
                "Tháng sáu",
                "Tháng bảy",
                "Tháng tám",
                "Tháng chín",
                "Tháng mười",
                "Tháng mười một",
                "Tháng mười hai",
            ],
        },
        firstDayOfWeek: 1,
        rangeSeparator: " đến ",
    };
    fp.l10ns.vn = Vietnamese;
    var vn = fp.l10ns;

    exports.Vietnamese = Vietnamese;
    exports.default = vn;

    Object.defineProperty(exports, '__esModule', { value: true });

})));

$("#dt_banner_end").flatpickr({
    "locale": "vn",
    enableTime: true,
    dateFormat: "m-d-Y H:i",
    time_24hr: true,
    minDate: "today",
});
$("#dt_banner_start").flatpickr({
    "locale": "vn",
    enableTime: true,
    dateFormat: "m-d-Y H:i",
    time_24hr: true,
    minDate: "today",
});
$("#dt_disount_start").flatpickr({
    "locale": "vn", 
    enableTime: true,
    dateFormat: "m-d-Y H:i",
    time_24hr: true,
    minDate: "today",

});
$("#dt_disount_end").flatpickr({
    "locale": "vn",
    enableTime: true,
    dateFormat: "m-d-Y H:i",
    time_24hr: true,
    minDate: "today",
});
$("#dt_dateofbirth").flatpickr({
    "locale": "vn",
    dateFormat: "M-d-Y",
    maxDate: "today",
});

$(".pick_report_start").flatpickr({
    "locale": "vn",
    dateFormat: "m-d-Y",
    enableTime: false,
    defaultDate:"today",
    maxDate: "today",
});


