
var aplicativos;
var ultimoApp = null;
var ultimoMenu = null;

// Function para montar o menu
function MontaMenu(aplicativos) {

    if (aplicativos.length == 0)
        return false;

    var menuFisico = '';

    for (var i = 0; i < aplicativos.length; i++) { // loop para iterar nos aplicativos
        var listaMenus = aplicativos[i].Menus;
        if (listaMenus.length == 0)
            continue;

        menuFisico += '<div id="app[' + i + ']"> ';

        // colocando aplicativos no menu
        /*if (listaMenus[0].Posicao == 0) {
            menuFisico += '<a onclick="acessa(' + i + ', 0, 0, \'App\')">' +
                            aplicativos[i].Descricao + '</a></div>';
            continue;
        }
        else
        {
            menuFisico += '<a onclick="libera(\'app\', ' + i + ', 0);" >' + 
                aplicativos[i].Descricao + "</a></div>";
        }*/
        for (var j = 0; j < listaMenus.length; j++) { // loop para iterar nos menus do aplicativo
            var listaSubMenus = listaMenus[j].SubMenus;
            var eventoClick = '';

            menuFisico += '<div id="menu[' + j + ']" > ';
            eventoClick = '<a onclick="libera(' + j + ');">' +
                  listaMenus[j].Descricao + '</a></div>';

            if (listaSubMenus[0].Posicao == 0) {
                menuFisico += '<a onclick="acessa(' + j + ', 0, \'Menu\');">' +
                        listaMenus[j].Descricao + '</a></div>';
                continue;
            }

            menuFisico += eventoClick; // salvando menu com submenus

            for (var k = 0; k < listaSubMenus.length; k++) { // loop para iterar nos submenus do aplicativo
                menuFisico += '<div id="submenu[' + j + '][' + k + ']" ' +
                    'style="margin-left: 10px;" class="ocultar" >';
                eventoClick = '<a onclick="acessa(' + j + ', ' + k + ', \'SubMenu\');">' +
                    listaSubMenus[k].Descricao + '</a></div>';

                menuFisico += eventoClick;
            } // for submenus
        } // for menus

    } // for aplicativos



    document.getElementById('menu').innerHTML = menuFisico;
}

function libera(indexY) {



    // desaparecer os que foram clicados anteriormente   
    for (var j = 0; j < aplicativos[0].Menus.length; j++) {

        for (var k = 0; k < aplicativos[0].Menus[j].SubMenus.length; k++) {
            if (!$('#submenu\\[' + j + '\\]\\[' + k + '\\]').hasClass('ocultar'))
                $('#submenu\\[' + j + '\\]\\[' + k + '\\]').addClass('ocultar');

        }
    }



    if (ultimoMenu == aplicativos[0].Menus[indexY]) // clicou em menu já aberto??
    {
        ultimoMenu = null;
        return;
    }


    // mostrando submenus do menu:
    ultimoMenu = aplicativos[0].Menus[indexY];
    for (var i = 0; i < ultimoMenu.SubMenus.length; i++) {
        $('#submenu\\[' + indexY + '\\]\\[' + i + '\\]').
                        removeClass('ocultar');

    }

}


function carregar() {
    // fazer gif que gira aparecer    
    $("#GridCarregando").removeClass('ocultar');
}

function desaparecer() {
    // fazer gif que gira desaparecer
    $('#GridCarregando').addClass('ocultar');
}

function jaSubmeteu() {
    if ($('#GridCarregando').hasClass('ocultar')) // não submeteu??
        return false;
    return true; // já submeteu
}

function AlterarOrgaoLogado() {
    var e = document.getElementById('OrgaoEscolhido');
    document.getElementById("IdOrgao").value = e.options[e.selectedIndex].value;

    if (jaSubmeteu()) // já submeteu??
        return false;

    return true;
}


function submeteAplicacao() { // submeter aplicação escolhida
    document.getElementById('acessaApp').click();
    return true;
}

function acessa(indexMenu, indexSubMenu, tipo) {
    if (jaSubmeteu())
        return false;
    location.href = aplicativos[0].Menus[indexMenu].SubMenus[indexSubMenu].ActionSubMenu;
}