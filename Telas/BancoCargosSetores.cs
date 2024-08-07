﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using CaixaDeFerramentasPerso;
using Logica;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Telas
{
    public class BancoCargosSetores : InterfacesBanco
    {
        private LabelP[] labelPs = new LabelP[7];
        private TextBoxP[] textBoxPs = new TextBoxP[5];
        private PanelP[] container = new PanelP[4];

        private DataGridViewP dgvCargo;
        private DataGridViewP dgvSetores;

        private ButtonP btnAddCargo;
        private ButtonP btnRemoveCargo;

        private ButtonP btnAddSetor;
        private ButtonP btnRemoveSetor;

        TelaPadrao tela;

        Panel painel = new Panel();

        private DAO dao = new DAO();
        public override void exibir(TelaPadrao tela)
        {
            this.tela = tela;
            inicializarCargos();
            painel.Width = 1200;
            painel.Height = 5;
            painel.Top = 375;
            painel.BackColor = Color.DarkBlue;
            tela.Controls.Add( painel );
            this.inicializarSetores();
        }

        //
        //
        //CARGOS
        //
        //
        public void inicializarCargos()
        {
            tela.Width = 1200;
            container[0] = new PanelP(490, 200, 85, 20, Color.White, tela);
            labelPs[0] = new LabelP(100, 20, 100, 25, "CARGOS", tela);
            labelPs[0].Font = new Font("Arial", 12, FontStyle.Bold);
            labelPs[1] = new LabelP(30, 20, 135, 25, "ID:", tela);
            labelPs[2] = new LabelP(50, 20, 135, 115, "Nome:", tela);
            labelPs[3] = new LabelP(75, 20, 135, 250, "Descrição:", tela);
            //
            textBoxPs[0] = new TextBoxP(50, 25, 135, 60, "", 3, tela);
            textBoxPs[0].Enabled = false;
            textBoxPs[1] = new TextBoxP(75, 25, 135, 165, "", 75, tela);
            textBoxPs[2] = new TextBoxP(150, 100, 135, 330, "", 300, tela);
            textBoxPs[2].Multiline = true;

            textBoxPs[1].TextChanged += new EventHandler(BancoCargos_TextChanged);
            textBoxPs[2].TextChanged += new EventHandler(BancoCargos_TextChanged);

            //
            dgvCargo = new DataGridViewP(310, 200, 85, 700, dao.lerTabela("select * from cargos"), tela);
            dgvCargo.SelectionChanged += DgvCargo_SelectionChanged;
            container[2] = new PanelP(310, 200, 85, 690, Color.FromArgb(99, 133, 199), tela);
            //

            btnAddCargo = new ButtonP(true, 150, 50, 295, 25, "Adicionar Cargo", tela);
            btnAddCargo.Enabled = false;
            btnAddCargo.Click += new EventHandler(BtnAddCargo_Click);
            btnRemoveCargo = new ButtonP(true, 150, 50, 295, 185, "Remover Cargo", tela);
            btnRemoveCargo.Click += new EventHandler(BtnRemoveCargo_Click);
        }

        private void BtnAddCargo_Click(object sender, EventArgs e)
        {
            string sql = $"insert into Cargos (nome, descricao) values ('{textBoxPs[1].Text}', '{textBoxPs[2].Text}')";
            dao.updateInsertDelete(sql);
            dgvCargo.DataSource = dao.lerTabela("select * from cargos");
            textBoxPs[1].Text = "";
            textBoxPs[2].Text = "";
        }
        private void BtnRemoveCargo_Click(object sender, EventArgs e)
        {
            if (dgvCargo.Rows.Count > 1 && dgvCargo.SelectedCells.Count > 0 && dgvCargo.SelectedRows[0].Index < dgvCargo.Rows.Count - 1)
            {
                int i = int.Parse(dgvCargo.SelectedCells[0].Value.ToString());
                dao.updateInsertDelete($"DELETE from Cargos where id={i}");
                dgvCargo.DataSource = dao.lerTabela("select * from cargos");
            }
        }
        private void BancoCargos_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPs[1].Text != "" && textBoxPs[2].Text != "")
            {
                btnAddCargo.Enabled = true;
            }
            else
            {
                btnAddCargo.Enabled = false;
            }
        }
        private void DgvCargo_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCargo.Rows.Count > 1 && dgvCargo.SelectedCells.Count > 0)
            {
                int i = dgvCargo.SelectedRows[0].Index;
                textBoxPs[0].Text = dgvCargo[0, i].Value.ToString();
                textBoxPs[1].Text = dgvCargo[1, i].Value.ToString();
                textBoxPs[2].Text = dgvCargo[2, i].Value.ToString();
            }
        }
        //
        //
        //SETORES
        //
        //
        public void inicializarSetores()
        {
            //
            container[1] = new PanelP(350, 100, 405, 20, Color.White, tela);
            labelPs[4] = new LabelP(150, 20, 415, 25, "SETORES", tela);
            labelPs[4].Font = new Font("Arial", 12, FontStyle.Bold);
            labelPs[5] = new LabelP(30, 20, 460, 25, "ID:", tela);
            labelPs[6] = new LabelP(50, 20, 460, 115, "Nome:", tela);

            //
            textBoxPs[3] = new TextBoxP(50, 25, 460, 60, "", 9, tela);
            textBoxPs[3].Enabled = false;
            textBoxPs[4] = new TextBoxP(75, 25, 460, 165, "", 9, tela);
            textBoxPs[4].TextChanged += new EventHandler(BancoSetores_TextChanged);

            //
            dgvSetores = new DataGridViewP(215, 200, 405, 700, dao.lerTabela("select * from setores"), tela);
            container[3] = new PanelP(215, 200, 405, 690, Color.FromArgb(99, 133, 199), tela);
            //

            btnAddSetor = new ButtonP(true, 150, 50, 525, 25, "Adicionar Setor", tela);
            btnAddSetor.Enabled = false;
            btnAddSetor.Click += new EventHandler(BtnAddSetor_Click);

            btnRemoveSetor = new ButtonP(true, 150, 50, 525, 185, "Remover Setor", tela);
            btnRemoveSetor.Click += new EventHandler(BtnRemoveSetor_Click);

        }
        private void BtnAddSetor_Click(object sender, EventArgs e)
        {
            string sql = $"insert into Setores (nome) values ('{textBoxPs[4].Text}')";
            dao.updateInsertDelete(sql);
            dgvSetores.DataSource = dao.lerTabela("select * from setores");
            textBoxPs[4].Text = "";
        }
        private void BtnRemoveSetor_Click(object sender, EventArgs e)
        {
            if (dgvSetores.Rows.Count > 1 && dgvSetores.SelectedCells.Count > 0 && dgvSetores.SelectedRows[0].Index < dgvSetores.Rows.Count - 1)
            {
                int i = int.Parse(dgvSetores.SelectedCells[0].Value.ToString());
                dao.updateInsertDelete($"DELETE from Setores where id={i}");
                dgvSetores.DataSource = dao.lerTabela("select * from setores");
            }
        }
        private void BancoSetores_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPs[4].Text != "")
            {
                btnAddSetor.Enabled = true;
            }
            else
            {
                btnAddSetor.Enabled = false;
            }
        }

        public override void fechar(TelaPadrao tela)
        {
            foreach (LabelP labelP in labelPs)
            {
                tela.Controls.Remove(labelP);
            }
            foreach(TextBoxP textBoxP in textBoxPs)
            {
                tela.Controls.Remove(textBoxP);
            }
            foreach(PanelP panelPs in container)
            {
                tela.Controls.Remove(panelPs);
            }
            tela.Controls.Remove(dgvCargo);
            
            tela.Controls.Remove(dgvSetores);
            tela.Controls.Remove(btnAddCargo);
            tela.Controls.Remove(btnAddSetor);
            tela.Controls.Remove(btnRemoveCargo);
            tela.Controls.Remove(btnRemoveSetor);
            tela.Controls.Remove(painel);
        }

        
    }
}
