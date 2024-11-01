using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        private Dictionary<string, IDisposable> activeListeners = new Dictionary<string, IDisposable>();
        private FirebaseClient firebaseClient;
        private string username;
        private HashSet<string> recipientsList = new HashSet<string>();
        private bool isManualTextChange = false;
       

        public Form1(string username, string fullname)
        {
            InitializeComponent();
            this.username = username;
            lblUsername.Text = $"{fullname}!";

            firebaseClient = new FirebaseClient("https://ltma-e2e8e-default-rtdb.firebaseio.com/");
            btnSend.Enabled = false;
            ListenForMessages();
            LoadRecipients();
            lstRecipients.Click += new EventHandler(lstRecipients_Click);
            LoadRecentConversations();
            MessageBox.Show($"Chào mừng {fullname}!");
            btnSend.Enabled = true;
        }

        private async Task SendPrivateMessage(string sender, string recipient, string content)
        {
            try
            {
                string chatId = GetChatId(sender, recipient);

                var message = new Message
                {
                    Sender = sender,
                    Recipient = recipient,
                    Content = content,
                    SentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                // Gửi tin nhắn lên Firebase
                await firebaseClient
                    .Child("PrivateMessages")
                    .Child(chatId)
                    .PostAsync(message);

                // Thêm người nhận vào danh sách người đã nhắn tin
                await firebaseClient
                    .Child("Users")
                    .Child(sender)
                    .Child("RecentContacts")
                    .Child(recipient)
                    .PutAsync(new { LastMessageTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });

                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi tin nhắn: {ex.Message}");
            }
        }

        private async void LoadRecentConversations()
        {
            try
            {
                // Lấy danh sách tất cả các cuộc hội thoại giữa người dùng hiện tại và người khác
                var conversations = await firebaseClient
                    .Child("PrivateMessages")
                    .OnceAsync<Message>();

                // Kiểm tra tất cả các cuộc trò chuyện để tìm những người đã có tin nhắn với người dùng hiện tại
                HashSet<string> recentContacts = new HashSet<string>(); // Dùng để lưu tên người dùng mà đã có tin nhắn

                foreach (var conversation in conversations)
                {
                    var chatId = conversation.Key; // Lấy chatId

                    // ChatId có thể ở định dạng: user1_user2. Ta sẽ tách để xem có phải liên quan đến người dùng hiện tại không
                    var users = chatId.Split('_');

                    if (users.Length == 2)
                    {
                        string otherUser = (users[0] == username) ? users[1] : (users[1] == username) ? users[0] : null;

                        if (!string.IsNullOrEmpty(otherUser) && !recentContacts.Contains(otherUser))
                        {
                            recentContacts.Add(otherUser);  // Thêm người nhận vào HashSet để tránh trùng lặp
                            lstRecipients.Items.Add(otherUser);  // Thêm vào ListBox
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải các cuộc trò chuyện gần đây: {ex.Message}");
            }
        }

        private void lstRecipients_Click(object sender, EventArgs e)
        {
            if (lstRecipients.SelectedItem != null)
            {
                isManualTextChange = true;  // Tạm thời vô hiệu hóa sự kiện TextChanged

                string selectedUser = lstRecipients.SelectedItem.ToString();
                txtRecipient.Text = selectedUser;

                LoadChatHistory(selectedUser);  // Tự động tải lịch sử tin nhắn

                isManualTextChange = false;  // Kích hoạt lại sự kiện TextChanged
            }
        }




        private async void LoadChatHistory(string recipient)
        {
            string chatId = GetChatId(username, recipient);

            // Xóa nội dung cũ trong RichTextBox
            rtbChat.Clear();

            try
            {
                // Tải các tin nhắn cũ từ Firebase cho cuộc hội thoại hiện tại
                var messages = await firebaseClient
                    .Child("PrivateMessages")
                    .Child(chatId)
                    .OrderByKey()
                    .OnceAsync<Message>();

                // Hiển thị tất cả các tin nhắn cũ trong RichTextBox
                foreach (var message in messages)
                {
                    var msg = message.Object;
                    rtbChat.AppendText($"{msg.Sender}: {msg.Content} ({msg.SentDate})\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải tin nhắn cũ: {ex.Message}");
            }
        }


        private void txtRecipient_TextChanged(object sender, EventArgs e)
        {
            string recipient = txtRecipient.Text.Trim();

            if (!string.IsNullOrEmpty(recipient))
            {
                // Gọi hàm tải lịch sử tin nhắn
                LoadChatHistory(recipient);

                // Bôi đen người dùng B trong ListBox nếu có
                HighlightUserInListBox(recipient);
            }
        }

        // Hàm bôi đen người dùng trong ListBox
        private void HighlightUserInListBox(string recipient)
        {
            // Tìm vị trí của người dùng B trong ListBox
            int index = lstRecipients.Items.IndexOf(recipient);

            if (index != -1)  // Nếu tìm thấy người dùng B trong ListBox
            {
                lstRecipients.SelectedIndex = index;  // Bôi đen người dùng B
            }
            else
            {
                lstRecipients.ClearSelected();  // Nếu không tìm thấy, xóa lựa chọn trước đó
            }
        }




        private async void btnSend_Click_1(object sender, EventArgs e)
        {
            string recipient = txtRecipient.Text;
            string messageContent = txtMessage.Text;

            // Kiểm tra nếu người nhận hoặc tin nhắn trống hoặc chỉ chứa khoảng trắng
            if (string.IsNullOrWhiteSpace(recipient) || string.IsNullOrWhiteSpace(messageContent))
            {
                MessageBox.Show("Vui lòng nhập tên người nhận và tin nhắn.");
                return;
            }

            // Gửi tin nhắn nếu có giá trị
            await SendPrivateMessage(username, recipient, messageContent);

            // Hiển thị tin nhắn tức thời ngay sau khi gửi (tin nhắn của người gửi - màu xanh, căn phải)
            AppendFormattedMessage(username, messageContent, true);

            // Kiểm tra nếu người nhận chưa có trong ListBox thì mới thêm
            if (!lstRecipients.Items.Contains(recipient))
            {
                lstRecipients.Items.Add(recipient);  // Chỉ thêm người nhận vào ListBox nếu chưa có
            }

            // Xóa nội dung trong ô nhập tin nhắn sau khi gửi thành công
            txtMessage.Clear();
        }








        private void txtMessage_Enter(object sender, EventArgs e)
        {
            if (txtMessage.Text == "Nhập tin nhắn vào đây...")
            {
                txtMessage.Text = "";
                txtMessage.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void txtMessage_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                txtMessage.Text = "Nhập tin nhắn vào đây...";
                txtMessage.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private async void LoadRecipients()
        {
            try
            {
                // Lấy danh sách người dùng từ Firebase
                var allUsers = await firebaseClient.Child("Users").OnceAsync<User>();

                // Kiểm tra nếu danh sách không rỗng
                if (allUsers == null || !allUsers.Any())
                {
                    MessageBox.Show("Không có người dùng để hiển thị.");
                    return;
                }

                // Tạo AutoComplete cho txtRecipient
                var autoCompleteCollection = new AutoCompleteStringCollection();

                foreach (var user in allUsers)
                {
                    if (!string.IsNullOrEmpty(user.Object.Username))
                        autoCompleteCollection.Add(user.Object.Username);  // Giả sử bạn có Username trong dữ liệu
                }

                txtRecipient.AutoCompleteCustomSource = autoCompleteCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách người dùng: {ex.Message}");
            }
        }




        private void txtRecipient_Enter(object sender, EventArgs e)
        {
            // Xóa văn bản placeholder khi người dùng nhấn vào ô
            if (txtRecipient.Text == "Nhập tên người nhận...")
            {
                txtRecipient.Text = "";
                txtRecipient.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void txtRecipient_Leave(object sender, EventArgs e)
        {
            // Khôi phục văn bản placeholder nếu người dùng để trống
            if (string.IsNullOrWhiteSpace(txtRecipient.Text))
            {
                txtRecipient.Text = "Nhập tên người nhận...";
                txtRecipient.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void txtRecipient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Ngăn tiếng "bíp"

                string recipient = txtRecipient.Text.Trim();

                if (!string.IsNullOrEmpty(recipient))
                {
                    // Tự động tải lịch sử tin nhắn với người dùng khi nhấn Enter
                    LoadChatHistory(recipient);
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập tên người nhận.");
                }
            }
        }








        private string GetChatId(string user1, string user2)
        {
            return string.Compare(user1, user2) < 0 ? $"{user1}_{user2}" : $"{user2}_{user1}";
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (activeListeners != null)
            {
                foreach (var listener in activeListeners.Values)
                {
                    listener.Dispose();
                }
                activeListeners.Clear();
            }

            this.Close();
            var loginForm = new Login();
            loginForm.Show();
        }

        private void lstUser_Click(object sender, EventArgs e)
        {
            if (lstUser.SelectedItem != null)
            {
                string recipient = lstUser.SelectedItem.ToString();
                txtRecipient.Text = recipient;
                lstUser.Visible = false;  // Ẩn ListBox sau khi chọn

                // Bắt đầu lắng nghe cuộc hội thoại với người nhận mới
                ListenForPrivateMessages(username, recipient);
            }
        }

        private void ListenForMessages()
        {
            firebaseClient
                .Child("Messages")
                .AsObservable<Message>()
                .ObserveOn(SynchronizationContext.Current)  // Đảm bảo UI được cập nhật đúng luồng
                .Subscribe(message =>
                {
                    if (message?.Object == null) return;

                    var msg = message.Object;
                    string sender = msg.Sender ?? "Unknown";
                    string content = msg.Content ?? "(No Content)";
                    string sentDate = msg.SentDate ?? DateTime.Now.ToString();

                    rtbChat.AppendText($"{sender}: {content} ({sentDate})\n");
                },
                ex => MessageBox.Show($"Lỗi khi nhận tin nhắn: {ex.Message}"));
        }

        private async void ListenForPrivateMessages(string sender, string recipient)
        {
            string chatId = GetChatId(sender, recipient);

            // Kiểm tra và hủy bỏ phiên lắng nghe cũ nếu tồn tại
            if (activeListeners.ContainsKey(chatId))
            {
                activeListeners[chatId].Dispose();
                activeListeners.Remove(chatId);
            }

            // Xóa nội dung cũ trong RichTextBox
            rtbChat.Clear();

            try
            {
                // Tải các tin nhắn cũ từ Firebase cho cuộc hội thoại hiện tại
                var messages = await firebaseClient
                    .Child("PrivateMessages")
                    .Child(chatId)
                    .OrderByKey()
                    .OnceAsync<Message>();

                // Hiển thị tất cả các tin nhắn cũ trong RichTextBox
                foreach (var message in messages)
                {
                    var msg = message.Object;

                    // Hiển thị tin nhắn (người nhận - màu xám, căn trái)
                    AppendFormattedMessage(msg.Sender, msg.Content, msg.Sender == username);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải tin nhắn cũ: {ex.Message}");
            }

            // Thiết lập lắng nghe cho tin nhắn mới
            var listener = firebaseClient
                .Child("PrivateMessages")
                .Child(chatId)
                .AsObservable<Message>()
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(message =>
                {
                    if (message?.Object != null)
                    {
                        var msg = message.Object;
                        AppendFormattedMessage(msg.Sender, msg.Content, msg.Sender == username);
                    }
                },
                ex => MessageBox.Show($"Lỗi khi nhận tin nhắn mới: {ex.Message}"));

            // Lưu phiên lắng nghe mới vào Dictionary
            activeListeners[chatId] = listener;
        }

        private void AppendFormattedMessage(string sender, string messageContent, bool isSender)
        {
            // Lưu lại vị trí hiện tại của RichTextBox
            int start = rtbChat.TextLength;

            // Thêm nội dung tin nhắn
            rtbChat.AppendText($"{sender}: {messageContent}\n");

            // Tạo kiểu định dạng cho đoạn văn bản
            rtbChat.Select(start, rtbChat.TextLength - start);

            // Kiểm tra nếu là tin nhắn của người gửi thì căn phải và đặt màu nền xanh
            if (isSender)
            {
                rtbChat.SelectionBackColor = System.Drawing.Color.LightGreen;  // Màu nền xanh
                rtbChat.SelectionAlignment = HorizontalAlignment.Right;  // Căn phải
            }
            else
            {
                rtbChat.SelectionBackColor = System.Drawing.Color.LightGray;  // Màu nền xám
                rtbChat.SelectionAlignment = HorizontalAlignment.Left;  // Căn trái
            }

            // Bỏ chọn định dạng để không ảnh hưởng đến các thao tác tiếp theo
            rtbChat.Select(rtbChat.TextLength, 0);
            rtbChat.SelectionBackColor = rtbChat.BackColor;
            rtbChat.SelectionAlignment = HorizontalAlignment.Left;
        }


    }
}
