(function () {
  'use strict';

  function StorageServiceProvider () {
    var _localStorage = null;
    var _storage = null;

    /**
     * Remove todos os elementos armazenados no storage.
     */
    var clear = function () {
      _storage.clear();
    };

    /**
     * Retorna o valor armazenado no storage correspondente a chave informada.
     *
     * @param(String) key: chave
     * @return(String): valor armazenado
     */
    var get = function (key) {
      return _storage.getItem(key);
    };

    /**
     * Retorna o valor armazeando no índice informado.
     *
     * @param(Number) i: índice
     * @return(String): valor armazenado no índice
     */
    var index = function (i) {
      if (i < 0) {
        return null;
      }

      // Busca a chave armazenada no índice informado
      var k = _storage.key(i);

      return _storage.getItem(k);
    };

    /**
     * Retorna o número total de chaves armazeandas no storage.
     *
     * @return(Number): tamanho total do storage
     */
    var length = function () {
      return _storage.length;
    };

    /**
     * Retorna a chave armazenada no índice informado.
     *
     * @param(Number) index: índice da chave procurada
     * @return(String): chave
     */
    var key = function (index) {
      return _storage.key(index);
    };

    /**
     * Retorna o valor armazenado na chave informada e em seguida o remove do storage.
     *
     * @param(String) key: chave
     * @return(String): valor armazenado na chave
     */
    var pull = function (key) {
      var value = get(key);

      remove(key);

      return value;
    };

    /**
     * Remove a chave informada do storage.
     *
     * @param(String) key: chave)
     */
    var remove = function (key) {
      _storage.removeItem(key);
    };

    /**
     * Seta o valor de uma chave no storage.
     *
     * @param(String) key: chave
     * @param(String) value: valor a ser armazenado
     */
    var set = function (key, value) {
      _storage.setItem(key, value);
    };

    // Implementação da factory
    return {
      $get: ['$window', function ($window) {

        // Armazena uma referência para o localStorage
        _localStorage = $window.localStorage;

        // Seta o localStorage como default
        _storage = _localStorage;

        return {
          clear: clear,
          get: get,
          index: index,
          length: length,
          key: key,
          pull: pull,
          remove: remove,
          set: set
        };
      }],
      setStorage: function (storage) {

        // Utiliza o localStorage como fallback caso seja infomado um valor null ou undefined
        _storage = storage || _localStorage;
      }
    };
  }

  var module = angular.module('storage', []);
  module.provider('StorageService', [StorageServiceProvider]);
})();