using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace WpfApp1
{
    public partial class MainWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            CommandBinding saveCommand = new CommandBinding(ApplicationCommands.Save, execute_Save, canExecute_Save);
            CommandBindings.Add(saveCommand);

            CommandBinding pasteCommand = new CommandBinding(ApplicationCommands.Paste, execute_Paste, canExecute_Paste);
            CommandBindings.Add(pasteCommand);

            CommandBinding copyCommand = new CommandBinding(ApplicationCommands.Copy, execute_Copy, canExecute_Copy);
            CommandBindings.Add(copyCommand);

            CommandBinding replaceCommand = new CommandBinding(ApplicationCommands.Delete, execute_Replace, canExecute_Replace);
            CommandBindings.Add(replaceCommand);

            CommandBinding openCommand = new CommandBinding(ApplicationCommands.Open, execute_Open, canExecute_Open);
            CommandBindings.Add(openCommand);


        }

        private void canExecute_Open(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        private void execute_Open(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            var initialDirectory = Assembly.GetEntryAssembly()?.Location;
            if (initialDirectory != null)
                openFileDialog.InitialDirectory = initialDirectory;
            if (openFileDialog.ShowDialog() == true)
            {
                textBox1.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void canExecute_Replace(object sender, CanExecuteRoutedEventArgs e)
        {
            if (isEmpty(textBox1)) e.CanExecute = false;
            else e.CanExecute = true;
        }
        private void execute_Replace(object sender, ExecutedRoutedEventArgs e)
        {
            textBox1.Text = "";
        }

        private void canExecute_Copy(object sender, CanExecuteRoutedEventArgs e)
        {
            if (isEmpty(textBox1)) e.CanExecute = false;
            else e.CanExecute = true;
        }
        private void execute_Copy(object sender, ExecutedRoutedEventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Copy();
        }

        void canExecute_Paste(object sender, CanExecuteRoutedEventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();
            // Если есть что-то в буфере обмена то кнопка активна
            if (iData.GetDataPresent(DataFormats.UnicodeText) || iData.GetDataPresent(DataFormats.Text) || iData.GetDataPresent(DataFormats.Html)) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void execute_Paste(object sender, ExecutedRoutedEventArgs e)
        {
            textBox1.Paste();
        }


        void canExecute_Save(object sender, CanExecuteRoutedEventArgs e)
        {
            if (isEmpty(textBox1)) e.CanExecute = false;
            else e.CanExecute = true;

        }
        void execute_Save(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt";
            saveFileDialog1.InitialDirectory = Assembly.GetEntryAssembly().Location;
            if (saveFileDialog1.ShowDialog() == true)
            {
                string data = textBox1.Text.Trim();
                byte[] info = new UTF8Encoding(true).GetBytes(data);
                FileStream fs = (FileStream)saveFileDialog1.OpenFile();
                fs.Write(info, 0, info.Length);
            }
        }

        private bool isEmpty(TextBox input)
        {
            bool empty = (input.Text.Trim().Length == 0) ? true : false;
            return empty;
        }

      
    }

}
