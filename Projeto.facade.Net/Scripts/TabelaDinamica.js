

var tabelas = {};

function adicionarLinha(NomeTabela) {
    var tabelaAtual = document.getElementById(NomeTabela);

    // Calculando índice da próxima linha
    var indexLinha = tabelaAtual.rows.length - tabelas[NomeTabela].LinhasExtrasAbaixo -
                                               tabelas[NomeTabela].LinhasExtrasAcima;
    // calculando linha de inserção
    var linha = tabelaAtual.insertRow(tabelaAtual.rows.length - tabelas[NomeTabela].LinhasExtrasAbaixo);

    // corrigindo HTML do campo, mudando @index para o índice correto
    for (var i = 0; i < tabelas[NomeTabela].Campos.length; i++) {
        linha.insertCell(i).innerHTML = tabelas[NomeTabela].Campos[i].HTML.replace(/@index/g, indexLinha);
    }


    // Se tiver campo de quantidade, atualizar o valor
    if (tabelas[NomeTabela].Campos[0].Qtde != null)
        document.getElementById(tabelas[NomeTabela].Campos[0].Qtde).value = tabelaAtual.rows.length -
            tabelas[NomeTabela].LinhasExtrasAbaixo - tabelas[NomeTabela].LinhasExtrasAcima;

    // se definiu foco, focalizar o campo
    if (tabelas[NomeTabela].Campos[0].Foco != null) // definiu foco??
        document.getElementById(tabelas[NomeTabela].Campos[0].Foco.replace(/@index/g, indexLinha)).focus();

    return tabelaAtual.rows.length -
        tabelas[NomeTabela].LinhasExtrasAbaixo - tabelas[NomeTabela].LinhasExtrasAcima;



}


function RegistrarTabela(NomeTabela) {
    tabelas[NomeTabela] = new Object();
    tabelas[NomeTabela].LinhasExtrasAcima = 1; // incluindo título
    tabelas[NomeTabela].LinhasExtrasAbaixo = 1; // todas as linhas depois dos campos
    tabelas[NomeTabela].Campos = new Array();
}

function adicionarCampo(NomeTabela, HTMLCampo, IdCampo) {
    var indice = tabelas[NomeTabela].Campos.length;
    tabelas[NomeTabela].Campos.push(new Object());
    tabelas[NomeTabela].Campos[indice].HTML = HTMLCampo;

    if (IdCampo.indexOf(' ') != -1) {
        var arrayCampos = tabelas[NomeTabela].Campos[indice].ID = new Array();
        var vetorIds = IdCampo.split(' ');

        for (var i = 0; i < vetorIds.length; i++)
            arrayCampos.push(vetorIds[i]);

        tabelas[NomeTabela].Campos[indice].EhVetor = true;

        return;
    }

    tabelas[NomeTabela].Campos[indice].ID = IdCampo;
}

function definirFoco(NomeTabela, IdFoco) {
    tabelas[NomeTabela].Campos[0].Foco = IdFoco;
}

// Para definir as linhas extras acima e abaixo dos campos
function DefinirLinhasExtras(NomeTabela, Acima, Abaixo) {
    tabelas[NomeTabela].LinhasExtrasAcima = Acima; // incluindo título
    tabelas[NomeTabela].LinhasExtrasAbaixo = Abaixo; // todas as linhas depois dos campos
}

// Para definir o campo que armazenará a quantidade de linhas na tabela
function DefinirCampoDeQuantidade(NomeTabela, IdCampo) {
    tabelas[NomeTabela].Campos[0].Qtde = IdCampo;
}

function remover(NomeTabela, index) {
    index = Number(index);
    var tabelaAtual = document.getElementById(NomeTabela);

    var tamanho = tabelaAtual.rows.length - tabelas[NomeTabela].LinhasExtrasAbaixo -
                                            tabelas[NomeTabela].LinhasExtrasAcima;

    if (tamanho == 1) {

        if (tabelas[NomeTabela].Campos[0].Foco != null)
            document.getElementById(tabelas[NomeTabela].Campos[0].Foco.replace(/@index/g, '' + index)).focus();
        return false;
    }

    tabelaAtual.deleteRow(index + tabelas[NomeTabela].LinhasExtrasAcima);
    tamanho--;

    for (var i = index; i < tamanho; i++) // loop pelo restante da tabela física
    {
        for (var j = 0; j < tabelas[NomeTabela].Campos.length; j++) // loop para passar pelos campos registrados
        {
            var valor;

            var valores = null;

            if (tabelas[NomeTabela].Campos[j].EhVetor) // Informou campos múltiplos??
            {
                valores = new Array();
                for (var k = 0; k < tabelas[NomeTabela].Campos[j].ID.length; k++) // loop para passar pelos campos
                {
                    if (document.getElementById(tabelas[NomeTabela].Campos[j].ID[k].
                            replace(/@index/g, (i + 1) + '')) != null) {
                        valores.push(document.getElementById(tabelas[NomeTabela].Campos[j].ID[k].
                                        replace(/@index/g, '' + (i + 1))).value);

                    }
                }
                if (valores.length == 0)
                    valores = null;
            }
            else if (document.getElementById(tabelas[NomeTabela].Campos[j].ID.
                replace(/@index/g, (i + 1) + '')) != null) {
                valor = document.getElementById(tabelas[NomeTabela].Campos[j].ID.
                    replace(/@index/g, '' + (i + 1))).value;
            }

            tabelaAtual.rows[i + tabelas[NomeTabela].LinhasExtrasAcima].cells[j].innerHTML =
                tabelas[NomeTabela].Campos[j].HTML.replace(/@index/g, i);

            if (valores != null) { // É um vetor??
                for (var k = 0; k < valores.length; k++) { // loop dos valores registrados
                    var NomeCampo = tabelas[NomeTabela].Campos[j].ID[k].replace(/@index/g, '' + i);
                    document.getElementById(NomeCampo).value = valores[k];
                }
            }
            else {
                document.getElementById(tabelas[NomeTabela].Campos[j].ID.
                    replace(/@index/g, '' + i)).value = valor;
            }

        } // for

    } // for

    if (tabelas[NomeTabela].Campos[0].Qtde != null) // salva quantidade da tabela??
        document.getElementById(tabelas[NomeTabela].Campos[0].Qtde).value = tamanho;

    if (tabelas[NomeTabela].Campos[0].Foco == null) // definiu foco??
        return;

    if (index == tamanho)
        document.getElementById(tabelas[NomeTabela].Campos[0].Foco.
                                replace(/@index/g, index - 1)).focus();
    else
        document.getElementById(tabelas[NomeTabela].Campos[0].Foco.
                                replace(/@index/g, index)).focus();



}

function DeveSalvarInnerHtml(NomeTabela) {
    tabelas[NomeTabela].SalvaInnerHtml = true;
}