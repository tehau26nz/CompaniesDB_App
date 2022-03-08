using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Windows.Forms;

namespace CompaniesDB
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        static readonly string stdConnection = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;

        int id; // This is for the record ID

        private void frmMain_Load(object sender, EventArgs e)
        {
            StateTextboxes(false);
            InitButtons();
            ReadData();
        }


        private void ReadData()
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(stdConnection))
                {
                    mySqlConnection.Open();
                    MySqlDataAdapter msda = new MySqlDataAdapter("BC_Get_All", mySqlConnection);
                    msda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    msda.Fill(dt);
                    dataGridView.DataSource = dt;
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Can't connect to the database", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }


        private void SaveUpdate()
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(stdConnection))
                {
                    mySqlConnection.Open();
                    MySqlCommand sqlCommand = new MySqlCommand("BC_Add_Update", mySqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("_id", id);  // The ID will tell the SP if this is an update or new Record
                    sqlCommand.Parameters.AddWithValue("_Company", txtCompany.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("_FirstName", txtFirstname.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("_LastName", txtLastname.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("_Title", txtTitle.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("_Email", txtEmail.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("_Website", txtWebsite.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("_Workphone", txtWorkphone.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("_Mobile", txtMobilephone.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("_Address", txtAddress.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("_Zipcode", txtZipcode.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("_City", txtCity.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("_Country", txtCountry.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("_Comments", txtComments.Text.Trim());
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't connect to the database", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MessageBox.Show(ex.Message);
            }
        }

        //***********************************************
        // To populate the datagrid with the MySqlDB data
        //***********************************************

        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            StateTextboxes(true);
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            btnNew.Enabled = false;
            btnClear.Enabled = true;
            DataGridToTextBoxes();
        }

        private void DataGridToTextBoxes()
        {
            txtID.Text = dataGridView.CurrentRow.Cells["id"].Value.ToString();
            id = Int32.Parse(txtID.Text);

            txtCompany.Text = dataGridView.CurrentRow.Cells["Company"].Value.ToString();
            txtFirstname.Text = dataGridView.CurrentRow.Cells["FirstName"].Value.ToString();
            txtLastname.Text = dataGridView.CurrentRow.Cells["LastName"].Value.ToString();
            txtTitle.Text = dataGridView.CurrentRow.Cells["Title"].Value.ToString();
            txtEmail.Text = dataGridView.CurrentRow.Cells["Email"].Value.ToString();
            txtWebsite.Text = dataGridView.CurrentRow.Cells["Website"].Value.ToString();
            txtWorkphone.Text = dataGridView.CurrentRow.Cells["Workphone"].Value.ToString();
            txtMobilephone.Text = dataGridView.CurrentRow.Cells["Mobile"].Value.ToString();
            txtAddress.Text = dataGridView.CurrentRow.Cells["Address"].Value.ToString();
            txtZipcode.Text = dataGridView.CurrentRow.Cells["Zipcode"].Value.ToString();
            txtCity.Text = dataGridView.CurrentRow.Cells["City"].Value.ToString();
            txtCountry.Text = dataGridView.CurrentRow.Cells["Country"].Value.ToString();
            txtComments.Text = dataGridView.CurrentRow.Cells["Comments"].Value.ToString();
        }


        //*****************
        // Buttons section
        //*****************


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveUpdate();
            ClearTextboxes();
            StateTextboxes(false);
            ReadData();
            MessageBox.Show(" Your record has been updated ", " Updated ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            id = 0;
            txtID.Text = "0";
            StateTextboxes(true);
            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveUpdate();
            ClearTextboxes();
            StateTextboxes(false);
            ReadData();
            MessageBox.Show(" Your record has been saved ", " Saved ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        

        //******************
        // To delete records
        //******************


        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(" Delete this record ? ", " Are you sure ? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(stdConnection))
                    {
                        mySqlConnection.Open();
                        MySqlCommand sqlCommand = new MySqlCommand("BC_Delete", mySqlConnection);
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        sqlCommand.Parameters.AddWithValue("_id", id);
                        sqlCommand.ExecuteNonQuery();
                        ReadData();
                        InitButtons();
                        StateTextboxes(false);
                        ClearTextboxes();
                        MessageBox.Show(" Record is deleted ", " Delete ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(" I can't connect to the database ", " Database Error ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearTextboxes();
            StateTextboxes(false);
            InitButtons();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(" Are you sure ? ", "Leave the application", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }


        //***********************
        // Helper methods section
        //***********************


        private void ClearTextboxes()
        {
            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                tb.Text = string.Empty;
            }
        }

        private void StateTextboxes(bool state)
        {
            if (state == false)
            {
                foreach (TextBox tb in this.Controls.OfType<TextBox>())
                {
                    tb.Enabled = false;
                }
            }
            else if (state == true)
            {
                foreach (TextBox tb in this.Controls.OfType<TextBox>())
                {
                    tb.Enabled = true;
                }
            }
        }

        private void InitButtons()
        {
            btnNew.Enabled = true;
            btnUpdate.Enabled = false;
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            btnClear.Enabled = false;
            rdbCompany.Checked = false;
            rdbFirst.Checked = false;
            rdbLast.Checked = false;
        }





        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

    }
}
