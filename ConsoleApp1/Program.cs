using CoreHelper;
using System;

namespace ConsoleApp1
{
    class Program
    {
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
            Console.Read();
        }
    }
}
