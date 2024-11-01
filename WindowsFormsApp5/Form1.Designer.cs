using System;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lstUser = new System.Windows.Forms.ListBox();
            this.rtbChat = new System.Windows.Forms.RichTextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtRecipient = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();  // Thêm nút đăng xuất
            this.SuspendLayout();
            // 
            // lstUser
            // 
            this.lstUser.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstUser.FormattingEnabled = true;
            this.lstUser.ItemHeight = 22;
            this.lstUser.Location = new System.Drawing.Point(12, 49);
            this.lstUser.Name = "lstUser";
            this.lstUser.Size = new System.Drawing.Size(156, 378);
            this.lstUser.TabIndex = 0;
            this.lstUser.Visible = false;
            this.lstUser.Click += new System.EventHandler(this.lstUser_Click);
            // 
            // rtbChat
            // 
            this.rtbChat.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbChat.Location = new System.Drawing.Point(210, 52);
            this.rtbChat.Name = "rtbChat";
            this.rtbChat.Size = new System.Drawing.Size(609, 379);
            this.rtbChat.TabIndex = 5;
            this.rtbChat.Text = "";
            // 
            // txtMessage
            // 
            this.txtMessage.Font = new System.Drawing.Font("Arial", 12F);
            this.txtMessage.Location = new System.Drawing.Point(210, 437);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(519, 30);
            this.txtMessage.TabIndex = 2;
            this.txtMessage.Text = "Nhập tin nhắn vào đây...";
            this.txtMessage.ForeColor = System.Drawing.Color.Gray;
            this.txtMessage.Enter += new System.EventHandler(this.txtMessage_Enter);
            this.txtMessage.Leave += new System.EventHandler(this.txtMessage_Leave);
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(735, 437);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(84, 32);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Gửi";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click_1);
            // 
            // txtRecipient
            // 
            this.txtRecipient.Font = new System.Drawing.Font("Arial", 12F);
            this.txtRecipient.Location = new System.Drawing.Point(210, 16);
            this.txtRecipient.Name = "txtRecipient";
            this.txtRecipient.Size = new System.Drawing.Size(609, 30);
            this.txtRecipient.TabIndex = 4;
            this.txtRecipient.Text = "Nhập tên người nhận...";
            this.txtRecipient.ForeColor = System.Drawing.Color.Gray;

            // Sự kiện Enter và Leave để xử lý văn bản placeholder
            this.txtRecipient.Enter += new System.EventHandler(this.txtRecipient_Enter);
            this.txtRecipient.Leave += new System.EventHandler(this.txtRecipient_Leave);

            // Cấu hình AutoComplete cho TextBox
            this.txtRecipient.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.txtRecipient.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.txtRecipient.AutoCompleteCustomSource = new AutoCompleteStringCollection();

            // Sự kiện KeyDown để phát hiện khi người dùng nhấn phím Enter
            this.txtRecipient.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRecipient_KeyDown);




            // 
            // lblUsername
            // 
            this.lblUsername.BackColor = System.Drawing.Color.White;
            this.lblUsername.Font = new System.Drawing.Font("Modern No. 20", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.Location = new System.Drawing.Point(12, 13);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(156, 33);
            this.lblUsername.TabIndex = 6;
            this.lblUsername.Text = "label1";
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // Thêm sau khi khai báo lblUsername
            this.lstRecipients = new System.Windows.Forms.ListBox();
            this.lstRecipients.Font = new System.Drawing.Font("Arial", 10F);
            this.lstRecipients.Location = new System.Drawing.Point(12, 50);  // Đặt vị trí bên dưới lblUsername
            this.lstRecipients.Size = new System.Drawing.Size(156, 150);  // Đặt kích thước phù hợp
            this.lstRecipients.TabIndex = 8;
            this.Controls.Add(this.lstRecipients);  // Thêm ListBox vào giao diện

            // 
            // btnLogout
            // 
            this.btnLogout.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.Location = new System.Drawing.Point(12, 437);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(120, 32); // Tăng chiều rộng lên 120
            this.btnLogout.TabIndex = 7;
            this.btnLogout.Text = "Đăng Xuất";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WindowsFormsApp5.Properties.Resources.Untitled;
            this.ClientSize = new System.Drawing.Size(881, 479);
            this.Controls.Add(this.btnLogout);  // Thêm nút vào giao diện
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtRecipient);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.rtbChat);
            this.Controls.Add(this.lstUser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat App";
            this.ResumeLayout(false);
            this.PerformLayout();


        }

        #endregion

        private System.Windows.Forms.ListBox lstUser;
        private System.Windows.Forms.RichTextBox rtbChat;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtRecipient;
        private Label lblUsername;
        private System.Windows.Forms.Button btnLogout;  // Khai báo nút đăng xuất
        private System.Windows.Forms.ListBox lstRecipients;

    }
}
