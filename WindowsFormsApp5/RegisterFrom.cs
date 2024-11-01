using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class RegisterFrom : Form
    {
        private FirebaseClient firebaseClient;
        public RegisterFrom()
        {
            InitializeComponent();
            firebaseClient = new FirebaseClient("https://ltma-e2e8e-default-rtdb.firebaseio.com/");
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtNewUsername.Text;
            string password = txtNewPassword.Text;
            string fullName = txtFullName.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            try
            {
                // Kiểm tra xem tên người dùng đã tồn tại chưa
                var allUsers = await firebaseClient
                    .Child("Users")
                    .OnceAsync<User>();

                var existingUser = allUsers
                    .FirstOrDefault(u => u.Object.Username == username);

                if (existingUser != null)
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.");
                    return;
                }

                // Đăng ký người dùng mới
                var newUser = new User
                {
                    Username = username,
                    Password = password,
                    FullName = fullName
                };

                await firebaseClient
                    .Child("Users")
                    .PostAsync(newUser);

                MessageBox.Show("Đăng ký thành công!");
                this.Close();  // Đóng form đăng ký
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đăng ký: {ex.Message}");
            }
        }

        private void RegisterFrom_Load(object sender, EventArgs e)
        {

        }
    }
}
