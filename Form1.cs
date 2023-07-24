using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace CRUD_Operations
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // basically connection string is Now, PUBLIC, so we can call it form anywahere in program!
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-49O23E5;Initial Catalog=MyDataBase;Integrated Security=True");
        public int StudentID;
        private void Form1_Load(object sender, EventArgs e)
        {
            GetStudentsRecord();
        }

        private void GetStudentsRecord()
        // GetStudentsRecord() took records form database and show in grid
        {
            // con is a connection object
            // SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-49O23E5;Initial Catalog=MyDataBase;Integrated Security=True");

            SqlCommand cmd = new SqlCommand("Select * from StudentsTb", con);

            // SqlCommand cmd = new SqlCommand("Students_GetRecord", con);
            // create a datatable, coz we get record
            DataTable dt = new DataTable();

            // here we open connection
            con.Open();
            // what ever data come, it comes in sqldatareader
            SqlDataReader sdr = cmd.ExecuteReader();
            // then we load that data
            dt.Load(sdr);
            // then we close that data
            con.Close();

            // we pass datatable with student 
            StudentRecordDataGridView.DataSource = dt;

            // Till Now GET data work is done, Now we will move on to Insert!!

        }

        private bool IsValid()
        {
            if (txtStudentName.Text == string.Empty)
            {
                MessageBox.Show("Student Name is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }


        //for search
        private void button5_Click(object sender, EventArgs e)
        {
            SearchStudentByStudentName();
        }

        private void SearchStudentByStudentName()
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-49O23E5;Initial Catalog=MyDataBase;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand("Select * from StudentsTb where Name = @StudentName", con))
                {
                    DataTable dt = new DataTable();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentName", txtSearch.Text);
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();
                    dt.Load(sdr);

                    if (dt == null)
                    {
                        MessageBox.Show("No Student " + txtStudentName.Text + " is found in the database, Please search with correct name", "Not Found");
                    }
                    else
                    {
                        StudentRecordDataGridView.DataSource = dt;
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(isValid())
            {
                // here we can also use txt_boxes but there is a chance of error, so we specified the parameters so we didn't get any single error in that section!
                SqlCommand cmd = new SqlCommand("INSERT INTO StudentsTb VALUES (@name, @FatherName, @Roll, @Address, @Mobile)",con);
                // and after query, we pass our connection object 
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@name", txtStudentName.Text);
                cmd.Parameters.AddWithValue("@FatherName", txtfatherName.Text);
                cmd.Parameters.AddWithValue("@Roll", txtRollNumber.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);


                // here we open connection

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("New student is sucessfully saved in the database", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // fetch records form database and show in grid
                GetStudentsRecord();
                // that function clear and call the rest button 
                ResetFormControl();
            }
        }

        private bool isValid()
        {
           // all validation related to from, we'll mention here!!

            // cond1 -> if studentName section is enpty show msg 
           if(txtStudentName.Text == string.Empty)
            {
                MessageBox.Show("Student Name is required", "Failed!", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;   
            }
           // otherwise show true!
           return true;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        // Reset funtion call!!
        private void button4_Click(object sender, EventArgs e)
        {
            ResetFormControl();

        }
        // Reset funciton with methods
        private void ResetFormControl()
        {
            StudentID = 0;
            txtStudentName.Clear();
            txtfatherName.Clear();
            txtRollNumber.Clear();
            txtAddress.Clear();
            txtMobile.Clear();
        }
        //Update
        private void button2_Click(object sender, EventArgs e)
        {// if that id is not greater than 0.
            if (StudentID > 0)
            {
                //  here we run sql command for update the specific parameter
                SqlCommand cmd = new SqlCommand("UPDATE StudentsTb SET Name = @name, FatherName = @FatherName, RollNumber = @Roll, Address = @Address, Mobile = @Mobile WHERE StudentID = @ID", con);
                // that code is copy from insert section & yep we use that perticualr code in (insert,Update,Delete & reset).
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@name", txtStudentName.Text);
                cmd.Parameters.AddWithValue("@FatherName", txtfatherName.Text);
                cmd.Parameters.AddWithValue("@Roll", txtRollNumber.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
                cmd.Parameters.AddWithValue("@ID", this.StudentID);


                // here we open connection

                con.Open();
                cmd.ExecuteNonQuery(); 
                con.Close();

                MessageBox.Show("Student Information updated successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // fetch records form database and show in grid
                GetStudentsRecord();
                // that function clear and call the rest button 
                ResetFormControl();
            }
            else
            {
                MessageBox.Show("Please Select an student to update his Information", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StudentRecordDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // when we click on Student Grid record it will show on the txt field
            StudentID = Convert.ToInt32(StudentRecordDataGridView.SelectedRows[0].Cells[0].Value);
            // Datagridview first row[0] of first column[1]
            txtStudentName.Text = StudentRecordDataGridView.SelectedRows[0].Cells[1].Value.ToString();
            txtfatherName.Text = StudentRecordDataGridView.SelectedRows[0].Cells[2].Value.ToString();
            txtRollNumber.Text = StudentRecordDataGridView.SelectedRows[0].Cells[3].Value.ToString();
            txtAddress.Text = StudentRecordDataGridView.SelectedRows[0].Cells[4].Value.ToString();
            txtMobile.Text = StudentRecordDataGridView.SelectedRows[0].Cells[5].Value.ToString();
         
        }

       
        // Delete
        private void button3_Click(object sender, EventArgs e)
        {
            if(StudentID > 0)
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM StudentsTb WHERE StudentID = @ID", con);
                // that code is copy from insert section & yep we use that perticualr code in (insert,Update,Delete & reset).
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", this.StudentID);


                // here we open connection

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Student is Delete from the system", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // fetch records form database and show in grid
                GetStudentsRecord();
                // that function clear and call the rest button 
                ResetFormControl();
            }
            else
            {
                MessageBox.Show("Please Select an student to delete his Information", "Delete?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchStudentByStudentName();
            }

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (button5.Text == string.Empty)
                GetStudentsRecord();
        }
    }
    }



















