//var INITURL = "http://192.168.1.54/CareGiver";
//var INITURL = "http://differenzuat.com/caregiverliteweb/";

var INITURL = "http://localhost:15177/";
//var INITURL = "http://192.168.1.118//LocalCaregiverLite/";

//var INITURL = "https://scheduler.paseva.com/";

function getLastWeek() {
    var today = new Date();
    var lastWeek = new Date(today.getFullYear(), today.getMonth(), today.getDate() - 7);
    return lastWeek;
}

format = function date2str(x, y) {
    var z = {
        M: x.getMonth() + 1,
        d: x.getDate(),
        h: x.getHours(),
        m: x.getMinutes(),
        s: x.getSeconds(),
        e: x.getDay(),
    };
    y = y.replace(/(M+|d+|e+|h+|m+|s+)/g, function (v) {
        if (v == "MMMM") {
            return (new Date()).getMonthName('en', eval('z.' + v.slice(-1)) -1);
        }
        if (v == "MMM") {
            return (new Date()).getMonthNameShort('en', eval('z.' + v.slice(-1)) - 1);
        }
        else if (v == "e") {
            return (new Date()).getDayName('en', eval('z.' + v.slice(-1)));
        }
        else {
            return ((v.length > 1 ? "0" : "") + eval('z.' + v.slice(-1))).slice(-2)
        }
    });

    return y.replace(/(y+)/g, function (v) {
        if (y.indexOf("January") > -1 || y.indexOf("February") > -1 || y.indexOf("May") > -1 || y.indexOf("July") > -1) {
            return v
        }
        else {
            return x.getFullYear().toString().slice(-v.length)
        }
    });
}

Date.prototype.getWeek = function () {
    //debugger;
    //var determinedate = new Date();
    //determinedate.setFullYear(this.getFullYear(), this.getMonth(), this.getDate());
    //var D = determinedate.getDay();
    //if (D == 0) D = 7;
    //determinedate.setDate(determinedate.getDate() + (4 - D));
    //var YN = determinedate.getFullYear();
    //var ZBDoCY = Math.floor((determinedate.getTime() - new Date(YN, 0, 1, -6)) / 86400000);
    //var WN = 1 + Math.floor(ZBDoCY / 7);
    //return WN + 1;
    var onejan = new Date(this.getFullYear(), 0, 1);
    return Math.ceil((((this - onejan) / 86400000) + onejan.getDay()) / 7);
}

function StartdateFromWeekNumber(year, week) {
    var d = new Date(year, 0, 1);
    var dayNum = d.getDay();
    var diff = --week * 7;

    // If 1 Jan is Friday to Sunday, go to next week
    //if (!dayNum || dayNum > 4) {
    //    diff += 7;
    //}

    // Add required number of days
    d.setDate(d.getDate() - d.getDay() + ++diff);
    return d.addDays(-1);
}

function EnddateFromWeekNumber(year, week) {
    return (new Date(StartdateFromWeekNumber(year, week)).addDays(6))
}

Date.prototype.addDays = function (days) {
    this.setDate(this.getDate() + parseInt(days));
    return this;
};

Date.prototype.getMonthNameShort = function (lang, month) {
    lang = lang && (lang in Date.locale) ? lang : 'en';
    return Date.locale[lang].month_names_short[month];
};

Date.prototype.getMonthName = function (lang, month) {
    lang = lang && (lang in Date.locale) ? lang : 'en';
    return Date.locale[lang].month_names[month];
};

Date.locale = {
    en: {
        month_names: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
        month_names_short: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        day_names_short : ['Sun', 'Mon', 'Tue', 'Wed','Thu', 'Fri', 'Sat']
    }
};

Date.prototype.getDayName = function (lang, day) {
    lang = lang && (lang in Date.locale) ? lang : 'en';
    return Date.locale[lang].day_names_short[day];
}

function Convert24HrFormatTo12HrFormat(time) {
    var hours = Number(time.match(/^(\d+)/)[1]);
    var minutes = Number(time.match(/:(\d+)/)[1]);
    var AMPM = time.match(/\s(.*)$/)[1];
    if (AMPM == "PM" && hours < 12) hours = hours + 12;
    if (AMPM == "AM" && hours == 12) hours = hours - 12;
    var sHours = hours.toString();
    var sMinutes = minutes.toString();
    if (hours < 10) sHours = "0" + sHours;
    if (minutes < 10) sMinutes = "0" + sMinutes;
    //alert(sHours + ":" + sMinutes);

    return sHours + ":" + sMinutes;
}

function calculateTime(startTime, endTime) {

    var startTimeArray = startTime.split(":");
    var startInputHrs = parseInt(startTimeArray[0]);
    var startInputMins = parseInt(startTimeArray[1]);

    var endTimeArray = endTime.split(":");
    var endInputHrs = parseInt(endTimeArray[0]);
    var endInputMins = parseInt(endTimeArray[1]);

    var startMin = startInputHrs * 60 + startInputMins;
    var endMin = endInputHrs * 60 + endInputMins;

    var result;

    if (endMin < startMin) {
        var minutesPerDay = 24 * 60;
        result = minutesPerDay - startMin;  // Minutes till midnight
        result += endMin; // Minutes in the next day
    } else {
        result = endMin - startMin;
    }

    var minutesElapsed = result % 60;
    var hoursElapsed = (result - minutesElapsed) / 60;

    var TotalHour = hoursElapsed + (minutesElapsed == 0 ? 0 : (minutesElapsed / 60))
    //alert("Elapsed Time : " + hoursElapsed + ":" + (minutesElapsed < 10 ?
    //         '0' + minutesElapsed : minutesElapsed));
    //alert(TotalHour);

    return TotalHour;
}

function convertDateFormat(date) {
    var arr = date.split("/");

    return arr[2] + "/" + arr[0] + "/" + arr[1];
}