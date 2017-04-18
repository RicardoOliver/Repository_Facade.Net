

function voltar() {
    if ($('#GridCarregando').hasClass('ocultar'))
        location.href = '/';
    return false; // impede submeter o formulário
}

function sair() {
    if ($('#GridCarregando').hasClass('ocultar')) {
        document.forms['frmSair'].submit();
        return true; // submete o formulário
    }
    return false; // impede submeter o formulário
}
