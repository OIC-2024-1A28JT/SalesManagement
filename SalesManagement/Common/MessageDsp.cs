using SalesManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.Common
{
    internal class MessageDsp
    {
        ///////////////////////////////
        //メソッド名：MsgDsp()
        //引　数   ：string型：msgId（メッセージ番号）
        //          ：int型   ：id（処理対象のID）
        //          :string型：nm（処理対処の名前）
        //戻り値   ：メッセージのボタン値
        //機　能   ：メッセージを取得して表示
        ///////////////////////////////
        public DialogResult MsgDsp(string msgId, int cnt, string nm)
        {
            DialogResult result = DialogResult.None;
            try
            {
                using var context = new SalesContext();
                var message = context.MMessages.Where(x => x.MsgId == msgId).First();
                MessageBoxButtons btn = new MessageBoxButtons();
                MessageBoxIcon icon = new MessageBoxIcon();
                btn = (MessageBoxButtons)message.MsgButton;
                icon = (MessageBoxIcon)message.MsgIcon;
                string str = "";
                switch (msgId.Substring(0, 2))
                {
                    case "M1":
                        str = "部署";
                        break;
                    case "M2":
                        str = "役職";
                        break;
                }
                result = MessageBox.Show(str + "ID：" + cnt + "　" + str + "名：" + nm + "\n\r" +
                                                message.MsgComments, message.MsgTitle, btn, icon);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        ///////////////////////////////
        //メソッド名：MsgDsp()
        //引　数   ：string型：msgId（メッセージ番号）
        //戻り値   ：メッセージのボタン値
        //機　能   ：メッセージを取得して表示
        ///////////////////////////////
        public DialogResult MsgDsp(string msgId)
        {
            DialogResult result = DialogResult.None;
            try
            {
                using var context = new SalesContext();
                var message = context.MMessages.Where(x => x.MsgId == msgId).First();
                MessageBoxButtons btn = new MessageBoxButtons();
                MessageBoxIcon icon = new MessageBoxIcon();
                btn = (MessageBoxButtons)message.MsgButton;
                icon = (MessageBoxIcon)message.MsgIcon;
                result = MessageBox.Show(message.MsgComments, message.MsgTitle, btn, icon);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
    }
}
