using Baidu.Aip.Ocr;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 百科知识
{
    public partial class FormOcr : Form
    {
        // 设置APPID/AK/SK
        string APP_ID = "18045209";
        string API_KEY = "bgEo5c5L25OqR3O87RDXXx0C";
        string SECRET_KEY = "rGD4muljRZnVGQMIEKjGrLY0HO90LqmF";
        Ocr client;

        public FormOcr()
        {
            InitializeComponent();
            client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDlg1.ShowDialog();
            string fileName = openFileDlg1.FileName;
            if (dr == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(fileName))
            {
                try
                {
                    pictureBox1.Load(fileName);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private void btnOcr_Click(object sender, EventArgs e)
        {

            Ocr(openFileDlg1.FileName, "", null);
        }

        private void btnHandOcr_Click(object sender, EventArgs e)
        {
            Ocr(openFileDlg1.FileName, "hand",null);
        }

        private void Ocr(string fileName, string type, Dictionary<string, object> options)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                return;
            }
            // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
            try
            {
                // 带参数调用通用文字识别, 图片参数为本地图片,options可选
                JToken result;
                byte[] image = File.ReadAllBytes(fileName);
                switch (type) {
                    case "hand":
                        if (null == options)
                        {
                            result = client.Handwriting(image);
                        }
                        else
                        {
                            result = client.Handwriting(image, options);
                        }
                        break;

                    default :
                        result = client.GeneralBasic(image);
                        break;
                }
                image = null;
                 
                JArray wordsArray = (JArray)result["words_result"];
                string wordsStr = "";
                for (int i = 0; i < wordsArray.Count; i++)
                {
                    wordsStr += wordsArray[i]["words"] + "\r\n";
                }
                textBox1.Text = wordsStr;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

}
