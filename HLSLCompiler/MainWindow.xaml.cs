using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

using SharpDX.D3DCompiler;

namespace HLSLCompiler
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public byte[] hlslbc = null;

        public MainWindow()
        {

            InitializeComponent();

            BtnCompile.Click += BtnCompile_Click;
            BtnExport.Click += BtnExport_Click;
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "shader.byte"; // Default file name
            dlg.DefaultExt = ".hlsl.byte"; // Default file extension
            dlg.Filter = "Shader (.hlsl.byte)|*.hlsl.byte"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                File.WriteAllBytes(filename, hlslbc);
            }
        }

        private void BtnCompile_Click(object sender, RoutedEventArgs e)
        {
            string source = SourceBox.Text;
            if (string.IsNullOrEmpty(source)) return;

            string profile = BoxProfile.Text;
            string entry = BoxEntry.Text;

            BtnExport.IsEnabled = false;

            CompilationResult result = null;

            try
            {
                result = ShaderBytecode.Compile(source, entry, profile);
            }
            catch(Exception except)
            {
                compileResult.Text = $"Error -{except.Message}";
                hlslbc = null;
                return;
            }
            

            if(result == null)
            {
                compileResult.Text = "Failed";
                hlslbc = null;
            }
            else if (result.HasErrors)
            {
                compileResult.Text = $"Error{result.ResultCode} -{result.Message}";
                hlslbc = null;
            }
            else
            {
                if(result.Bytecode != null)
                {
                    compileResult.Text = "Success";
                    hlslbc = result.Bytecode;

                    BtnExport.IsEnabled = true;
                }
                else
                {
                    compileResult.Text = "Failed";
                    hlslbc = null;
                }
            }
        }
    }
}
