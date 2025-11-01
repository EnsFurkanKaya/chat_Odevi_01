using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat_ödevi_01
{
    public static class ChatHub
    {
        // Bir mesaj alındığında tetiklenecek olay (event)
        // Action<string, string> -> (Gönderenin Adı, Mesaj)
        public static event Action<string, string> OnMessageReceived;

        // Formların mesaj göndermek için çağıracağı metot
        public static void SendMessage(string userName, string message)
        {
            // Olayı tetikle
            OnMessageReceived?.Invoke(userName, message);
        }
    }
}
