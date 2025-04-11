using Microsoft.Data.SqlClient;
using SalesManagement.Common;
using SalesManagement.Model;
using System.Data;
using System.Text;

namespace SalesManagement
{
    public partial class TopForm : Form
    {
        //メッセージ表示用クラスのインスタンス化
        MessageDsp msg = new MessageDsp();

        public TopForm()
        {
            fncMessageImport();
            InitializeComponent();
        }

        private void buttonDivision_Click(object sender, EventArgs e)
        {
            fncDivisionForm();
        }

        private void ToolStripMenuItemDivisionM_Click(object sender, EventArgs e)
        {
            fncDivisionForm();
        }

        ///////////////////////////////
        //メソッド名：fncDivisionForm()
        //引　数   ：なし
        //戻り値   ：なし
        //機　能   ：部署マスタのフォームを開く
        ///////////////////////////////
        private void fncDivisionForm()
        {
            //フォームを透明化
            Opacity = 0;

            //frmMenuをfrmという名前で開く
            DivisionForm frmD = new DivisionForm();
            frmD.ShowDialog();

            //開いたフォームから戻ってきたら
            //メモリを解放する
            frmD.Dispose();
        }

        ///////////////////////////////
        //メソッド名：fncMessageImport()
        //引　数   ：なし
        //戻り値   ：なし
        //機　能   ：メッセージテーブルを確認しデータが
        //          ：存在していなければインポート
        ///////////////////////////////
        private void fncMessageImport()
        {
            try
            {
                using (var context = new SalesContext())
                {
                    //DBのM_Messageテーブルデータ有無チェック
                    //データが存在していなけばデータをインポート
                    int cntMsg = context.MMessages.Count();
                    if (cntMsg > 0)
                    {
                        return;
                    }
                }

                //インポートするCSVファイルの指定
                string csvpth = Path.Combine(Environment.CurrentDirectory, "Message.csv");

                //データテーブルの設定
                DataTable dt = new DataTable();
                dt.TableName = "M_Message";

                //csvファイルの内容をDataTableへ
                // 全行読み込み
                var rows = File.ReadAllLines(csvpth, Encoding.GetEncoding("Shift-JIS")).Select(x => x.Split(','));
                // 列設定
                dt.Columns.AddRange(rows.First().Select(s => new DataColumn(s)).ToArray());
                // 行追加
                foreach (var row in rows.Skip(1))
                {
                    dt.Rows.Add(row);
                }

                //DB接続情報の取得
                var dbpth = System.Configuration.ConfigurationManager.ConnectionStrings["SalesContext"].ConnectionString;
                //DataTableの内容をDBへ追加
                using (var bulkCopy = new SqlBulkCopy(dbpth))
                {
                    bulkCopy.DestinationTableName = dt.TableName;
                    bulkCopy.WriteToServer(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // フォームを閉じる確認メッセージの表示
            DialogResult result = msg.MsgDsp("M00001");

            if (result == DialogResult.OK)
            {
                // OKの時の処理
                Close();
            }
            else
            {
                // キャンセルの時の処理
            }
        }

        private void TopForm_Activated(object sender, EventArgs e)
        {
            if (Opacity == 0)
            {
                Opacity = 1;
            }
        }
    }
}
