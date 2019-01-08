(function () {
    'use strict';

    angular.module('common')
      .directive('reportViewer', ['UserSessionService', 'ApiEndpoints', 'SpaConfig', '$sce', function (userSessionService, apiEndpoints, spaConfig, $sce) {
          return {
              restrict: 'E',
              templateUrl: 'Scripts/common/directives/reportviewer.directive.html',
              requires: 'ngModel',
              replace: true,
              scope: {
                  reportName: '@reportName'
              },
              link: function (scope, element, attrs) {
                  var currentUser = userSessionService.getCurrentUser();

                  var params = {
                      idCurrentUser: currentUser.id,
                      cultureCode: currentUser.culture,
                      currentReport: scope.reportName
                  };

                  var reportUrl = spaConfig.reportViewerUrl;
                  var parameters = apiEndpoints.serialize(params);

                  var url = reportUrl + '?' + parameters;

                  $('#viewer_frame').attr('src', $sce.getTrustedUrl($sce.trustAsUrl(url)));
              }
          };
      }]);
})();
