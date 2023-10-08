using System;
using System.IO;
using System.Windows.Forms;

namespace Multiple_document_interface
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDirectoriesToTreeView(treeView1.Nodes, @"F:\"); // ��������� ������� ��� �������
        }

 

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem selectedItem = listView1.SelectedItems[0];
            string filePath = Path.Combine(treeView1.SelectedNode.FullPath, selectedItem.Text);

            try
            {
                string fileExtension = Path.GetExtension(filePath).ToLower();
                if (fileExtension == ".txt" || fileExtension == ".rtf" || fileExtension == ".htm" || fileExtension == ".html")
                {
                    richTextBox1.LoadFile(filePath, fileExtension == ".rtf" ? RichTextBoxStreamType.RichText : RichTextBoxStreamType.PlainText);
                }
                else
                {
                    MessageBox.Show("��������� ���� �� �������� ��������� ������.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������: {ex.Message}");
            }
        }

        private void LoadDirectoriesToTreeView(TreeNodeCollection nodes, string path)
        {
            try
            {
                var directories = Directory.GetDirectories(path);
                foreach (var dir in directories)
                {
                    if (dir.Contains("$Recycle.Bin")) continue;

                    TreeNode newNode = new TreeNode(Path.GetFileName(dir));
                    nodes.Add(newNode);
                    LoadDirectoriesToTreeView(newNode.Nodes, dir);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // ��������� ����������, ���� � ��� ��� � ��� �������
            }
            catch (DirectoryNotFoundException)
            {
                // ���������� �� �������. 
                MessageBox.Show($"���������� {path} �� �������.");
            }
            catch (IOException)
            {
                // ������ �����-������ (��������, �������� � ������)
                MessageBox.Show($"������ �����-������ ��� ������ � ����������� {path}.");
            }
            catch (Exception ex)
            {
                // ��� ���� ������ ������
                MessageBox.Show($"��������� ������: {ex.Message}");
            }
        }



        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listView1.Items.Clear();  // ������� ������

            try
            {
                string directoryPath = e.Node.FullPath;

                string[] directories = Directory.GetDirectories(directoryPath);
                foreach (string dir in directories)
                {
                    ListViewItem dirItem = new ListViewItem(Path.GetFileName(dir));
                    listView1.Items.Add(dirItem);
                }

                string[] files = Directory.GetFiles(directoryPath);
                foreach (string file in files)
                {
                    ListViewItem fileItem = new ListViewItem(Path.GetFileName(file));
                    listView1.Items.Add(fileItem);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // ������ ��������
                MessageBox.Show($"� ��� ��� ������� � ���������� {e.Node.FullPath}.");
            }
            catch (DirectoryNotFoundException)
            {
                // ���������� �� �������
                MessageBox.Show($"���������� {e.Node.FullPath} �� �������.");
            }
            catch (IOException)
            {
                // ������ �����-������
                MessageBox.Show($"������ �����-������ ��� ������ � ����������� {e.Node.FullPath}.");
            }
            catch (Exception ex)
            {
                // ��� ���� ������ ������
                MessageBox.Show($"��������� ������: {ex.Message}");
            }
        }


    }
}
