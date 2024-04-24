﻿using CaixaFerramentas;
using Funcionarios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL_Gerente;
using TL_Principal;

namespace TL_Estoque
{
    public class TelaPrincipalEstoque : TelaPrincipal
    {
        protected TelaAcessoEstoque tela_acesso_estoque;
        protected BtnImage Estoque;
        public Repositor repositor;
        public TelaPrincipalEstoque(Repositor repositor) 
        {
            this.repositor = repositor;
            Estoque = new BtnImage(true, 100, 50, 0, 100, null, null, "Estoque", this);
            Estoque.btn.BackColor = Color.DarkGray;
            Estoque.btn.BringToFront();
            Estoque.btn.Click += new EventHandler(estoque_Click);
        }

        private void estoque_Click(object sender, EventArgs e)
        {
            if(Estoque.atv == true) {
                tela_acesso_estoque = new TelaAcessoEstoque(this, repositor);
                Estoque.atv = false;
            }
            else
            {
                tela_acesso_estoque.fechar();
                Estoque.atv = true;
            }
        }
    }
}
