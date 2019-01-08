(function () {
    'use strict';

    if (!Array.prototype.remove) {
        Array.prototype.remove = function (item) {
            var index = this.indexOf(item);

            if (index > -1) {
                this.splice(index, 1);
            }
        };
    }

    if (!Array.prototype.removeAt) {
        Array.prototype.removeAt = function (index) {
            if (index > -1) {
                this.splice(index, 1);
            }
        };
    }

    if (!Array.prototype.insert) {
        Array.prototype.insert = function (index, item) {
            this.splice(index, 0, item);
        };
    }

    if (!Array.prototype.isEmpty) {
        Array.prototype.isEmpty = function () {
            return this.length === 0;
        };
    }

    if (!Array.prototype.contains) {
        Array.prototype.contains = function (item) {
            return this.indexOf(item) > -1;
        };
    }

    if (!Array.prototype.containsByProperty) {
        Array.prototype.containsByProperty = function (propertyName, propertyValue) {
            return this.filter(function (obj) {
                return obj[propertyName] == propertyValue;
            }).length > 0;
        };
    }

    if (!Array.prototype.any) {
        Array.prototype.any = Array.prototype.containsByProperty;
    }

    if (!Array.prototype.removeByProperty) {
        Array.prototype.removeByProperty = function (propertyName, propertyValue) {
            var items = this.filter(function (obj) {
                return obj[propertyName] == propertyValue;
            });
            
            for (var i in items) {
                this.remove(items[i]);
            }                 
        };
    }

    if (!Array.prototype.all) {
        Array.prototype.all = function (propertyName, propertyValue) {
            return this.length > 0 && this.filter(function (obj) {
                return obj[propertyName] == propertyValue;
            }).length == this.length;
        };
    }

    if (!Array.prototype.setAll) {
        Array.prototype.setAll = function (propertyName, propertyValue) {            
            for (var i in this) {
                var obj = this[i];

                if (obj !== null && typeof obj === 'object') {
                    obj[propertyName] = propertyValue;
                }
            }
        };
    }

    if (!Array.prototype.count) {
        Array.prototype.count = function (propertyName, propertyValue) {
            return this.filter(function (obj) {
                return obj[propertyName] == propertyValue;
            }).length;
        };
    }

    if (!Array.prototype.where) {
        Array.prototype.where = function (propertyName, propertyValue) {
            return this.filter(function (obj) {
                return obj[propertyName] == propertyValue;
            });
        };
    }

    if (!Array.prototype.firstOrDefault) {
        Array.prototype.firstOrDefault = function (propertyName, propertyValue) {
            var r = this.where(propertyName, propertyValue);

            return r.length == 0 ? null : r[0];
        };
    }

    if (!Array.prototype.sum) {
    	Array.prototype.sum = function (prop) {
    		if (this == null) {
    			return 0;
    		}
    		return this.reduce(function (a, b) {
    		    var isInt = Number(b) === b && b % 1 === 0;

    		    if (isInt) {
    		        return b[prop] == null ? parseInt(a) : parseInt(a) + parseInt(b[prop]);
    		    }
    		    else {
    		        return b[prop] == null ? parseFloat(a) : parseFloat(a) + parseFloat(b[prop]);
    		    }
    			
    		}, 0);
    	};
    }
})();
