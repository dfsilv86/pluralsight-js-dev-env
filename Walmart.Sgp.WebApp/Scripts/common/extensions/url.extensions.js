(function () {
	'use strict';

	if (!String.prototype.asVersioned) {
		String.prototype.asVersioned = function (version) {
			var url = this;
			var versionedUrl = url;

			if (!url.contains('v=') && !url.contains('$$')) {
				var queryStringInit = url.contains('?') ? '&' : '?';
				versionedUrl = '{0}{1}v={2}'.format(url, queryStringInit, version);
			}

			return versionedUrl;
		};
	}
})();
