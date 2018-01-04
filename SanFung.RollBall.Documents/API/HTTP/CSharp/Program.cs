using System;
using System.Threading;
using Hprose.Client;

namespace SanFung.RollBall.API.HTTP.CSharp
{
    internal class Program
    {
        private static string server_ip = "127.0.0.1";      // *服務器地址，請連絡服務器管理員取得正確值。
        private static string server_port = "0000";         // *服務器端口，請連絡服務器管理員取得正確值。

        private static void Main(string[] args)
        {
            HproseClient _http = HproseClient.Create($"http://{server_ip}:{server_port}");
            // 函式登錄
            API.IHello hello = _http.UseService<API.IHello>();
            API.IVerify verify = _http.UseService<API.IVerify>();
            API.IReadStateOfEnable readStateOfEnable = _http.UseService<API.IReadStateOfEnable>();
            API.IReadScriptRemain readScriptRemain = _http.UseService<API.IReadScriptRemain>();
            API.IReadQueryLastOperate readQueryLastOperate = _http.UseService<API.IReadQueryLastOperate>();
            API.IReadFlowOfOperate readFlowOfOperate = _http.UseService<API.IReadFlowOfOperate>();
            API.IReadGetAnswer readGetAnswer = _http.UseService<API.IReadGetAnswer>();
            API.IWriteCommand writeCommand = _http.UseService<API.IWriteCommand>();
            // 函式測試
            string empty = "";
            Console.WriteLine("[RollBall Game API]");
            try
            {
                int requet_remain_limit = 700;      // 注意：取得服務間隔時間低於500豪秒會回應錯誤: (403) 禁止
                Console.WriteLine("");
                Console.WriteLine($"Hello: { hello.Hello("World")}");
                Thread.Sleep(requet_remain_limit);  // 程式實做時你可以使用 Timer
                Console.WriteLine($"Verify: {verify.Verify(new string[] { "127.0.0.1", "admin@52farfar.com", "阿管先生", "發財公司", "我在珠海市" })}");
                Thread.Sleep(requet_remain_limit);
                Console.WriteLine($"ReadStateOfEnable: {string.Join(", ", readStateOfEnable.ReadStateOfEnable(empty))}");
                Thread.Sleep(requet_remain_limit);
                Console.WriteLine($"ReadScriptRemain: {string.Join(", ", readScriptRemain.ReadScriptRemain(empty))}");
                Thread.Sleep(requet_remain_limit);
                Console.WriteLine($"ReadQueryLastOperate: {string.Join(", ", readQueryLastOperate.ReadQueryLastOperate(empty))}");
                Thread.Sleep(450);                  // 遠端伺服器傳回一個錯誤: (403) 禁止
                Console.WriteLine($"ReadFlowOfOperate: {string.Join(", ", readFlowOfOperate.ReadFlowOfOperate(empty))}");
                Thread.Sleep(requet_remain_limit);
                Console.WriteLine($"ReadGetAnswer: {string.Join(", ", readGetAnswer.ReadGetAnswer(empty))}");
                Thread.Sleep(requet_remain_limit);
                Console.WriteLine($"WriteCommand: {writeCommand.WriteCommand("0")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Ending, Press any key to exit!");
            Console.ReadKey();
        }
    }
}