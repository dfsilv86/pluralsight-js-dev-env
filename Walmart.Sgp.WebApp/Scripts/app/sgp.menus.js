(function () {
    'use strict';

    // TODO: globalizar menus

    window.sgpMenus = [
        // ACESSOS
        {
            'name': 'Anuncios',
            'children': [
                { 'name': 'Novo', route: '/Anuncio/novo' },
                { 'name': 'Importação de Usuários', route: '/acessos/usuario/importacao' },
                { 'name': 'Auditoria de Dados' }
            ]
        }       
    ];
})();
