using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class TransObj
        {
            public string from { get; set; }
            public string to { get; set; }
            public List<TransResult> trans_result { get; set; }
        }

        public class TransResult
        {
            public string src { get; set; }
            public string dst { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var text = this.textBox1.Text;
            var q = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(text));
            var from = "en";
            var to = "zh";
            var appId = "20171225000108434";
            var solt = "1435660288";
            var key = "XmsVJvxWfJMMbaFjV5Qe";

            var sign = GetMD5(appId + q + solt + key);

            var meg = "http://api.fanyi.baidu.com/api/trans/vip/translate?q=" + q + "&from=" + from + "&to=" + to + "&appid=" + appId + "&salt=" + solt + "&sign=" + sign + "";

            try
            {
                Uri uri = new Uri("http://api.fanyi.baidu.com");
                HttpClient client = new HttpClient();
                client.BaseAddress = uri;
                var pushGet = client.GetAsync("/api/trans/vip/translate?q=" + q + "&from=" + from + "&to=" + to + "&appid=" + appId + "&salt=" + solt + "&sign=" + sign + "");
                var responseResult = pushGet.Result;

                var data = responseResult.Content.ReadAsStringAsync().Result;
                var jsonData = JsonConvert.DeserializeObject<TransObj>(data);
                this.label1.Text = jsonData.trans_result[0].dst;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:" + ex.Message);
            }


            //try
            //{
            //    WebClient client = new WebClient();
            //    string txtInput = q;
            //    string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?q=" + q + "&from=" + from + "&to=" + to + "&appid=" + appId + "&salt=" + solt + "&sign=" + sign + "";
            //    var buffer = client.DownloadData(url);
            //    string result = Encoding.UTF8.GetString(buffer);
            //    StringReader sr = new StringReader(result);
            //    JsonTextReader jsonReader = new JsonTextReader(sr);
            //    JsonSerializer serializer = new JsonSerializer();
            //    var r = serializer.Deserialize<TransObj>(jsonReader);
            //    this.label1.Text = r.trans_result[0].dst;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("错误:" + ex.Message);
            //}

        }

        public string GetMD5(string str)
        {
            byte[] result = Encoding.Default.GetBytes(str.Trim());
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            string aa = null;
            for (int i = 0; i < output.Length; i++)
            {
                aa += output[i].ToString("x");
            }
            return aa;

            //string cl = str;
            //string pwd = "";
            //MD5 md5 = MD5.Create();//实例化一个md5对像
            //// 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            //byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            //// 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            //for (int i = 0; i < s.Length; i++)
            //{
            //    // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
            //    pwd = pwd + s[i].ToString("x");

            //}
            //return pwd;
        }
    }
}
