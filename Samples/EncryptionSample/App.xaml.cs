using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using MondoCore.Security.Encryption;

namespace EncryptionSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static App _instance;

        public App()
        {
            _instance = this;

            var cipherText = this.ViewModel.Encryptor.Encrypt("bob").Result;

            var decrypted = this.ViewModel.Encryptor.Decrypt(cipherText).Result;

            var blah = decrypted;
        }

        public static App Instance => _instance;

        public EncryptionViewModel ViewModel { get; } = new EncryptionViewModel();
    }
}
