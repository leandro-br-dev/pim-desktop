﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;

namespace FlatLoginWatermark
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        #region Drag Form/ Mover Arrastrar Formulario
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        #endregion

        #region Placeholder or WaterMark
        private void txtuser_Enter(object sender, EventArgs e)
        {
            if (txtuser.Text == "Usuário (CPF)")
            {
                txtuser.Text = "";
                txtuser.ForeColor = Color.LightGray;
            }
        }

        private void txtuser_Leave(object sender, EventArgs e)
        {
            if (txtuser.Text == "")
            {
                txtuser.Text = "Usuário (CPF)";
                txtuser.ForeColor = Color.Silver;
            }
        }

        private void txtpass_Enter(object sender, EventArgs e)
        {
            if (txtpass.Text == "Senha")
            {
                txtpass.Text = "";
                txtpass.ForeColor = Color.LightGray;
                txtpass.UseSystemPasswordChar = true;
            }
        }

        private void txtpass_Leave(object sender, EventArgs e)
        {
            if (txtpass.Text == "")
            {
                txtpass.Text = "Senha";
                txtpass.ForeColor = Color.Silver;
                txtpass.UseSystemPasswordChar = false;
            }
        }

        #endregion 

        private void btncerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnminimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

           private Boolean ConectarDB(string sqlQuery)
        {
            
            try
            {
                SqlConnection sqlConn = new SqlConnection("Data Source=sqlserver-pim-2020.cayxzrbvzs1v.sa-east-1.rds.amazonaws.com;Initial Catalog=db_sqlserver;User ID=admin_sql_pim;Password=unip_123");
                SqlCommand selectCMD = new SqlCommand(sqlQuery, sqlConn);
                selectCMD.CommandTimeout = 30;
                selectCMD.Connection.Open();
    
                SqlDataReader reader = selectCMD.ExecuteReader();

                while (reader.Read())
                {
                    if(reader["perfil"].ToString() == "administrador")
                    {
                        return true;
                    }
                  
                }


                // Call Close when done reading.
                reader.Close();
                sqlConn.Close();

                return false;

            }

            catch (Exception ex)
            {                
                throw ex;                
            }
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {

            if (ConectarDB("SELECT perfil FROM TB_CLIENTEs WHERE cpf_cnpj = '" + txtuser.Text + "' AND password = '" + txtpass.Text + "'" ) == true)
            {
                this.Hide();
                FormCad frmCadastro = new FormCad();
                frmCadastro.ShowDialog();
            }
            else
            {
                MessageBox.Show("O usuário ou senha inválido. Acesso permitido somente para usuários com o perfil de Administrador");
            }

            
            
        }

    }
}
