using Firebase.Database;
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
    public partial class Login : Form
    {
        private FirebaseClient firebaseClient;

        public Login()
        {
            InitializeComponent();

            // Khởi tạo Firebase Client
            firebaseClient = new FirebaseClient("https://ltma-e2e8e-default-rtdb.firebaseio.com/");
        }

        // Hàm đăng nhập
        private async Task<User> LoginUser(string username, string password)
        {
            try
            {
                // Lấy tất cả người dùng từ Firebase
                var allUsers = await firebaseClient
                    .Child("Users")
                    .OnceAsync<User>();

                // Tìm người dùng có username và password khớp
                var user = allUsers
                    .FirstOrDefault(u => u.Object.Username == username && u.Object.Password == password);

                if (user != null)
                {
                    MessageBox.Show("Đăng nhập thành công!");
                    return user.Object;
                }
                else
                {
                    MessageBox.Show("Thông tin đăng nhập không đúng.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đăng nhập: {ex.Message}");
                return null;
            }
        }


        // Sự kiện khi nhấn nút đăng nhập
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            
        }

        private async void btnLogin_Click_1(object sender, EventArgs e)
        {
            var user = await LoginUser(txtUsername.Text, txtPassword.Text);
            if (user != null)
            {
                // Nếu đăng nhập thành công, mở Form1 và truyền FullName của người dùng
                Form1 chatForm = new Form1(user.Username,user.FullName);
                chatForm.Show();
                this.Hide();  // Ẩn form đăng nhập
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterFrom registerForm = new RegisterFrom();  // Khởi tạo form đăng ký
            registerForm.ShowDialog();  // Mở form đăng ký dưới dạng hộp thoại
        }
    }

        
    }



