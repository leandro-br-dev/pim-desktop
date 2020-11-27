using System;
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
    public partial class FormCad : Form
    {        
        public FormCad()
        {
            InitializeComponent();
        }
        List<Class_User> Usuarios = new List<Class_User>();

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

            
            dgvCadastro.DataSource = null;
            //dgvCadastro.DataSource = Usuarios;

            ConectarDB("SELECT nome, cpf_cnpj, email, perfil FROM TB_CLIENTEs WHERE perfil <> 'cliente'");

            cbxFuncao.SelectedIndex = 1;
            btnCad.Focus();
        }

        private void ConectarDB(string sqlQuery)
        {
            
            try
            {
                SqlConnection sqlConn = new SqlConnection("Data Source=sqlserver-pim-2020.cayxzrbvzs1v.sa-east-1.rds.amazonaws.com;Initial Catalog=db_sqlserver;User ID=admin_sql_pim;Password=unip_123");
                SqlCommand selectCMD = new SqlCommand(sqlQuery, sqlConn);
                selectCMD.CommandTimeout = 30;
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = selectCMD;
                sqlConn.Open();

                DataTable table = new DataTable();
                sqlDA.Fill(table);

                BindingSource bSource = new BindingSource();
                bSource.DataSource = table;

                dgvCadastro.DataSource = bSource;

                sqlConn.Close();

                             
                
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ComandoDB(string sqlQuery)
        {

            try
            {
                SqlConnection sqlConn = new SqlConnection("Data Source=sqlserver-pim-2020.cayxzrbvzs1v.sa-east-1.rds.amazonaws.com;Initial Catalog=db_sqlserver;User ID=admin_sql_pim;Password=unip_123");
                SqlCommand selectCMD = new SqlCommand(sqlQuery, sqlConn);
                selectCMD.CommandTimeout = 30;
                selectCMD.Connection.Open();
                selectCMD.ExecuteNonQuery();

                sqlConn.Close();

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void btnCad_Click(object sender, EventArgs e)
        {
            if(txtuser.Text != "Usuário (CPF)" && txtNome.Text != "Nome Completo" && txtEmail.Text != "E-mail")
            {

                Class_User User = new Class_User();

                User.Usuario = txtuser.Text;
                User.Nome = txtNome.Text;
                User.Email = txtEmail.Text;
                User.Senha= txtSenha.Text;
                User.Funcao = cbxFuncao.Text;
                
                string data = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();

                if (btnCad.Text == "CADASTRAR / ALTERAR")
                {
                    ComandoDB("UPDATE  TB_CLIENTEs SET nome = '" + User.Nome + "', cpf_cnpj = '" + User.Usuario + "', email = '" + User.Email + "', perfil = '" + User.Funcao + "', password = '" + User.Senha + "', createdAt = '" + data + "', updatedAt ='" + data + "' WHERE cpf_cnpj = '" + User.Usuario + "'");
                }
                else
                {
                    ComandoDB("INSERT INTO TB_CLIENTEs (nome, cpf_cnpj, email, perfil, password, createdAt, updatedAt) VALUES ('" + User.Nome + "', '" + User.Usuario + "', '" + User.Email + "', '" + User.Funcao + "', '" + User.Senha + "', '" + data + "', '" + data + "' )");
                }

                

                dgvCadastro.DataSource = null;
                ConectarDB("SELECT nome, cpf_cnpj, email, perfil FROM TB_CLIENTEs WHERE perfil <> 'cliente'");

                Limpar_Campos();

            }
            else
            {
                MessageBox.Show("Atenção! Preencha todos os campos.");
            }

                

       

        }

        private void Limpar_Campos()
        {
            btnCad.Text = "CADASTRAR";
            btnExcluir.Enabled = false;
            txtuser.Text = "Usuário (CPF)";
            txtNome.Text = "Nome Completo";
            txtEmail.Text = "E-mail";
            txtSenha.Text = "*******";
            cbxFuncao.SelectedIndex = 1;
        }

        private void txtNome_Enter(object sender, EventArgs e)
        {
            if (txtNome.Text == "Nome Completo")
            {
                txtNome.Text = "";
                txtNome.ForeColor = Color.LightGray;
            }
        }

        private void txtNome_Leave(object sender, EventArgs e)
        {
            if (txtNome.Text == "")
            {
                txtNome.Text = "Nome Completo";
                txtNome.ForeColor = Color.Silver;
            }
        }

        private void txtEmail_Enter(object sender, EventArgs e)
        {
            if (txtEmail.Text == "E-mail")
            {
                txtEmail.Text = "";
                txtEmail.ForeColor = Color.LightGray;
            }
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (txtEmail.Text == "")
            {
                txtEmail.Text = "E-mail";
                txtEmail.ForeColor = Color.Silver;
            }
            else 
            {
                if (!IsValidEmail(txtEmail.Text))
                {
                    MessageBox.Show("Endereço de e-mail inválido.");
                    txtEmail.Focus();
                }
            }
        }


        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void txtuser_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dgvCadastro_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            Limpar_Campos();

            btnCad.Text = "CADASTRAR / ALTERAR";
            btnExcluir.Enabled = true;            

            txtuser.Text = dgvCadastro.CurrentRow.Cells["cpf_cnpj"].Value.ToString();
            txtNome.Text = dgvCadastro.CurrentRow.Cells["nome"].Value.ToString();
            txtEmail.Text = dgvCadastro.CurrentRow.Cells["email"].Value.ToString();
            cbxFuncao.Text = dgvCadastro.CurrentRow.Cells["perfil"].Value.ToString();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            ComandoDB("DELETE FROM TB_CLIENTEs WHERE cpf_cnpj='" + txtuser.Text + "'");
            Limpar_Campos();

            dgvCadastro.DataSource = null;
            ConectarDB("SELECT nome, cpf_cnpj, email, perfil FROM TB_CLIENTEs WHERE perfil <> 'cliente'");


        }

        private void dgvCadastro_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cbxFuncao_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
