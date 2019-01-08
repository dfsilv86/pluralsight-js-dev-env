(function () {
    'use strict';

    /// <widget title="Foo" hasBox="true" isOpen="true">...</widget>
    /// <widget title="Foo" hasBox="false" isOpen="false">...</widget>
    /// <widget title="Foo" hasBox="no-padding" isOpen="true">...</widget>

    angular
        .module('common')
        .directive('widget', Widget);

    function Widget() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/common/directives/widget.template.html',
            transclude: true,
            scope: false,
            link: function ($scope, elem, attr) {

                var title = attr.title, isOpen = (attr.isOpen || 'true') !== 'false', boxMode = attr.hasBox;

                var toggleIsOpen = function (value) {

                    isOpen = value || !isOpen;

                    var widgetContent = elem.find("div.widget-content");

                    if (isOpen) {
                        elem.removeClass('widget-closed');
                        elem.find('.widget-collapse i').removeClass('icon-angle-up').addClass('icon-angle-down');
                        widgetContent.css('display', 'block');
                    } else {
                        elem.addClass('widget-closed');
                        elem.find('.widget-collapse i').removeClass('icon-angle-down').addClass('icon-angle-up');
                        widgetContent.css('display', 'none');
                    }
                };

                var setBoxMode = function (value) {

                    boxMode = value || (boxMode !== 'false');
                    var widgetContent = elem.find("div.widget-content");

                    if (!!boxMode) {
                        elem.addClass('box');

                        if (boxMode === 'no-padding') {
                            widgetContent.addClass('no-padding');
                        } else {
                            widgetContent.removeClass('no-padding');
                        }
                    } else {
                        elem.removeClass('box');
                        widgetContent.removeClass('no-padding');
                    }
                };

                var setTitle = function (value) {

                    title = value || title;

                    var spanTitle = elem.find("span.the-title");

                    spanTitle[0].innerText = title;

                    var widgetHeader = elem.find("div.widget-header");

                    if (!!title) {
                        widgetHeader.css('display', 'block');
                    } else {
                        widgetHeader.css('display', 'none');
                    }
                };

                toggleIsOpen(isOpen);
                setTitle(title);
                setBoxMode(boxMode);

                elem
                    .find('.widget-collapse')
                    .on('click', function () {
                        $scope.$apply(function () {
                            toggleIsOpen();
                        });
                    });

                attr.$observe('title', function (value) {
                    setTitle(value);
                });

                attr.$observe('isOpen', function (value) {
                    toggleIsOpen((value || 'true') !== 'false');
                });

                attr.$observe('hasBox', function (value) {
                    setBoxMode(value);
                });
            },
        };
    }
})();