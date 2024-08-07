﻿using CaixaDeFerramentasPerso;
using Logica;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telas
{
    internal class BancoEmprestimos : InterfacesBanco
    {
        LabelP[] labelPs = new LabelP[8];
        TextBoxP[] textBoxPs = new TextBoxP[5];
        DateTimePickerP dtLiberacao;
        ComboBoxP cbContaBancaria;
        DataTable dt;
        DAO dao = new DAO();
        string[] lista;
        DateTime dataatual = DateTime.Now;
        DataGridViewP dgv;
        ButtonP btnAdd;
        private PanelP container;
        string tabelasql = "select emprestimos.id as 'ID', ContasBancarias.NumeroConta as 'Conta do Banco', valor_emprestimo as 'Valor Empréstimo', " +
            "data_liberacao as 'Data do Empréstimo', taxa_juros_anual as 'Taxa Anual', prazo_meses as 'Prazo em Meses', valor_pago as 'Valor Pago' from Emprestimos " +
            "inner join ContasBancarias on emprestimos.idContaBancariafk = ContasBancarias.id";

        public override void exibir(TelaPadrao tela)
        {
            int top = 100;
            int left = 35;
            int incrementoTop = 40;
            container = new PanelP(340, 335, 85, 15, Color.White, tela);
            labelPs[0] = new LabelP(150, 20, top, left, "EMPRÉSTIMOS", tela);
            labelPs[0].Font = new Font("Arial", 12, FontStyle.Bold);
            top += incrementoTop;
            labelPs[1] = new LabelP(150, 20, top, left, "ID do Empréstimo:", tela);
            textBoxPs[0] = new TextBoxP(25, 20, top, left + 160, "", 10, tela);
            textBoxPs[0].Enabled = false;
            top += incrementoTop;

            labelPs[2] = new LabelP(150, 20, top, left, "Conta Bancária:", tela);
            cbContaBancaria = new ComboBoxP(50, 20, top, left+160, pegaID("NumeroConta","ContasBancarias"), tela);
            top += incrementoTop;

            labelPs[3] = new LabelP(150, 20, top, left, "Valor do Empréstimo:", tela);
            textBoxPs[1] = new TextBoxP(150, 20, top, left + 160, "", 20, tela, true);
            top += incrementoTop;

            labelPs[4] = new LabelP(150, 20, top, left, "Data de Liberação:", tela);
            dtLiberacao = new DateTimePickerP(150, 20, top, left+160, tela);
            dtLiberacao.MaxDate = dataatual;
            top += incrementoTop;

            labelPs[5] = new LabelP(150, 20, top, left, "Taxa de Juros (%):", tela);
            textBoxPs[2] = new TextBoxP(100, 20, top, left + 160, "", 5, tela, isQuant: true);
            top += incrementoTop;

            labelPs[6] = new LabelP(150, 20, top, left, "Prazo (meses):", tela);
            textBoxPs[3] = new TextBoxP(100, 20, top, left + 160, "", 3, tela, isQuant: true);
            top += incrementoTop;

            labelPs[7] = new LabelP(150, 20, top, left, "Valor pago:", tela);
            textBoxPs[4] = new TextBoxP(100, 20, top, left + 160, "", 3, tela, true);
            textBoxPs[4].Text = "00";
            top += incrementoTop+20;

            btnAdd = new ButtonP(true, 120, 50, top, left, "Cadastrar empréstimo", tela);
            btnAdd.Enabled = false;
            btnAdd.Click += new EventHandler(Btn_add_Click);

            for (int i = 1; i < textBoxPs.Length; i++)
            {
                textBoxPs[i].TextChanged += new EventHandler(BancoEmprestimos_TextChanged);
            }

            dgv = new DataGridViewP(700, 500, 85, 400, dao.lerTabela(tabelasql), tela);
            dgv.SelectionChanged += Dgv_SelectionChanged;
        }
        private void Dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.Rows.Count > 1 && dgv.SelectedCells.Count > 0)
            {
                int i = dgv.SelectedRows[0].Index;
                textBoxPs[0].Text = dgv[0, i].Value.ToString();
                textBoxPs[1].Text = dgv[2, i].Value.ToString();
                for (int j = 2; j < textBoxPs.Length; j++)
                {
                    textBoxPs[j].Text = dgv[j+2, i].Value.ToString().Replace(",", ".");
                }
            }
        }
        private void Btn_add_Click(object sender, EventArgs e)
        {
            string sql = $"insert into emprestimos(idContaBancariafk, valor_emprestimo, data_liberacao, taxa_juros_anual, prazo_meses, valor_pago) values('{cbContaBancaria.SelectedIndex+1}', " +
                $"'{textBoxPs[1].Text}', '{dtLiberacao.pegarData()}', '{textBoxPs[2].Text}', '{textBoxPs[3].Text}', '{textBoxPs[4].Text}');";
            dao.updateInsertDelete(sql);
            dgv.DataSource = dao.lerTabela(tabelasql);
        }

        private void BancoEmprestimos_TextChanged(object sender, EventArgs e)
        {
            int cont = 0;
            for (int i = 1; i < textBoxPs.Length; i++)
            {
                if (textBoxPs[i].Text == "") { cont++; }
            }
            if (cbContaBancaria.SelectedIndex == -1) { cont++; }
            if (cont == 0) { btnAdd.Enabled = true; }
            else { btnAdd.Enabled = false; }
        }
        public string[] pegaID(string dado, string tabela)
        {
            string sql = $"select {dado} from {tabela} order by {dado}";
            dt = dao.lerTabela(sql);
            lista = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lista[i] = dt.Rows[i].ItemArray[0].ToString();
            }
            return lista;
        }
        public override void fechar(TelaPadrao tela)
        {
            
            foreach (TextBoxP tbp in textBoxPs)
            {
                tela.Controls.Remove(tbp);
            }
            foreach (LabelP labelP in labelPs)
            {
                tela.Controls.Remove(labelP);
            }
            tela.Controls.Remove(btnAdd);
            tela.Controls.Remove(cbContaBancaria);
            tela.Controls.Remove(dtLiberacao);
            tela.Controls.Remove(container);
            tela.Controls.Remove(dgv);
        }
    }
}
