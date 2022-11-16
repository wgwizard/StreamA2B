using System.IO;
using System.Threading;
using System;
using System.Text;

namespace StreamA2B
{
    public partial class Form1 : Form
    {
        //文件夹列表
        private static List<string> DirectorysList = new List<string>();
        //文件列表
        private static List<string> FilesinfoList = new List<string>();

        string InitSource;//源路径，只含文件夹不含文件名
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

            folderBrowserDialog1.Description = "请选择文件夹";
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
            folderBrowserDialog1.Description = "请选择文件夹";
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
        /// 获取一个文件夹下的所有文件和文件夹集合
        /// </summary>
        /// <param name="oldSource"></param>
        private void GetDirectoryFileList(string oldSource,string oldTarget)
        {
            DirectoryInfo directory = new DirectoryInfo(oldSource);
            FileSystemInfo[] filesArray = directory.GetFileSystemInfos();

            string appendPath = "";


            foreach (var item in filesArray)
            {
                //是否是一个文件夹
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
                //是文件
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

            //将创建文件流对象的过程写在using当中，会自动关闭流并释放流占用的资源
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