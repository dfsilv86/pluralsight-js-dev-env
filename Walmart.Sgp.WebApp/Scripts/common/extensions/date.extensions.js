(function () {
    'use strict';

    if (!Date.prototype.isEmpty) {
        Date.prototype.isEmpty = function () {            
            return this.year == 1900;
        };
    }

    if (!Date.prototype.endOfDay) {
        Date.prototype.endOfDay = function () {
            return moment(this).endOf('day').toDate();
        };
    }

    if (!Date.prototype.isToday) {
        Date.prototype.isToday = function () {
            return moment().isSame(this, 'day');
        };
    }
})();
