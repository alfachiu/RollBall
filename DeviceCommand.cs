using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// DeviceCommand 裝置命令：
/// 如果你不嫌我囉嗦 …………^^，統一格式定義可以避免無畏的狀況發生，及未來延展性。
/// 此程式是給你建議指令格式參考： 分類(前綴+後綴)_長度_項目_資料
/// 分類只用一個位元組時分類只有15個 分類 = 0xF0(前綴) + 後綴0x0F;
/// 命令由你定義
/// 有告知可以避免一直反覆傳送沒必要的相同指令
/// </summary>
namespace SanFung.RollBall
{
    internal class DeviceCommand
    {
        /// <summary>
        /// 定義範例
        /// </summary>
        public enum CmdPrifix
        {
            None,
            Verify,     // 驗證
            Approve,    // 批准
            Answer,     // 落格(答案)
            // ...
            // 操作模式：是自動測試或是正常營業
            // 工作執行：執行工作或是停止工作(機器停止工作)
        }

        public enum CmdSuffix
        {
            Cmd = 0,        // 命令
            Ack = 1,        // 告知

            // 添加如果再次送指令後面的答覆等於查詢
            // 情境：
            AckKnow = 2,    // 告知知道了：(管理)麻煩去撿球一下->(裝置)哦！好的->

            AckBusy = 3,    // 告知忙碌中：(管理)麻煩去撿球一下->(裝置)已經在撿了啦！->
            AckFinish = 4,   // 告知完成了：(管理)麻煩去撿球一下->(裝置)都已經撿好了別再問了！
            AckNo = 5,      // 告知拒絕：(管理)麻煩去撿球一下->(裝置)沒球可以撿，正在做別的事情哦！別煩我。
        }

        /// <summary>
        /// 命令：驗證，(裝置)->(管理)，你要求我驗證
        /// 分類(前綴+後綴)_長度_項目_資料
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public byte[] VerifyCmd(byte[] code)
        {
            List<byte> lists = new List<byte>();    // 暫存器
            lists.Add((byte)CmdPrifix.Verify);      // (分類前綴)
            lists.Add((byte)CmdSuffix.Cmd);         // (分類後綴)
            lists.Add(0);                           // (長度)
            //lists.Add(0);                         // (項目)無
            lists.Add(code[0]);                     // (資料)
            lists.Add(code[1]);                     // (資料)
            lists.Add(code[2]);                     // (資料)
            lists.Add(code[3]);                     // (資料)
            byte[] data = lists.ToArray();
            data[2] = (byte)data.Length;
            return data;                            // _client.Send(data);
        }

        /// <summary>
        /// 告知：驗證，(管理)->(裝置)，答覆給你我運算的結果
        /// </summary>
        /// <param name="ackType">告知類型</param>
        /// <param name="code"></param>
        /// <returns></returns>
        public byte[] VerifyAck(CmdSuffix ackType, byte[] code)
        {
            List<byte> lists = new List<byte>();    // 暫存器
            lists.Add((byte)CmdPrifix.Verify);      // (分類前綴)
            lists.Add((byte)CmdSuffix.Ack);         // (分類後綴)
            lists.Add(0);                           // (長度)
            lists.Add((byte)ackType);               // (項目)無
            lists.Add(code[0]);                     // (資料)
            lists.Add(code[1]);                     // (資料)
            lists.Add(code[2]);                     // (資料)
            lists.Add(code[3]);                     // (資料)
            byte[] data = lists.ToArray();
            data[2] = (byte)data.Length;
            return data;                            // _client.Send(data);
        }

        /// <summary>
        /// 命令：批准，(裝置)->(管理)，已經批准
        /// 分類(前綴+後綴)_長度_項目_資料
        /// </summary>
        /// <returns></returns>
        public byte[] ApproveCmd()
        {
            List<byte> lists = new List<byte>();    // 暫存器
            lists.Add((byte)CmdPrifix.Approve);     // (分類前綴)
            lists.Add((byte)CmdSuffix.Cmd);         // (分類後綴)
            lists.Add(0);                           // (長度)
            // lists.Add(0);                        // (項目)無
            // lists.Add(0);                        // (資料)無
            byte[] data = lists.ToArray();
            data[2] = (byte)data.Length;
            return data;                            // _client.Send(data);
        }

        /// <summary>
        /// 告知：批准，(管理)->(裝置)，答覆給你感謝
        /// </summary>
        /// <param name="ackType">告知類型</param>
        /// <returns></returns>
        public byte[] ApproveAck(CmdSuffix ackType = CmdSuffix.AckKnow)
        {
            List<byte> lists = new List<byte>();    // 暫存器
            lists.Add((byte)CmdPrifix.Approve);      // (分類前綴)
            lists.Add((byte)CmdSuffix.Ack);         // (分類後綴)
            lists.Add(0);                           // (長度)
            lists.Add((byte)ackType);               // (項目)
            // lists.Add(0);                        // (資料)無
            byte[] data = lists.ToArray();
            data[2] = (byte)data.Length;
            return data;                            // _client.Send(data);
        }

        /// <summary>
        /// 命令：答案(停格)，(裝置)->(管理)，通知取答案
        /// 分類(前綴+後綴)_項目_資料
        /// </summary>
        /// <param name="num"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public byte[] AnswerCmd(byte num, byte x, byte y)
        {
            List<byte> lists = new List<byte>();    // 暫存器
            lists.Add((byte)CmdPrifix.Answer);      // (分類前綴)
            lists.Add((byte)CmdSuffix.Cmd);         // (分類後綴)
            lists.Add(0);                           // (長度)
            lists.Add(num);                         // (項目)
            lists.Add(x);                           // (資料)
            lists.Add(y);                           // (資料)
            byte[] data = lists.ToArray();
            data[2] = (byte)data.Length;
            return data;                            // _client.Send(data);
        }

        /// <summary>
        /// 告知：答案(停格)，(管理)->(裝置)，答覆給你我取得資料了
        /// </summary>
        /// <param name="ackType">告知類型</param>
        /// <returns></returns>
        public byte[] AnswerAck(CmdSuffix ackType = CmdSuffix.AckKnow)
        {
            List<byte> lists = new List<byte>();    // 暫存器
            lists.Add((byte)CmdPrifix.Answer);      // (分類前綴)
            lists.Add((byte)CmdSuffix.Ack);         // (分類後綴)
            lists.Add(0);                           // (長度)
            lists.Add((byte)ackType);               // (項目)
            // lists.Add(0);                        // (資料)無
            byte[] data = lists.ToArray();
            data[2] = (byte)data.Length;
            return data;                            // _client.Send(data);
        }
    }
}