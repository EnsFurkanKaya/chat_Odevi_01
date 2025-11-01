using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chat_ödevi_01
{
    public partial class ChatForm : Form
    {
        // Bu formun kullanıcısının adı
        private string _userName;

        // 1. KURUCU METOT (Tasarımcının çalışması için GEREKLİ)
        // Önceki hatanın ana sebebi bu metodun olmamasıydı.
        public ChatForm()
        {
            InitializeComponent();
        }

        // 2. KURUCU METOT (Bizim kullanacağımız)
        public ChatForm(string userName)
        {
            InitializeComponent();

            _userName = userName;
            this.Text = "Sohbet - " + _userName; // Formun başlığını ayarla

            // ChatHub'dan gelen mesajları dinlemeye başla
            ChatHub.OnMessageReceived += ChatHub_OnMessageReceived;
        }

        // ChatHub'dan bir mesaj geldiğinde bu metot tetiklenir
        private void ChatHub_OnMessageReceived(string senderName, string message)
        {
            // Mesajı chat geçmişine ekle
            Color textColor = (senderName == _userName) ? Color.Green : Color.Blue;
            AppendTextToHistory($"{senderName}: ", textColor, true);
            AppendTextToHistory($"{message}{Environment.NewLine}", Color.Black, false);
        }

        // "Gönder" butonuna tıklandığında
        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        // Mesaj kutusundayken bir tuşa basıldığında (Enter'ı yakalamak için)
        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Enter sesini engelle
                SendMessage();
            }
        }

        // Asıl mesaj gönderme işini yapan metot
        private void SendMessage()
        {
            string message = txtMessage.Text.Trim();
            if (!string.IsNullOrEmpty(message))
            {
                // Mesajı ChatHub aracılığıyla gönder
                ChatHub.SendMessage(_userName, message);
                txtMessage.Clear();
            }
        }

        // Sohbet geçmişine renkli yazı eklemek için yardımcı metot
        private void AppendTextToHistory(string text, Color color, bool isBold)
        {
            // Bu kod, farklı thread'lerden (iş parçacıkları) gelse bile
            // txtChatHistory'e güvenle yazı eklemeyi sağlar.
            if (txtChatHistory.InvokeRequired)
            {
                txtChatHistory.Invoke(new Action(() => AppendTextToHistory(text, color, isBold)));
                return;
            }

            txtChatHistory.SelectionStart = txtChatHistory.TextLength;
            txtChatHistory.SelectionLength = 0;

            txtChatHistory.SelectionColor = color;
            txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, isBold ? FontStyle.Bold : FontStyle.Regular);
            txtChatHistory.AppendText(text);

            txtChatHistory.ScrollToCaret();
        }

        // Form kapatılırken
        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ÖNEMLİ: Bellek sızıntısını önlemek için olay aboneliğini iptal et
            ChatHub.OnMessageReceived -= ChatHub_OnMessageReceived;
        }
    }
}
