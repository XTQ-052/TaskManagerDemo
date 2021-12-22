using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace TaskManagerDemo
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public List<string> blackList { get; set; } = new List<string>();

		public MainWindow()
		{
			InitializeComponent();
			fillListView();
		}

		private void fillListView()
		{
			foreach (var item in Process.GetProcesses())
			{
				tasks_listview.Items.Add(item.ProcessName);
			}
		}

		private void main_txtbox_GotFocus(object sender, RoutedEventArgs e)
		{
			TextBox txtbox = (TextBox)sender;
			txtbox.Text = string.Empty;
			txtbox.Foreground = Brushes.Black;
			txtbox.GotFocus -= main_txtbox_GotFocus;
		}

		private void createTask_btn_Click(object sender, RoutedEventArgs e)
		{
			if (!find())
			{
				Process.Start(main_txtbox.Text);
			}
			else
			{
				MessageBox.Show("You can't start application in \"Black List\"!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private bool find()
		{
			bool finded = false;

			foreach (var item in Process.GetProcesses())
			{
				for (int i = 0; i < blackList.Count; i++)
				{
					if (item.ProcessName == blackList[i])
					{
						finded = true;
					}
					else
					{
						finded = false;
					}
				}
			}
			return finded;
		}

		private void endTask_btn_Click(object sender, RoutedEventArgs e)
		{
			string processName = tasks_listview.SelectedItem.ToString();

			foreach (var item in Process.GetProcesses())
			{
				if (item.ProcessName == processName)
				{
					item.Kill();
				}
			}
		}

		private void blackList_btn_Click(object sender, RoutedEventArgs e)
		{
			blackList.Add(main_txtbox.Text.ToString());
			MessageBox.Show($"\"{main_txtbox.Text}\" succesfully added in black list!", "Adding Was Succesfully", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void tasks_listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			string processName = tasks_listview.SelectedItem.ToString();
			string processId = "";
			string machine = "";

			foreach (var item in Process.GetProcesses())
			{
				if (item.ProcessName == processName)
				{
					processId = item.Id.ToString();
					machine = item.MachineName;
				}
			}

			MessageBox.Show($"Process Name = {processName}\nProcess Id = {processId}\nMachine = {machine}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}
