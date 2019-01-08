(function () {
    'use strict';

    angular
        .module('common')
        .service('ValidationService', ValidationService);

    ValidationService.$inject = ['$log', 'toaster', 'ToastService', 'PropagationService'];

    function ValidationService($log, toaster, toastService, propagationService) {

        var self = this;

        self.isValid = function ($scope, formName) {

            if (!!$scope.formValidationExtension) {

                return $scope.formValidationExtension.isValid(formName);

            } else {

                $log.warn('Não foi possível encontrar a extensão de validação.');
            }
        };

        self.validate = function ($scope, formName) {
            var result = self.isValid($scope, formName);
            
            if (!result) {

                var errors = $scope.formValidationExtension.getErrors(formName);

                var msg = [];
                msg.push('<b>{0}</b>:\n<ul>'.format(globalization.texts.theFollowingErrorsWereFound));
                
                angular.forEach(errors, function (value, key) {
                    for (var i = 0; i < value.length; i++) {
                        msg.push("<li>{0}</li>".format(value[i]));                        
                    }
                });

                var finalmsg = msg.join('').replace(/\n/gi, '<br>') + "</ul>";
                
                toaster.pop({
                    type: 'warning',
                    body: finalmsg,
                    bodyOutputType: 'trustedHtml',
                    timeout: 10000
                });
                
                // causa todos os campos inválidos ficarem vermelhos caso já não estejam
                $scope.formValidationExtension.setAllDirty(formName);

                $scope.formValidationExtension.focusFirstField(formName);
            }

            return result;
        };

        self.prepare = function ($scope) {
            $scope.formValidationExtension = new FormValidationApi();

            // Bug #4301 - lida com as inconsistências do banco em relação ao cadastro de estrutura mercadológica.
            propagationService.prepare($scope);
        };

        self.requiresAtLeastOne = function (hash) {

            var theKeys = Object.keys(hash);

            for (var i = 0; i < theKeys.length; i++) {
                if (!!hash[theKeys[i]]) {
                    return true;
                }
            }

            var translatedNames = [];

            for (var i = 0; i < theKeys.length; i++) {
                translatedNames.push(globalization.getText(theKeys[i], true));
            }

            var theNames = translatedNames.join(', ');

            toastService.warning(globalization.texts.atLeastOneMustBeInformed.format(theNames));
        };

        self.accept = function ($scope, formName) {
            $scope.formValidationExtension.setAllPristine(formName);
        }
    };

    window.FormValidationApi = function FormValidationApi() {
        this.formNames = {};
    };

    FormValidationApi.prototype.formNames = null;
    FormValidationApi.prototype.addForm = function (formName, delegate) {
        this.formNames[formName] = delegate;
    };
    FormValidationApi.prototype.removeForm = function (formName) {
        this.formNames[formName] = null;
        delete this.formNames[formName];
    };;
    FormValidationApi.prototype.isValid = function (formName) {
        var result = true;
        //for (var i = 0; i < this.formNames.length && result == true; i++) {
        angular.forEach(this.formNames, function (value, key) {
            if (!result) return;
            if ((formName || key) == key) {
                var theForm = value();
                var kk = Object.keys(theForm);
                for (var i = 0; i < kk.length; i++) {
                    var theField = kk[i];
                    if (theField[0] != '$') {
                        theForm[theField].$validate();
                    }
                }
                result = result && skipDateDisabled(theForm);
            }
        });
        return result;

        function skipDateDisabled(form) {

            // verifica a validade da form, pulando erros de dateDisabled (o datePicker não sabe o que faz)

            // Atalho
            if (!!form.$valid) return true;

            var hasAnyErrors = false;

            // Verifica erros
            angular.forEach(form.$error, function (value, key) {
                if (key !== 'dateDisabled') {
                    hasAnyErrors = true;
                }
            });

            return !hasAnyErrors;
        }
    };
    FormValidationApi.prototype.isPristine = function (formName) {
        var result = true;
        angular.forEach(this.formNames, function (value, key) {
            if (!result) return;
            if ((formName || key) == key) {
                result = result && value().$pristine;
            }
        });
        return result;
    };
    FormValidationApi.prototype.isDirty = function (formName) {
        var result = false;
        angular.forEach(this.formNames, function (value, key) {
            if (!result) return;
            if ((formName || key) == key) {
                result = result || value().$dirty;
            }
        });
        return result;
    };
    FormValidationApi.prototype.getErrors = function (formName) {
        var result = {};
        angular.forEach(this.formNames, function (value, key) {
            if ((formName || key) == key) {
                var theform = value();
                angular.forEach(theform, function (value2, key2) {
                    if (!key2.startsWith('$') && !!value2) {
                        var fieldName = key2;
                        var errorMessages = [];
                        angular.forEach(value2.$error, function (key3, validationKey) {
                            
                            var theValidationValue = '', theFieldElement = $('[name="' + fieldName + '"]');
                            if (validationKey == 'max') {
                                theValidationValue = theFieldElement.attr('max');
                            }
                            else if (validationKey == 'min') {
                                theValidationValue = theFieldElement.attr('min');
                            }
                            else if (validationKey == 'greaterThan') {
                                theValidationValue = formatMilitaryTime(theFieldElement.attr('greater-than'));
                            }
                            else if (validationKey == 'lesserThan') {
                                theValidationValue = formatMilitaryTime(theFieldElement.attr('lesser-than'));
                            }
                            else if (validationKey == 'minValue') {
                                theValidationValue = formatNumber(theFieldElement.attr('min-value'), theFieldElement.attr('ui-number-mask') || 0, null, '.', ',');
                                validationKey = 'min';
                            }
                            else if (validationKey == 'maxValue') {
                                theValidationValue = formatNumber(theFieldElement.attr('max-value'), theFieldElement.attr('ui-number-mask') || 0, null, '.', ',');
                                validationKey = 'max';
                            }
                            else {
                                theValidationValue = theFieldElement.attr(validationKey);
                            }

                            var attrValidationKey = theFieldElement.attr('data-validation-key-' + validationKey);
                            if (attrValidationKey) {
                                validationKey = attrValidationKey;
                            }

                            errorMessages.push(
                                (globalization.getText('errorMessage' + validationKey.capitalize(), false) || '[TEXT NOT FOUND] errorMessage' + validationKey.capitalize() + ": O campo '$name' é $error.")
                                .replace('$name', globalization.getText(fieldName.indexOf(' ') == -1 ? fieldName : fieldName.split(' ')[0], true))
                                .replace('$error', globalization.getText(validationKey, true))
                                .replace('$value', globalization.getText(theValidationValue || '', false) || theValidationValue));
                        });
                        if (errorMessages.length) {
                            result[fieldName] = errorMessages;
                        }
                    }
                });
            }
        });
        return result;
    };
    FormValidationApi.prototype.focusFirstField = function (formName) {
        var primeiro = null;
        angular.forEach(this.formNames, function (value, key) {
            if (null != primeiro) return;
            if ((formName || key) == key) {
                var theform = value();
                angular.forEach(theform, function (value2, key2) {
                    if (!key2.startsWith('$') && !!value2) {
                        if (value2.$invalid && primeiro === null) primeiro = value2;
                        if (value2.$invalid && value2.$setViewValue) value2.$setViewValue(value2.$viewValue);
                    }
                });
            }
        });
        if (!!primeiro && primeiro.$name) {
            $("[name='" + primeiro.$name + "']:visible").focus().select();
        }
    };
    FormValidationApi.prototype.setAllDirty = function (formName) {
        angular.forEach(this.formNames, function (value, key) {
            if ((formName || key) == key) {
                var theform = value();
                angular.forEach(theform, function (value2, key2) {
                    if (!key2.startsWith('$') && !!value2 && value2.$invalid && value2.$pristine && !!value2.$setDirty) {
                        if (value2.$name && $('[name="' + value2.$name + '"]').is(':disabled')) {
                            return;
                        }
                        value2.$setDirty();
                    }
                });
            }
        });
    };
    FormValidationApi.prototype.setAllPristine = function (formName) {
        angular.forEach(this.formNames, function (value, key) {
            if ((formName || key) == key) {
                var theform = value();
                angular.forEach(theform, function (value2, key2) {
                    if (!key2.startsWith('$') && !!value2 && value2.$invalid && value2.$pristine && !!value2.$setDirty) {
                        if (value2.$name && $('[name="' + value2.$name + '"]').is(':disabled')) {
                            return;
                        }
                        value2.$setPristine();
                    }
                });
            }
        });
    };
})();