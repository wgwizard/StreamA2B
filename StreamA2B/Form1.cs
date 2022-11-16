using System.IO;
using System.Threading;
using System;
using System.Text;

namespace StreamA2B
{
    public partial class Form1 : Form
    {
        //�ļ����б�
        private static List<string> DirectorysList = new List<string>();
        //�ļ��б�
        private static List<string> FilesinfoList = new List<string>();

        string InitSource;//Դ·����ֻ���ļ��в����ļ���
        string InitTarget;
        //string appendPath="";
        //string Name;
        //string NewName;
        //string NewSource;
        //string NewTarget;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void btnSource_Click(object sender, EventArgs e)
        {

            folderBrowserDialog1.Description = "��ѡ���ļ���";
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog1.ShowNewFolderButton = true;
            if (tbxSource.Text.Length > 0) folderBrowserDialog1.SelectedPath = tbxSource.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbxSource.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnTarget_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "��ѡ���ļ���";
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog1.ShowNewFolderButton = true;
            if (tbxTarget.Text.Length > 0) folderBrowserDialog1.SelectedPath = tbxTarget.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbxTarget.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            //tbxSource.Text = @"C:\Users\castle.wu\Desktop\test";
            //tbxDes.Text = @"C:\Users\castle.wu\Desktop\target";
            InitSource = tbxSource.Text;
            InitTarget = tbxTarget.Text;
            //
            GetDirectoryFileList(InitSource,InitTarget);
            


            //Console.ReadKey();
        }

        /// <summary>
        /// ��ȡһ���ļ����µ������ļ����ļ��м���
        /// </summary>
        /// <param name="oldSource"></param>
        private void GetDirectoryFileList(string oldSource,string oldTarget)
        {
            DirectoryInfo directory = new DirectoryInfo(oldSource);
            FileSystemInfo[] filesArray = directory.GetFileSystemInfos();

            string appendPath = "";


            foreach (var item in filesArray)
            {
                //�Ƿ���һ���ļ���
                if (item.Attributes == FileAttributes.Directory)
                {
                    string[] newArray = item.FullName.Split('\\');
                    string[] oldArray = oldSource.Split('\\');
                    //for (int i = oldArray.Length; i < newArray.Length; i++)
                    //{
                    //    //appendPath = "\\" + newArray[i];
                    //    appendPath =  newArray[i];
                    //}
                    appendPath = newArray[oldArray.Length];
                    string newTarget = oldTarget + "\\" + appendPath;
                    DirectorysList.Add(item.FullName);

                    if (!Directory.Exists(newTarget))
                    {
                        Directory.CreateDirectory(newTarget);
                    }

                    GetDirectoryFileList(item.FullName, newTarget);
                }
                //���ļ�
                else if(!item.FullName.Contains("xml"))
                {
                    FilesinfoList.Add(item.FullName);

                   string NewTarget = oldTarget + "\\"+ appendPath + item.Name + ".xml";

                    ReadWrite(item.FullName, NewTarget);
                }
            }

        }

        private void ReadWrite(string source,string target)
        {//@"C:\Users\castle.wu\Desktop\test";

            //�������ļ�������Ĺ���д��using���У����Զ��ر������ͷ���ռ�õ���Դ
            using (FileStream fsRead = new FileStream(source, FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (FileStream fsWrite = new FileStream(target, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    while (true)
                    {
                        byte[] buffer = new byte[1024 * 1024 * 5];
                        int r = fsRead.Read(buffer, 0, buffer.Length);

                        if (r == 0)
                        {
                            break;
                        }

                        fsWrite.Write(buffer, 0, r);
                    }
                }
            }

        }


    }
}