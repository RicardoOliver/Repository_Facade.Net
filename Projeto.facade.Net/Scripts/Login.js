

// para validação client-side do login
function validaLogin() {

    if (!$('#GridCarregando').hasClass('ocultar')) // está carregando??
        return false; // impede submissão do relatório

    var valida = true;
    var textoErro = ''; // por enquanto, não houve erro
    $('#txtLogin').removeClass('Erro'); // para não acumular erros de validação posteriores
    $('#txtSenha').removeClass('Erro');

    document.getElementById('GridRetorno').innerHTML = '';
    if (document.getElementById('txtLogin').value == '') {
        textoErro += 'Informe o Login';
        $('#txtLogin').addClass('Erro');
        $('#txtLogin').focus();
        valida = false; // para impedir que o form seja submetido
    }
    if (document.getElementById('txtSenha').value == '') {
        if (textoErro != '')
            textoErro += '<br />';
        $('#txtSenha').addClass('Erro');
        $('#txtSenha').focus();
        textoErro += 'Informe a Senha';
        valida = false; // para impedir que o form seja submetido
    }

    document.getElementById('GridRetorno').innerHTML = textoErro;

    if (valida == true) { // deve submeter o form??
        $('#txtLogin').removeClass('Erro'); // retira erro do campo de login
        $('#txtSenha').removeClass('Erro'); // e também do campo de senha
        $("#GridCarregando").removeClass('ocultar'); // colocar símbolo de carregando
    }




    return valida;
}