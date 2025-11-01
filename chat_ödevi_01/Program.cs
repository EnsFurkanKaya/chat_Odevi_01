using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chat_ödevi_01
{
    internal static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Varsayılan Application.Run(new Form1()); satırını siliyoruz.
            // (Zaten Form1'i sildiğimiz için o satır hata verir)

            // 1. Kullanıcı: Ali
            ChatForm user1 = new ChatForm("Furkan");
            user1.Show(); // Formu göster

            // 2. Kullanıcı: Veli
            ChatForm user2 = new ChatForm("Bera");
            user2.Show(); // Diğer formu da göster

            // Uygulamanın çalışmasını (mesaj döngüsünü) başlat.
            Application.Run();
        }
    }
}
