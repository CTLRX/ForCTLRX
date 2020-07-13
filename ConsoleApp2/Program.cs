using Core.ConvertHelper;
using Core.Encrypt;
using CoreHelper;
using CoreHelper.Helper;
using CoreHelper.RabbitMQ;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Omipay.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApp2
{
    class Program
    {
        public static string sqlConnection2 = "Server=172.18.8.42;Database=Region;Uid=root;Pwd=GB8XRIE1PXGkrjRqWKog;max pool size=512;SslMode=none;";
        public static IMessageQueue messageQueue=null;
        static void Main(string[] args)
        {
            var data = Post.Request("http://172.31.226.153:8001/Disbursement/GetPageForManage", "{page_index:1,page_size:20,target_account_type:99}");
            //var d = Omipay.Core.Post.Request("http://172.18.8.39:8001/merchantaccount/GetAccountByCurrency", "{merchant_id:3584966748442460160,currency_id:1}", ContentType.Json, null, null, null, null);

            var source = "Server=172.18.8.42;Database=omipay_account;Uid=omipay;Pwd=pGB8XRIE1PXGkrjRqWKog;max pool size=512;port=3307";
            //wHZBwT8mo5ymNKjgYrJ8h2xs+1Zjm11Y9P2tgNZBS8yf+sjYOTWMUl/xuWogmsmnHThVNLbYq/nW8W8KoQ/NbPUXRZ908V42VqeCHh2A/8I++NoPsxocmJOqU4yY39BJ
            //wHZBwT8mo5ymNKjgYrJ8h2xs+1Zjm11Y9P2tgNZBS8yf+sjYOTWMUl/xuWogmsmnHThVNLbYq/nW8W8KoQ/NbPUXRZ908V42VqeCHh2A/8I++NoPsxocmJOqU4yY39BJ
            var encryptSource = Core.Encrypt.AESEncrypt.Encrypt("wHZBwT8mo5ymNKjgYrJ8h2xs+1Zjm11Y9P2tgNZBS8yf+sjYOTWMUl/xuWogmsmnHThVNLbYq/nW8W8KoQ/NbPUXRZ908V42VqeCHh2A/8I++NoPsxocmJOqU4yY39BJ", "a43a7de74b2049338dc52f14edf4b6f4", "2473bf104dd74196a931159b7fe520e7");
            var decryptionSource = Core.Encrypt.AESEncrypt.Decryption("wHZBwT8mo5ymNKjgYrJ8h2xs+1Zjm11Y9P2tgNZBS8yf+sjYOTWMUl/xuWogmsmnHThVNLbYq/nW8W8KoQ/NbPUXRZ908V42VqeCHh2A/8I++NoPsxocmJOqU4yY39BJ", "a43a7de74b2049338dc52f14edf4b6f4", "2473bf104dd74196a931159b7fe520e7");

            //var account =  Omipay.Core.Encrypt.AESEncrypt.Encrypt("Server=172.18.8.42;Database=omipay_account;Uid=root;Pwd=GB8XRIE1PXGkrjRqWKog", "a43a7de74b2049338dc52f14edf4b6f4", "2473bf104dd74196a931159b7fe520e7");
            //var merchant = Omipay.Core.Encrypt.AESEncrypt.Encrypt("Server=172.18.8.42;Database=omipay_merchant;Uid=root;Pwd=GB8XRIE1PXGkrjRqWKog", "a43a7de74b2049338dc52f14edf4b6f4", "2473bf104dd74196a931159b7fe520e7");
            //var payment = Omipay.Core.Encrypt.AESEncrypt.Encrypt("Server=172.18.8.42;Database=omipay_payment;Uid=root;Pwd=GB8XRIE1PXGkrjRqWKog", "a43a7de74b2049338dc52f14edf4b6f4", "2473bf104dd74196a931159b7fe520e7");
            //var common = Omipay.Core.Encrypt.AESEncrypt.Encrypt("Server=172.18.8.42;Database=omipay_common;Uid=root;Pwd=GB8XRIE1PXGkrjRqWKog", "a43a7de74b2049338dc52f14edf4b6f4", "2473bf104dd74196a931159b7fe520e7");
            //var risk  = Omipay.Core.Encrypt.AESEncrypt.Encrypt("Server=172.18.8.42;Database=omipay_riskmanage;Uid=root;Pwd=GB8XRIE1PXGkrjRqWKog", "a43a7de74b2049338dc52f14edf4b6f4", "2473bf104dd74196a931159b7fe520e7");
            //Console.Read();

            TestXmlConvert();
            TestSign();
            TestGetEnumDescription();
            TestToEnum();
            user user = new user()
            {
                id = 1,
                name = "测试RabbitMQ"
            };
            MQ.SendMessage("user2", user);
            MQ.ListenQueue<TestUse>("user2").OnMessageRecieved += Test.Sync;
            messageQueue.SendMessage("jjjjjj", user);
            SqlConnection con = SqlHelper.GetConnection();
            Console.WriteLine("数据库连接成功");
            SqlParameter[] pms = new SqlParameter[]
            {
                new SqlParameter("@TableName",SqlDbType.VarChar,50){ Value ="merchant"},
                new SqlParameter("@ReFieldsStr",SqlDbType.VarChar,200){ Value ="*"},
                new SqlParameter("@OrderString",SqlDbType.VarChar,200){ Value ="id"},
                new SqlParameter("@WhereString",SqlDbType.VarChar,500){ Value ="industry_id=1 and payment_scene=99"},
                new SqlParameter("@PageSize",SqlDbType.Int){ Value =10},
                new SqlParameter("@PageIndex",SqlDbType.Int){ Value =1},
                new SqlParameter("@TotalRecord",SqlDbType.Int){ Value =0},
            };
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "merchant_page",pms);
            foreach (DataRow col in ds.Tables[0].Rows)
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine(col[i].ToString());
                }
            }
        }
        public static void TestXmlConvert()
        {
            Model m = new Model();
            var str = XmlConvert.ObjectToXmlString(m);
            Assert.IsNotNull(str);
        }
        public static void TestSign()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("name", "tom");
            parameters.Add("age", "20");
            parameters.Add("sex", "1");
            parameters.Add("city", "beijing");

            List<string> excludeKeys = new List<string>() { "sex" };

            var result = SignHelper.Sign(parameters, "secret", true, "=", "&", excludeKeys);
            Assert.AreEqual("F81BC2FA90E0B5C79D4E5AD659E057A8", result);
        }
        public static void TestGetEnumDescription()
        {
            var typeDescription = EnumHelper.GetEnumDescription(PaymentType.WaitPay);
            Assert.IsNotNull(typeDescription);
            Assert.AreEqual(typeDescription, "等待支付");
        }
        public static void TestToEnum()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(SnowflakeHelper.BuildId<Class1>());
            }

            Console.WriteLine("------------------------");
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(SnowflakeHelper.BuildId<Class2>());
            }
        }
        public static void TestExcel()
        {
            var data = GetDataMerchant();
            ExcelHelper.Export(data);
        }
        public static DataTable GetDataMerchant()
        {
            string sql = @"SELECT  number,re.`name` from merchant_info m LEFT JOIN region re on m.province_id=re.id where   EXISTS(SELECT id from transaction_list_report  r where r.turnover_type=1  and r.merchant_id=m.id and r.transaction_datetime>='2018-06-01 00:00:00' and r.transaction_datetime<='2020-04-24 23:59:59')";
            using (MySqlConnection conn = new MySqlConnection(sqlConnection2))
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }
    }
    [XmlRoot("xml")]
    public class Model
    {
        public Model()
        {
            this.return_code = "SUCCESS";
            this.return_msg = "OK";
        }

        public string return_code { get; set; }

        public string return_msg { get; set; }
    }

    public enum PaymentType
    {
        [System.ComponentModel.Description("未知")]
        None = 1,

        [System.ComponentModel.Description("等待支付")]
        WaitPay = 2,

        [System.ComponentModel.Description("已经支付")]
        Pay = 3,
    }
    public class Class1
    {

    }

    public class Class2
    {

    }
    public class user
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
